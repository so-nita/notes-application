function parse(value: string | null): Date | null {
    if (!value) return null
    const date = new Date(value)
    return Number.isNaN(date.getTime()) ? null : date
}

const dayFormat = new Intl.DateTimeFormat(undefined, {
    day: 'numeric',
    month: 'short',
    year: 'numeric',
})

const fullFormat = new Intl.DateTimeFormat(undefined, {
    day: 'numeric',
    month: 'short',
    year: 'numeric',
    hour: 'numeric',
    minute: '2-digit',
})

export function formatDate(value: string | null): string {
    const date = parse(value)
    return date ? dayFormat.format(date) : '—'
}

export function formatDateTime(value: string | null): string {
    const date = parse(value)
    return date ? fullFormat.format(date) : '—'
}