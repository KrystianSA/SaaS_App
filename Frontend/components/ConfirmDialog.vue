<template>
    <v-dialog v-model="visible" width="500" absolute scroll-strategy="none">
        <v-card class="py-4">
            <v-card-title class="px-6 text-h5">
                <span>{{ viewModel.title }}</span>
            </v-card-title>

            <v-card-text>
                {{ viewModel.text }}
            </v-card-text>

            <v-card-actions class="pb-2 px-6">
                <v-spacer></v-spacer>
                <VBtn class="px-4" @click="cancel" :disabled="saving">
                    Cancel
                </VBtn>
                <VBtn :color="viewModel.confirmBtnColor" variant="flat" class="px-4" @click="ok" :loading="saving">
                    {{ viewModel.confirmBtnText }}
                </VBtn>
            </v-card-actions>
        </v-card>
    </v-dialog>
</template>

<script setup>

const viewModel = ref({
    title: "",
    text: "",
    confirmBtnText: "",
    confirmBtnColor: "",
});

const defaultOptions = () => {
    return {
        title: "Confirm",
        text: "Are you sure?",
        confirmBtnText: "Ok",
        confirmBtnColor: "primary",
    }
}

const saving = ref(false);
const visible = ref(false);

let promiseCallbacks = {};

const show = (options) => {
    visible.value = true
    viewModel.value = {...defaultOptions(), ...options}

    return new Promise((resolve, reject) => {
        promiseCallbacks = { resolve, reject };
    });
}

const ok = () => {
    saving.value = true;
    if (promiseCallbacks?.resolve) {
        promiseCallbacks.resolve(true);
    }

    saving.value = false;
    close();
}

const cancel = () => {
    if (promiseCallbacks?.resolve) {
        promiseCallbacks.resolve(false);
    }

    close();
}

const close = () => {
    visible.value = false;
}

defineExpose({
    show
});

</script>