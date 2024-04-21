<template>
    <div class="d-flex justify-center mt-10">
        <VCard width="600">
            <VForm :disabled="loading" @submit.prevent="submit">
                <VCardTitle class="text-center">New Url</VCardTitle>
                <VCardText>
                    <VTextField :rules="[ruleRequired,ruleUrl]" variant="solo-filled" label="Url"
                        v-model="modelData.url"></VTextField>
                </VCardText>
                <VCardActions>
                    <VBtn :loading="loading" class="mx-auto" variant="elevated" text="Submit" type="submit">
                    </VBtn>
                </VCardActions>
            </VForm>
        </VCard>
    </div>
</template>

<script setup>
const { ruleRequired, ruleUrl } = useFormValidationRules();
const loading = ref(false);
const modelData = ref({
    url: ''
});
const submit = async (ev) => {
    const { valid } = await ev;
    if (valid) {
    }
}
</script>