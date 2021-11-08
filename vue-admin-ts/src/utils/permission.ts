import { UserModule } from '@/store/modules/user'

const checkPermission = (value: string[]): boolean => {
    if (value && value instanceof Array && value.length > 0) {
        const permissions = UserModule.getPermissions
        const permissionRoles = value
        const hasPermission = permissions.some(p => {
            return permissionRoles.includes(p)
        })
        console.log("[checkPermission]:", value, hasPermission)
        return hasPermission
    } else {
        console.error(`need roles! Like v-permission="['admin','editor']"`)
        return false
    }
}

const checkRole = (value: string[]): boolean => {
    if (value && value instanceof Array && value.length > 0) {
        const roles = UserModule.getRoles
        const permissionRoles = value
        const hasPermission = roles.some(p => {
            return permissionRoles.includes(p)
        })
        console.log("[checkRole]:", value, hasPermission)
        return hasPermission
    } else {
        console.error(`need roles! Like v-permission="['admin','editor']"`)
        return false
    }
}


export {
    checkPermission,
    checkRole
}