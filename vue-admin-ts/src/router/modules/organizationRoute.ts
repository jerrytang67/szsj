import {
  RouteConfig
} from 'vue-router'
import Layout from '@/layout/index.vue'

const organizationRoute: RouteConfig = {
  path: '/organizations',
  component: Layout,
  redirect: '/organizations/list',
  name: 'organizations',
  meta: {
    title: '组织机构管理',
    icon: 'organization',
    alwaysShow: true
  },
  children: [

    {
      path: 'adminUserManager',
      name: 'AdminUserManager',
      component: () => import(/* webpackChunkName: "ouUserManager" */ '@/views/ou/adminUserManager.vue'),
      meta: {
        title: '组织机构列表',
        icon: 'organization',
        roles: ['Admin', "Region"],
        permissions: ['Pages.Administration']
      }
    },
    // {
    //   path: 'userManager',
    //   name: 'UserManager',
    //   component: () => import(/* webpackChunkName: "userManager" */ '@/views/ou/userManager.vue'),
    //   meta: {
    //     title: '机构成员管理',
    //     icon: 'user',
    //     permissions: ['Pages.Administration']
    //   }
    // },

    // {
    //   path: 'ouApplyList',
    //   name: 'OuApplyList',
    //   component: () => import(/* webpackChunkName: "userManager" */ '@/views/ou/ouApplyList.vue'),
    //   meta: {
    //     title: '机构申请列表',
    //     icon: 'user',
    //     permissions: ['Pages.Administration']
    //   }
    // },
    {
      path: 'myOuList',
      name: 'MyOuList',
      component: () => import(/* webpackChunkName: "ouList" */ '@/views/ou/myOuList.vue'),
      meta: {
        title: '我的组织机构',
        icon: 'user',
        permissions: ['Pages.Organization']
      }
    },
  ]
}


export default organizationRoute
