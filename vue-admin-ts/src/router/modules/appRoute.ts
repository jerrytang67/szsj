
import { RouteConfig } from 'vue-router'
import Layout from '@/layout/index.vue'


const appRoute: RouteConfig = {
    path: '/app',
    component: Layout,
    redirect: '/app/swiperList',
    name: '应用管理',
    meta: {
        title: '应用管理',
        icon: 'app',
        permissions: ['Pages.Administration']
    },
    children: [
        {
            path: 'appList',
            name: 'AppList',
            component: () => import(/* webpackChunkName: "swiperList" */'@/views/app/appList.vue'),
            meta: {
                title: '应用列表',
                icon: 'app',
                permissions: ['Pages.Administration']
            }
        },
        {
            path: 'swiperList',
            name: 'SwiperList',
            component: () => import(/* webpackChunkName: "swiperList" */'@/views/app/swiperList.vue'),
            meta: {
                title: '滚动图片导航',
                icon: 'swiper',
                permissions: ['Pages.Administration']
            }
        }
    ]
}

export default appRoute



