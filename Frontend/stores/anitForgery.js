import { defineStore } from 'pinia'

export const useAntiForgeryStore = defineStore({
    id: 'antiforgery-store',
    state: () => {
        return {
            token: null,
        }
    },
    actions: {
        async loadAntiForgeryToken() {
            await useWebApiFetch('/User/AntiforgeryToken')
                .then(({ data, error }) => {
                    if (data.value) {
                        this.token = data.value;
                    } else if (error.value) {
                        this.token = null;
                    } 
                });
        },
    }
})