// import this after install `@mdi/font` package
import '@mdi/font/css/materialdesignicons.css'

import 'vuetify/styles'
import { createVuetify } from 'vuetify'
import { aliases, mdi } from 'vuetify/iconsets/mdi-svg'
import colors from 'vuetify/util/colors'

export default defineNuxtPlugin((app) => {
  const vuetify = createVuetify({

    icons: {
      defaultSet: 'mdi',
      aliases,
      sets: {
        mdi,
      },
    },

    theme: {
      defaultTheme: 'light',
      themes: {
        light: {
          colors: {
          }
        },
        dark: {
          colors: {    
          }
        }
      }
    }
  })
  app.vueApp.use(vuetify)
})
