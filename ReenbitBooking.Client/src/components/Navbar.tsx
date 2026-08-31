import { Link, useNavigate } from 'react-router-dom'
import { useAuth } from '../context/AuthContext'

export function Navbar() {
  const { user, isAdmin, logout } = useAuth()
  const navigate = useNavigate()

  const handleLogout = () => {
    logout()
    navigate('/login')
  }

  return (
    <nav className="flex items-center justify-between bg-slate-800 px-6 py-3 text-white">
      <Link to="/" className="text-lg font-semibold">
        Reenbit Room Booking
      </Link>
      {user && (
        <div className="flex items-center gap-4 text-sm">
          <Link to="/" className="hover:underline">
            Rooms
          </Link>
          <Link to="/my-bookings" className="hover:underline">
            My Bookings
          </Link>
          {isAdmin && (
            <Link to="/all-bookings" className="hover:underline">
              All Bookings
            </Link>
          )}
          <span className="text-slate-300">
            {user.email} ({user.role})
          </span>
          <button
            onClick={handleLogout}
            className="rounded bg-slate-600 px-3 py-1 hover:bg-slate-500"
          >
            Logout
          </button>
        </div>
      )}
    </nav>
  )
}
