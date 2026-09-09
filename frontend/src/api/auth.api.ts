import type { ApiResponse, AuthResponse, LoginRequest, RegisterRequest } from '@/types'
import { axiosClient, unwrap } from '@/lib/axios-client'

export const AuthApi = {
    async loginAsync(payload: LoginRequest): Promise<ApiResponse<AuthResponse>> {
        const { data } = await axiosClient.post<ApiResponse<AuthResponse>>('/api/Auth/Login', payload)
        return data;
    },

    async registerAsync(payload: RegisterRequest): Promise<ApiResponse<AuthResponse>> {
        const { data } = await axiosClient.post<ApiResponse<AuthResponse>>('/api/Auth/Register', payload)
        return data;
    },
}
