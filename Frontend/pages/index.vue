<template>
    <v-container>
        <v-row class="justify-center mt-10">
            <v-col v-for="contentCard in contentCards" :key="id" cols="3">
                <VBadge color="red" :content="postStore.$state.items.length">
                    <VCard :width="150" height="150" @click="$router.push(contentCard.route)">
                        <VCardTitle class="text-center mt-12">{{ contentCard.name }}</VCardTitle>
                    </VCard>
                </VBadge>
            </v-col>
            <v-col v-for="actionCard in actionCards" :key="id" cols="3">
                <VCard :width="150" height="150" @click="$router.push(actionCard.route)">
                    <VCardTitle class="text-center mt-12">{{ actionCard.name }}</VCardTitle>
                </VCard>
            </v-col>
        </v-row>
    </v-container>
</template>

<script setup>
const postStore = usePostsStore();
postStore.fetchItems();

const contentCards = [
    {
        id: 1,
        name: 'Posts',
        route: '/posts',
        number: 9,
    },
    {
        id: 3,
        name: 'Bin',
        route: '/bin',
        number: 9,
    },
]

const actionCards = [
    {
        id: 1,
        name: 'Subscribe',
        route: '/add-feed'
    },
]

</script>