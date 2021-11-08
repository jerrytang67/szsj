import request from '@/utils/request'
import { serviceOptions, AuditFlowService, AccountService, ClientService, SessionService, TenantSettingsService, UserService, OrganizationUnitService, WechatUserinfoService, RoleService, SwiperService, AuditLogService, TokenAuthService, TenantService, AbpFeatureService, UploadService, OrganizationApplyService, WorkflowService, CmsCategoryService, CmsContentService, AppService, CraftsmanService, CraftsmanRecommendService, DashboardService, PointActivityService, LuckDrawService, LuckDrawPrizeService, UserPrizeService, QaPlanService, QaQuestionService, UserQuestionLogService, TimelineEventService, TimelineCategoryService, TimelineFileService, VotePlanService, VoteItemService } from './appService';

serviceOptions.axios = request;

//const basrUrl = process.env.VUE_APP_BASE_API;
const app = AppService;
const auditFlow = AuditFlowService;
const auditLog = AuditLogService;
const account = AccountService;
const client = ClientService;
const session = SessionService;
const tenantSetting = TenantSettingsService;
const user = UserService;
const wechatUserinfo = WechatUserinfoService;
const role = RoleService;
const swiper = SwiperService;
const tokenAuth = TokenAuthService;
const tenant = TenantService;
const abpFeature = AbpFeatureService;
const upload = UploadService;

const organizationUnit = OrganizationUnitService;
const organizationApply = OrganizationApplyService;
const workflow = WorkflowService;

const cmsContent = CmsContentService;
const cmsCategory = CmsCategoryService;

const craftsman = CraftsmanService;
const craftsmanRecommend = CraftsmanRecommendService;

const pointActivity = PointActivityService;
const luckDraw = LuckDrawService;
const luckDrawPrize = LuckDrawPrizeService;
const userPrize = UserPrizeService;


const qaPlan = QaPlanService;
const qaQuestion = QaQuestionService;
const userQuestionLog = UserQuestionLogService;

const votePlan = VotePlanService;
const voteItem = VoteItemService;


const timelineCategory = TimelineCategoryService;
const timelineEvent = TimelineEventService;
const timelineFile = TimelineFileService;

const dashboard = DashboardService;


const guid = '00000000-0000-0000-0000-000000000000';

export default {
  app,
  auditFlow,
  auditLog,
  account,
  client,
  session,
  tenantSetting,
  tenant,
  user,
  role,
  swiper,
  wechatUserinfo,
  tokenAuth,
  abpFeature,
  upload,
  organizationUnit,
  organizationApply,
  workflow,

  cmsContent,
  cmsCategory,

  craftsman,
  craftsmanRecommend,

  pointActivity,
  luckDraw,
  luckDrawPrize,
  userPrize,

  qaPlan,
  qaQuestion,
  userQuestionLog,

  votePlan,
  voteItem,

  timelineCategory,
  timelineEvent,
  timelineFile,

  dashboard,
  guid
}
