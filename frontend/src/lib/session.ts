import {UserDto} from "../types";

const TOKEN_KEY = 'note_app.access_token'
const USER_KEY = 'note_app.user'
const EXPIRES_KEY = 'note_app.expires_at'

function read(key: string): string | null {
    try {
        return localStorage.getItem(key)
    } catch {
        return null
    }
}

function write(key: string, value: string): void {
    localStorage.setItem(key, value)
}

export interface StoredSession {
    token: string
    user: UserDto
    expiresAt: string | null
}

export function getSession(): StoredSession | null {
    const token = read(TOKEN_KEY)
    const rawUser = read(USER_KEY)
    if (!token || !rawUser) return null

    const expiresAt = read(EXPIRES_KEY)
    if (isExpired(expiresAt)) {
        clearSession()
        return null
    }

    try {
        return { token, user: JSON.parse(rawUser) as UserDto, expiresAt }
    } catch {
        clearSession()
        return null
    }
}

export function setSession(session: StoredSession): void {
    write(TOKEN_KEY, session.token)
    write(USER_KEY, JSON.stringify(session.user))
    if (session.expiresAt) write(EXPIRES_KEY, session.expiresAt)
}

export function clearSession(): void {
    localStorage.removeItem(TOKEN_KEY)
    localStorage.removeItem(USER_KEY)
    localStorage.removeItem(EXPIRES_KEY)
}

export function getToken(): string | null {
    return getSession()?.token ?? null
}

function isExpired(expiresAt: string | null): boolean {
    if (!expiresAt) return false
    const at = Date.parse(expiresAt)
    return Number.isFinite(at) && at <= Date.now()
}