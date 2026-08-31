import { useEffect, useState, type FormEvent } from 'react'
import { Link } from 'react-router-dom'
import { useAuth } from '../context/AuthContext'
import { api, getApiErrorMessage } from '../services/api'
import type { Room } from '../types'

export function DashboardPage() {
  const { isAdmin } = useAuth()
  const [rooms, setRooms] = useState<Room[]>([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)

  const [showCreateForm, setShowCreateForm] = useState(false)
  const [name, setName] = useState('')
  const [location, setLocation] = useState('')
  const [capacity, setCapacity] = useState(4)
  const [creating, setCreating] = useState(false)

  const loadRooms = async () => {
    setLoading(true)
    try {
      const response = await api.get<Room[]>('/api/rooms')
      setRooms(response.data)
    } catch (err) {
      setError(getApiErrorMessage(err, 'Could not load rooms.'))
    } finally {
      setLoading(false)
    }
  }

  useEffect(() => {
    loadRooms()
  }, [])

  const handleCreateRoom = async (event: FormEvent) => {
    event.preventDefault()
    setCreating(true)
    setError(null)
    try {
      await api.post('/api/rooms', { name, location, capacity })
      setName('')
      setLocation('')
      setCapacity(4)
      setShowCreateForm(false)
      await loadRooms()
    } catch (err) {
      setError(getApiErrorMessage(err, 'Could not create room.'))
    } finally {
      setCreating(false)
    }
  }

  const handleDeleteRoom = async (roomId: string) => {
    if (!confirm('Delete this room?')) return
    try {
      await api.delete(`/api/rooms/${roomId}`)
      await loadRooms()
    } catch (err) {
      setError(getApiErrorMessage(err, 'Could not delete room.'))
    }
  }

  return (
    <div className="mx-auto mt-8 max-w-3xl px-4">
      <div className="mb-4 flex items-center justify-between">
        <h1 className="text-xl font-semibold">Meeting Rooms</h1>
        {isAdmin && (
          <button
            onClick={() => setShowCreateForm((v) => !v)}
            className="rounded bg-slate-800 px-3 py-1.5 text-sm text-white hover:bg-slate-700"
          >
            {showCreateForm ? 'Cancel' : 'Add Room'}
          </button>
        )}
      </div>

      {error && <p className="mb-4 text-sm text-red-600">{error}</p>}

      {showCreateForm && (
        <form
          onSubmit={handleCreateRoom}
          className="mb-6 flex flex-wrap items-end gap-3 rounded border border-slate-200 p-4"
        >
          <input
            type="text"
            required
            placeholder="Name"
            value={name}
            onChange={(e) => setName(e.target.value)}
            className="rounded border border-slate-300 px-3 py-2"
          />
          <input
            type="text"
            required
            placeholder="Location"
            value={location}
            onChange={(e) => setLocation(e.target.value)}
            className="rounded border border-slate-300 px-3 py-2"
          />
          <input
            type="number"
            required
            min={1}
            placeholder="Capacity"
            value={capacity}
            onChange={(e) => setCapacity(Number(e.target.value))}
            className="w-24 rounded border border-slate-300 px-3 py-2"
          />
          <button
            type="submit"
            disabled={creating}
            className="rounded bg-emerald-600 px-3 py-2 text-white hover:bg-emerald-500 disabled:opacity-50"
          >
            {creating ? 'Saving…' : 'Save'}
          </button>
        </form>
      )}

      {loading ? (
        <p className="text-slate-500">Loading rooms…</p>
      ) : rooms.length === 0 ? (
        <p className="text-slate-500">No rooms yet.</p>
      ) : (
        <ul className="flex flex-col gap-3">
          {rooms.map((room) => (
            <li
              key={room.id}
              className="flex items-center justify-between rounded border border-slate-200 p-4"
            >
              <Link to={`/rooms/${room.id}`} className="flex-1">
                <p className="font-medium">{room.name}</p>
                <p className="text-sm text-slate-500">
                  {room.location} · Capacity {room.capacity}
                </p>
              </Link>
              {isAdmin && (
                <button
                  onClick={() => handleDeleteRoom(room.id)}
                  className="rounded bg-red-600 px-3 py-1.5 text-sm text-white hover:bg-red-500"
                >
                  Delete
                </button>
              )}
            </li>
          ))}
        </ul>
      )}
    </div>
  )
}
