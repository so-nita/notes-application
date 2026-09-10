import axios, { AxiosError } from 'axios'
import { clearSession, getToken } from './session'
import type {ApiResponse} from "../types";

export const axiosClient = axios.create({
    baseURL: import.meta.env.DEV ? '' : (import.meta.env.VITE_API_BASE_URL ?? ''),
    headers: { 'Content-Type': 'application/json' },
    timeout: 15000,
})

axiosClient.interceptors.request.use((config) => {
    const token = getToken()
    if (token) {
        config.headers.Authorization = `Bearer ${token}`
    }
    return config
})

let onUnauthorized: (() => void) | null = null

export function setUnauthorizedHandler(handler: () => void): void {
    onUnauthorized = handler
}

axiosClient.interceptors.response.use(
    (response) => response,
    (error: AxiosError) => {
        if (error.response?.status === 401) {
            clearSession()
            onUnauthorized?.()
        }
        return Promise.reject(error)
    },
)
interface ProblemDetails {
    title?: string
    detail?: string
    errors?: Record<string, string[]>
}

export function toErrorMessage(error: unknown, fallback = 'Something went wrong.'): string {
    if (!(error instanceof AxiosError)) {
        return error instanceof Error ? error.message : fallback
    }

    if (error.code === 'ECONNABORTED') {
        return 'The request timed out. Please try again.'
    }

    if (!error.response) {
        return 'Cannot reach the API. Check your connection and try again.'
    }

    if (error.response.status === 401) {
        return 'Your session has expired. Please sign in again.'
    }

    const body = error.response.data as (ApiResponse<unknown> & ProblemDetails) | undefined
    if (!body || typeof body !== 'object') return fallback

    const validation = body.errors && Object.values(body.errors).flat()

    return body.message ?? validation?.[0] ?? body.detail ?? body.title ?? fallback
}

export function unwrap<T>(response: ApiResponse<T>, fallback: string): T {
    if (!response.isSuccess) {
        throw new Error(response.message ?? fallback)
    }
    return response.data
}