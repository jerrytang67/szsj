import {
    RouteConfig
} from 'vue-router'
import Layout from '@/layout/index.vue'

const adminRoute: RouteConfig =
{
    path: '/admin',
    component: Layout,
    redirect: '/admin/audits',
    name: 'admin',
    meta: {
        title: '系统管理',
        icon: 'setting',
        permissions: ['Pages.Administration'],
    },
    children: [
        {
            path: 'userList',
            name: 'UserList',
            component: () => import(/* webpackChunkName: "userList" */ '@/views/admin/userList.vue'),
            meta: {
                title: '用户管理',
                icon: 'user',
                permissions: ['Pages.Administration'],
            }
        },
        {
            path: 'user-detail/:id',
            name: 'user-detail',
            component: () => import(/* webpackChunkName: "userDetail" */ '@/views/admin/user/detail.vue'),
            meta: {
                title: '用户详情',
                icon: 'user',
                permissions: ['Pages.Administration'],
                hidden: true
            },
        },
        {
            path: 'roles',
            name: 'RoleList',
            component: () => import(/* webpackChunkName: "roleList" */ '@/views/admin/roleList.vue'),
            meta: {
                title: '角色管理',
                icon: 'mg-role',
                permissions: ['Pages.Administration'],

            }
        },
        // {
        //     path: 'tenantList',
        //     name: 'TenantList',
        //     component: () => import(/* webpackChunkName: "tenantList" */ '@/views/admin/tenantList.vue'),
        //     meta: {
        //         title: '租户列表',
        //         icon: 'user',
        //         permissions: ['Pages.Administration']
        //     }
        // },
        {
            path: 'audits',
            name: 'AuditLogList',
            component: () => import(/* webpackChunkName: "AuditLogList" */'@/views/admin/AuditLogList.vue'),
            meta: {
                title: '操作日志',
                icon: 'documentation',
                permissions: ['Pages.Administration'],
            }
        },
        // {
        //     path: 'setting',
        //     name: 'Setting',
        //     component: () => import('@/views/setting/setting.vue'),
        //     meta: {
        //         title: '系统设置',
        //         icon: 'setting',
        //         permissions: ['Pages.Administration']
        //     }
        // },
    ]
}
    ;

export default adminRoute;
