<script setup lang="ts">
import { computed, ref, watch } from 'vue'
import type { Note } from '@/types'
import BaseModal from '@/components/ui/BaseModal.vue'

const props = defineProps<{
  open: boolean
  note: Note | null
  busy?: boolean
  error?: string | null
}>()

const emit = defineEmits<{
  submit: [payload: { title: string; content: string | null }]
  close: []
}>()

const TITLE_MAX = 200

const title = ref('')
const content = ref('')
const submitted = ref(false)

const isEditing = computed(() => props.note !== null)

const titleError = computed(() => {
  const value = title.value.trim()
  if (!value) return 'Title is required.'
  if (value.length > TITLE_MAX) return `Title must be ${TITLE_MAX} characters or fewer.`
  return ''
})

watch(
    () => props.open,
    (open) => {
      if (!open) return
      title.value = props.note?.title ?? ''
      content.value = props.note?.content ?? ''
      submitted.value = false
    },
    { immediate: true },
)

function submit(): void {
  submitted.value = true
  if (titleError.value) return

  const body = content.value.trim()
  emit('submit', { title: title.value.trim(), content: body ? body : null })
}
</script>

<template>
  <BaseModal :open="open" labelled-by="note-editor-title" size="lg" @close="emit('close')">
    <form novalidate class="p-6">
      <h2 id="note-editor-title" class="text-lg font-semibold text-gray-900">
        {{ isEditing ? 'Edit note' : 'New note' }}
      </h2>

      <div class="mt-5">
        <label class="label" for="note-title">Title <span class="text-red-500">*</span></label>
        <input
            id="note-title"
            v-model="title"
            type="text"
            :maxlength="TITLE_MAX"
            placeholder="Input title"
            class="field"
            data-autofocus
            :aria-invalid="submitted && !!titleError"
        />
        <p v-if="submitted && titleError" class="error-text">{{ titleError }}</p>
      </div>

      <div class="mt-4">
        <label class="label" for="note-content">
          Content <span class="font-normal text-gray-400">(optional)</span>
        </label>
        <textarea
            id="note-content"
            v-model="content"
            rows="8"
            placeholder="Write somthinf"
            class="field resize-y"
        />
      </div>

      <p v-if="error" role="alert" class="mt-4 rounded-lg bg-red-50 px-3 py-2 text-sm text-red-700">
        {{ error }}
      </p>

      <div class="mt-6 flex flex-col-reverse gap-2 sm:flex-row sm:justify-end">
        <button type="button" class="btn-secondary" :disabled="busy" @click="emit('close')">
          Cancel
        </button>
        <button type="submit" class="btn-primary" :disabled="busy" @click.prevent="submit">
          {{ busy ? 'Saving…' : isEditing ? 'Save changes' : 'Create note' }}
        </button>
      </div>
    </form>
  </BaseModal>
</template>
