import { createApp } from 'vue'
import { createPinia } from 'pinia'
import App from './App.vue'
import router from './router'
import './assets/main.css'
import { useAuthStore } from '@/stores/auth.store'
import { useNoteStore } from '@/stores/note.store'
import { setUnauthorizedHandler } from '@/lib/axios-client'

const app = createApp(App)
const pinia = createPinia()

app.use(pinia)
app.use(router)

setUnauthorizedHandler(() => {
    useAuthStore(pinia).logout()
    useNoteStore(pinia).reset()
    void router.replace({ name: 'login' })
})

app.mount('#app')
