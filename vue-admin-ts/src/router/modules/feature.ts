import {
    RouteConfig
} from 'vue-router'
import Layout from '@/layout/index.vue'

const featureRoute: RouteConfig = {
    path: "/feature",
    component: Layout,
    redirect: "/feature/featureList",
    name: "feature",
    meta: {
        title: "服务订购",
        icon: "feature",
        permissions: ['Pages.Administration'],
    },
    children: [
        {
            path: "featureList",
            name: "AbpFeatureList",
            component: () => import("@/views/feature/abpFeatureList.vue"),
            meta: {
                title: "功能服务",
                icon: "feature",
                permissions: ['Pages.Administration'],
            }
        },
        {
            path: "featureManagement",
            name: "FeatureManagement",
            component: () => import("@/views/feature/featureManagement.vue"),
            meta: {
                title: "功能开通管理",
                icon: "feature",
                role: ["Pages.Administration"]
            }
        }
    ]
};

export default featureRoute;
