<template>
    <div>
        <VCard flat v-if="items.length > 1">
            <v-card-title class="d-flex align-center mx-5">
                <VMenu location="end">
                    <template v-slot:activator="{ props }">
                        <VCheckbox v-bind="props" v-model="checkboxState" class="d-flex align-center">
                        </VCheckbox>
                    </template>
                    <VCard>
                        <VList>
                            <VListItem @click="deleteItems(items.length)" title="Delete All"></VListItem>
                        </VList>
                    </VCard>
                </VMenu>
                <v-spacer></v-spacer>
                <v-text-field v-model="search" density="compact" label="Search" :prepend-inner-icon=mdiMagnify flat
                    hide-details single-line></v-text-field>
            </v-card-title>
            <VList v-for="(item, index) in filteredItems" :key="index" class="mx-5">
                <VCard>
                    <template v-slot:title>{{ item.title }}</template>
                    <template v-slot:subtitle>
                        <a :href="item.url" target="_blank">{{ item.url }}</a>
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

const binStore = useBinStore();
binStore.pushItems();
const items = binStore.$state.items;

const filteredItems = computed(() => {
    return items.filter(item => {
        return item.title.toLowerCase().includes(search.value.toLowerCase());
    });
});

const search = ref('');
const checkboxState = ref(false);

const deleteItems = (itemIndex) => {
    if (items.length == itemIndex) {
        items.splice(0, itemIndex)
    }
    else {
        items.splice(itemIndex, 1)
    }
    checkboxState.value = false;
}
</script>