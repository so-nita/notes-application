import { useAuthStore } from "@/stores/auth.store";
import {createRouter, createWebHistory} from "vue-router";


const router = createRouter({
    history: createWebHistory(),
    routes: [
        {
            path: '/',
            name: "note",
            meta: {
                requireAuth: true,
            },
            component: () => import("@/views/NoteView.vue")
        },
        {
            path: '/',
            name: "login",
            meta: { guestOnly: true },
            component: () => import("@/views/LoginView.vue")
        },
        {
            path: '/',
            name: "register",
            meta: { guestOnly: true },
            component: () => import("@/views/RegisterView.vue")
        }
    ]
});

router.beforeEach((to) => {
    const auth = useAuthStore()
    if (to.meta.requiresAuth && !auth.isAuthenticated) {
        return { name: 'login', query: { redirect: to.fullPath } }
    }

    if (to.meta.guestOnly && auth.isAuthenticated) {
        return { name: 'notes' }
    }

    return true
})

router.afterEach((to) => {
    const title = to.meta.title as string | undefined
    document.title = title ? `${title} · Notes` : 'Notes'
})

export default router
