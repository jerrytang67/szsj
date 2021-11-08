import { DirectiveOptions } from 'vue'
import { UserModule } from '@/store/modules/user'

export const permission: DirectiveOptions = {
  inserted(el, binding) {
    const { value } = binding

    const permissions = UserModule.permissions

    if (value && value instanceof Array && value.length > 0) {

      const permissionRoles = value
      const hasPermission = permissions.some(role => {
        return permissionRoles.includes(role)
      })

      if (!hasPermission) {
        el.style.display = 'none'
      }
    } else {
      throw new Error('need roles! Like v-permission="[\'admin\',\'editor\']"')
    }
  }
}
