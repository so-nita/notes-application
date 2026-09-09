import {defineStore} from "pinia";
import {computed, ref} from "vue";
import {NoteApi} from "@/api/note.api";
import {toErrorMessage} from "@/lib/axios-client";
import type {Note, NoteCreateRequest, NoteFilter, NoteUpdateRequest, SortDirection, SortKey} from "@/types";

function timeOf(note: Note, key: SortKey): number {
    const raw = key === 'updatedAt' ? (note.updatedAt ?? note.createdAt) : note.createdAt
    const parsed = Date.parse(raw)
    return Number.isFinite(parsed) ? parsed : 0
}

function matchesFilter(note: Note, filter: NoteFilter): boolean {
    switch (filter) {
        case 'withContent':
            return (note.content ?? '').trim().length > 0
        case 'empty':
            return (note.content ?? '').trim().length === 0
        case 'edited':
            return note.updatedAt !== null
        default:
            return true
    }
}

export const useNoteStore = defineStore('note', () => {
    const error = ref<string | null>(null)
    const loading = ref(false)
    const notes = ref<Note[]>();
    const deletingId = ref<string | null>(null)
    const saving = ref(false)

    const search = ref("");
    const filter = ref<NoteFilter>("all");
    const sortValue = ref<SortKey>("createdAt");
    const sortDirection = ref<SortDirection>('desc')

    async function fetchNotesAsync(): Promise<boolean> {
        loading.value = true
        error.value = null;
        try {
            notes.value = await NoteApi.getAllAsync()
            return true
        } catch (err) {
            error.value = toErrorMessage(err, 'Could not load your notes.')
            return false
        } finally {
            loading.value = false
        }
    }

    async function createNoteAsync(payload: NoteCreateRequest): Promise<boolean> {
        return mutate(() => NoteApi.createAsync(payload), 'Could not create the note.')
    }

    async function updateNoteAsync(id: string, payload: NoteUpdateRequest): Promise<boolean> {
        return mutate(() => NoteApi.updateAsync(id, payload), 'Could not save the note.')
    }

    async function deleteNoteAsync(id: string): Promise<boolean> {
        deletingId.value = id
        try {
            return await mutate(() => NoteApi.deleteAsync(id), 'Could not delete the note.')
        } finally {
            deletingId.value = null
        }
    }

    async function mutate(action: () => Promise<void>, fallback: string): Promise<boolean> {
        saving.value = true
        error.value = null
        try {
            await action()
            await fetchNotesAsync()
            return true
        } catch (err) {
            error.value = toErrorMessage(err, fallback)
            return false
        } finally {
            saving.value = false
        }
    }

    const visibleNotes = computed<Note[] | undefined>(() => {
        const term = search.value.trim().toLowerCase()

        const result = notes?.value?.filter((note) => {
            if (!matchesFilter(note, filter.value)) return false
            if (!term) return true
            return (
                note.title.toLowerCase().includes(term) ||
                (note.content ?? '').toLowerCase().includes(term)
            )
        })

        const direction = sortDirection.value === 'asc' ? 1 : -1
        return result?.sort((a, b) => {
            const order =
                sortValue.value === 'title'
                    ? a.title.localeCompare(b.title, undefined, { sensitivity: 'base' })
                    : timeOf(a, sortValue.value) - timeOf(b, sortValue.value)
            return order * direction
        })
    })
    const isFiltered = computed(() => search.value.trim().length > 0 || filter.value !== 'all')
    function resetFilters(): void {
        search.value = ''
        filter.value = 'all'
    }

    function reset(): void {
        notes.value = []
        loading.value = false
        error.value = null
        resetFilters()
        sortValue.value = 'createdAt'
        sortDirection.value = 'desc'
    }
    function clearError () {
        error.value = null
    }

    return {
        error, visibleNotes, isFiltered, loading,
        notes,
        deletingId,
        saving,
        search,
        filter,
        sortValue,
        sortDirection,
        fetchNotesAsync,
        createNoteAsync,
        updateNoteAsync,
        deleteNoteAsync,
        resetFilters,
        reset,
        clearError,
    }
})