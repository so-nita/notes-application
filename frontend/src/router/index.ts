import { useAuthStore } from "@/stores/auth.store";
import {createRouter, createWebHistory} from "vue-router";


const router = createRouter({
    history: createWebHistory(),
    routes: [
        {
            path: '/',
            name: "notes",
            meta: {
                requiresAuth: true,
                title: 'Your notes',
            },
            component: () => import("@/views/NoteView.vue")
        },
        {
            path: '/login',
            name: "login",
            meta: { guestOnly: true, title: 'Sign in' },
            component: () => import("@/views/LoginView.vue")
        },
        {
            path: '/register',
            name: "register",
            meta: { guestOnly: true, title: 'Create an account' },
            component: () => import("@/views/RegisterView.vue")
        },
        {
            path: '/:pathMatch(.*)*',
            redirect: { name: 'notes' },
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
