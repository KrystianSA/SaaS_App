<template>
    <template>
        <v-snackbar v-model="visible" :color="color">
            {{ currentMessage?.text }}

            <template v-slot:actions>
                <v-btn variant="text" @click="visible = false">
                    OK
                </v-btn>
            </template>
        </v-snackbar>
    </template>
</template>

<style lang="scss"></style>

<script setup>

const visible = ref(false);

const globalMessageStore = useGlobalMessageStore();
globalMessageStore.$subscribe((mutation, state) => {
    if (state.message && state.message.text) {
        visible.value = false;
        nextTick(() => {
            visible.value = true;
        });
    }
})

const currentMessage = computed(() => {
    return globalMessageStore.$state.message;
});

const color = computed(() => {
    if (currentMessage.value?.type === 'Success') {
        return "success"
    } else if (currentMessage.value?.type === 'Error') {
        return "error"
    } else {
        return "primary";
    }
});

</script>