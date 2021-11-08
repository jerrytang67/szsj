import {
    RouteConfig
} from 'vue-router'
import Layout from '@/layout/index.vue'

const auditManagement: RouteConfig = {
    path: "/AuditManagement",
    component: Layout,
    redirect: "/AuditManagement/auditFlowList",
    name: "审核中心",
    meta: {
        title: "审核中心",
        icon: "审核",
        permissions: ["Pages"]
    },
    children: [
        {
            path: "myAudit",
            name: "MyAudit",
            component: () => import("@/views/auditManagement/myAudit.vue"),
            meta: {
                title: "我的审核",
                icon: "审核",
                // num: 12,
                permissions: ["Pages"]
            }
        },
        {
            path: "auditFlowList",
            name: "AuditFlowList",
            component: () => import("@/views/auditManagement/auditFlowList.vue"),
            meta: {
                title: "审核流程配置",
                icon: "流程",
                permissions: ['Pages.Administration'],
            }
        },
        {
            path: 'auditFlowDetail/:hostType/:hostId/:auditFlowId',
            name: 'AuditFlowDetail',
            component: () => import(/* webpackChunkName: "auditNodePreDefineList" */ '@/views/auditManagement/auditFlowDetail.vue'),
            meta: {
                title: '审核信息详情',
                hidden: true
            }
        }
    ]
};

export default auditManagement;
