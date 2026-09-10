<script setup lang="ts">
import { computed, onUnmounted, reactive, ref } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/auth.store'

const auth = useAuthStore()
const router = useRouter()
const route = useRoute()

const form = reactive({ username: '', password: '' })
const submitted = ref(false)

const errors = computed(() => ({
  username: form.username.trim() ? '' : 'Username is required.',
  password: form.password ? '' : 'Password is required.',
}))

const isValid = computed(() => !errors.value.username && !errors.value.password)

async function submit(): Promise<void> {
  submitted.value = true
  if (!isValid.value) return

  const ok = await auth.loginAsync({ username: form.username.trim(), password: form.password })
  if (!ok) return

  const redirect = route.query.redirect
  const target = typeof redirect === 'string' && redirect.startsWith('/') ? redirect : null
  await router.replace(target ?? { name: 'notes' })
}

onUnmounted(() => auth.clearError())
</script>

<template>

  <div class="w-full max-w-sm rounded-xl border border-gray-200 bg-white p-8 shadow-sm">

    <form novalidate @submit.prevent="submit">
      <h1 class="text-2xl font-semibold text-gray-900">Sign in</h1>
      <p class="mt-1 text-sm text-gray-500">Enter your details to continue.</p>

      <div class="mt-5">
        <label class="mb-1.5 block text-sm font-medium text-gray-700" for="login-username">
          Username
        </label>
        <input
            id="login-username"
            v-model="form.username"
            type="text"
            autocomplete="username"
            placeholder="Enter username"
            class="w-full rounded-lg border border-gray-300 px-3 py-2 text-sm text-gray-900 placeholder:text-gray-400 focus:border-blue-600 focus:outline-none focus:ring-2 focus:ring-blue-600/20"
            :aria-invalid="submitted && !!errors.username"
        />
        <p v-if="submitted && errors.username" class="error-text">{{ errors.username }}</p>
      </div>

      <div class="mt-4">
        <label class="mb-1.5 block text-sm font-medium text-gray-700" for="login-password">
          Password
        </label>
        <input
            id="login-password"
            v-model="form.password"
            type="password"
            autocomplete="current-password"
            placeholder="••••••••"
            class="w-full rounded-lg border border-gray-300 px-3 py-2 text-sm text-gray-900 placeholder:text-gray-400 focus:border-blue-600 focus:outline-none focus:ring-2 focus:ring-blue-600/20"
            :aria-invalid="submitted && !!errors.password"
        />
        <p v-if="submitted && errors.password" class="error-text">{{ errors.password }}</p>
      </div>

      <p v-if="auth.error" role="alert" class="mt-4 rounded-lg bg-red-50 px-3 py-2 text-sm text-red-700">
        {{ auth.error }}
      </p>

      <button
          type="submit"
          :disabled="auth.loading"
          class="mt-5 w-full rounded-lg bg-blue-600 px-4 py-2.5 text-sm font-semibold text-white transition hover:bg-blue-700 focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-blue-600 disabled:cursor-not-allowed disabled:opacity-60"
      >
        {{ auth.loading ? 'Signing in…' : 'Sign in' }}
      </button>

      <p class="mt-5 text-center text-sm text-gray-500">
        New here?
        <RouterLink :to="{ name: 'register' }" class="font-medium text-blue-600 hover:underline">
          Create an account
        </RouterLink>
      </p>

    </form>
    <div class="mt-5">
      <span class="text-slate-400 text-sm">Demo</span>
      <div class="flex justify-between">
        <div class="text-gray-400 text-sm">username: <span class="text-gray-500">testing</span></div>
        <div class="text-slate-400 text-sm">password: <span class="text-gray-500">test@2026</span></div>
      </div>
    </div>
  </div>
</template>
