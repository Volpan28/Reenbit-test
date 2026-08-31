import { useEffect, useState } from 'react'
import { api, getApiErrorMessage } from '../services/api'
import type { Booking } from '../types'

export function AllBookingsPage() {
  const [bookings, setBookings] = useState<Booking[]>([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)

  useEffect(() => {
    api
      .get<Booking[]>('/api/bookings/all')
      .then((response) => setBookings(response.data))
      .catch((err) => setError(getApiErrorMessage(err, 'Could not load bookings.')))
      .finally(() => setLoading(false))
  }, [])

  return (
    <div className="mx-auto mt-8 max-w-3xl px-4">
      <h1 className="mb-4 text-xl font-semibold">All Bookings</h1>
      {error && <p className="mb-4 text-sm text-red-600">{error}</p>}
      {loading ? (
        <p className="text-slate-500">Loading…</p>
      ) : bookings.length === 0 ? (
        <p className="text-slate-500">No bookings yet.</p>
      ) : (
        <ul className="flex flex-col gap-2">
          {bookings.map((booking) => (
            <li key={booking.id} className="rounded border border-slate-200 p-3">
              <p className="font-medium">
                {booking.roomName} · {booking.roomLocation}
              </p>
              <p className="text-sm text-slate-500">
                {new Date(booking.startTimeUtc).toLocaleString()} —{' '}
                {new Date(booking.endTimeUtc).toLocaleString()}
              </p>
              <p className="text-xs text-slate-500">Booked by {booking.userEmail ?? 'unknown'}</p>
              <span className="text-xs font-medium text-slate-600">{booking.status}</span>
            </li>
          ))}
        </ul>
      )}
    </div>
  )
}
