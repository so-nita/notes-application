<script setup lang="ts">
import { computed, ref } from 'vue'
import { useAuthStore } from '@/stores/auth.store'

const auth = useAuthStore()
const emit = defineEmits<{ logout: [] }>()

const menuOpen = ref(false)

const initial = computed(() => (auth.user?.username ?? '?').charAt(0).toUpperCase())
</script>

<template>
  <header class="border-b border-gray-200 bg-white">
    <div class="mx-auto flex max-w-5xl items-center justify-between gap-4 px-4 py-3 sm:px-6">
      <span class="text-base font-semibold text-gray-900">Notes</span>

      <div class="relative">
        <button
            type="button"
            class="flex items-center gap-2 rounded-lg border border-gray-300 py-1 pl-1 pr-2.5 text-sm transition hover:bg-gray-50"
            :aria-expanded="menuOpen"
            aria-haspopup="menu"
            @click="menuOpen = !menuOpen"
        >
          <span
              class="flex h-6 w-6 items-center justify-center rounded-full bg-blue-600 text-xs font-semibold text-white"
          >
            {{ initial }}
          </span>
          <span class="max-w-[9rem] truncate font-medium text-gray-700">
            {{ auth.user?.username }}
          </span>
        </button>

        <template v-if="menuOpen">
          <!-- Click-away layer, so the menu closes on any outside click. -->
          <div class="fixed inset-0 z-10" @click="menuOpen = false" />
          <div
              role="menu"
              class="absolute right-0 z-20 mt-2 w-48 overflow-hidden rounded-lg border border-gray-200 bg-white py-1 shadow-lg"
          >
            <p class="truncate border-b border-gray-100 px-3 py-2 text-xs text-gray-500">
              Signed in as
              <span class="font-medium text-gray-700">{{ auth.user?.username }}</span>
            </p>
            <button
                type="button"
                role="menuitem"
                class="w-full px-3 py-2 text-left text-sm text-red-400 transition  hover:bg-red-50 "
                @click="menuOpen = false; emit('logout')"
            >
              Logout
            </button>
          </div>
        </template>
      </div>
    </div>
  </header>
</template>
