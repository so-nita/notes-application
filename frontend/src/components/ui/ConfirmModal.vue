<script setup lang="ts">
import BaseModal from './BaseModal.vue'

defineProps<{
  open: boolean
  title: string
  message: string
  confirmLabel?: string
  busy?: boolean
}>()

const emit = defineEmits<{ confirm: []; cancel: [] }>()
</script>

<template>
  <BaseModal :open="open" labelled-by="confirm-title" @close="emit('cancel')">
    <div class="p-6">
      <h2 id="confirm-title" class="text-lg font-semibold text-gray-900">{{ title }}</h2>
      <p class="mt-1.5 text-sm text-gray-500">{{ message }}</p>

      <div class="mt-6 flex flex-col-reverse gap-2 sm:flex-row sm:justify-end">
        <button type="button" class="btn-secondary" :disabled="busy" @click="emit('cancel')">
          Cancel
        </button>
        <button
            type="button"
            class="btn-danger"
            :disabled="busy"
            data-autofocus
            @click="emit('confirm')"
        >
          {{ busy ? 'Working…' : (confirmLabel ?? 'Confirm') }}
        </button>
      </div>
    </div>
  </BaseModal>
</template>
