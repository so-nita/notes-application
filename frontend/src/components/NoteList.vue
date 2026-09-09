<script setup lang="ts">

import AppHeader from "@/components/AppHeader.vue";
import NoteCard from "@/components/NoteCard.vue";
import {useAuthStore} from "@/stores/auth.store";
import {useNoteStore} from "@/stores/note.store";
import {useRouter} from "vue-router";
import {computed, onMounted, ref} from "vue";
import {storeToRefs} from "pinia";
// import Toast from "@/components/ui/Toast.vue";
import Alert from "@/components/ui/Alert.vue";
import ConfirmModal from "@/components/ui/ConfirmModal.vue";
import type {Note} from "@/types";

const router = useRouter();
const authStore = useAuthStore();
const noteStore = useNoteStore();
const { search, filter, sortValue, sortDirection, visibleNotes, isFiltered } = storeToRefs(noteStore)
const editorOpen = ref(false)
const editorTarget = ref<Note | null>(null)
const editorError = ref<string | null>(null)
const detailTarget = ref<Note | null>(null)
const deleteTarget = ref<Note | null>(null)

onMounted(() => {
  noteStore.fetchNotesAsync();
})

const logout = () => {
  authStore.logout();
  noteStore.reset();
  router.replace({name: "login"});
}

const openCreate = () => {}
const confirmDelete = () => {}
const openEdit = (note: Note) => {
  console.log(note);
}
const saveNote = () => {}

const detailNote = computed(() => detailTarget.value
        ? (noteStore.notes?.find((note) => note.id === detailTarget.value?.id) ?? null)
        : null
)

</script>

<template>
  <div class="min-h-screen">
    <AppHeader @logout="logout" />

    <main class="mx-auto lg:max-w-7xl  px-4 py-8 sm:px-6">
      <div class="flex flex-wrap items-center justify-between gap-3">
        <div>
          <h1 class="text-xl font-semibold text-gray-900">Your notes</h1>
          <p class="mt-0.5 text-sm text-gray-500">Create, search and organise your notes.</p>
        </div>

        <button type="button" class="btn-primary" @click="openCreate">New note</button>
      </div>

      <div class="mt-5">
        <NoteToolbar
            v-model:search="search"
            v-model:filter="filter"
            v-model:sort-key="sortValue"
            v-model:sort-direction="sortDirection"
            :result-count="visibleNotes?.length"
            :total-count="noteStore.notes?.length"
            :is-filtered="isFiltered"
            @reset="noteStore.resetFilters"
        />
      </div>

      <Alert
          v-if="noteStore.error && !editorOpen"
          class="mt-4"
          :message="noteStore.error"
          dismissible
          @dismiss="noteStore.clearError"
      />

      <div v-if="noteStore.loading" class="mt-5 grid gap-4 sm:grid-cols-2 lg:grid-cols-3">
        <div
            v-for="n in 6"
            :key="n"
            class="h-36 animate-pulse rounded-xl border border-gray-200 bg-white"
        />
      </div>

      <div
          v-else-if="visibleNotes?.length"
          class="mt-5 grid gap-4 sm:grid-cols-2 lg:grid-cols-4"
          :aria-busy="noteStore.saving"
      >
        <NoteCard
            v-for="note in visibleNotes"
            :key="note.id"
            :note="note"
            :deleting="noteStore.deletingId === note.id"
            @open="detailTarget = note"
            @edit="openEdit(note)"
            @remove="deleteTarget = note"
        />
      </div>

      <div v-else class="mt-5 rounded-xl border border-gray-200 bg-white px-6 py-14 text-center">
        <template v-if="isFiltered">
          <h2 class="text-base font-semibold text-gray-900">No matches</h2>
          <p class="mx-auto mt-1 max-w-sm text-sm text-gray-500">
            No note matches your search or filter.
          </p>
          <button type="button" class="btn-secondary mt-5" @click="noteStore.resetFilters">
            Clear filters
          </button>
        </template>
        <template v-else>
          <h2 class="text-base font-semibold text-gray-900">No notes yet</h2>
          <p class="mx-auto mt-1 max-w-sm text-sm text-gray-500">
            Your first note is one click away.
          </p>
          <button type="button" class="btn-primary mt-5" @click="openCreate">
            Create your first note
          </button>
        </template>
      </div>
    </main>

    <NoteEditor
        :open="editorOpen"
        :note="editorTarget"
        :busy="noteStore.saving"
        :error="editorError"
        @submit="saveNote"
        @close="editorOpen = false"
    />

    <NoteDetail
        :open="!!detailNote"
        :note="detailNote"
        @edit="detailNote && openEdit(detailNote)"
        @remove="deleteTarget = detailNote"
        @close="detailTarget = null"
    />

    <ConfirmModal
        :open="!!deleteTarget"
        title="Delete this note?"
        :message="`“${deleteTarget?.title ?? ''}” will be permanently removed.`"
        confirm-label="Delete note"
        :busy="noteStore.saving"
        @confirm="confirmDelete"
        @cancel="deleteTarget = null"
    />
  </div>
</template>

