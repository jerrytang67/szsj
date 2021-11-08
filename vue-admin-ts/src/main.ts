import Vue, { DirectiveOptions } from 'vue'

import "@/styles/tailwind.scss";//引入tailwind

import 'normalize.css'
import ElementUI from 'element-ui'
import SvgIcon from 'vue-svgicon'
import '@/styles/element-variables.scss'
import '@/styles/index.scss'


import "zoom-vanilla.js/dist/zoom.css"
import "zoom-vanilla.js/dist/zoom-vanilla.min.js"


import App from '@/App.vue'
import store from '@/store'
import { AppModule } from '@/store/modules/app'
import router from '@/router'
import i18n from '@/lang'
import '@/icons/components/index'
import '@/permission'
import '@/utils/error-log'

import * as directives from '@/directives'
import * as filters from '@/filters'
import * as components from '@/components'

Vue.prototype.$uploadUrl = process.env.VUE_APP_BASE_API + '​/api​/services​/app​/Upload​/Upload'
Vue.prototype.baseURL = process.env.VUE_APP_BASE_API

Vue.use(ElementUI, {
  size: AppModule.size, // Set element-ui default size
  i18n: (key: string, value: string) => i18n.t(key, value)
})

Vue.use(SvgIcon, {
  tagName: 'svg-icon',
  defaultWidth: '1em',
  defaultHeight: '1em'
})


// Register global directives
Object.keys(directives).forEach(key => {
  Vue.directive(key, (directives as { [key: string]: DirectiveOptions })[key])
})

// Register global filter functions
Object.keys(filters).forEach(key => {
  Vue.filter(key, (filters as { [key: string]: Function })[key])
})


// Register global components functions
Object.keys(components).forEach(key => {
  Vue.component(key, (components as { [key: string]: Function })[key])
})




Vue.config.productionTip = false

new Vue({
  router,
  store,
  i18n,
  render: (h) => h(App)
}).$mount('#app')
