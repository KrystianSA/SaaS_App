<template>
    <v-app>
        <v-navigation-drawer width="auto" v-model="drawer" v-if="userStore.$state.isLoggedIn === true">
            <VList>
                <VListGroup>
                    <template v-slot:activator="{ props }">
                        <v-list-item v-bind="props" prepend-icon="mdi-account-circle">
                            <template v-slot:prepend>
                                <v-avatar color="red" v-if="userStore.$state.userData?.email">
                                    {{ userStore.$state.userData.email[0].toUpperCase() }}
                                </v-avatar>
                            </template>
                            <VListItemTitle v-if="accountStore.$state.accountData?.email">
                                {{ accountStore.$state.accountData.email }}
                            </VListItemTitle>
                        </v-list-item>
                    </template>
                    <VListItem>
                        <v-menu v-model="showCard" :close-on-content-click="false" location="end">
                            <template v-slot:activator="{ props }">
                                <VBtn v-bind="props" :prepend-icon="mdiCog" variant="plain" text="Settings"></VBtn>
                            </template>
                            <v-card min-width="auto">
                                <v-list>
                                    <v-list-item>
                                        <VBtn to="/forget-password" variant="plain" text="Change password"></VBtn>
                                    </v-list-item>
                                    <v-list-item>
                                        <VBtn @click=toggleTheme() variant="plain" text="Toogle Theme"></VBtn>
                                    </v-list-item>
                                    <v-list-item>
                                        <VBtn variant="plain" text="Other Settings"></VBtn>
                                    </v-list-item>
                                </v-list>
                            </v-card>
                        </v-menu>
                    </VListItem>
                    <VListItem>
                        <VListItemAction>
                            <VBtn @click="logout" :prepend-icon="mdiLogout" variant="plain" text="Logout"></VBtn>
                        </VListItemAction>
                    </VListItem>
                </VListGroup>
            </VList>
            <VDivider></VDivider>
            <v-list>
                <v-list-item v-for="item in listSideNavBar" :key="item.name" :prepend-icon="item.icon"
                    :title="item.name" :to="item.route">
                </v-list-item>
            </v-list>
        </v-navigation-drawer>
        <v-app-bar v-if="userStore.$state.isLoggedIn === true">
            <v-app-bar-nav-icon @click="drawer = !drawer"></v-app-bar-nav-icon>
            <!-- <VAppBarTitle text="Feed Reader"></VAppBarTitle> -->
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
    mdiPlus,
    mdiEmail,
    mdiTrashCan,
    mdiDomain ,
    mdiCog,
    mdiLogout
} from '@mdi/js';
import { useTheme } from 'vuetify'
import LoginDialog from '~/components/LoginDialog.vue';
import ConfirmDialog from '~/components/ConfirmDialog.vue';
import { useStorage } from '@vueuse/core';

const showCard = ref(false);
const confirmDialog = ref(null);
const antiForgeryStore = useAntiForgeryStore();

const userStore = useUserStore();
const accountStore = useAccountStore();

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
    { name: 'Main', icon: mdiDomain , route: '/' },
    { name: 'Subscribe Website', icon: mdiPlus, route: '/add-feed' },
    { name: 'Posts', icon: mdiEmail, route: '/posts' },
    { name: 'Bin', icon: mdiTrashCan, route: '/bin' },
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