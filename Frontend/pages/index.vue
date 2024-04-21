<template>
    <v-container>
        <v-row class="justify-center mt-10">
            <v-col v-for="card in cards" :key="id" cols="3">
                <VCard :width="150" height="150" @click="$router.push(card.route)">
                    <VCardTitle class="text-center mt-12">{{ card.name }}</VCardTitle>
                </VCard>
            </v-col>
        </v-row>
    </v-container>
</template>

<script setup>

const cards = [
    {
        id: 1,
        name: 'Posts',
        route : '/posts'
    },
    {
        id: 2,
        name: 'New Feed',
        route : '/add-feed'
    },
    {
        id: 3,
        name: 'Bin',
        route : '/bin'
    },
]

</script>