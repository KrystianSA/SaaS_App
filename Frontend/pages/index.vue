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
                            <VListItem title="All Post Read"></VListItem>
                        </VList>
                    </VCard>
                </VMenu>
                <v-spacer></v-spacer>
                <v-text-field v-model="search" density="compact" label="Search" :prepend-inner-icon=mdiMagnify flat
                    hide-details single-line></v-text-field>
            </v-card-title>
            <VList v-for="(item, index) in filteredItems" :key="index" class="mx-5">
                <VCard>
                    <template v-slot:prepend>
                        <VCheckbox @click="changeColor(index)" :model-value="item.checked"></VCheckbox>
                    </template>
                    <template v-slot:title>{{ item.title }}</template>
                    <template v-slot:subtitle>
                        <a :href="item.url" target="_blank">{{ item.url }}</a></template>
                    <template v-slot:append>
                        <VBtn :icon=mdiDelete @click="deleteItems(index)"></VBtn>
                    </template>
                </VCard>
            </VList>
        </VCard>
    </div>
</template>

<script setup>
import { mdiDelete, mdiMagnify } from '@mdi/js';

const deleteItems = (itemIndex) => {
    if (items.value.length == itemIndex) {
        items.value.splice(0, itemIndex)
    }
    else {
        items.value.splice(itemIndex, 1)
    }
}

const checkboxState = ref(false);
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

onMounted(() => {
    readFeed();
});

const filteredItems = computed(() => {
    return items.value.filter(item => {
        return item.title.toLowerCase().includes(search.value.toLowerCase());
    });
});

const search = ref('');
const items = ref([
    {
        index: 1,
        title: 'How to be safe in Network?',
        url: 'https://example.pl',
        checked: false,
    },
    {
        index: 2,
        title: 'The best antimalware in 2024 !',
        url: 'https://example1.pl',
        checked: false,
    },
    {
        index: 3,
        title: 'Password Manager, is it safe?',
        url: 'https://example.pl',
        checked: false,
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