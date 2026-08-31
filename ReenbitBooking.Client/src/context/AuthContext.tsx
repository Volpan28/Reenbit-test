import { createContext, useContext, useMemo, useState, type ReactNode } from 'react'
import { api, TOKEN_STORAGE_KEY } from '../services/api'
import { decodeAuthUser } from '../services/jwt'
import type { AuthUser } from '../types'

interface AuthContextValue {
  user: AuthUser | null
  isAdmin: boolean
  login: (email: string, password: string) => Promise<void>
  register: (fullName: string, email: string, password: string) => Promise<void>
  logout: () => void
}

const AuthContext = createContext<AuthContextValue | null>(null)

function loadInitialUser(): AuthUser | null {
  const token = localStorage.getItem(TOKEN_STORAGE_KEY)
  return token ? decodeAuthUser(token) : null
}

export function AuthProvider({ children }: { children: ReactNode }) {
  const [user, setUser] = useState<AuthUser | null>(loadInitialUser)

  const login = async (email: string, password: string) => {
    const response = await api.post<{ token: string }>('/api/auth/login', { email, password })
    const { token } = response.data
    localStorage.setItem(TOKEN_STORAGE_KEY, token)
    setUser(decodeAuthUser(token))
  }

  const register = async (fullName: string, email: string, password: string) => {
    await api.post('/api/auth/register', { fullName, email, password })
    await login(email, password)
  }

  const logout = () => {
    localStorage.removeItem(TOKEN_STORAGE_KEY)
    setUser(null)
  }

  const value = useMemo(
    () => ({ user, isAdmin: user?.role === 'Admin', login, register, logout }),
    [user],
  )

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>
}

export function useAuth(): AuthContextValue {
  const context = useContext(AuthContext)
  if (!context) throw new Error('useAuth must be used within an AuthProvider')
  return context
}
