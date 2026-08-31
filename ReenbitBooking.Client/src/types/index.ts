export type UserRole = 'Admin' | 'RegularUser'

export interface AuthUser {
  id: string
  email: string
  role: UserRole
}

export interface Room {
  id: string
  name: string
  location: string
  capacity: number
}

export type SlotStatus = 'Available' | 'Booked'

export interface Slot {
  id: string
  startTimeUtc: string
  endTimeUtc: string
  status: SlotStatus
}

export interface Booking {
  id: string
  roomName: string
  roomLocation: string
  startTimeUtc: string
  endTimeUtc: string
  status: string
  userEmail?: string
}

/**
 * Shape of the ProblemDetails body returned by the API's global exception
 * handler. Unlike the rest of the API (camelCase, System.Text.Json default),
 * this is serialized manually with plain JsonSerializer.Serialize, so the
 * property names come back PascalCase.
 */
export interface ApiProblemDetails {
  Status?: number
  Title?: string
  Detail?: string
  Instance?: string
  errors?: Record<string, string[]>
}

export interface SlotUpdatedEvent {
  slotId: string
  status: SlotStatus
}

export interface SlotDeletedEvent {
  slotId: string
}
