<template>

</template>

<style lang="scss" scoped></style>

<script setup>
import { useRoute, useRouter } from 'vue-router';

const route = useRoute();
const router = useRouter();

definePageMeta({
    layout: "no-auth",
})

onMounted(() => {
    activateAccount();
});

const error = ref("");
const loading = ref(false);

const tokenValidation = () => {
    var token = route.query.token;
    var newToken = token.replace(' ', '+');
    return newToken
}

const activateAccount = () => {
    loading.value = true;
    error.value = "";

    useWebApiFetch('/Account/ActivateAccount', {
        method: 'POST',
        body: { 
            'token' : tokenValidation()
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