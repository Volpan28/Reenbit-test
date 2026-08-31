import axios, { type AxiosError } from 'axios'
import type { ApiProblemDetails } from '../types'

export const API_BASE_URL =
  import.meta.env.VITE_API_BASE_URL || 'http://localhost:5169'

export const TOKEN_STORAGE_KEY = 'rb_token'

export const api = axios.create({
  baseURL: API_BASE_URL,
})

api.interceptors.request.use((config) => {
  const token = localStorage.getItem(TOKEN_STORAGE_KEY)
  if (token) {
    config.headers.Authorization = `Bearer ${token}`
  }
  return config
})

/**
 * Error bodies from the API are inconsistent: validation/business errors go
 * through the global exception handler as PascalCase ProblemDetails, while
 * everything else uses the framework's camelCase default. Check both casings
 * so callers get a readable message regardless of which path produced it.
 */
export function getApiErrorMessage(error: unknown, fallback: string): string {
  const axiosError = error as AxiosError<ApiProblemDetails>
  const data = axiosError.response?.data
  if (!data) return fallback
  if (data.errors) {
    const firstField = Object.values(data.errors)[0]
    if (firstField?.length) return firstField[0]
  }
  return data.Detail || data.Title || fallback
}
