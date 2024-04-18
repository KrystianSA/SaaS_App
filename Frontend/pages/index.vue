<template>
    <div>
        <VCard flat>
            <v-card-title class="d-flex align-center mx-5">
                <VCheckbox @change="changeAll" v-model="checkboxState" class="d-flex align-center"></VCheckbox>
                <v-spacer></v-spacer>
                <v-text-field v-model="search" density="compact" label="Search" :prepend-inner-icon=mdiMagnify flat
                    hide-details single-line></v-text-field>
            </v-card-title>
            <VList v-for="item in filteredItems" :key="item.title" class="mx-5">
                <VCard>
                    <template v-slot:prepend>
                        <VCheckbox :model-value="item.checked"></VCheckbox>
                    </template>
                    <template v-slot:title>{{ item.title }}</template>
                    <template v-slot:subtitle>{{ item.url }}</template>
                    <template v-slot:append>
                        <VBtn :icon=mdiDelete></VBtn>
                    </template>
                </VCard>
            </VList>
        </VCard>
    </div>
</template>

<script setup>
import { mdiDelete, mdiMagnify } from '@mdi/js';

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
        title: 'How to be safe in Network?',
        url: 'https://example.pl',
        checked: false,
    },
    {
        title: 'The best antimalware in 2024 !',
        url: 'https://example1.pl',
        checked: false,
    },
    {
        title: 'Password Manager, is it safe?',
        url: 'https://example2.pl',
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