import {
    RouteConfig
} from 'vue-router'
import Layout from '@/layout/index.vue'

const topAdminRouter: RouteConfig = {
    path: '/topAdmin',
    component: Layout,
    redirect: '/topAdmin/adminUserManager',
    name: 'topAdmin',
    meta: {
        title: '超级管理',
        icon: 'organization',
        permissions: ['Pages.Administration']
    },
    children: [
        {
            path: 'wechatUserinfoList',
            name: 'WechatUserinfoList',
            component: () => import(/* webpackChunkName: "WechatUserinfoList" */'@/views/admin/WechatUserinfoList.vue'),
            meta: {
                title: '微信用户',
                icon: 'weixin',
                permissions: ['Pages.Administration']
            }
        },
        {
            path: 'UI',
            name: 'UI',
            component: () => import('@/views/admin/ui.vue'),
            meta: {
                title: 'UI',
                icon: 'icon',
                permissions: ['Pages.Administration']
            }
        }
    ]
}
export default topAdminRouter