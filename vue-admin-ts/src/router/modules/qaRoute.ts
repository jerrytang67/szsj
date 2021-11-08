
import { RouteConfig } from 'vue-router'
import Layout from '@/layout/index.vue'


const qaRoute: RouteConfig = {
    path: '/qa',
    component: Layout,
    redirect: '/qa/qaPlanList',
    name: '问答系统',
    meta: {
        title: '问答系统',
        icon: 'qa',
        permissions: ['QA.Default']
    },
    children: [
        {
            path: 'qaPlanList',
            name: 'QAPlanList',
            component: () => import(/* webpackChunkName: "qaPlanList" */'@/views/qa/qaPlanList.vue'),
            meta: {
                title: '问答活动',
                icon: 'exam',
                permissions: ['QA.Default']
            }
        },
        {
            path: 'qaQuestionList',
            name: 'QAQuestionList',
            component: () => import(/* webpackChunkName: "qaQuestionList" */'@/views/qa/qaQuestionList.vue'),
            meta: {
                title: '题目管理',
                icon: 'doc',
                permissions: ['QA.Default']
            }
        },
        {
            path: 'userQuestionLogList',
            name: 'UserQuestionLogList',
            component: () => import(/* webpackChunkName: "userQuestionLogList" */'@/views/qa/userQuestionLogList.vue'),
            meta: {
                title: '用户答题记录',
                icon: 'user',
                permissions: ['QA.Default']
            }
        }
    ]
}

export default qaRoute



