<template>
    <div>
        <VCard flat>
            <v-card-title class="d-flex align-center mx-5">
                <VMenu location="end">
                    <template v-slot:activator="{ props }">
                        <VCheckbox v-bind="props" @change="changeAll" v-model="checkboxState"
                            class="d-flex align-center"></VCheckbox>
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
            <VList v-for="(item, index) in filteredItems" :key="index" class="mx-5">
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

const checkboxState = ref(false);

onMounted(() => {
    readFeed();
});

const readPost = (index) => {
    if (items.value.length == index) {
        items.value.forEach(item => {
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

const deleteItems = (itemIndex) => {
    if (items.value.length == itemIndex) {
        items.value.splice(0, itemIndex)
    }
    else {
        items.value.splice(itemIndex, 1)
    }
    checkboxState.value = false;
}

const changeAll = () => {
    if (checkboxState.value) {
        items.value.forEach(item => {
            item.checked = true
        })
    }
    else {
        items.value.forEach(item => {
            item.checked = false
        })
    }
}

const filteredItems = computed(() => {
    return items.value.filter(item => {
        return item.title.toLowerCase().includes(search.value.toLowerCase());
    });
});

const search = ref('');
const items = ref([
    {
        title: 'How to be safe in Network?',
        url: 'https://example.pl',
        checked: false,
        color: 'red',
    },
    {
        title: 'The best antimalware in 2024 !',
        url: 'https://example1.pl',
        checked: false,
        color: 'red',
    },
    {
        title: 'Password Manager, is it safe?',
        url: 'https://example.pl',
        checked: false,
        color: 'red',
    },
]);

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