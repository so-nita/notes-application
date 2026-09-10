<script setup lang="ts">
import { computed, onUnmounted, reactive, ref } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/auth.store'

const auth = useAuthStore()
const router = useRouter()

const form = reactive({ fullName: '', username: '', password: '', confirmPassword: '' })
const submitted = ref(false)

const errors = computed(() => {
  const username = form.username.trim()
  return {
    username: !username
        ? 'Username is required.'
        : username.length > 50 ? 'Username must be 50 characters or fewer.' : '',
    password: !form.password
        ? 'Password is required.'
        : form.password.length < 6 ? 'Password must be at least 6 characters.' : '',
    confirmPassword: form.confirmPassword !== form.password ? 'Passwords do not match.' : '',
  }
})

const isValid = computed(() => Object.values(errors.value).every((message) => !message))

async function submit(): Promise<void> {
  submitted.value = true
  if (!isValid.value) return

  const fullName = form.fullName.trim()
  const ok = await auth.registerAsync({
    fullName: fullName || undefined,
    username: form.username.trim(),
    password: form.password,
  })
  if (ok) await router.replace({ name: 'notes' })
}

onUnmounted(() => auth.clearError())
</script>

<template>
  <div class="w-full max-w-sm rounded-xl border border-gray-200 bg-white p-8 shadow-sm">
    <form novalidate @submit.prevent="submit">
      <h1 class="text-2xl font-semibold text-gray-900">Create an account</h1>
      <p class="mt-1 text-sm text-gray-500">It only takes a moment.</p>

      <div class="mt-5">
        <label class="label" for="register-fullname">
          Full name <span class="font-normal text-gray-400">(optional)</span>
        </label>
        <input
            id="register-fullname"
            v-model="form.fullName"
            type="text"
            autocomplete="name"
            placeholder="Enter name"
            class="field"
        />
      </div>

      <div class="mt-4">
        <label class="label" for="register-username">Username</label>
        <input
            id="register-username"
            v-model="form.username"
            type="text"
            autocomplete="username"
            maxlength="50"
            placeholder="Enter username"
            class="field"
            :aria-invalid="submitted && !!errors.username"
        />
        <p v-if="submitted && errors.username" class="error-text">{{ errors.username }}</p>
      </div>

      <div class="mt-4">
        <label class="label" for="register-password">Password</label>
        <input
            id="register-password"
            v-model="form.password"
            type="password"
            autocomplete="new-password"
            placeholder="At least 6 characters"
            class="field"
            :aria-invalid="submitted && !!errors.password"
        />
        <p v-if="submitted && errors.password" class="error-text">{{ errors.password }}</p>
      </div>

      <div class="mt-4">
        <label class="label" for="register-confirm">Confirm password</label>
        <input
            id="register-confirm"
            v-model="form.confirmPassword"
            type="password"
            autocomplete="new-password"
            placeholder="••••••••"
            class="field"
            :aria-invalid="submitted && !!errors.confirmPassword"
        />
        <p v-if="submitted && errors.confirmPassword" class="error-text">
          {{ errors.confirmPassword }}
        </p>
      </div>

      <p v-if="auth.error" role="alert" class="mt-4 rounded-lg bg-red-50 px-3 py-2 text-sm text-red-700">
        {{ auth.error }}
      </p>

      <button
          type="submit"
          :disabled="auth.loading"
          class="btn-primary mt-5 w-full"
      >
        {{ auth.loading ? 'Creating account…' : 'Create account' }}
      </button>

      <p class="mt-5 text-center text-sm text-gray-500">
        Already have an account?
        <RouterLink :to="{ name: 'login' }" class="font-medium text-blue-600 hover:underline">
          Sign in
        </RouterLink>
      </p>
    </form>
  </div>
</template>
