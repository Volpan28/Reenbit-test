import { useState, type FormEvent } from 'react'
import { Link, useNavigate } from 'react-router-dom'
import { useAuth } from '../context/AuthContext'
import { getApiErrorMessage } from '../services/api'

export function RegisterPage() {
  const { register } = useAuth()
  const navigate = useNavigate()
  const [fullName, setFullName] = useState('')
  const [email, setEmail] = useState('')
  const [password, setPassword] = useState('')
  const [error, setError] = useState<string | null>(null)
  const [submitting, setSubmitting] = useState(false)

  const handleSubmit = async (event: FormEvent) => {
    event.preventDefault()
    setError(null)
    setSubmitting(true)
    try {
      await register(fullName, email, password)
      navigate('/')
    } catch (err) {
      setError(getApiErrorMessage(err, 'Could not register. Please try again.'))
    } finally {
      setSubmitting(false)
    }
  }

  return (
    <div className="mx-auto mt-16 w-full max-w-sm rounded-lg border border-slate-200 p-6 shadow-sm">
      <h1 className="mb-4 text-xl font-semibold">Register</h1>
      <form onSubmit={handleSubmit} className="flex flex-col gap-3">
        <input
          type="text"
          required
          placeholder="Full name"
          value={fullName}
          onChange={(e) => setFullName(e.target.value)}
          className="rounded border border-slate-300 px-3 py-2"
        />
        <input
          type="email"
          required
          placeholder="Email"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          className="rounded border border-slate-300 px-3 py-2"
        />
        <input
          type="password"
          required
          minLength={8}
          placeholder="Password (min. 8 characters)"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          className="rounded border border-slate-300 px-3 py-2"
        />
        {error && <p className="text-sm text-red-600">{error}</p>}
        <button
          type="submit"
          disabled={submitting}
          className="rounded bg-slate-800 px-3 py-2 text-white hover:bg-slate-700 disabled:opacity-50"
        >
          {submitting ? 'Creating account…' : 'Register'}
        </button>
      </form>
      <p className="mt-4 text-sm text-slate-600">
        Already have an account?{' '}
        <Link to="/login" className="text-slate-900 underline">
          Log in
        </Link>
      </p>
    </div>
  )
}
