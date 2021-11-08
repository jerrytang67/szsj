
import { RouteConfig } from 'vue-router'
import Layout from '@/layout/index.vue'


const craftsmanRoute: RouteConfig = {
    path: '/craftsman',
    component: Layout,
    redirect: '/craftsman/CraftsmanList',
    name: '红色工匠',
    meta: {
        title: '红色工匠',
        icon: 'craftsman',
        permissions: ['Pages.Administration', 'LaborUnion.Default']
    },
    children: [
        {
            path: 'CraftsmanList',
            name: 'CraftsmanList',
            component: () => import(/* webpackChunkName: "swiperList" */'@/views/craftsman/CraftsmanList.vue'),
            meta: {
                title: '工匠自荐管理',
                icon: 'craftsman',
                permissions: ['Pages.Administration', 'LaborUnion.Default']
            }
        },
        {
            path: 'CraftsmanRecommandList',
            name: 'CraftsmanRecommandList',
            component: () => import(/* webpackChunkName: "swiperList" */'@/views/craftsman/CraftsmanRecommendList.vue'),
            meta: {
                title: '工匠推荐管理',
                icon: 'craftsman',
                permissions: ['Pages.Administration', 'LaborUnion.Default']
            }
        }
    ]
}

export default craftsmanRoute



