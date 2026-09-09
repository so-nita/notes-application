
export interface ApiResponse<T> {
    isSuccess: boolean
    statusCode: number
    message: string
    data: T
}

export interface RegisterRequest {
    fullName?: string;
    username: string;
    password: string;
}

export interface LoginRequest {
    username: string;
    password: string;
}

export interface AuthResponse {
    accessToken: string;
    accessTokenExpiresAt: string;
    id: string;
    username: string;
}

export interface UserDto {
    id: string;
    username: string;
}

export interface Note {
    id: string;
    title: string;
    content: string | null;
    createdAt: string;
    updatedAt?: string | null;
}

export interface NoteCreateRequest {
    title: string
    content: string | null
}

export interface NoteUpdateRequest {
    title: string
    content: string | null
}

export type SortKey = 'createdAt' | 'updatedAt' | 'title'
export type SortDirection = 'asc' | 'desc'
export type NoteFilter = 'all' | 'withContent' | 'empty' | 'edited'
