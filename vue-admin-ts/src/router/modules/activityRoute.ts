
import { RouteConfig } from 'vue-router'
import Layout from '@/layout/index.vue'


const activityRoute: RouteConfig = {
    path: '/activity',
    component: Layout,
    redirect: '/activity/pointActivityList',
    name: '活动管理',
    meta: {
        title: '活动管理',
        icon: 'app',
        permissions: ['Activity.Default']
    },
    children: [
        {
            path: 'pointActivityList',
            name: 'PointActivityList',
            component: () => import(/* webpackChunkName: "pointActivityList" */'@/views/activity/pointActivityList.vue'),
            meta: {
                title: '积分活动列表',
                icon: 'app',
                permissions: ['Activity.Default']
            }
        }
        ,
        {
            path: 'luckDrawList',
            name: 'LuckDrawList',
            component: () => import(/* webpackChunkName: "luckDrawList" */'@/views/activity/luckDrawList.vue'),
            meta: {
                title: '幸运抽奖列表',
                icon: 'app',
                permissions: ['Activity.Default']
            }
        }
        ,
        {
            path: 'luckDrawPrizeList',
            name: 'LuckDrawPrizeList',
            component: () => import(/* webpackChunkName: "luckDrawList" */'@/views/activity/luckDrawPrizeList.vue'),
            meta: {
                title: '抽奖奖品列表',
                icon: 'app',
                permissions: ['Activity.Default']
            }
        }
        ,
        {
            path: 'userPrizeList',
            name: 'UserPrizeList',
            component: () => import(/* webpackChunkName: "luckDrawList" */'@/views/activity/userPrizeList.vue'),
            meta: {
                title: '用户中奖记录',
                icon: 'app',
                permissions: ['Activity.Default']
            }
        }
    ]
}

export default activityRoute



