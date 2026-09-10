<script setup lang="ts">
import type { Note } from '@/types'
import BaseModal from '@/components/ui/BaseModal.vue'
import { formatDateTime } from '@/lib/formate';

defineProps<{ open: boolean; note: Note | null }>()

const emit = defineEmits<{ edit: []; remove: []; close: [] }>()
</script>

<template>
  <BaseModal :open="open && !!note" labelled-by="note-detail-title" size="lg" @close="emit('close')">
    <div v-if="note" class="p-6">
      <div class="flex items-start justify-between gap-4">
        <h2 id="note-detail-title" class="text-lg font-semibold text-gray-900">
          {{ note.title }}
        </h2>
        <button
            type="button"
            class="shrink-0 rounded px-2 text-xl leading-none text-gray-400 transition hover:text-gray-700"
            aria-label="Close"
            data-autofocus
            @click="emit('close')"
        >
          &times;
        </button>
      </div>

      <div class="mt-4 max-h-[50vh] overflow-y-auto">
        <p
            v-if="(note.content ?? '').trim()"
            class="whitespace-pre-wrap break-words text-sm text-gray-700"
        >{{ note.content }}</p>
        <p v-else class="text-sm italic text-gray-400">This note has no content.</p>
      </div>

      <dl class="mt-5 ">
        <div>
          <dt class="font-medium text-gray-500">Created</dt>
          <dd class="mt-0.5 text-gray-700">{{ formatDateTime(note.createdAt) }}</dd>
        </div>
        <div>
          <dt class="font-medium text-gray-500">Last updated</dt>
          <dd class="mt-0.5 text-gray-700">
            <template v-if="note.updatedAt">{{ formatDateTime(note.updatedAt) }}</template>
            <span v-else class="text-gray-400">Not edited yet</span>
          </dd>
        </div>
      </dl>

      <div class="mt-6 flex flex-col-reverse gap-2 sm:flex-row sm:justify-end">
        <button
            type="button"
            class="btn-secondary text-red-700 hover:bg-red-50"
            @click="emit('remove')"
        >
          Delete
        </button>
        <button type="button" class="btn-primary" @click="emit('edit')">Edit note</button>
      </div>
    </div>

  </BaseModal>

</template>
