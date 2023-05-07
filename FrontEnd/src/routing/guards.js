import {useAuthStore} from "@/stores/auth-store";
import {RouteNames} from "@/routing/router";

export function authenticatedGuard() {
    const authStore = useAuthStore();

    if (!authStore.authenticated) {
        return { name: RouteNames.Accounts_Login }
    }
}