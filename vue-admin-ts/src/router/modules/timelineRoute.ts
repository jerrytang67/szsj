
import { RouteConfig } from 'vue-router'
import Layout from '@/layout/index.vue'


const timelineRoute: RouteConfig = {
    path: '/timeline',
    component: Layout,
    redirect: '/timeline/timelineEventList',
    name: '时间轴管理',
    meta: {
        title: '时间轴管理',
        icon: 'timeline',
        permissions: ['Timeline.Default']
    },
    children: [
        {
            path: 'timelineCategoryList',
            name: 'TimelineCategoryList',
            component: () => import(/* webpackChunkName: "qaQuestionList" */'@/views/timeline/timelineCategoryList.vue'),
            meta: {
                title: '分类管理',
                icon: 'category',
                permissions: ['Timeline.Default']
            }
        },
        {
            path: 'timelineEventList',
            name: 'TimelineEventList',
            component: () => import(/* webpackChunkName: "timelineEventList" */'@/views/timeline/timelineEventList.vue'),
            meta: {
                title: '活动管理',
                icon: 'timeline',
                permissions: ['Timeline.Default']
            }
        }
    ]
}

export default timelineRoute



