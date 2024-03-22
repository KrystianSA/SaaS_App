<template>
    <div class="d-flex align-center justify-center fill-height">
        <VCard width="600">
            <VForm :disabled="loading" @submit.prevent="submit">
                <VCardTitle class="text-center">Remind Password</VCardTitle>
                <VCardText>
                    <VTextField :rules="[ruleRequired, ruleEmail]" variant="solo-filled" label="Email"
                        v-model="modelData.email"></VTextField>
                    <VTextField :rules="[ruleRequired]" variant="solo-filled" label="Password" type="password"
                        v-model="modelData.newPassword"></VTextField>
                    <VTextField :rules="[ruleRequired,rules.samePasswords]" variant="solo-filled" label="Confirm Password" type="password">
                    </VTextField>
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
const { getErrorMessage } = useWebApiResponseParser();
const router = useRouter();
const globalMessageStore = useGlobalMessageStore();

const rules = {
    samePasswords: (v) => v === modelData.value.newPassword || "The passwords are not the same"
};


definePageMeta({
    layout: "no-auth",
})

const modelData = ref({
    email: '',
    newPassword: ''
});

const submit = async (ev) => {
    const { valid } = await ev;
    if (valid) {
        changePassword();
    }
}

const error = ref("");
const loading = ref(false);

const changePassword = () => {
    loading.value = true;
    error.value = "";

    useWebApiFetch('/User/ChangePassword', {
        method: 'POST',
        body: { ...modelData.value },
        onResponseError: ({ response }) => {
            error.value = getErrorMessage(response, {
                "InvalidAdressEmail": "Invalid adress email",
                "SomethingWentWrong": "Something went wrong"
            });
        }
    })
        .then((response) => {
            if (response.data.value) {
                globalMessageStore.showSuccessMessage("Password Changed !");
                router.push({ path: '/' });
            }
        })
        .finally(() => {
            loading.value = false;
        });
};

</script>