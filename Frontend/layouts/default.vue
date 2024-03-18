<template>
    <v-app>
        <v-navigation-drawer v-model="drawer" v-if="userStore.$state.isLoggedIn === true">
            <v-list-item lines="two">
                <template v-slot:prepend>
                    <v-avatar color="red" v-if="userStore.$state.userData?.email">
                        {{ userStore.$state.userData.email[0].toUpperCase() }}
                    </v-avatar>
                </template>
                <VListItemTitle v-if="accountStore.$state.accountData?.email">{{ accountStore.$state.accountData.email
                    }}
                </VListItemTitle>
                <VListItemSubtitle v-if="userStore.$state.userData?.email">{{ userStore.$state.userData.email }}
                </VListItemSubtitle>
            </v-list-item>
            <VDivider></VDivider>
            <v-list>
                <v-list-item v-for="item in listSideNavBar" :key="item.name" :prepend-icon="item.icon"
                    :title="item.name" :to="item.route">
                </v-list-item>
            </v-list>
            <template v-slot:append>
                <div class="pa-2">
                    <v-btn block @click="logout">
                        Logout
                    </v-btn>
                </div>
            </template>
        </v-navigation-drawer>
        <v-app-bar v-if="userStore.$state.isLoggedIn === true">
            <v-app-bar-nav-icon @click="drawer = !drawer"></v-app-bar-nav-icon>
            <v-spacer></v-spacer>
            <v-btn @click=toggleTheme() :icon=mdiThemeLightDark title="Przełącz motyw"></v-btn>
        </v-app-bar>
        <v-main>
            <div>
                <NuxtPage v-if="userStore.$state.isLoggedIn === true" />
            </div>
        </v-main>
        <LoginDialog></LoginDialog>
        <ConfirmDialog ref="confirmDialog"></ConfirmDialog>
    </v-app>
</template>

<script setup>
import {
    mdiAccount,
    mdiAccessPoint,
    mdiAccountCowboyHatOutline,
    mdiThemeLightDark
} from '@mdi/js';
import { useTheme } from 'vuetify'
import LoginDialog from '~/components/LoginDialog.vue';
import ConfirmDialog from '~/components/ConfirmDialog.vue';
import { useStorage } from '@vueuse/core';

const confirmDialog = ref(null);

const antiForgeryStore = useAntiForgeryStore();

const userStore = useUserStore();
const accountStore = useAccountStore();
const global = useGlobalMessageStore()

const logout = () => {
    confirmDialog.value.show({
        title: 'Confirm logout',
        text: 'Are you sure you want to logout?',
        confirmBtnText: 'Logout',
        confirmBtnColor: 'error'
    }).then((confirm) => {
        if (confirm) {
            userStore.logout();
        }
    })
}


const drawer = ref(true)

const listSideNavBar = [
    { name: 'Position number 1', icon: mdiAccount, route: '/' },
    { name: 'Position number 2', icon: mdiAccessPoint, route: '/' },
    { name: 'Position number 3', icon: mdiAccountCowboyHatOutline, route: '/' },
]

const theme = useTheme()
const currentTheme = useStorage("currentTheme", 'light');

function toggleTheme() {
    let newTheme = theme.global.current.value.dark ? 'light' : 'dark'
    theme.global.name.value = newTheme
    currentTheme.value = newTheme
}

await antiForgeryStore.loadAntiForgeryToken();

onMounted(() => {
    theme.global.name.value = currentTheme.value
    userStore.loadLoggedInUser()
});
</script>