import { defineStore } from "pinia";

export const useBinStore = defineStore({
    id: 'bin-store',
    state: () => {
        return {
            items:[],
            checked: false,
        }
    },
    actions: {
        pushItems(title, url) {
            const items = ref([
                {
                    title: '',
                    url: '',
                },
            ]);
            items.title = title;
            items.url = url;
            
            this.numberPosts = items.value.length;
            this.items = items.value;
        }
    }
})