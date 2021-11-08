import { VuexModule, Module, Mutation, Action, getModule } from 'vuex-module-decorators'
import { getSidebarStatus, setSidebarStatus, getSize, setLanguage, setSize } from '@/utils/cookies'
import store from '@/store'
import { getLocale } from '@/lang'
import { TenantLoginInfoDto } from '@/api/appService'

export enum DeviceType {
  Mobile = 0,
  Desktop = 1,
}

// export interface IAppState {
//   device: DeviceType
//   sidebar: {
//     opened: boolean
//     withoutAnimation: boolean
//   },
//   name: string,
//   version: string,
//   tenant: ITenantLoginInfoDto,
//   tenantId: number | string | undefined,
//   routers: RouteConfig[] | undefined
// }

@Module({ dynamic: true, store, name: 'app' })
class App extends VuexModule {
  public sidebar = {
    opened: getSidebarStatus() !== 'closed',
    withoutAnimation: false
  }
  device = DeviceType.Desktop
  public language = getLocale()
  public size = getSize() || 'medium'

  public name = ''
  public version = ''
  public tenant: TenantLoginInfoDto | undefined = undefined
  tenantId: any = null;

  get getTenantId() {
    return this.tenantId;
  }

  @Mutation
  private TOGGLE_SIDEBAR(withoutAnimation: boolean) {
    this.sidebar.opened = !this.sidebar.opened
    this.sidebar.withoutAnimation = withoutAnimation
    if (this.sidebar.opened) {
      setSidebarStatus('opened')
    } else {
      setSidebarStatus('closed')
    }
  }


  @Mutation
  private CLOSE_SIDEBAR(withoutAnimation: boolean) {
    this.sidebar.opened = false
    this.sidebar.withoutAnimation = withoutAnimation
    setSidebarStatus('closed')
  }

  @Mutation
  private TOGGLE_DEVICE(device: DeviceType) {
    this.device = device
  }

  @Mutation
  private SET_TENANTID(tenantId?: number) {
    console.log("SETTENANTID", tenantId);

    this.tenantId = tenantId;
  }

  @Mutation
  private SET_TENANT(tenant: TenantLoginInfoDto | undefined) {
    this.tenant = tenant
  }


  @Mutation
  private SET_LANGUAGE(language: string) {
    this.language = language
    setLanguage(this.language)
  }

  @Mutation
  private SET_SIZE(size: string) {
    this.size = size
    setSize(this.size)
  }

  @Action
  public ToggleSideBar(withoutAnimation: boolean) {
    this.TOGGLE_SIDEBAR(withoutAnimation)
  }

  @Action
  public CloseSideBar(withoutAnimation: boolean) {
    this.CLOSE_SIDEBAR(withoutAnimation)
  }

  @Action
  public ToggleDevice(device: DeviceType) {
    this.TOGGLE_DEVICE(device)
  }

  @Action
  public SetTenantId(tenantId: number) {
    this.SET_TENANTID(tenantId);
  }

  @Action
  public RemoveTenantId() {
    this.SET_TENANTID(undefined);
  }

  @Action
  public SetTenant(tenant: TenantLoginInfoDto | undefined) {
    this.SET_TENANT(tenant);
  }


  @Action
  public SetLanguage(language: string) {
    this.SET_LANGUAGE(language)
  }

  @Action
  public SetSize(size: string) {
    this.SET_SIZE(size)
  }
}

export const AppModule = getModule(App)
