import router from './router'
import NProgress from 'nprogress'
import 'nprogress/nprogress.css'
import { Route } from 'vue-router'
import { getOu } from './utils/cookies'
import { UserModule } from './store/modules/user'
import { Message } from 'element-ui'

NProgress.configure({ showSpinner: false })

const whiteList = [
  '/login',
  '/login2',
  '/auth-redirect',
  '/AuditManagement/myAudit'

]

router.beforeEach(async (to: Route, _: Route, next: any) => {
  // Start progress bar
  NProgress.start()


  // Determine whether the user has logged in
  if (UserModule.token) {
    if (to.path === '/login') {
      // If is logged in, redirect to the home page
      next({ path: '/' })
      NProgress.done()
    } else {
      // Check whether the user has obtained his permission roles
      if (UserModule.roles.length === 0) {

        try {
          // Note: roles must be a object array! such as: ['admin'] or ['developer', 'editor']
          await UserModule.GetUserInfo()
          // Generate accessible routes map based on role
          // Dynamically add accessible routes
          // Hack: ensure addRoutes is complete
          // Set the replace: true, so the navigation will not leave a history record
          next({ ...to, replace: true })
        } catch (err) {
          // Remove token and redirect to login page
          UserModule.ResetToken()
          Message.error(err || 'Has Error')
          next(`/login?redirect=${to.path}`)
          NProgress.done();
        }
      } else {
        let ou = getOu();
        if (ou == undefined && to.path !== '/selectStore') {
          next(`/selectStore?redirect=${to.path}`);
          NProgress.done();
        }
        else {
          next();
          NProgress.done();
        }
      }
    }
  }
  else {
    if (whiteList.indexOf(to.path) !== -1) {
      // In the free login whitelist, go directly
      next()
    } else {
      // Other pages that do not have permission to access are redirected to the login page.
      next(`/login?redirect=${to.path}`)
      NProgress.done()
    }
  }
})

router.afterEach((to: Route) => {
  // Finish progress bar
  NProgress.done()

  // set page title
  document.title = to.meta!.title
})
