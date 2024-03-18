<template>
    <VDialog :model-value="show" persistent width="400" scroll-strategy="none">
        <VCard class="pa-4">
            <div v-if="userStore.$state.loading === true" class="ma-auto">
                <VProgressCircular indeterminate></VProgressCircular>
            </div>
            <VForm :disabled="loading" @submit.prevent="submit" v-else>
                <VCardTitle class="text-center">Login</VCardTitle>
                <VCardText>
                    <VTextField :rules="[ruleRequired, ruleEmail]" variant="solo-filled" label="Email"
                        v-model="loginData.email"></VTextField>
                    <VTextField :rules="[ruleRequired]" variant="solo-filled" label="Password" type="password"
                        v-model="loginData.password"></VTextField>
                    <VAlert v-if="error" type="error">{{ error }}</VAlert>
                </VCardText>
                <VCardActions>
                    <VBtn :loading="loading" class="mx-auto" variant="elevated" type="submit" text="Submit">
                    </VBtn>
                </VCardActions>
                <VCardText class="text-caption text-center">
                    Haven't account yet ?
                    <NuxtLink class="text-blue" to="/register">Click to register</NuxtLink>
                </VCardText>
            </VForm>
        </VCard>
    </VDialog>
</template>

<style lang="scss" scoped></style>

<script setup>
const { ruleRequired, ruleEmail } = useFormValidationRules();
const { getErrorMessage } = useWebApiResponseParser();
const userStore = useUserStore();
const show = computed(() => {
    return userStore.$state.isLoggedIn === false || userStore.$state.loading === true
});

const loginData = ref({
    email: '',
    password: ''
});

const error = ref("");
const loading = ref(false);

const submit = async (ev) => {
    const { valid } = await ev;
    if (valid) {
        login();
    }
}

const antiForgeryStore = useAntiForgeryStore();

const login = () => {
    loading.value = true;
    error.value = "";

    useWebApiFetch('/User/Login', {
        method: 'POST',
        body: { ...loginData.value },
        onResponseError: ({ response }) => {
            error.value = getErrorMessage(response, {
                "InvalidLoginOrPassword": "Błędny login lub hasło"
            });
        },
    })
        .then((response) => {
            if (response.data.value) {
                loginData.value.password = '';
                userStore.loadLoggedInUser();
            }
        })
        .finally(() => {
            loading.value = false;
        });
};

</script>