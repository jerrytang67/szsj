
import { RouteConfig } from 'vue-router'
import Layout from '@/layout/index.vue'


const qaRoute: RouteConfig = {
    path: '/vote',
    component: Layout,
    redirect: '/vote/votePlanList',
    name: '投票系统',
    meta: {
        title: '投票系统',
        icon: 'vote',
        permissions: ['Activity.Default']
    },
    children: [
        {
            path: 'votePlanList',
            name: 'VotePlanList',
            component: () => import(/* webpackChunkName: "votePlanList" */'@/views/vote/votePlanList.vue'),
            meta: {
                title: '投票活动',
                icon: 'vote',
                permissions: ['QA.Default']
            }
        },
        {
            path: 'voteItemList',
            name: 'voteItemList',
            component: () => import(/* webpackChunkName: "votePlanList" */'@/views/vote/voteItemList.vue'),
            meta: {
                title: '投票项列表',
                icon: 'vote',
                permissions: ['QA.Default']
            }
        }
    ]
}

export default qaRoute
