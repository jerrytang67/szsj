import Vue from 'vue'
import Router, { RouteConfig } from 'vue-router'
import Layout from '@/layout/index.vue'
import auditManagement from './modules/auditManagemeng'
import workFlowRoute from './modules/workflow'
import adminRoute from './modules/admin'
import cmsRoute from './modules/cms'
import appRoute from './modules/appRoute'
import activityRoute from './modules/activityRoute'
import organizationRoute from './modules/organizationRoute'

Vue.use(Router)

/*
  redirect:                      if set to 'noredirect', no redirect action will be trigger when clicking the breadcrumb
  meta: {
    title: 'title'               the name showed in subMenu and breadcrumb (recommend set)
    icon: 'svg-name'             the icon showed in the sidebar
    breadcrumb: false            if false, the item will be hidden in breadcrumb (default is true)
    hidden: true                 if true, this route will not show in the sidebar (default is false)
  }
*/

export const constantRoutes: RouteConfig[] = [
   {
      path: '/selectStore',
      component: () => import(/* webpackChunkName: "login" */ '@/views/selectStore.vue'),
      meta: {
         title: '选择门店',
         hidden: true
      }
   },
   {
      path: '/redirect',
      component: Layout,
      meta: { hidden: true },
      children: [{
         path: '/redirect/:path(.*)',
         component: () => import(/* webpackChunkName: "redirect" */ '@/views/redirect/index.vue')
      }]
   },
   {
      path: '/login',
      component: () => import(/* webpackChunkName: "login" */ '@/views/login/index.vue'),
      meta: { hidden: true }
   },
   {
      path: '/auth-redirect',
      component: () => import(/* webpackChunkName: "auth-redirect" */ '@/views/login/auth-redirect.vue'),
      meta: { hidden: true }
   },
   {
      path: '/404',
      component: () => import(/* webpackChunkName: "404" */ '@/views/404.vue'),
      meta: { hidden: true }
   },
   {
      path: '/401',
      component: () => import(/* webpackChunkName: "401" */ '@/views/error-page/401.vue'),
      meta: { hidden: true }
   },
   {
      path: '/',
      component: Layout,
      redirect: '/dashboard',
      children: [
         {
            path: 'dashboard',
            component: () => import(/* webpackChunkName: "dashboard" */ '@/views/dashboard/index.vue'),
            name: 'Dashboard',
            meta: {
               title: '数据面板',
               icon: 'dashboard',
               affix: true
            }
         }
      ]
   },
]

// 动态加载菜单
export const asyncRouter: RouteConfig[] = [
   appRoute,
   cmsRoute,
   activityRoute,

   auditManagement,
   organizationRoute,
   adminRoute,
   // topAdminRoute,
   workFlowRoute,
   {
      path: '*',
      redirect: '/404',
      meta: { hidden: true }
   },
]

const createRouter = () => new Router({
   // mode: 'history',  // Enable this if you need.
   scrollBehavior: (to: any, from: any, savedPosition: any) => {
      if (savedPosition) {
         return savedPosition
      } else {
         return { x: 0, y: 0 }
      }
   },
   base: process.env.BASE_URL,
   routes: [...constantRoutes, ...asyncRouter]
})


const router = createRouter()

// Detail see: https://github.com/vuejs/vue-router/issues/1234#issuecomment-357941465
export function resetRouter() {
   const newRouter = createRouter();
   (router as any).matcher = (newRouter as any).matcher // reset router
}

export default router
