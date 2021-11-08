import Vue from 'vue'
import App from './App.vue'

import "./css/icon.css";
import "./css/tailwind.scss";
import { i18n } from './local';

Vue.prototype.i18n = i18n;


import * as filters from '@/filters'
// Register global filter functions
Object.keys(filters).forEach(key => {
    Vue.filter(key, (filters as { [key: string]: Function })[key])
})

new App().$mount()

// #ifdef H5  
import wechat from './utils/wechat'
if (wechat.isWechat()) {
    Vue.prototype.$wechat = wechat;
    wechat.initJssdk(() => {
    });
}
// #endif