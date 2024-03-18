import { defineStore } from 'pinia'
import { useAccountStore } from './account';

export const useUserStore = defineStore({
    id: 'user-store',
    state: () => {
        return {
            isLoggedIn: false,
            loading: false,
            userData: null,
        }
    },
    actions: {
        loadLoggedInUser() {
            const accountStore = useAccountStore();
            this.loading = true;
            useWebApiFetch('/User/GetLoggedInUser')
                .then(({ data, error }) => {
                    if (data.value) {
                        this.isLoggedIn = true;
                        this.userData = data.value;
                        accountStore.loadCurrentAccount();
                    } else if (error.value) {
                        this.isLoggedIn = false;
                        this.userData = null;
                    }
                })
                .finally(() => {
                    this.loading = false;
                });
        },

        logout() {
            useWebApiFetch('/User/Logout', {
                method: 'POST',
            })
                .then((response) => {
                    if (response.data.value) {
                        this.isLoggedIn = false;
                        this.userData = null;
                    }
                });
        },
    }
})