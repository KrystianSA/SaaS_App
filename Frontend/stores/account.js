import { defineStore } from 'pinia'

export const useAccountStore = defineStore({
    id: 'account-store',
    state: () => {
        return {
            loading: false,
            accountData: null,
        }
    },
    actions: {
        loadCurrentAccount() {
            this.loading = true;
            useWebApiFetch('/Account/GetCurrentAccount')
                .then(({ data, error }) => {
                    if (data.value) {
                        this.accountData = data.value;
                    } else if (error.value) {
                        this.accountData = null;
                    }
                })
                .finally(() => {
                    this.loading = false;
                });
        },
    }
})