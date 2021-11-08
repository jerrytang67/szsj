import { VuexModule, Module, Mutation, Action, getModule } from 'vuex-module-decorators'
import { RouteConfig } from 'vue-router'
import store from '@/store'
import { constantRoutes, asyncRouter } from '@/router'

const hasPermission = (permissions: string[], roles: string[], route: RouteConfig) => {
    if (route.meta) {
        let b1 = true;
        let b2 = true;
        if (route.meta.permissions && permissions.length) {
            b1 = permissions.some(p => route.meta!.permissions.includes(p))
        }
        if (route.meta.roles && roles.length) {
            b2 = roles.some(role => route.meta!.roles.includes(role));
        }
        return b1 && b2;
    } else {
        return true
    }
}

export const filterAsyncRoutes = (routes: RouteConfig[], permissions: string[], roles: string[]) => {

    const res: RouteConfig[] = []
    routes.forEach(route => {
        const r = { ...route }
        if (hasPermission(permissions, roles, r)) {
            if (r.children) {
                r.children = filterAsyncRoutes(r.children, permissions, roles)
            }
            res.push(r)
        }
    })
    return res
}

export interface IPermissionState {
    routes: RouteConfig[]
    dynamicRoutes: RouteConfig[]
}

// Create module later in your code (it will register itself automatically)
// In the decorator we pass the store object into which module is injected
// NOTE: When you set dynamic true, make sure you give module a name
@Module({ dynamic: true, store, name: 'permission' })
class Permission extends VuexModule {

    public routes: RouteConfig[] = []

    public dynamicRoutes: RouteConfig[] = []

    @Mutation
    private SET_ROUTES(routes: RouteConfig[]) {
        this.dynamicRoutes = routes
        this.routes = [...constantRoutes!, ...routes];
    }

    @Action
    public GenerateRoutes(input: { permissions: string[], roles: string[] }) {
        if (input.permissions !== undefined) {
            let accessedRoutes
            // if (input.roles !== undefined && input.roles!.includes('Admin')) {
            //     accessedRoutes = asyncRouter
            // } else {
            accessedRoutes = filterAsyncRoutes(asyncRouter, input.permissions!, input.roles!)
            // }
            this.SET_ROUTES(accessedRoutes)
        }
    }
}

export const PermissionModule = getModule(Permission)
