<script setup lang="ts">
import type { NoteFilter, SortDirection, SortKey } from '@/types'

defineProps<{ resultCount: number; totalCount: number; isFiltered: boolean }>()

const search = defineModel<string>('search', { required: true })
const filter = defineModel<NoteFilter>('filter', { required: true })
const sortKey = defineModel<SortKey>('sortKey', { required: true })
const sortDirection = defineModel<SortDirection>('sortDirection', { required: true })

const emit = defineEmits<{ reset: [] }>()

const FILTERS: { value: NoteFilter; label: string }[] = [
  { value: 'all', label: 'All' },
  { value: 'withContent', label: 'With content' },
  { value: 'empty', label: 'Title only' },
  { value: 'edited', label: 'Edited' },
]
</script>

<template>
  <div class="rounded-xl border border-gray-200 bg-white p-4">
    <div class="flex flex-col gap-3 sm:flex-row">
      <input
          v-model="search"
          type="search"
          class="field flex-1"
          placeholder="Search title and content…"
          aria-label="Search notes"
      />

      <div class="flex gap-2">
        <label class="sr-only" for="sort-key">Sort by</label>
        <select id="sort-key" v-model="sortKey" class="field w-auto">
          <option value="createdAt">Created</option>
          <option value="updatedAt">Last edited</option>
          <option value="title">Title</option>
        </select>

        <label class="sr-only" for="sort-direction">Sort direction</label>
        <select id="sort-direction" v-model="sortDirection" class="field w-auto">
          <option value="desc">Newest first</option>
          <option value="asc">Oldest first</option>
        </select>
      </div>
    </div>

    <div class="mt-3 flex flex-wrap items-center gap-2">
      <div class="flex flex-wrap gap-1.5" role="group" aria-label="Filter notes">
        <button
            v-for="option in FILTERS"
            :key="option.value"
            type="button"
            class="rounded-lg border px-3 py-1.5 text-xs font-medium transition"
            :class="
            filter === option.value
              ? 'border-blue-600 bg-blue-600 text-white'
              : 'border-gray-300 bg-white text-gray-600 hover:bg-gray-50'
          "
            :aria-pressed="filter === option.value"
            @click="filter = option.value"
        >
          {{ option.label }}
        </button>
      </div>

      <p class="ml-auto text-xs text-gray-500" aria-live="polite">
        <template v-if="isFiltered">{{ resultCount }} of {{ totalCount }}</template>
        <template v-else>{{ totalCount }} {{ totalCount === 1 ? 'note' : 'notes' }}</template>
        <button
            v-if="isFiltered"
            type="button"
            class="ml-2 font-medium text-blue-600 hover:underline"
            @click="emit('reset')"
        >
          Clear
        </button>
      </p>
    </div>
  </div>
</template>
