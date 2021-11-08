import {
  RouteConfig
} from 'vue-router'
import Layout from '@/layout/index.vue'


const settingsRouter: RouteConfig = {
  path: '/settings',
  component: Layout,
  redirect: '/settings/tenantSetting',
  name: '设置',
  meta: {
    title: '设置',
    icon: 'setting',
    permissions: ['Pages.Administration']
  },
  children: [
    {
      path: 'tenantSetting',
      name: '系统设置',
      component: () => import('@/views/admin/setting/tenantSetting.vue'),
      meta: {
        title: '系统设置',
        icon: 'setting',
        permissions: ['Pages.Administration.Tenant.Setting']
      }
    },
    {
      path: 'UI',
      name: 'UI',
      component: () => import('@/views/admin/ui.vue'),
      meta: {
        title: 'UI',
        icon: 'icon',
        permissions: ['Pages.Administration.Tenant.Setting']
      }
    }
  ]
}

export default settingsRouter
