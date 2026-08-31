import { useEffect, useRef, useState, type FormEvent } from 'react'
import { Link, useParams } from 'react-router-dom'
import type { HubConnection } from '@microsoft/signalr'
import { useAuth } from '../context/AuthContext'
import { api, getApiErrorMessage } from '../services/api'
import { createScheduleHubConnection } from '../services/signalr'
import type { Room, Slot, SlotDeletedEvent, SlotUpdatedEvent } from '../types'

export function RoomSchedulePage() {
  const { roomId } = useParams<{ roomId: string }>()
  const { isAdmin } = useAuth()

  const [room, setRoom] = useState<Room | null>(null)
  const [slots, setSlots] = useState<Slot[]>([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)
  const [notice, setNotice] = useState<string | null>(null)
  const [bookingSlotId, setBookingSlotId] = useState<string | null>(null)

  const [showCreateForm, setShowCreateForm] = useState(false)
  const [startTime, setStartTime] = useState('')
  const [endTime, setEndTime] = useState('')
  const [creatingSlot, setCreatingSlot] = useState(false)

  const connectionRef = useRef<HubConnection | null>(null)

  useEffect(() => {
    if (!roomId) return

    let cancelled = false

    const loadData = async () => {
      setLoading(true)
      try {
        const [roomResponse, slotsResponse] = await Promise.all([
          api.get<Room>(`/api/rooms/${roomId}`),
          api.get<Slot[]>(`/api/slots/room/${roomId}`),
        ])
        if (cancelled) return
        setRoom(roomResponse.data)
        setSlots(slotsResponse.data)
      } catch (err) {
        if (!cancelled) setError(getApiErrorMessage(err, 'Could not load the room schedule.'))
      } finally {
        if (!cancelled) setLoading(false)
      }
    }

    loadData()

    // Real-time sync: join this room's SignalR group so booking/slot changes
    // made by any client are reflected here immediately, without polling.
    const connection = createScheduleHubConnection()
    connectionRef.current = connection

    connection.on('SlotUpdated', (data: SlotUpdatedEvent) => {
      setSlots((current) =>
        current.map((slot) => (slot.id === data.slotId ? { ...slot, status: data.status } : slot)),
      )
    })

    connection.on('SlotDeleted', (data: SlotDeletedEvent) => {
      setSlots((current) => current.filter((slot) => slot.id !== data.slotId))
    })

    connection
      .start()
      .then(() => connection.invoke('JoinRoomGroup', roomId))
      .catch(() => {
        if (!cancelled) setError('Could not connect to live schedule updates.')
      })

    return () => {
      cancelled = true
      if (connection.state === 'Connected') {
        connection.invoke('LeaveRoomGroup', roomId).catch(() => {})
      }
      connection.stop()
    }
  }, [roomId])

  const handleBook = async (slotId: string) => {
    setBookingSlotId(slotId)
    setNotice(null)
    setError(null)
    try {
      await api.post('/api/bookings', { slotId })
      setNotice('Slot booked successfully.')
      // The slot's status flips to "Booked" via the SlotUpdated broadcast.
    } catch (err) {
      const axiosError = err as { response?: { status?: number } }
      if (axiosError.response?.status === 409) {
        setError('This slot was just booked by someone else. Please choose another.')
      } else {
        setError(getApiErrorMessage(err, 'Could not book this slot.'))
      }
    } finally {
      setBookingSlotId(null)
    }
  }

  const handleCreateSlot = async (event: FormEvent) => {
    event.preventDefault()
    if (!roomId) return
    setCreatingSlot(true)
    setError(null)
    try {
      await api.post('/api/slots', {
        roomId,
        startTimeUtc: new Date(startTime).toISOString(),
        endTimeUtc: new Date(endTime).toISOString(),
      })
      setStartTime('')
      setEndTime('')
      setShowCreateForm(false)
      const response = await api.get<Slot[]>(`/api/slots/room/${roomId}`)
      setSlots(response.data)
    } catch (err) {
      setError(getApiErrorMessage(err, 'Could not create slot.'))
    } finally {
      setCreatingSlot(false)
    }
  }

  const handleDeleteSlot = async (slotId: string) => {
    try {
      await api.delete(`/api/slots/${slotId}`)
      // The slot is removed from the list via the SlotDeleted broadcast.
    } catch (err) {
      setError(getApiErrorMessage(err, 'Could not delete slot.'))
    }
  }

  if (loading) return <p className="mx-auto mt-8 max-w-3xl px-4 text-slate-500">Loading…</p>

  return (
    <div className="mx-auto mt-8 max-w-3xl px-4">
      <Link to="/" className="text-sm text-slate-500 hover:underline">
        ← Back to rooms
      </Link>

      {room && (
        <div className="mb-4 mt-2">
          <h1 className="text-xl font-semibold">{room.name}</h1>
          <p className="text-sm text-slate-500">
            {room.location} · Capacity {room.capacity}
          </p>
        </div>
      )}

      {notice && <p className="mb-4 text-sm text-emerald-600">{notice}</p>}
      {error && <p className="mb-4 text-sm text-red-600">{error}</p>}

      {isAdmin && (
        <div className="mb-6">
          <button
            onClick={() => setShowCreateForm((v) => !v)}
            className="rounded bg-slate-800 px-3 py-1.5 text-sm text-white hover:bg-slate-700"
          >
            {showCreateForm ? 'Cancel' : 'Add Slot'}
          </button>
          {showCreateForm && (
            <form
              onSubmit={handleCreateSlot}
              className="mt-3 flex flex-wrap items-end gap-3 rounded border border-slate-200 p-4"
            >
              <label className="flex flex-col text-sm text-slate-600">
                Start
                <input
                  type="datetime-local"
                  required
                  value={startTime}
                  onChange={(e) => setStartTime(e.target.value)}
                  className="rounded border border-slate-300 px-3 py-2"
                />
              </label>
              <label className="flex flex-col text-sm text-slate-600">
                End
                <input
                  type="datetime-local"
                  required
                  value={endTime}
                  onChange={(e) => setEndTime(e.target.value)}
                  className="rounded border border-slate-300 px-3 py-2"
                />
              </label>
              <button
                type="submit"
                disabled={creatingSlot}
                className="rounded bg-emerald-600 px-3 py-2 text-white hover:bg-emerald-500 disabled:opacity-50"
              >
                {creatingSlot ? 'Saving…' : 'Save'}
              </button>
            </form>
          )}
        </div>
      )}

      {slots.length === 0 ? (
        <p className="text-slate-500">No slots scheduled for this room.</p>
      ) : (
        <ul className="flex flex-col gap-2">
          {slots.map((slot) => (
            <li
              key={slot.id}
              className="flex items-center justify-between rounded border border-slate-200 p-3"
            >
              <div>
                <p className="text-sm">
                  {new Date(slot.startTimeUtc).toLocaleString()} —{' '}
                  {new Date(slot.endTimeUtc).toLocaleString()}
                </p>
                <span
                  className={`text-xs font-medium ${
                    slot.status === 'Available' ? 'text-emerald-600' : 'text-red-600'
                  }`}
                >
                  {slot.status}
                </span>
              </div>
              <div className="flex gap-2">
                {!isAdmin && slot.status === 'Available' && (
                  <button
                    onClick={() => handleBook(slot.id)}
                    disabled={bookingSlotId === slot.id}
                    className="rounded bg-slate-800 px-3 py-1.5 text-sm text-white hover:bg-slate-700 disabled:opacity-50"
                  >
                    {bookingSlotId === slot.id ? 'Booking…' : 'Book'}
                  </button>
                )}
                {isAdmin && (
                  <button
                    onClick={() => handleDeleteSlot(slot.id)}
                    className="rounded bg-red-600 px-3 py-1.5 text-sm text-white hover:bg-red-500"
                  >
                    Delete
                  </button>
                )}
              </div>
            </li>
          ))}
        </ul>
      )}
    </div>
  )
}
