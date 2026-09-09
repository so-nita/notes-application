import type {ApiResponse, Note, NoteCreateRequest, NoteUpdateRequest} from "@/types";
import {axiosClient, unwrap} from "@/lib/axios-client";

export const NoteApi = {
    async getAllAsync(): Promise<Note[]> {
        const { data } = await axiosClient.get<ApiResponse<Note[]>>('/api/Note')
        return unwrap(data, data.message ?? 'Could not load your notes.');
    },
    async createAsync(payload: NoteCreateRequest): Promise<void> {
        const { data } = await axiosClient.post<ApiResponse<string>>('/api/Note', payload)
        unwrap(data, data.message ?? 'Could not create the note.')
    },

    async updateAsync(id: string, payload: NoteUpdateRequest): Promise<void> {
        const { data } = await axiosClient.put<ApiResponse<string>>(`/api/Note/${encodeURIComponent(id)}`, payload,)
        unwrap(data, data.message ?? 'Could not save the note.')
    },

    async deleteAsync(id: string): Promise<void> {
        const { data } = await axiosClient.delete<ApiResponse<string>>(
            `/api/Note/${encodeURIComponent(id)}`,
        )
        unwrap(data, data.message ?? 'Could not delete the note.')
    },
}