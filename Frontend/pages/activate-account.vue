<template>

</template>

<style lang="scss" scoped></style>

<script setup>
import { useRoute, useRouter } from 'vue-router';

const globalMessageStore = useGlobalMessageStore();
const route = useRoute();
const router = useRouter();

definePageMeta({
    layout: "no-auth",
})

onMounted(() => {
    activateAccount();
    console.log('work')
});

const error = ref("");
const loading = ref(false);

const activateAccount = () => {
    loading.value = true;
    error.value = "";

    useWebApiFetch('/User/ActivateUserWithAccount', {
        method: 'POST',
        body: { 
            'token' : route.query.token,
            'email' : route.query.email
         },
    })
        .then(() => {
            router.push('/');
        })
        .finally(() => {
            loading.value = false;
        });
};



</script>