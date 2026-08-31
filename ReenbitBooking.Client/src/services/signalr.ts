import { HubConnectionBuilder, LogLevel, type HubConnection } from '@microsoft/signalr'
import { API_BASE_URL, TOKEN_STORAGE_KEY } from './api'

/**
 * ScheduleHub currently allows anonymous connections, but the access token
 * factory is wired up anyway so this keeps working if the hub is later
 * locked down with [Authorize].
 */
export function createScheduleHubConnection(): HubConnection {
  return new HubConnectionBuilder()
    .withUrl(`${API_BASE_URL}/hubs/schedule`, {
      accessTokenFactory: () => localStorage.getItem(TOKEN_STORAGE_KEY) ?? '',
    })
    .withAutomaticReconnect()
    .configureLogging(LogLevel.Warning)
    .build()
}
