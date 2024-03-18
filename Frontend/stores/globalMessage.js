import { defineStore } from 'pinia'

export const useGlobalMessageStore = defineStore({
    id: 'global-message-store',
    state: () => {
        return {
            message: null
        }
    },
    actions: {
        showMessage(data) {
            this.message = data;
        },

        showErrorMessage(text) {
            this.showMessage({
                text: text,
                type: 'Error'
            })
        },

        showSuccessMessage(text) {
            this.showMessage({
                text: text,
                type: 'Success'
            })
        },
    }
})