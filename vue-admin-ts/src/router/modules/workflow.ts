import {
  RouteConfig
} from 'vue-router'
import Layout from '@/layout/index.vue'

const workFlowRoute: RouteConfig = {
  path: "/workflow",
  component: Layout,
  redirect: "/workflow/design",
  name: "workflow",
  meta: {
    title: "工作流",
    icon: "workflow",
    permissions: ['Pages.Administration'],
    alwaysShow: true
  },
  children: [
    {
      path: "workflowList",
      name: "WorkflowList",
      component: () => import("@/views/workflow/workflowList.vue"),
      meta: {
        title: "工作流列表",
        icon: "workflow",
        permissions: ['Pages.Administration'],
      }
    },
    // {
    //   path: "design",
    //   name: "WorkFlowDesgin",
    //   component: () => import("@/views/workflow/design.vue"),
    //   meta: {
    //     title: "工作流设计器",
    //     icon: "workflow",
    //     permissions: ['Pages.Administration'],
    //   }
    // }
  ]
};

export default workFlowRoute;
