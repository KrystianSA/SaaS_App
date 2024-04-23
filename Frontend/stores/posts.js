import { defineStore } from "pinia";

export const usePostsStore = defineStore({
    id: 'posts-store',
    state: () => {
        return {
            numberPosts: 0,
            items:[],
            checked: false,
        }
    },
    actions: {
        fetchItems() {
            const items = ref([
                {
                    title: 'How to be safe in Network?',
                    url: 'https://example.pl',
                    color: 'red',
                },
                {
                    title: 'The best antimalware in 2024 !',
                    url: 'https://example1.pl',
                    color: 'red',
                }
            ]);
            this.numberPosts = items.value.length;
            this.items = items.value;
        }
    }
})