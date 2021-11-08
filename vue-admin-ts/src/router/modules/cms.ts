



import {
    RouteConfig
} from 'vue-router'
import Layout from '@/layout/index.vue'

const cmsRouter: RouteConfig = {
    path: "/cms",
    component: Layout,
    redirect: "/cms/cmsContentList",
    name: "cms",
    meta: {
        title: "内容管理",
        icon: "docYes",
        permissions: ['Pages.Administration'],
    },
    children: [
        {
            path: 'cmsCategoryList',
            name: 'CmsCategoryList',
            component: () => import('@/views/cms/categoryList.vue'),
            meta: {
                title: '分类管理',
                icon: 'cate',
                permissions: ['Pages.Administration']
            }
        },

        {
            path: 'cmsContentList',
            name: 'CmsContentList',
            component: () => import('@/views/cms/cmsContentList.vue'),
            meta: {
                title: '内容列表列表',
                icon: 'docYes',
                permissions: ['Pages.Administration']
            }
        }
    ]
};
export default cmsRouter;
