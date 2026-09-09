import {defineStore} from "pinia";
import {computed, ref} from "vue";
import type {AuthResponse, LoginRequest, RegisterRequest, UserDto} from "@/types";
import {toErrorMessage} from "@/lib/axios-client";
import {AuthApi} from "@/api/auth.api";
import {clearSession, getSession, setSession} from "@/lib/session";

export const useAuthStore = defineStore('auth', () => {
    const restored = getSession()
    const user = ref<UserDto | null>(restored?.user ?? null)
    const token = ref<string | null>(restored?.token ?? null)
    const loading = ref(false)
    const error = ref<string | null>(null)

    const isAuthenticated = computed(() => token.value !== null)

    function applySession(auth: AuthResponse): void {
        const nextUser: UserDto = { id: auth.id, username: auth.username }
        token.value = auth.accessToken
        user.value = nextUser
        setSession({
            token: auth.accessToken,
            user: nextUser,
            expiresAt: auth.accessTokenExpiresAt ?? null,
        })
    }
    async function loginAsync (payload: LoginRequest) : Promise<boolean> {
        return run(async () => {
            const resposnse = await AuthApi.loginAsync(payload);

            if(resposnse.isSuccess){
                let data = resposnse.data;
                applySession(data);
            }
        }, "")
    }

    async function registerAsync(payload: RegisterRequest): Promise<boolean> {
        return run(async () => {
            const resposnse = await AuthApi.registerAsync(payload)
            if(resposnse.isSuccess){
                let data = resposnse.data;
                applySession(data);
            }
        }, 'Could not create the account.')
    }

    function logout(): void {
        token.value = null
        user.value = null
        error.value = null
        clearSession()
    }
    function clearError() {
        error.value = null
    }

    async function run(action: () => Promise<void>, fallback: string): Promise<boolean> {
        loading.value = true
        error.value = null
        try {
            await action()
            return true
        } catch (err) {
            error.value = toErrorMessage(err, fallback)
            return false
        } finally {
            loading.value = false
        }
    }

    return {
        loading, isAuthenticated, user, token, error, loginAsync, registerAsync, logout, clearError,
    }
})