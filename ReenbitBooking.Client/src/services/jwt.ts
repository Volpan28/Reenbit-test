import type { AuthUser, UserRole } from '../types'

// Long-form claim URIs used by ClaimTypes.* on the backend (JwtProvider.cs) —
// the token payload does not use short claim names like "sub"/"email"/"role".
const NAME_IDENTIFIER_CLAIM = 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'
const EMAIL_CLAIM = 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'
const ROLE_CLAIM = 'http://schemas.microsoft.com/ws/2008/06/identity/claims/role'

/** Decodes a JWT's payload segment into the user info embedded by the API. Returns null if the token is malformed. */
export function decodeAuthUser(token: string): AuthUser | null {
  try {
    const payloadSegment = token.split('.')[1]
    const base64 = payloadSegment.replace(/-/g, '+').replace(/_/g, '/')
    const json = decodeURIComponent(
      atob(base64)
        .split('')
        .map((c) => '%' + c.charCodeAt(0).toString(16).padStart(2, '0'))
        .join(''),
    )
    const payload = JSON.parse(json) as Record<string, string>

    const id = payload[NAME_IDENTIFIER_CLAIM]
    const email = payload[EMAIL_CLAIM]
    const role = payload[ROLE_CLAIM] as UserRole
    if (!id || !email || !role) return null

    return { id, email, role }
  } catch {
    return null
  }
}
