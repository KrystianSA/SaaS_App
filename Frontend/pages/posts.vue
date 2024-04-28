<template>
    <div>
        <VCard flat>
            <v-card-title class="d-flex align-center mx-5">
                <VMenu location="end">
                    <template v-slot:activator="{ props }">
                        <VCheckbox v-bind="props" @change="changeAll" v-model="checkboxState"
                            class="d-flex align-center">
                        </VCheckbox>
                    </template>
                    <VCard>
                        <VList>
                            <VListItem @click="deleteItems(items.length)" title="Delete All"></VListItem>
                            <VListItem @click="readPost(items.length)" title="All Post Read"></VListItem>
                        </VList>
                    </VCard>
                </VMenu>
                <v-spacer></v-spacer>
                <v-text-field v-model="search" density="compact" label="Search" :prepend-inner-icon=mdiMagnify flat
                    hide-details single-line></v-text-field>
            </v-card-title>
            <VList v-for="(item,index) in filteredItems" :key="index" class="mx-5">
                <VCard :color="item.color">
                    <template v-slot:prepend>
                        <VCheckbox :model-value="item.checked"></VCheckbox>
                    </template>
                    <template v-slot:title>{{ item.title }}</template>
                    <template v-slot:subtitle>
                        <a :href="item.url" target="_blank" @click="readPost(index)">{{ item.url }}</a>
                        <VIcon :icon=mdiOpenInNew class="ml-1"></VIcon>
                    </template>
                    <template v-slot:append>
                        <VBtn :icon=mdiDelete @click="deleteItems(index)"></VBtn>
                    </template>
                </VCard>
            </VList>
        </VCard>
    </div>
</template>

<style>
a {
    text-decoration: none;
    color: black
}
</style>

<script setup>
import { mdiDelete, mdiMagnify, mdiOpenInNew } from '@mdi/js';

const postStore = usePostsStore();
postStore.fetchItems();
const items = postStore.$state.items;

const filteredItems = computed(() => {
    return items.filter(item => {
        return item.title.toLowerCase().includes(search.value.toLowerCase());
    });
});

const search = ref('');
const checkboxState = ref(false);

onMounted(() => {
    readFeed();
});

const readPost = (index) => {
    if (items.length == index) {
        console.log(items.length);
        items.forEach(item => {
            item.color = 'white';
            item.checked = false;
        })
    }
    else {
        var element = items.value[index];
        element.color = 'white';
    }

    checkboxState.value = false;
}

const binStore = useBinStore();

const deleteItems = (itemIndex) => {
    console.log(itemIndex);
    if (items.length == itemIndex) {
        items.splice(0, itemIndex)
    }
    else {
        let element = items[itemIndex];
        binStore.pushItems(element.title, element.url);
        items.splice(itemIndex, 1)
     }
    checkboxState.value = false;
}

const changeAll = () => {
    if (checkboxState.value) {
        items.forEach(item => {
            item.checked = true
        })
    }
    else {
        items.forEach(item => {
            item.checked = false
        })
    }
}

const modelData = ref([{
    url: '',
}]);

const readFeed = () => {

    useWebApiFetch('/RssFeed/FeedReader', {
        method: 'POST',
        body: { Url: "https://niebezpiecznik.pl" },
    })
        .then((response) => {
            modelData.value.url = response.data.value;
        })
};

</script>