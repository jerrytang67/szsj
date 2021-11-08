import { VuexModule, Module, Action, Mutation, getModule } from 'vuex-module-decorators'
import { getToken, setToken, removeToken, removeOu } from '@/utils/cookies'
import router, { resetRouter } from '@/router'

import { AppModule } from './app'

import api from '@/api/index'
import { getOu, setOu } from '@/utils/cookies'

import { PermissionModule } from './permission'
import { TagsViewModule } from './tags-view'
import store from '@/store'
import { UserLoginInfoDto, OrganizationUnitDto } from '@/api/appService'
import { setStore, getStore } from '@/utils/storage'

// export interface IUserState {
//   token: string
//   name: string
//   avatar: string
//   introduction: string
//   roles: string[]
//   user: any
// }

@Module({ dynamic: true, store, name: 'user' })
class User extends VuexModule {

  public token = getToken() || ''
  public roles: string[] = ["admin"]
  public permissions: string[] = []

  public ou: OrganizationUnitDto = getStore<OrganizationUnitDto>("currentOrganization") || {
    displayName: "vue-admin-ts", detail: {
      logoImgUrl: "https://wpimg.wallstcn.com/69a1c46c-eb1c-4b46-8bd4-e9e686ef5251.png"
    }
  };

  get ouId() {
    let ouid = this.ou!.id || getOu();
    return ouid == -1 ? null : ouid
  }

  get getOU() {
    return this.ou;
  }

  public user: UserLoginInfoDto = { id: -1, isSubscribe: false, name: undefined, surname: undefined, userName: undefined, emailAddress: undefined, headImgUrl: undefined };

  get getToken() {
    return this.token;
  }

  get getRoles() {
    return this.roles;
  }

  get getIsAdmin() {
    let isADMIN = this.roles.indexOf("Admin") > -1
    return isADMIN;
  }

  get getAvatar() {
    return this.user.headImgUrl;
  }

  get getName() {
    return this.user.name;
  }

  get getPermissions() {
    return this.permissions;
  }

  @Mutation
  private SET_TOKEN(token: string) {
    this.token = token
  }

  @Mutation
  private SET_ROLES(roles: string[] | string) {
    if (Array.isArray(roles))
      this.roles = [...roles]
    else
      this.roles = [roles];
  }


  @Mutation
  private SET_PERMISSIONS(permissions: string[]) {
    this.permissions = permissions;
  }

  @Mutation
  private SET_USER(user: any) {
    this.user = user
  }

  @Mutation
  private SET_OU(ou: OrganizationUnitDto) {
    this.ou = ou
    setStore("currentOrganization", ou);
    setOu(`${ou.id!}`)
  }


  @Action
  public SetToken(token: string) {
    this.SET_TOKEN(token)
  }

  @Action
  public SetRoles(roles: string[] | string) {
    if (Array.isArray(roles))
      this.SET_ROLES(roles)
    else
      this.SET_ROLES([roles])
  }

  @Action
  public async Login(userInfo: { username: string, password: string }) {
    return new Promise(async (resolve, reject) => {
      let { username, password } = userInfo
      username = username.trim()
      await api.tokenAuth.authenticate(
        {
          body: {
            userNameOrEmailAddress: username.trim(),
            password: password
          }
        }
      ).then(res => {
        setToken(res.accessToken!)
        this.SET_TOKEN(res.accessToken!);
        resolve();
      }, error => {
        reject(error);
      })

    })

  }

  @Action
  public ResetToken() {
    removeToken()
    this.SET_TOKEN('')
    this.SET_ROLES([])
  }

  @Action
  public async GetUserInfo(roles: string[] = []) {
    await api.session.getCurrentLoginInformations().then((res) => {
      //console.log(res);
      AppModule.SetTenant(res.tenant!)

      if (res.user == null) {
        this.SET_USER({})
        this.ResetToken();
      }
      else {
        this.SET_USER(res.user);
      }

      if (res.permissions)
        this.SET_PERMISSIONS(res.permissions);

      if (res.roles)
        this.SetRoles(res.roles);

      PermissionModule.GenerateRoutes({ permissions: res.permissions!, roles: roles })

      if (res.organizationUnit) {
        this.SET_OU(res.organizationUnit!)
      }
    });
  }
  @Action
  public Set_Ou(ou: OrganizationUnitDto) {
    this.SET_OU(ou);
    setStore("currentOrganization", ou);
    PermissionModule.GenerateRoutes({ permissions: this.permissions!, roles: this.roles })
  }

  @Action
  public async LogOut() {
    if (this.token === '') {
      throw Error('LogOut: token is undefined!')
    }

    // todo:api logout 

    await removeToken()
    await resetRouter()
    await removeOu();

    // Reset visited views and cached views
    await TagsViewModule.delAllViews()
    this.SET_TOKEN('')
    this.SET_ROLES([])
  }
}
export const UserModule = getModule(User)
