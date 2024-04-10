<template>
    <div class="d-flex align-center justify-center fill-height">
        <VCard width="600">
            <VForm :disabled="loading" @submit.prevent="submit">
                <VCardTitle class="text-center">Register</VCardTitle>
                <VCardText>
                    <VTextField :rules="[ruleRequired, rules.length]" variant="solo-filled" label="Name"
                        v-model="registerData.name"></VTextField>
                    <VTextField :rules="[ruleRequired, rules.length]" variant="solo-filled" label="Surname"
                        v-model="registerData.surname"></VTextField>
                    <VTextField :rules="[ruleRequired, ruleEmail]" variant="solo-filled" label="Email"
                        v-model="registerData.email"></VTextField>
                    <VTextField :rules="[ruleRequired, rulePassword]" variant="solo-filled" label="Password" type="password"
                        v-model="registerData.password"></VTextField>
                    <VTextField :rules="[ruleRequired, rules.samePasswords]" variant="solo-filled"
                        label="Confirm Password" type="password">
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
const { ruleRequired, ruleEmail, rulePassword } = useFormValidationRules();
const { getErrorMessage } = useWebApiResponseParser();
const router = useRouter();
const globalMessageStore = useGlobalMessageStore();

const rules = {
    length: (v) => v.length >= 3 || "The length  must be at least 3 characters",
    samePasswords: (v) => v === registerData.value.password || "The passwords are not the same"
};


definePageMeta({
    layout: "no-auth",
})

const registerData = ref({
    name: '',
    surname:'',
    email: '',
    password: ''
});

const submit = async (ev) => {
    const { valid } = await ev;
    if (valid) {
        register();
    }
}

const error = ref("");
const loading = ref(false);

const register = () => {
    loading.value = true;
    error.value = "";

    useWebApiFetch('/User/CreateUserWithAccount', {
        method: 'POST',
        body: { ...registerData.value },
        onResponseError: ({ response }) => {
            error.value = getErrorMessage(response, {
                "AccountWithThisEmailAlreadyExists": "Account with this email already exist"
            });
        }
    })
        .then(() => {
                globalMessageStore.showSuccessMessage("Account created successfully !");
                router.push({ path: '/' });
        })
        .finally(() => {
            loading.value = false;
        });
};

</script>