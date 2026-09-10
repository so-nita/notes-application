<script setup lang="ts">

import {Note} from "@/types";
import {computed} from "vue";
import {formatDate} from "@/lib/formate";

const props = defineProps<{
  note: Note;
  deleting?: boolean;
}>();

const emit = defineEmits<{
  open: [];
  edit: [];
  remove: [];
}>();

const preview = computed(() => (props.note.content ?? '').replace(/\n{2,}/g, '\n').trim())

</script>

<template>
  <article
      class="relative flex flex-col rounded-xl border border-gray-200 bg-white p-5 shadow-sm transition hover:border-gray-300"
      :class="deleting && 'pointer-events-none opacity-50'"
  >
    <!-- The whole card opens the note; the buttons below sit above this overlay. -->
    <button
        type="button"
        class="absolute inset-0 z-0 rounded-xl"
        :aria-label="`Open note: ${note.title}`"
        @click="emit('open')"
    />

    <h3 class="pointer-events-none relative z-10 line-clamp-2 text-base font-semibold text-gray-900">
      {{ note.title }}
    </h3>

    <p
        v-if="preview"
        class="pointer-events-none relative z-10 mt-2 line-clamp-3 flex-1 whitespace-pre-line text-sm text-gray-600"
    >
      {{ preview }}
    </p>
    <p v-else class="pointer-events-none relative z-10 mt-2 flex-1 text-sm italic text-gray-400">
      No content
    </p>

    <div class="relative z-10 mt-4 flex items-center justify-between gap-3 border-t border-gray-100 pt-3">
      <p class="pointer-events-none truncate text-xs text-gray-500">
        {{ note.updatedAt ? `Updated ${formatDate(note.updatedAt)}` : formatDate(note.createdAt) }}
      </p>

      <div class="flex shrink-0 gap-1">
        <button
            type="button"
            class="rounded px-2 py-1 text-xs font-medium text-gray-600 transition hover:bg-gray-100 hover:text-gray-900"
            :aria-label="`Edit note: ${note.title}`"
            @click.stop="emit('edit')"
        >
          Edit
        </button>
        <button
            type="button"
            class="rounded px-2 py-1 text-xs font-medium text-gray-600 transition hover:bg-red-50 hover:text-red-700"
            :aria-label="`Delete note: ${note.title}`"
            @click.stop="emit('remove')"
        >
          Delete
        </button>
      </div>
    </div>
  </article>
</template>
