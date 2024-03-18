import vuetify, { transformAssetUrls } from 'vite-plugin-vuetify'
export default defineNuxtConfig({
  runtimeConfig: {
    public: {
      BASE_URL: "http://localhost:5164", //replace with ENV: NUXT_PUBLIC_BASE_URL 
    },
  },
  devtools:{enabled:false},
  ssr:false,
  build: {
    transpile: ['vuetify'],
  },
  modules: [
    (_options, nuxt) => {
      nuxt.hooks.hook('vite:extendConfig', (config) => {
        // @ts-expect-error
        config.plugins.push(vuetify({ autoImport: true }))
      })
    },
    
    '@pinia/nuxt',
    '@vueuse/nuxt',
    'nuxt-lodash',
    'dayjs-nuxt'

  ],
  imports: {
    dirs: ['stores'],
  },

  lodash: {
    prefix: "_",
    prefixSkip: false,
    upperAfterPrefix: false,
  },

  vite: {
    vue: {
      template: {
        transformAssetUrls,
      },
    },
  },
})
