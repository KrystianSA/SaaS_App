<template>
    <div class="d-flex align-center justify-center fill-height">
        <VCard width="600">
            <VForm :disabled="loading" @submit.prevent="submit">
                <VCardTitle class="text-center">Send Reset Link</VCardTitle>
                <VCardText>
                    <VTextField :rules="[ruleRequired, ruleEmail]" variant="solo-filled" label="Email"
                        v-model="modelData.email"></VTextField>
                    <VAlert v-if="error" type="error">{{ error }}</VAlert>
                </VCardText>
                <VCardActions>
                    <VBtn :loading="loading" class="mx-auto" variant="elevated" text="Submit" type="submit">
                    </VBtn>
                </VCardActions>
            </VForm>
        </VCard>
    </div>
</template>

<style lang="scss" scoped></style>

<script setup>
const { ruleRequired, ruleEmail } = useFormValidationRules();
const globalMessageStore = useGlobalMessageStore();

definePageMeta({
    layout: "no-auth",
})

const modelData = ref({
    email: ''
});

const submit = async (ev) => {
    const { valid } = await ev;
    if (valid) {
        ResetLink();
    }
}

const error = ref("");
const loading = ref(false);

const ResetLink = () => {
    loading.value = true;
    error.value = "";

    useWebApiFetch('/User/SendResetLink', {
        method: 'POST',
        body: { ...modelData.value },
        onResponseError: ({ response }) => {
            globalMessageStore.showSuccessMessage("If email adress is valid, you will achieve link to reset your password");
        }
    })
        .then(() => {
            globalMessageStore.showSuccessMessage("If email adress is valid, you will achieve link to reset your password");
        })
        .finally(() => {
            modelData.value.email = '';
            loading.value = false;
        });
};

</script>