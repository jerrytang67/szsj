/** Generate by swagger-axios-codegen */
// tslint:disable
/* eslint-disable */
import axiosStatic, { AxiosInstance } from 'axios';

export interface IRequestOptions {
  headers?: any;
  baseURL?: string;
  responseType?: string;
}

export interface IRequestConfig {
  method?: any;
  headers?: any;
  url?: any;
  data?: any;
  params?: any;
}

// Add options interface
export interface ServiceOptions {
  axios?: AxiosInstance;
}

// Add default options
export const serviceOptions: ServiceOptions = {};

// Instance selector
export function axios(configs: IRequestConfig, resolve: (p: any) => void, reject: (p: any) => void): Promise<any> {
  if (serviceOptions.axios) {
    return serviceOptions.axios
      .request(configs)
      .then(res => {
        resolve(res.data);
      })
      .catch(err => {
        reject(err);
      });
  } else {
    throw new Error('please inject yourself instance like axios  ');
  }
}

export function getConfigs(method: string, contentType: string, url: string, options: any): IRequestConfig {
  const configs: IRequestConfig = { ...options, method, url };
  configs.headers = {
    ...options.headers,
    'Content-Type': contentType
  };
  return configs;
}

export class IList<T> extends Array<T> {}
export class List<T> extends Array<T> {}

export interface IListResult<T> {
  items?: T[];
}

export class ListResultDto<T> implements IListResult<T> {
  items?: T[];
}

export interface IPagedResult<T> extends IListResult<T> {
  totalCount: number;
}

export class PagedResultDto<T> implements IPagedResult<T> {
  totalCount!: number;
}

// customer definition
// empty

export class AbpFeatureService {
  /**
   *
   */
  static getAllFeatureDefinition(options: IRequestOptions = {}): Promise<FeatureDefinition[]> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/FeatureManagement/AbpFeature/GetAllFeatureDefinition';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);

      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getForEdit(
    params: {
      /**  */
      id?: string;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<AbpFeatureDtoGetForEditOutput> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/FeatureManagement/AbpFeature/GetForEdit';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static create(
    params: {
      /** requestBody */
      body?: AbpFeatureDto;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<AbpFeatureDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/FeatureManagement/AbpFeature/Create';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static update(
    params: {
      /** requestBody */
      body?: AbpFeatureDto;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<AbpFeatureDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/FeatureManagement/AbpFeature/Update';

      const configs: IRequestConfig = getConfigs('put', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getAll(
    params: {
      /**  */
      keyword?: string;
      /**  */
      isActive?: boolean;
      /**  */
      sorting?: string;
      /**  */
      from?: string;
      /**  */
      to?: string;
      /**  */
      skipCount?: number;
      /**  */
      maxResultCount?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<AbpFeatureDtoPagedResultDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/FeatureManagement/AbpFeature/GetAll';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = {
        Keyword: params['keyword'],
        IsActive: params['isActive'],
        Sorting: params['sorting'],
        From: params['from'],
        To: params['to'],
        SkipCount: params['skipCount'],
        MaxResultCount: params['maxResultCount']
      };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static get(
    params: {
      /**  */
      id?: string;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<AbpFeatureDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/FeatureManagement/AbpFeature/Get';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static delete(
    params: {
      /**  */
      id?: string;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<any> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/FeatureManagement/AbpFeature/Delete';

      const configs: IRequestConfig = getConfigs('delete', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
}

export class AccountService {
  /**
   *
   */
  static isTenantAvailable(
    params: {
      /** requestBody */
      body?: IsTenantAvailableInput;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<IsTenantAvailableOutput> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/Account/IsTenantAvailable';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static register(
    params: {
      /** requestBody */
      body?: RegisterInput;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<RegisterOutput> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/Account/Register';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static updatePhone(options: IRequestOptions = {}): Promise<UserDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/Account/UpdatePhone';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
}

export class AppService {
  /**
   *
   */
  static getAll(
    params: {
      /**  */
      sorting?: string;
      /**  */
      skipCount?: number;
      /**  */
      maxResultCount?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<AppDtoPagedResultDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/AppManagement/App/GetAll';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = {
        Sorting: params['sorting'],
        SkipCount: params['skipCount'],
        MaxResultCount: params['maxResultCount']
      };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getPublishList(options: IRequestOptions = {}): Promise<AppDtoListResultDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/AppManagement/App/GetPublishList';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);

      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static get(
    params: {
      /**  */
      id?: string;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<AppDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/AppManagement/App/Get';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static create(
    params: {
      /** requestBody */
      body?: AppCreateOrUpdateDto;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<AppDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/AppManagement/App/Create';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static update(
    params: {
      /** requestBody */
      body?: AppCreateOrUpdateDto;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<AppDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/AppManagement/App/Update';

      const configs: IRequestConfig = getConfigs('put', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static delete(
    params: {
      /**  */
      id?: string;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<any> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/AppManagement/App/Delete';

      const configs: IRequestConfig = getConfigs('delete', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
}

export class AuditFlowService {
  /**
   *
   */
  static get(
    params: {
      /**  */
      id?: string;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<AuditFlowDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/AuditManagement/AuditFlow/Get';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getForEdit(
    params: {
      /**  */
      id?: string;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<AuditFlowCreateOrEditDtoGetForEditOutput> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/AuditManagement/AuditFlow/GetForEdit';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getAll(
    params: {
      /**  */
      keyword?: string;
      /**  */
      isActive?: boolean;
      /**  */
      sorting?: string;
      /**  */
      from?: string;
      /**  */
      to?: string;
      /**  */
      skipCount?: number;
      /**  */
      maxResultCount?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<AuditFlowDtoPagedResultDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/AuditManagement/AuditFlow/GetAll';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = {
        Keyword: params['keyword'],
        IsActive: params['isActive'],
        Sorting: params['sorting'],
        From: params['from'],
        To: params['to'],
        SkipCount: params['skipCount'],
        MaxResultCount: params['maxResultCount']
      };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static create(
    params: {
      /** requestBody */
      body?: AuditFlowCreateOrEditDto;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<AuditFlowDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/AuditManagement/AuditFlow/Create';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static update(
    params: {
      /** requestBody */
      body?: AuditFlowCreateOrEditDto;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<AuditFlowDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/AuditManagement/AuditFlow/Update';

      const configs: IRequestConfig = getConfigs('put', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getByName(
    params: {
      /**  */
      auditName?: string;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<AuditFlowDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/AuditManagement/AuditFlow/GetByName';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = { auditName: params['auditName'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static delete(
    params: {
      /**  */
      id?: string;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<any> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/AuditManagement/AuditFlow/Delete';

      const configs: IRequestConfig = getConfigs('delete', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
}

export class AuditLogService {
  /**
   *
   */
  static getAuditLogs(
    params: {
      /**  */
      startDate?: string;
      /**  */
      endDate?: string;
      /**  */
      userName?: string;
      /**  */
      serviceName?: string;
      /**  */
      methodName?: string;
      /**  */
      browserInfo?: string;
      /**  */
      hasException?: boolean;
      /**  */
      minExecutionDuration?: number;
      /**  */
      maxExecutionDuration?: number;
      /**  */
      sorting?: string;
      /**  */
      maxResultCount?: number;
      /**  */
      skipCount?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<AuditLogListDtoPagedResultDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/AuditLog/GetAuditLogs';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = {
        StartDate: params['startDate'],
        EndDate: params['endDate'],
        UserName: params['userName'],
        ServiceName: params['serviceName'],
        MethodName: params['methodName'],
        BrowserInfo: params['browserInfo'],
        HasException: params['hasException'],
        MinExecutionDuration: params['minExecutionDuration'],
        MaxExecutionDuration: params['maxExecutionDuration'],
        Sorting: params['sorting'],
        MaxResultCount: params['maxResultCount'],
        SkipCount: params['skipCount']
      };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getEntityPropertyChanges(
    params: {
      /**  */
      entityChangeId?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<EntityPropertyChangeDto[]> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/AuditLog/GetEntityPropertyChanges';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = { entityChangeId: params['entityChangeId'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
}

export class ClientService {
  /**
   *
   */
  static getSettings(options: IRequestOptions = {}): Promise<any> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/LaborUnion/Client/GetSettings';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);

      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static init(options: IRequestOptions = {}): Promise<any> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/Client/Init';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);

      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getJssdk(
    params: {
      /**  */
      url?: string;
      /**  */
      appName?: string;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<JssdkResultDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/Client/GetJssdk';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = { url: params['url'], appName: params['appName'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static oAuth2(
    params: {
      /**  */
      code?: string;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<any> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/Client/OAuth2';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = { code: params['code'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static miniCode2Session(
    params: {
      /**  */
      code?: string;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<any> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/Client/MiniCode2Session';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = { code: params['code'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getMyInfo(options: IRequestOptions = {}): Promise<any> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/Client/GetMyInfo';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);

      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getQr(
    params: {
      /**  */
      code?: string;
      /**  */
      appName?: string;
      /**  */
      page?: string;
      /**  */
      width?: number;
      /**  */
      useCache?: boolean;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<string> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/Client/GetQr';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = {
        code: params['code'],
        appName: params['appName'],
        page: params['page'],
        width: params['width'],
        useCache: params['useCache']
      };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getSignature(
    params: {
      /**  */
      data?: string;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<any> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/Client/GetSignature';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = { data: params['data'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getPlaceSuggestion(
    params: {
      /**  */
      query?: string;
      /**  */
      region?: string;
      /**  */
      type?: string;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<Place[]> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/Client/GetPlaceSuggestion';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = { query: params['query'], region: params['region'], type: params['type'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
}

export class CmsCategoryService {
  /**
   *
   */
  static getAll(
    params: {
      /**  */
      organizationUnitId?: number;
      /**  */
      status?: number;
      /**  */
      userId?: number;
      /**  */
      pid?: number;
      /**  */
      keyword?: string;
      /**  */
      isActive?: boolean;
      /**  */
      sorting?: string;
      /**  */
      from?: string;
      /**  */
      to?: string;
      /**  */
      skipCount?: number;
      /**  */
      maxResultCount?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<CmsCategoryDtoPagedResultDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/CmsCategory/GetAll';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = {
        OrganizationUnitId: params['organizationUnitId'],
        Status: params['status'],
        UserId: params['userId'],
        Pid: params['pid'],
        Keyword: params['keyword'],
        IsActive: params['isActive'],
        Sorting: params['sorting'],
        From: params['from'],
        To: params['to'],
        SkipCount: params['skipCount'],
        MaxResultCount: params['maxResultCount']
      };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getForEdit(
    params: {
      /**  */
      id?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<CmsCategoryCreateOrUpdateDtoGetForEditOutput> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/CmsCategory/GetForEdit';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static update(
    params: {
      /** requestBody */
      body?: CmsCategoryCreateOrUpdateDto;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<CmsCategoryDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/CmsCategory/Update';

      const configs: IRequestConfig = getConfigs('put', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static get(
    params: {
      /**  */
      id?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<CmsCategoryDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/CmsCategory/Get';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static create(
    params: {
      /** requestBody */
      body?: CmsCategoryCreateOrUpdateDto;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<CmsCategoryDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/CmsCategory/Create';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static delete(
    params: {
      /**  */
      id?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<any> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/CmsCategory/Delete';

      const configs: IRequestConfig = getConfigs('delete', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
}

export class CmsContentService {
  /**
   *
   */
  static getForEdit(
    params: {
      /**  */
      id?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<CmsContentCreateOrUpdateDtoGetForEditOutput> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/CmsContent/GetForEdit';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getAll(
    params: {
      /**  */
      organizationUnitId?: number;
      /**  */
      status?: number;
      /**  */
      userId?: number;
      /**  */
      pid?: number;
      /**  */
      keyword?: string;
      /**  */
      isActive?: boolean;
      /**  */
      sorting?: string;
      /**  */
      from?: string;
      /**  */
      to?: string;
      /**  */
      skipCount?: number;
      /**  */
      maxResultCount?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<CmsContentDtoPagedResultDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/CmsContent/GetAll';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = {
        OrganizationUnitId: params['organizationUnitId'],
        Status: params['status'],
        UserId: params['userId'],
        Pid: params['pid'],
        Keyword: params['keyword'],
        IsActive: params['isActive'],
        Sorting: params['sorting'],
        From: params['from'],
        To: params['to'],
        SkipCount: params['skipCount'],
        MaxResultCount: params['maxResultCount']
      };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getAllPublish(
    params: {
      /**  */
      organizationUnitId?: number;
      /**  */
      status?: number;
      /**  */
      userId?: number;
      /**  */
      pid?: number;
      /**  */
      keyword?: string;
      /**  */
      isActive?: boolean;
      /**  */
      sorting?: string;
      /**  */
      from?: string;
      /**  */
      to?: string;
      /**  */
      skipCount?: number;
      /**  */
      maxResultCount?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<CmsContentDtoPagedResultDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/CmsContent/GetAllPublish';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = {
        OrganizationUnitId: params['organizationUnitId'],
        Status: params['status'],
        UserId: params['userId'],
        Pid: params['pid'],
        Keyword: params['keyword'],
        IsActive: params['isActive'],
        Sorting: params['sorting'],
        From: params['from'],
        To: params['to'],
        SkipCount: params['skipCount'],
        MaxResultCount: params['maxResultCount']
      };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static delete(
    params: {
      /**  */
      id?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<any> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/CmsContent/Delete';

      const configs: IRequestConfig = getConfigs('delete', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   * 获取新闻单条记录
   */
  static get(
    params: {
      /**  */
      id?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<CmsContentDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/CmsContent/Get';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   * 收藏/分享/点赞
   */
  static postEvent(
    params: {
      /** requestBody */
      body?: Int32EntityEventDto;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<any> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/CmsContent/PostEvent';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static create(
    params: {
      /** requestBody */
      body?: CmsContentCreateOrUpdateDto;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<CmsContentDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/CmsContent/Create';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static update(
    params: {
      /** requestBody */
      body?: CmsContentCreateOrUpdateDto;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<CmsContentDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/CmsContent/Update';

      const configs: IRequestConfig = getConfigs('put', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   * 发布文章
   */
  static publish(
    params: {
      /** requestBody */
      body?: Int32EntityDto;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<any> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/CmsContent/Publish';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   * 取消发布
   */
  static cancelPublish(
    params: {
      /** requestBody */
      body?: Int32EntityDto;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<any> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/CmsContent/CancelPublish';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
}

export class ConfigurationService {
  /**
   *
   */
  static changeUiTheme(
    params: {
      /** requestBody */
      body?: ChangeUiThemeInput;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<any> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/Configuration/ChangeUiTheme';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
}

export class CraftsmanService {
  /**
   *
   */
  static getForEdit(
    params: {
      /**  */
      id?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<CraftsmanCreateOrUpdateDtoGetForEditOutput> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/LaborUnion/Craftsman/GetForEdit';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static create(
    params: {
      /** requestBody */
      body?: CraftsmanCreateOrUpdateDto;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<CraftsmanDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/LaborUnion/Craftsman/Create';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getRedpacket(
    params: {
      /**  */
      id?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<CraftsmanDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/LaborUnion/Craftsman/GetRedpacket';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static exportExcel(
    params: {
      /** requestBody */
      body?: AppResultRequestDto;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<string> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/LaborUnion/Craftsman/ExportExcel';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static dateAnlayse(
    params: {
      /**  */
      organizationUnitId?: number;
      /**  */
      status?: number;
      /**  */
      userId?: number;
      /**  */
      pid?: number;
      /**  */
      keyword?: string;
      /**  */
      isActive?: boolean;
      /**  */
      sorting?: string;
      /**  */
      from?: string;
      /**  */
      to?: string;
      /**  */
      skipCount?: number;
      /**  */
      maxResultCount?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<any> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/LaborUnion/Craftsman/DateAnlayse';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = {
        OrganizationUnitId: params['organizationUnitId'],
        Status: params['status'],
        UserId: params['userId'],
        Pid: params['pid'],
        Keyword: params['keyword'],
        IsActive: params['isActive'],
        Sorting: params['sorting'],
        From: params['from'],
        To: params['to'],
        SkipCount: params['skipCount'],
        MaxResultCount: params['maxResultCount']
      };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static update(
    params: {
      /** requestBody */
      body?: CraftsmanCreateOrUpdateDto;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<CraftsmanDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/LaborUnion/Craftsman/Update';

      const configs: IRequestConfig = getConfigs('put', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static startAudit(
    params: {
      /** requestBody */
      body?: Int64EntityDto;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<any> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/LaborUnion/Craftsman/StartAudit';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static delete(
    params: {
      /**  */
      id?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<any> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/LaborUnion/Craftsman/Delete';

      const configs: IRequestConfig = getConfigs('delete', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static audit(
    params: {
      /** requestBody */
      body?: AuditUserLog;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<any> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/LaborUnion/Craftsman/Audit';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getMyAll(
    params: {
      /**  */
      organizationUnitId?: number;
      /**  */
      status?: number;
      /**  */
      userId?: number;
      /**  */
      pid?: number;
      /**  */
      keyword?: string;
      /**  */
      isActive?: boolean;
      /**  */
      sorting?: string;
      /**  */
      from?: string;
      /**  */
      to?: string;
      /**  */
      skipCount?: number;
      /**  */
      maxResultCount?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<CraftsmanDtoPagedResultDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/LaborUnion/Craftsman/GetMyAll';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = {
        OrganizationUnitId: params['organizationUnitId'],
        Status: params['status'],
        UserId: params['userId'],
        Pid: params['pid'],
        Keyword: params['keyword'],
        IsActive: params['isActive'],
        Sorting: params['sorting'],
        From: params['from'],
        To: params['to'],
        SkipCount: params['skipCount'],
        MaxResultCount: params['maxResultCount']
      };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getLogs(
    params: {
      /**  */
      id?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<AuditUserLogDto[]> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/LaborUnion/Craftsman/GetLogs';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static get(
    params: {
      /**  */
      id?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<CraftsmanDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/LaborUnion/Craftsman/Get';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getAll(
    params: {
      /**  */
      organizationUnitId?: number;
      /**  */
      status?: number;
      /**  */
      userId?: number;
      /**  */
      pid?: number;
      /**  */
      keyword?: string;
      /**  */
      isActive?: boolean;
      /**  */
      sorting?: string;
      /**  */
      from?: string;
      /**  */
      to?: string;
      /**  */
      skipCount?: number;
      /**  */
      maxResultCount?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<CraftsmanDtoPagedResultDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/LaborUnion/Craftsman/GetAll';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = {
        OrganizationUnitId: params['organizationUnitId'],
        Status: params['status'],
        UserId: params['userId'],
        Pid: params['pid'],
        Keyword: params['keyword'],
        IsActive: params['isActive'],
        Sorting: params['sorting'],
        From: params['from'],
        To: params['to'],
        SkipCount: params['skipCount'],
        MaxResultCount: params['maxResultCount']
      };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
}

export class CraftsmanRecommendService {
  /**
   *
   */
  static get(
    params: {
      /**  */
      id?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<CraftsmanRecommendDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/LaborUnion/CraftsmanRecommend/Get';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getRedpacket(
    params: {
      /**  */
      id?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<CraftsmanRecommendDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/LaborUnion/CraftsmanRecommend/GetRedpacket';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getAllMy(
    params: {
      /**  */
      organizationUnitId?: number;
      /**  */
      status?: number;
      /**  */
      userId?: number;
      /**  */
      pid?: number;
      /**  */
      keyword?: string;
      /**  */
      isActive?: boolean;
      /**  */
      sorting?: string;
      /**  */
      from?: string;
      /**  */
      to?: string;
      /**  */
      skipCount?: number;
      /**  */
      maxResultCount?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<CraftsmanRecommendDtoPagedResultDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/LaborUnion/CraftsmanRecommend/GetAllMy';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = {
        OrganizationUnitId: params['organizationUnitId'],
        Status: params['status'],
        UserId: params['userId'],
        Pid: params['pid'],
        Keyword: params['keyword'],
        IsActive: params['isActive'],
        Sorting: params['sorting'],
        From: params['from'],
        To: params['to'],
        SkipCount: params['skipCount'],
        MaxResultCount: params['maxResultCount']
      };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static create(
    params: {
      /** requestBody */
      body?: CraftsmanRecommendCreateOrUpdateDto;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<CraftsmanRecommendDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/LaborUnion/CraftsmanRecommend/Create';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static update(
    params: {
      /** requestBody */
      body?: CraftsmanRecommendCreateOrUpdateDto;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<CraftsmanRecommendDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/LaborUnion/CraftsmanRecommend/Update';

      const configs: IRequestConfig = getConfigs('put', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static exportExcel(
    params: {
      /** requestBody */
      body?: AppResultRequestDto;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<string> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/LaborUnion/CraftsmanRecommend/ExportExcel';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static dateAnlayse(
    params: {
      /**  */
      organizationUnitId?: number;
      /**  */
      status?: number;
      /**  */
      userId?: number;
      /**  */
      pid?: number;
      /**  */
      keyword?: string;
      /**  */
      isActive?: boolean;
      /**  */
      sorting?: string;
      /**  */
      from?: string;
      /**  */
      to?: string;
      /**  */
      skipCount?: number;
      /**  */
      maxResultCount?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<any> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/LaborUnion/CraftsmanRecommend/DateAnlayse';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = {
        OrganizationUnitId: params['organizationUnitId'],
        Status: params['status'],
        UserId: params['userId'],
        Pid: params['pid'],
        Keyword: params['keyword'],
        IsActive: params['isActive'],
        Sorting: params['sorting'],
        From: params['from'],
        To: params['to'],
        SkipCount: params['skipCount'],
        MaxResultCount: params['maxResultCount']
      };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static startAudit(
    params: {
      /** requestBody */
      body?: Int64EntityDto;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<any> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/LaborUnion/CraftsmanRecommend/StartAudit';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static delete(
    params: {
      /**  */
      id?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<any> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/LaborUnion/CraftsmanRecommend/Delete';

      const configs: IRequestConfig = getConfigs('delete', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static audit(
    params: {
      /** requestBody */
      body?: AuditUserLog;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<any> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/LaborUnion/CraftsmanRecommend/Audit';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getMyAll(
    params: {
      /**  */
      organizationUnitId?: number;
      /**  */
      status?: number;
      /**  */
      userId?: number;
      /**  */
      pid?: number;
      /**  */
      keyword?: string;
      /**  */
      isActive?: boolean;
      /**  */
      sorting?: string;
      /**  */
      from?: string;
      /**  */
      to?: string;
      /**  */
      skipCount?: number;
      /**  */
      maxResultCount?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<CraftsmanRecommendDtoPagedResultDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/LaborUnion/CraftsmanRecommend/GetMyAll';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = {
        OrganizationUnitId: params['organizationUnitId'],
        Status: params['status'],
        UserId: params['userId'],
        Pid: params['pid'],
        Keyword: params['keyword'],
        IsActive: params['isActive'],
        Sorting: params['sorting'],
        From: params['from'],
        To: params['to'],
        SkipCount: params['skipCount'],
        MaxResultCount: params['maxResultCount']
      };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getLogs(
    params: {
      /**  */
      id?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<AuditUserLogDto[]> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/LaborUnion/CraftsmanRecommend/GetLogs';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getForEdit(
    params: {
      /**  */
      id?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<CraftsmanRecommendCreateOrUpdateDtoGetForEditOutput> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/LaborUnion/CraftsmanRecommend/GetForEdit';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getAll(
    params: {
      /**  */
      organizationUnitId?: number;
      /**  */
      status?: number;
      /**  */
      userId?: number;
      /**  */
      pid?: number;
      /**  */
      keyword?: string;
      /**  */
      isActive?: boolean;
      /**  */
      sorting?: string;
      /**  */
      from?: string;
      /**  */
      to?: string;
      /**  */
      skipCount?: number;
      /**  */
      maxResultCount?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<CraftsmanRecommendDtoPagedResultDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/LaborUnion/CraftsmanRecommend/GetAll';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = {
        OrganizationUnitId: params['organizationUnitId'],
        Status: params['status'],
        UserId: params['userId'],
        Pid: params['pid'],
        Keyword: params['keyword'],
        IsActive: params['isActive'],
        Sorting: params['sorting'],
        From: params['from'],
        To: params['to'],
        SkipCount: params['skipCount'],
        MaxResultCount: params['maxResultCount']
      };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
}

export class DashboardService {
  /**
   *
   */
  static getDashboard(options: IRequestOptions = {}): Promise<any> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/LaborUnion/Dashboard/GetDashboard';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);

      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
}

export class HostSettingsService {
  /**
   *
   */
  static getAllSettings(options: IRequestOptions = {}): Promise<HostSettingsEditDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/HostSettings/GetAllSettings';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);

      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static updateAllSettings(
    params: {
      /** requestBody */
      body?: HostSettingsEditDto;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<any> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/HostSettings/UpdateAllSettings';

      const configs: IRequestConfig = getConfigs('put', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
}

export class LuckDrawService {
  /**
   *
   */
  static luckDraw(
    params: {
      /**  */
      id?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<UserPrizeDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/Activity/LuckDraw/LuckDraw';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = { id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static get(
    params: {
      /**  */
      id?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<LuckDrawDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/Activity/LuckDraw/Get';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static delete(
    params: {
      /**  */
      id?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<any> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/Activity/LuckDraw/Delete';

      const configs: IRequestConfig = getConfigs('delete', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getForEdit(
    params: {
      /**  */
      id?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<LuckDrawCreateOrUpdateDtoGetForEditOutput> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/Activity/LuckDraw/GetForEdit';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static update(
    params: {
      /** requestBody */
      body?: LuckDrawCreateOrUpdateDto;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<LuckDrawDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/Activity/LuckDraw/Update';

      const configs: IRequestConfig = getConfigs('put', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getAll(
    params: {
      /**  */
      organizationUnitId?: number;
      /**  */
      status?: number;
      /**  */
      userId?: number;
      /**  */
      pid?: number;
      /**  */
      keyword?: string;
      /**  */
      isActive?: boolean;
      /**  */
      sorting?: string;
      /**  */
      from?: string;
      /**  */
      to?: string;
      /**  */
      skipCount?: number;
      /**  */
      maxResultCount?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<LuckDrawDtoPagedResultDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/Activity/LuckDraw/GetAll';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = {
        OrganizationUnitId: params['organizationUnitId'],
        Status: params['status'],
        UserId: params['userId'],
        Pid: params['pid'],
        Keyword: params['keyword'],
        IsActive: params['isActive'],
        Sorting: params['sorting'],
        From: params['from'],
        To: params['to'],
        SkipCount: params['skipCount'],
        MaxResultCount: params['maxResultCount']
      };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static create(
    params: {
      /** requestBody */
      body?: LuckDrawCreateOrUpdateDto;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<LuckDrawDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/Activity/LuckDraw/Create';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
}

export class LuckDrawPrizeService {
  /**
   *
   */
  static getAll(
    params: {
      /**  */
      organizationUnitId?: number;
      /**  */
      status?: number;
      /**  */
      userId?: number;
      /**  */
      pid?: number;
      /**  */
      keyword?: string;
      /**  */
      isActive?: boolean;
      /**  */
      sorting?: string;
      /**  */
      from?: string;
      /**  */
      to?: string;
      /**  */
      skipCount?: number;
      /**  */
      maxResultCount?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<LuckDrawPrizeDtoPagedResultDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/Activity/LuckDrawPrize/GetAll';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = {
        OrganizationUnitId: params['organizationUnitId'],
        Status: params['status'],
        UserId: params['userId'],
        Pid: params['pid'],
        Keyword: params['keyword'],
        IsActive: params['isActive'],
        Sorting: params['sorting'],
        From: params['from'],
        To: params['to'],
        SkipCount: params['skipCount'],
        MaxResultCount: params['maxResultCount']
      };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getForEdit(
    params: {
      /**  */
      id?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<LuckDrawPrizeCreateOrUpdateDtoGetForEditOutput> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/Activity/LuckDrawPrize/GetForEdit';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static sendMessage(
    params: {
      /** requestBody */
      body?: LuckDrawPrizeMessageInput;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<any> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/Activity/LuckDrawPrize/SendMessage';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static update(
    params: {
      /** requestBody */
      body?: LuckDrawPrizeCreateOrUpdateDto;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<LuckDrawPrizeDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/Activity/LuckDrawPrize/Update';

      const configs: IRequestConfig = getConfigs('put', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static get(
    params: {
      /**  */
      id?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<LuckDrawPrizeDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/Activity/LuckDrawPrize/Get';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static create(
    params: {
      /** requestBody */
      body?: LuckDrawPrizeCreateOrUpdateDto;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<LuckDrawPrizeDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/Activity/LuckDrawPrize/Create';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static delete(
    params: {
      /**  */
      id?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<any> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/Activity/LuckDrawPrize/Delete';

      const configs: IRequestConfig = getConfigs('delete', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
}

export class OrganizationApplyService {
  /**
   *
   */
  static getAll(
    params: {
      /**  */
      organizationUnitId?: number;
      /**  */
      status?: number;
      /**  */
      userId?: number;
      /**  */
      pid?: number;
      /**  */
      keyword?: string;
      /**  */
      isActive?: boolean;
      /**  */
      sorting?: string;
      /**  */
      from?: string;
      /**  */
      to?: string;
      /**  */
      skipCount?: number;
      /**  */
      maxResultCount?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<OrganizationApplyDtoPagedResultDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/OrganizationApply/GetAll';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = {
        OrganizationUnitId: params['organizationUnitId'],
        Status: params['status'],
        UserId: params['userId'],
        Pid: params['pid'],
        Keyword: params['keyword'],
        IsActive: params['isActive'],
        Sorting: params['sorting'],
        From: params['from'],
        To: params['to'],
        SkipCount: params['skipCount'],
        MaxResultCount: params['maxResultCount']
      };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static create(
    params: {
      /** requestBody */
      body?: CreatorOrUpdateOrganizationApplyDto;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<OrganizationApplyDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/OrganizationApply/Create';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static update(
    params: {
      /** requestBody */
      body?: CreatorOrUpdateOrganizationApplyDto;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<OrganizationApplyDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/OrganizationApply/Update';

      const configs: IRequestConfig = getConfigs('put', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getMyApplyList(options: IRequestOptions = {}): Promise<OrganizationApplyDto[]> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/OrganizationApply/GetMyApplyList';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);

      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static startAudit(
    params: {
      /** requestBody */
      body?: Int64EntityDto;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<any> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/OrganizationApply/StartAudit';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static delete(
    params: {
      /**  */
      id?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<any> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/OrganizationApply/Delete';

      const configs: IRequestConfig = getConfigs('delete', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static audit(
    params: {
      /** requestBody */
      body?: AuditUserLog;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<any> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/OrganizationApply/Audit';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getMyAll(
    params: {
      /**  */
      organizationUnitId?: number;
      /**  */
      status?: number;
      /**  */
      userId?: number;
      /**  */
      pid?: number;
      /**  */
      keyword?: string;
      /**  */
      isActive?: boolean;
      /**  */
      sorting?: string;
      /**  */
      from?: string;
      /**  */
      to?: string;
      /**  */
      skipCount?: number;
      /**  */
      maxResultCount?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<OrganizationApplyDtoPagedResultDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/OrganizationApply/GetMyAll';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = {
        OrganizationUnitId: params['organizationUnitId'],
        Status: params['status'],
        UserId: params['userId'],
        Pid: params['pid'],
        Keyword: params['keyword'],
        IsActive: params['isActive'],
        Sorting: params['sorting'],
        From: params['from'],
        To: params['to'],
        SkipCount: params['skipCount'],
        MaxResultCount: params['maxResultCount']
      };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getLogs(
    params: {
      /**  */
      id?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<AuditUserLogDto[]> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/OrganizationApply/GetLogs';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getForEdit(
    params: {
      /**  */
      id?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<CreatorOrUpdateOrganizationApplyDtoGetForEditOutput> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/OrganizationApply/GetForEdit';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static get(
    params: {
      /**  */
      id?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<OrganizationApplyDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/OrganizationApply/Get';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
}

export class OrganizationUnitService {
  /**
   *
   */
  static getOrganizationUnit(
    params: {
      /**  */
      id?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<OrganizationUnitDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/OrganizationUnit/GetOrganizationUnit';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getAllMinify(
    params: {
      /**  */
      organizationUnitId?: number;
      /**  */
      status?: number;
      /**  */
      userId?: number;
      /**  */
      pid?: number;
      /**  */
      keyword?: string;
      /**  */
      isActive?: boolean;
      /**  */
      sorting?: string;
      /**  */
      from?: string;
      /**  */
      to?: string;
      /**  */
      skipCount?: number;
      /**  */
      maxResultCount?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<OrganizationUnitDtoBaseListResultDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/OrganizationUnit/GetAllMinify';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = {
        OrganizationUnitId: params['organizationUnitId'],
        Status: params['status'],
        UserId: params['userId'],
        Pid: params['pid'],
        Keyword: params['keyword'],
        IsActive: params['isActive'],
        Sorting: params['sorting'],
        From: params['from'],
        To: params['to'],
        SkipCount: params['skipCount'],
        MaxResultCount: params['maxResultCount']
      };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getAllPublic(
    params: {
      /**  */
      organizationUnitId?: number;
      /**  */
      status?: number;
      /**  */
      userId?: number;
      /**  */
      pid?: number;
      /**  */
      keyword?: string;
      /**  */
      isActive?: boolean;
      /**  */
      sorting?: string;
      /**  */
      from?: string;
      /**  */
      to?: string;
      /**  */
      skipCount?: number;
      /**  */
      maxResultCount?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<ProjectNameOrganizationUnitDtoBasePagedResultDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/OrganizationUnit/GetAllPublic';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = {
        OrganizationUnitId: params['organizationUnitId'],
        Status: params['status'],
        UserId: params['userId'],
        Pid: params['pid'],
        Keyword: params['keyword'],
        IsActive: params['isActive'],
        Sorting: params['sorting'],
        From: params['from'],
        To: params['to'],
        SkipCount: params['skipCount'],
        MaxResultCount: params['maxResultCount']
      };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getPublic(
    params: {
      /**  */
      id?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<ProjectNameOrganizationUnitDtoBase> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/OrganizationUnit/GetPublic';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getAllOrganizationUnits(
    params: {
      /**  */
      organizationUnitId?: number;
      /**  */
      status?: number;
      /**  */
      userId?: number;
      /**  */
      pid?: number;
      /**  */
      keyword?: string;
      /**  */
      isActive?: boolean;
      /**  */
      sorting?: string;
      /**  */
      from?: string;
      /**  */
      to?: string;
      /**  */
      skipCount?: number;
      /**  */
      maxResultCount?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<OrganizationUnitDtoListResultDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/OrganizationUnit/GetAllOrganizationUnits';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = {
        OrganizationUnitId: params['organizationUnitId'],
        Status: params['status'],
        UserId: params['userId'],
        Pid: params['pid'],
        Keyword: params['keyword'],
        IsActive: params['isActive'],
        Sorting: params['sorting'],
        From: params['from'],
        To: params['to'],
        SkipCount: params['skipCount'],
        MaxResultCount: params['maxResultCount']
      };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getOrganizationUnits(
    params: {
      /**  */
      organizationUnitId?: number;
      /**  */
      status?: number;
      /**  */
      userId?: number;
      /**  */
      pid?: number;
      /**  */
      keyword?: string;
      /**  */
      isActive?: boolean;
      /**  */
      sorting?: string;
      /**  */
      from?: string;
      /**  */
      to?: string;
      /**  */
      skipCount?: number;
      /**  */
      maxResultCount?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<OrganizationUnitDtoListResultDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/OrganizationUnit/GetOrganizationUnits';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = {
        OrganizationUnitId: params['organizationUnitId'],
        Status: params['status'],
        UserId: params['userId'],
        Pid: params['pid'],
        Keyword: params['keyword'],
        IsActive: params['isActive'],
        Sorting: params['sorting'],
        From: params['from'],
        To: params['to'],
        SkipCount: params['skipCount'],
        MaxResultCount: params['maxResultCount']
      };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getCurrent(options: IRequestOptions = {}): Promise<OrganizationUnitDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/OrganizationUnit/GetCurrent';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);

      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getOrganizationUnitUsers(
    params: {
      /**  */
      id?: number;
      /**  */
      sorting?: string;
      /**  */
      maxResultCount?: number;
      /**  */
      skipCount?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<OrganizationUnitUserListDtoPagedResultDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/OrganizationUnit/GetOrganizationUnitUsers';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = {
        Id: params['id'],
        Sorting: params['sorting'],
        MaxResultCount: params['maxResultCount'],
        SkipCount: params['skipCount']
      };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static createOrganizationUnit(
    params: {
      /** requestBody */
      body?: CreateOrganizationUnitInput;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<OrganizationUnitDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/OrganizationUnit/CreateOrganizationUnit';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getForEdit(
    params: {
      /**  */
      id?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<CreateOrganizationUnitInputGetForEditOutput> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/OrganizationUnit/GetForEdit';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   * 更新机构信息
   */
  static updateOrganizationUnit(
    params: {
      /** requestBody */
      body?: UpdateOrganizationUnitInput;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<OrganizationUnitDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/OrganizationUnit/UpdateOrganizationUnit';

      const configs: IRequestConfig = getConfigs('put', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static moveOrganizationUnit(
    params: {
      /** requestBody */
      body?: MoveOrganizationUnitInput;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<OrganizationUnitDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/OrganizationUnit/MoveOrganizationUnit';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static deleteOrganizationUnit(
    params: {
      /**  */
      id?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<any> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/OrganizationUnit/DeleteOrganizationUnit';

      const configs: IRequestConfig = getConfigs('delete', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static removeUsersFromOrganizationUnit(
    params: {
      /** requestBody */
      body?: UsersToOrganizationUnitInput;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<any> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/OrganizationUnit/RemoveUsersFromOrganizationUnit';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static removeUserFromOrganizationUnit(
    params: {
      /**  */
      userId?: number;
      /**  */
      organizationUnitId?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<any> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/OrganizationUnit/RemoveUserFromOrganizationUnit';

      const configs: IRequestConfig = getConfigs('delete', 'application/json', url, options);
      configs.params = { UserId: params['userId'], OrganizationUnitId: params['organizationUnitId'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static addUsersToOrganizationUnit(
    params: {
      /** requestBody */
      body?: UsersToOrganizationUnitInput;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<any> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/OrganizationUnit/AddUsersToOrganizationUnit';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static findUsers(
    params: {
      /** requestBody */
      body?: FindOrganizationUnitUsersInput;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<NameValueDtoPagedResultDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/OrganizationUnit/FindUsers';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static postEvent(
    params: {
      /** requestBody */
      body?: Int32EntityEventDto;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<string> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/OrganizationUnit/PostEvent';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
}

export class PayService {
  /**
   * JS-SDK支付回调地址（在统一下单接口中设置notify_url）
   */
  static payNotifyUrl(
    params: {
      /** requestBody */
      body?: TenPayNotifyXml;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<any> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/Pay/PayNotifyUrl';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
}

export class PayOrderService {
  /**
   *
   */
  static getAll(
    params: {
      /**  */
      organizationUnitId?: number;
      /**  */
      status?: number;
      /**  */
      userId?: number;
      /**  */
      pid?: number;
      /**  */
      keyword?: string;
      /**  */
      isActive?: boolean;
      /**  */
      sorting?: string;
      /**  */
      from?: string;
      /**  */
      to?: string;
      /**  */
      skipCount?: number;
      /**  */
      maxResultCount?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<PayOrderDtoPagedResultDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/PayOrder/GetAll';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = {
        OrganizationUnitId: params['organizationUnitId'],
        Status: params['status'],
        UserId: params['userId'],
        Pid: params['pid'],
        Keyword: params['keyword'],
        IsActive: params['isActive'],
        Sorting: params['sorting'],
        From: params['from'],
        To: params['to'],
        SkipCount: params['skipCount'],
        MaxResultCount: params['maxResultCount']
      };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getAllRefundOrders(
    params: {
      /**  */
      organizationUnitId?: number;
      /**  */
      status?: number;
      /**  */
      userId?: number;
      /**  */
      pid?: number;
      /**  */
      keyword?: string;
      /**  */
      isActive?: boolean;
      /**  */
      sorting?: string;
      /**  */
      from?: string;
      /**  */
      to?: string;
      /**  */
      skipCount?: number;
      /**  */
      maxResultCount?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<RefundLogPagedResultDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/PayOrder/GetAllRefundOrders';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = {
        OrganizationUnitId: params['organizationUnitId'],
        Status: params['status'],
        UserId: params['userId'],
        Pid: params['pid'],
        Keyword: params['keyword'],
        IsActive: params['isActive'],
        Sorting: params['sorting'],
        From: params['from'],
        To: params['to'],
        SkipCount: params['skipCount'],
        MaxResultCount: params['maxResultCount']
      };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getRefundDetail(
    params: {
      /**  */
      billNo?: string;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<RefundDetailDto[]> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/PayOrder/GetRefundDetail';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = { billNo: params['billNo'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
}

export class PointActivityService {
  /**
   *
   */
  static postGetPoint(
    params: {
      /** requestBody */
      body?: GetPointRequestDto;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<PointActivityUserLogDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/Activity/PointActivity/PostGetPoint';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static delete(
    params: {
      /**  */
      id?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<any> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/Activity/PointActivity/Delete';

      const configs: IRequestConfig = getConfigs('delete', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getForEdit(
    params: {
      /**  */
      id?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<PointActivityCreateOrUpdateDtoGetForEditOutput> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/Activity/PointActivity/GetForEdit';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static update(
    params: {
      /** requestBody */
      body?: PointActivityCreateOrUpdateDto;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<PointActivityDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/Activity/PointActivity/Update';

      const configs: IRequestConfig = getConfigs('put', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static get(
    params: {
      /**  */
      id?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<PointActivityDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/Activity/PointActivity/Get';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getAll(
    params: {
      /**  */
      organizationUnitId?: number;
      /**  */
      status?: number;
      /**  */
      userId?: number;
      /**  */
      pid?: number;
      /**  */
      keyword?: string;
      /**  */
      isActive?: boolean;
      /**  */
      sorting?: string;
      /**  */
      from?: string;
      /**  */
      to?: string;
      /**  */
      skipCount?: number;
      /**  */
      maxResultCount?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<PointActivityDtoPagedResultDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/Activity/PointActivity/GetAll';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = {
        OrganizationUnitId: params['organizationUnitId'],
        Status: params['status'],
        UserId: params['userId'],
        Pid: params['pid'],
        Keyword: params['keyword'],
        IsActive: params['isActive'],
        Sorting: params['sorting'],
        From: params['from'],
        To: params['to'],
        SkipCount: params['skipCount'],
        MaxResultCount: params['maxResultCount']
      };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static create(
    params: {
      /** requestBody */
      body?: PointActivityCreateOrUpdateDto;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<PointActivityDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/Activity/PointActivity/Create';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
}

export class PosterService {
  /**
   *
   */
  static getForEdit(
    params: {
      /**  */
      id?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<PosterDtoGetForEditOutput> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/Poster/GetForEdit';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getPosterImage(
    params: {
      /**  */
      id?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<string> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/Poster/GetPosterImage';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static update(
    params: {
      /** requestBody */
      body?: PosterDto;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<PosterDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/Poster/Update';

      const configs: IRequestConfig = getConfigs('put', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static get(
    params: {
      /**  */
      id?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<PosterDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/Poster/Get';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getAll(
    params: {
      /**  */
      organizationUnitId?: number;
      /**  */
      status?: number;
      /**  */
      userId?: number;
      /**  */
      pid?: number;
      /**  */
      keyword?: string;
      /**  */
      isActive?: boolean;
      /**  */
      sorting?: string;
      /**  */
      from?: string;
      /**  */
      to?: string;
      /**  */
      skipCount?: number;
      /**  */
      maxResultCount?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<PosterDtoPagedResultDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/Poster/GetAll';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = {
        OrganizationUnitId: params['organizationUnitId'],
        Status: params['status'],
        UserId: params['userId'],
        Pid: params['pid'],
        Keyword: params['keyword'],
        IsActive: params['isActive'],
        Sorting: params['sorting'],
        From: params['from'],
        To: params['to'],
        SkipCount: params['skipCount'],
        MaxResultCount: params['maxResultCount']
      };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static create(
    params: {
      /** requestBody */
      body?: PosterDto;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<PosterDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/Poster/Create';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static delete(
    params: {
      /**  */
      id?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<any> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/Poster/Delete';

      const configs: IRequestConfig = getConfigs('delete', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
}

export class QaPlanService {
  /**
   *
   */
  static get(
    params: {
      /**  */
      id?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<QAPlanDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/QA/QAPlan/Get';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getForEdit(
    params: {
      /**  */
      id?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<QAPlanCreateOrUpdateDtoGetForEditOutput> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/QA/QAPlan/GetForEdit';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static checkUserInfo(options: IRequestOptions = {}): Promise<any> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/QA/QAPlan/CheckUserInfo';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);

      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static postUserInfo(
    params: {
      /** requestBody */
      body?: QAUserRequestDto;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<any> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/QA/QAPlan/PostUserInfo';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static update(
    params: {
      /** requestBody */
      body?: QAPlanCreateOrUpdateDto;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<QAPlanDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/QA/QAPlan/Update';

      const configs: IRequestConfig = getConfigs('put', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getAll(
    params: {
      /**  */
      organizationUnitId?: number;
      /**  */
      status?: number;
      /**  */
      userId?: number;
      /**  */
      pid?: number;
      /**  */
      keyword?: string;
      /**  */
      isActive?: boolean;
      /**  */
      sorting?: string;
      /**  */
      from?: string;
      /**  */
      to?: string;
      /**  */
      skipCount?: number;
      /**  */
      maxResultCount?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<QAPlanDtoPagedResultDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/QA/QAPlan/GetAll';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = {
        OrganizationUnitId: params['organizationUnitId'],
        Status: params['status'],
        UserId: params['userId'],
        Pid: params['pid'],
        Keyword: params['keyword'],
        IsActive: params['isActive'],
        Sorting: params['sorting'],
        From: params['from'],
        To: params['to'],
        SkipCount: params['skipCount'],
        MaxResultCount: params['maxResultCount']
      };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static create(
    params: {
      /** requestBody */
      body?: QAPlanCreateOrUpdateDto;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<QAPlanDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/QA/QAPlan/Create';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static delete(
    params: {
      /**  */
      id?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<any> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/QA/QAPlan/Delete';

      const configs: IRequestConfig = getConfigs('delete', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
}

export class QaQuestionService {
  /**
   *
   */
  static getForEdit(
    params: {
      /**  */
      id?: string;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<QAQuestionCreateOrUpdateDtoGetForEditOutput> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/QA/QAQuestion/GetForEdit';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getAll(
    params: {
      /**  */
      organizationUnitId?: number;
      /**  */
      status?: number;
      /**  */
      userId?: number;
      /**  */
      pid?: number;
      /**  */
      keyword?: string;
      /**  */
      isActive?: boolean;
      /**  */
      sorting?: string;
      /**  */
      from?: string;
      /**  */
      to?: string;
      /**  */
      skipCount?: number;
      /**  */
      maxResultCount?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<QAQuestionDtoPagedResultDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/QA/QAQuestion/GetAll';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = {
        OrganizationUnitId: params['organizationUnitId'],
        Status: params['status'],
        UserId: params['userId'],
        Pid: params['pid'],
        Keyword: params['keyword'],
        IsActive: params['isActive'],
        Sorting: params['sorting'],
        From: params['from'],
        To: params['to'],
        SkipCount: params['skipCount'],
        MaxResultCount: params['maxResultCount']
      };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static create(
    params: {
      /** requestBody */
      body?: QAQuestionCreateOrUpdateDto;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<QAQuestionDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/QA/QAQuestion/Create';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static update(
    params: {
      /** requestBody */
      body?: QAQuestionCreateOrUpdateDto;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<QAQuestionDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/QA/QAQuestion/Update';

      const configs: IRequestConfig = getConfigs('put', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static get(
    params: {
      /**  */
      id?: string;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<QAQuestionDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/QA/QAQuestion/Get';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static delete(
    params: {
      /**  */
      id?: string;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<any> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/QA/QAQuestion/Delete';

      const configs: IRequestConfig = getConfigs('delete', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
}

export class RefundLogService {
  /**
   *
   */
  static getAll(
    params: {
      /**  */
      sorting?: string;
      /** 订单号 */
      billNo?: string;
      /** 退款金额（元） */
      price?: number;
      /** 审核状态 */
      auditStatus?: number;
      /** 退款状态 */
      isSuccess?: boolean;
      /**  */
      creationTime?: string;
      /**  */
      skipCount?: number;
      /**  */
      maxResultCount?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<RefundLogDtoPagedResultDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/RefundLog/GetAll';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = {
        Sorting: params['sorting'],
        BillNo: params['billNo'],
        Price: params['price'],
        AuditStatus: params['auditStatus'],
        IsSuccess: params['isSuccess'],
        CreationTime: params['creationTime'],
        SkipCount: params['skipCount'],
        MaxResultCount: params['maxResultCount']
      };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getRefundDetail(
    params: {
      /**  */
      billNo?: string;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<RefundDetailDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/RefundLog/GetRefundDetail';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = { billNo: params['billNo'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static create(
    params: {
      /** requestBody */
      body?: RefundLogDto;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<RefundLogDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/RefundLog/Create';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static update(
    params: {
      /** requestBody */
      body?: RefundLogDto;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<RefundLogDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/RefundLog/Update';

      const configs: IRequestConfig = getConfigs('put', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static startAudit(
    params: {
      /** requestBody */
      body?: Int64EntityDto;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<any> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/RefundLog/StartAudit';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static delete(
    params: {
      /**  */
      id?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<any> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/RefundLog/Delete';

      const configs: IRequestConfig = getConfigs('delete', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static audit(
    params: {
      /** requestBody */
      body?: AuditUserLog;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<any> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/RefundLog/Audit';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getMyAll(
    params: {
      /**  */
      sorting?: string;
      /** 订单号 */
      billNo?: string;
      /** 退款金额（元） */
      price?: number;
      /** 审核状态 */
      auditStatus?: number;
      /** 退款状态 */
      isSuccess?: boolean;
      /**  */
      creationTime?: string;
      /**  */
      skipCount?: number;
      /**  */
      maxResultCount?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<RefundLogDtoPagedResultDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/RefundLog/GetMyAll';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = {
        Sorting: params['sorting'],
        BillNo: params['billNo'],
        Price: params['price'],
        AuditStatus: params['auditStatus'],
        IsSuccess: params['isSuccess'],
        CreationTime: params['creationTime'],
        SkipCount: params['skipCount'],
        MaxResultCount: params['maxResultCount']
      };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getLogs(
    params: {
      /**  */
      id?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<AuditUserLogDto[]> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/RefundLog/GetLogs';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getForEdit(
    params: {
      /**  */
      id?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<RefundLogDtoGetForEditOutput> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/RefundLog/GetForEdit';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static get(
    params: {
      /**  */
      id?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<RefundLogDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/RefundLog/Get';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
}

export class RoleService {
  /**
   *
   */
  static create(
    params: {
      /** requestBody */
      body?: CreateRoleDto;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<RoleDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/Role/Create';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getRoles(
    params: {
      /**  */
      permission?: string;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<RoleListDtoListResultDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/Role/GetRoles';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = { Permission: params['permission'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static update(
    params: {
      /** requestBody */
      body?: RoleDto;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<RoleDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/Role/Update';

      const configs: IRequestConfig = getConfigs('put', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static delete(
    params: {
      /**  */
      id?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<any> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/Role/Delete';

      const configs: IRequestConfig = getConfigs('delete', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getAllPermissions(options: IRequestOptions = {}): Promise<PermissionDtoListResultDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/Role/GetAllPermissions';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);

      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getRoleForEdit(
    params: {
      /**  */
      id?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<GetRoleForEditOutput> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/Role/GetRoleForEdit';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static get(
    params: {
      /**  */
      id?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<RoleDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/Role/Get';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getAll(
    params: {
      /**  */
      keyword?: string;
      /**  */
      skipCount?: number;
      /**  */
      maxResultCount?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<RoleDtoPagedResultDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/Role/GetAll';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = {
        Keyword: params['keyword'],
        SkipCount: params['skipCount'],
        MaxResultCount: params['maxResultCount']
      };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
}

export class SessionService {
  /**
   *
   */
  static getCurrentLoginInformations(options: IRequestOptions = {}): Promise<GetCurrentLoginInformationsOutput> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/Session/GetCurrentLoginInformations';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);

      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
}

export class StsService {
  /**
   *
   */
  static get(options: IRequestOptions = {}): Promise<StsResultDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/Sts/Get';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);

      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
}

export class SwiperService {
  /**
   *
   */
  static getByGroupId(
    params: {
      /**  */
      id?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<SwiperDto[]> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/Swiper/GetByGroupId';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getForEdit(
    params: {
      /**  */
      id?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<SwiperDtoGetForEditOutput> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/Swiper/GetForEdit';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getAll(
    params: {
      /**  */
      organizationUnitId?: number;
      /**  */
      status?: number;
      /**  */
      userId?: number;
      /**  */
      pid?: number;
      /**  */
      keyword?: string;
      /**  */
      isActive?: boolean;
      /**  */
      sorting?: string;
      /**  */
      from?: string;
      /**  */
      to?: string;
      /**  */
      skipCount?: number;
      /**  */
      maxResultCount?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<SwiperDtoPagedResultDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/Swiper/GetAll';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = {
        OrganizationUnitId: params['organizationUnitId'],
        Status: params['status'],
        UserId: params['userId'],
        Pid: params['pid'],
        Keyword: params['keyword'],
        IsActive: params['isActive'],
        Sorting: params['sorting'],
        From: params['from'],
        To: params['to'],
        SkipCount: params['skipCount'],
        MaxResultCount: params['maxResultCount']
      };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static get(
    params: {
      /**  */
      id?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<SwiperDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/Swiper/Get';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static create(
    params: {
      /** requestBody */
      body?: SwiperDto;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<SwiperDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/Swiper/Create';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static update(
    params: {
      /** requestBody */
      body?: SwiperDto;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<SwiperDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/Swiper/Update';

      const configs: IRequestConfig = getConfigs('put', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static delete(
    params: {
      /**  */
      id?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<any> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/Swiper/Delete';

      const configs: IRequestConfig = getConfigs('delete', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
}

export class TenantService {
  /**
   *
   */
  static create(
    params: {
      /** requestBody */
      body?: CreateTenantDto;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<TenantDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/Tenant/Create';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static delete(
    params: {
      /**  */
      id?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<any> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/Tenant/Delete';

      const configs: IRequestConfig = getConfigs('delete', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static get(
    params: {
      /**  */
      id?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<TenantDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/Tenant/Get';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getAll(
    params: {
      /**  */
      keyword?: string;
      /**  */
      isActive?: boolean;
      /**  */
      skipCount?: number;
      /**  */
      maxResultCount?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<TenantDtoPagedResultDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/Tenant/GetAll';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = {
        Keyword: params['keyword'],
        IsActive: params['isActive'],
        SkipCount: params['skipCount'],
        MaxResultCount: params['maxResultCount']
      };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static update(
    params: {
      /** requestBody */
      body?: TenantDto;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<TenantDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/Tenant/Update';

      const configs: IRequestConfig = getConfigs('put', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
}

export class TenantDashboardService {
  /**
   *
   */
  static getDashboardData(
    params: {
      /**  */
      salesSummaryDatePeriod?: SalesSummaryDatePeriod;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<GetDashboardDataOutput> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/TenantDashboard/GetDashboardData';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = { SalesSummaryDatePeriod: params['salesSummaryDatePeriod'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
}

export class TenantSettingsService {
  /**
   *
   */
  static getAllSettings(options: IRequestOptions = {}): Promise<TenantSettingsEditDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/TenantSettings/GetAllSettings';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);

      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static updateAllSettings(
    params: {
      /** requestBody */
      body?: TenantSettingsEditDto;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<any> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/TenantSettings/UpdateAllSettings';

      const configs: IRequestConfig = getConfigs('put', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
}

export class TimelineCategoryService {
  /**
   *
   */
  static create(
    params: {
      /** requestBody */
      body?: TimelineCategoryCreateOrUpdateDto;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<TimelineCategoryDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/Timeline/TimelineCategory/Create';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getForEdit(
    params: {
      /**  */
      id?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<TimelineCategoryCreateOrUpdateDtoGetForEditOutput> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/Timeline/TimelineCategory/GetForEdit';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static update(
    params: {
      /** requestBody */
      body?: TimelineCategoryCreateOrUpdateDto;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<TimelineCategoryDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/Timeline/TimelineCategory/Update';

      const configs: IRequestConfig = getConfigs('put', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static get(
    params: {
      /**  */
      id?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<TimelineCategoryDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/Timeline/TimelineCategory/Get';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getAll(
    params: {
      /**  */
      organizationUnitId?: number;
      /**  */
      status?: number;
      /**  */
      userId?: number;
      /**  */
      pid?: number;
      /**  */
      keyword?: string;
      /**  */
      isActive?: boolean;
      /**  */
      sorting?: string;
      /**  */
      from?: string;
      /**  */
      to?: string;
      /**  */
      skipCount?: number;
      /**  */
      maxResultCount?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<TimelineCategoryDtoPagedResultDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/Timeline/TimelineCategory/GetAll';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = {
        OrganizationUnitId: params['organizationUnitId'],
        Status: params['status'],
        UserId: params['userId'],
        Pid: params['pid'],
        Keyword: params['keyword'],
        IsActive: params['isActive'],
        Sorting: params['sorting'],
        From: params['from'],
        To: params['to'],
        SkipCount: params['skipCount'],
        MaxResultCount: params['maxResultCount']
      };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static delete(
    params: {
      /**  */
      id?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<any> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/Timeline/TimelineCategory/Delete';

      const configs: IRequestConfig = getConfigs('delete', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
}

export class TimelineEventService {
  /**
   *
   */
  static getForEdit(
    params: {
      /**  */
      id?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<TimelineEventCreateOrUpdateDtoGetForEditOutput> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/Timeline/TimelineEvent/GetForEdit';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static create(
    params: {
      /** requestBody */
      body?: TimelineEventCreateOrUpdateDto;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<TimelineEventDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/Timeline/TimelineEvent/Create';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static update(
    params: {
      /** requestBody */
      body?: TimelineEventCreateOrUpdateDto;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<TimelineEventDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/Timeline/TimelineEvent/Update';

      const configs: IRequestConfig = getConfigs('put', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static startAudit(
    params: {
      /** requestBody */
      body?: Int64EntityDto;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<any> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/Timeline/TimelineEvent/StartAudit';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static delete(
    params: {
      /**  */
      id?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<any> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/Timeline/TimelineEvent/Delete';

      const configs: IRequestConfig = getConfigs('delete', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static audit(
    params: {
      /** requestBody */
      body?: AuditUserLog;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<any> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/Timeline/TimelineEvent/Audit';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getMyAll(
    params: {
      /**  */
      organizationUnitId?: number;
      /**  */
      status?: number;
      /**  */
      userId?: number;
      /**  */
      pid?: number;
      /**  */
      keyword?: string;
      /**  */
      isActive?: boolean;
      /**  */
      sorting?: string;
      /**  */
      from?: string;
      /**  */
      to?: string;
      /**  */
      skipCount?: number;
      /**  */
      maxResultCount?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<TimelineEventDtoPagedResultDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/Timeline/TimelineEvent/GetMyAll';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = {
        OrganizationUnitId: params['organizationUnitId'],
        Status: params['status'],
        UserId: params['userId'],
        Pid: params['pid'],
        Keyword: params['keyword'],
        IsActive: params['isActive'],
        Sorting: params['sorting'],
        From: params['from'],
        To: params['to'],
        SkipCount: params['skipCount'],
        MaxResultCount: params['maxResultCount']
      };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getLogs(
    params: {
      /**  */
      id?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<AuditUserLogDto[]> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/Timeline/TimelineEvent/GetLogs';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static get(
    params: {
      /**  */
      id?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<TimelineEventDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/Timeline/TimelineEvent/Get';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getAll(
    params: {
      /**  */
      organizationUnitId?: number;
      /**  */
      status?: number;
      /**  */
      userId?: number;
      /**  */
      pid?: number;
      /**  */
      keyword?: string;
      /**  */
      isActive?: boolean;
      /**  */
      sorting?: string;
      /**  */
      from?: string;
      /**  */
      to?: string;
      /**  */
      skipCount?: number;
      /**  */
      maxResultCount?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<TimelineEventDtoPagedResultDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/Timeline/TimelineEvent/GetAll';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = {
        OrganizationUnitId: params['organizationUnitId'],
        Status: params['status'],
        UserId: params['userId'],
        Pid: params['pid'],
        Keyword: params['keyword'],
        IsActive: params['isActive'],
        Sorting: params['sorting'],
        From: params['from'],
        To: params['to'],
        SkipCount: params['skipCount'],
        MaxResultCount: params['maxResultCount']
      };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
}

export class TimelineFileService {
  /**
   *
   */
  static create(
    params: {
      /** requestBody */
      body?: TimelineFileCreateOrUpdateDto;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<TimelineFileDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/Timeline/TimelineFile/Create';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getAll(
    params: {
      /**  */
      organizationUnitId?: number;
      /**  */
      status?: number;
      /**  */
      userId?: number;
      /**  */
      pid?: number;
      /**  */
      keyword?: string;
      /**  */
      isActive?: boolean;
      /**  */
      sorting?: string;
      /**  */
      from?: string;
      /**  */
      to?: string;
      /**  */
      skipCount?: number;
      /**  */
      maxResultCount?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<TimelineFileDtoPagedResultDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/Timeline/TimelineFile/GetAll';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = {
        OrganizationUnitId: params['organizationUnitId'],
        Status: params['status'],
        UserId: params['userId'],
        Pid: params['pid'],
        Keyword: params['keyword'],
        IsActive: params['isActive'],
        Sorting: params['sorting'],
        From: params['from'],
        To: params['to'],
        SkipCount: params['skipCount'],
        MaxResultCount: params['maxResultCount']
      };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static postPublishList(
    params: {
      /** requestBody */
      body?: FilePublishRequestDto;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<any> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/Timeline/TimelineFile/PostPublishList';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static postAddFiles(
    params: {
      /** requestBody */
      body?: AddFilesReqestDto;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<any> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/Timeline/TimelineFile/PostAddFiles';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
}

export class TokenAuthService {
  /**
   *
   */
  static authenticate(
    params: {
      /** requestBody */
      body?: AuthenticateModel;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<AuthenticateResultModel> {
    return new Promise((resolve, reject) => {
      let url = '/api/TokenAuth/Authenticate';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getExternalAuthenticationProviders(options: IRequestOptions = {}): Promise<ExternalLoginProviderInfoModel[]> {
    return new Promise((resolve, reject) => {
      let url = '/api/TokenAuth/GetExternalAuthenticationProviders';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);

      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static externalAuthenticate(
    params: {
      /** requestBody */
      body?: ExternalAuthenticateModel;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<ExternalAuthenticateResultModel> {
    return new Promise((resolve, reject) => {
      let url = '/api/TokenAuth/ExternalAuthenticate';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getWeixinMiniPhone(
    params: {
      /** requestBody */
      body?: WeChatMiniProgramAuthenticateModel;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<any> {
    return new Promise((resolve, reject) => {
      let url = '/api/TokenAuth/GetWeixinMiniPhone';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static weixinAuthenticate(
    params: {
      /**  */
      code?: string;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<ExternalAuthenticateResultModel> {
    return new Promise((resolve, reject) => {
      let url = '/api/TokenAuth/WeixinAuthenticate';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = { code: params['code'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static weixinMiniAuthenticate(
    params: {
      /** requestBody */
      body?: WeChatMiniProgramAuthenticateModel;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<ExternalAuthenticateResultModel> {
    return new Promise((resolve, reject) => {
      let url = '/api/TokenAuth/WeixinMiniAuthenticate';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static weixinMiniPhoneAuthenticate(
    params: {
      /** requestBody */
      body?: WeChatMiniProgramAuthenticateModel;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<ExternalAuthenticateResultModel> {
    return new Promise((resolve, reject) => {
      let url = '/api/TokenAuth/WeixinMiniPhoneAuthenticate';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
}

export class UploadService {
  /**
   *
   */
  static getSignature(
    params: {
      /**  */
      data?: string;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<any> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/Upload/GetSignature';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = { data: params['data'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static upload(
    params: {
      /**  */
      id?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<string> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/Upload/Upload';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);
      configs.params = { id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
}

export class UserService {
  /**
   *
   */
  static create(
    params: {
      /** requestBody */
      body?: CreateUserDto;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<UserDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/User/Create';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getUserForEdit(
    params: {
      /**  */
      id?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<GetUserForEditOutput> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/User/GetUserForEdit';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static createOrUpdateUser(
    params: {
      /** requestBody */
      body?: CreateOrUpdateUserInput;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<any> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/User/CreateOrUpdateUser';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static update(
    params: {
      /** requestBody */
      body?: UserDto;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<UserDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/User/Update';

      const configs: IRequestConfig = getConfigs('put', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static delete(
    params: {
      /**  */
      id?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<any> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/User/Delete';

      const configs: IRequestConfig = getConfigs('delete', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getRoles(options: IRequestOptions = {}): Promise<RoleDtoListResultDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/User/GetRoles';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);

      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static changeLanguage(
    params: {
      /** requestBody */
      body?: ChangeUserLanguageDto;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<any> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/User/ChangeLanguage';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static changePassword(
    params: {
      /** requestBody */
      body?: ChangePasswordDto;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<boolean> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/User/ChangePassword';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getUserLoginKey(
    params: {
      /**  */
      id?: number;
      /**  */
      clientType?: ClientTypeEnum;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<string> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/User/GetUserLoginKey';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = { id: params['id'], clientType: params['clientType'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static resetPassword(
    params: {
      /** requestBody */
      body?: ResetPasswordDto;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<boolean> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/User/ResetPassword';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getForEdit(
    params: {
      /**  */
      id?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<CreateUserDtoGetForEditOutput> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/User/GetForEdit';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static get(
    params: {
      /**  */
      id?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<UserDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/User/Get';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getAll(
    params: {
      /**  */
      organizationUnitId?: number;
      /**  */
      status?: number;
      /**  */
      userId?: number;
      /**  */
      pid?: number;
      /**  */
      keyword?: string;
      /**  */
      isActive?: boolean;
      /**  */
      sorting?: string;
      /**  */
      from?: string;
      /**  */
      to?: string;
      /**  */
      skipCount?: number;
      /**  */
      maxResultCount?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<UserDtoPagedResultDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/User/GetAll';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = {
        OrganizationUnitId: params['organizationUnitId'],
        Status: params['status'],
        UserId: params['userId'],
        Pid: params['pid'],
        Keyword: params['keyword'],
        IsActive: params['isActive'],
        Sorting: params['sorting'],
        From: params['from'],
        To: params['to'],
        SkipCount: params['skipCount'],
        MaxResultCount: params['maxResultCount']
      };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
}

export class UserEventService {
  /**
   *
   */
  static create(
    params: {
      /** requestBody */
      body?: UserEventDto;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<UserEventDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/UserEvent/Create';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static update(
    params: {
      /** requestBody */
      body?: UserEventDto;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<UserEventDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/UserEvent/Update';

      const configs: IRequestConfig = getConfigs('put', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static delete(
    params: {
      /**  */
      id?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<any> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/UserEvent/Delete';

      const configs: IRequestConfig = getConfigs('delete', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static get(
    params: {
      /**  */
      id?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<UserEventDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/UserEvent/Get';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getAll(
    params: {
      /**  */
      organizationUnitId?: number;
      /**  */
      status?: number;
      /**  */
      userId?: number;
      /**  */
      pid?: number;
      /**  */
      keyword?: string;
      /**  */
      isActive?: boolean;
      /**  */
      sorting?: string;
      /**  */
      from?: string;
      /**  */
      to?: string;
      /**  */
      skipCount?: number;
      /**  */
      maxResultCount?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<UserEventDtoPagedResultDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/UserEvent/GetAll';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = {
        OrganizationUnitId: params['organizationUnitId'],
        Status: params['status'],
        UserId: params['userId'],
        Pid: params['pid'],
        Keyword: params['keyword'],
        IsActive: params['isActive'],
        Sorting: params['sorting'],
        From: params['from'],
        To: params['to'],
        SkipCount: params['skipCount'],
        MaxResultCount: params['maxResultCount']
      };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
}

export class UserPointLogService {
  /**
   *
   */
  static getAll(
    params: {
      /**  */
      organizationUnitId?: number;
      /**  */
      status?: number;
      /**  */
      userId?: number;
      /**  */
      pid?: number;
      /**  */
      keyword?: string;
      /**  */
      isActive?: boolean;
      /**  */
      sorting?: string;
      /**  */
      from?: string;
      /**  */
      to?: string;
      /**  */
      skipCount?: number;
      /**  */
      maxResultCount?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<UserPointLogDtoPagedResultDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/Activity/UserPointLog/GetAll';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = {
        OrganizationUnitId: params['organizationUnitId'],
        Status: params['status'],
        UserId: params['userId'],
        Pid: params['pid'],
        Keyword: params['keyword'],
        IsActive: params['isActive'],
        Sorting: params['sorting'],
        From: params['from'],
        To: params['to'],
        SkipCount: params['skipCount'],
        MaxResultCount: params['maxResultCount']
      };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getMyLogs(
    params: {
      /**  */
      organizationUnitId?: number;
      /**  */
      status?: number;
      /**  */
      userId?: number;
      /**  */
      pid?: number;
      /**  */
      keyword?: string;
      /**  */
      isActive?: boolean;
      /**  */
      sorting?: string;
      /**  */
      from?: string;
      /**  */
      to?: string;
      /**  */
      skipCount?: number;
      /**  */
      maxResultCount?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<UserPointLogListResultDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/Activity/UserPointLog/GetMyLogs';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = {
        OrganizationUnitId: params['organizationUnitId'],
        Status: params['status'],
        UserId: params['userId'],
        Pid: params['pid'],
        Keyword: params['keyword'],
        IsActive: params['isActive'],
        Sorting: params['sorting'],
        From: params['from'],
        To: params['to'],
        SkipCount: params['skipCount'],
        MaxResultCount: params['maxResultCount']
      };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static anlayse(
    params: {
      /**  */
      id?: string;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<any> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/Activity/UserPointLog/Anlayse';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static dateAnlayse(
    params: {
      /**  */
      organizationUnitId?: number;
      /**  */
      status?: number;
      /**  */
      userId?: number;
      /**  */
      pid?: number;
      /**  */
      keyword?: string;
      /**  */
      isActive?: boolean;
      /**  */
      sorting?: string;
      /**  */
      from?: string;
      /**  */
      to?: string;
      /**  */
      skipCount?: number;
      /**  */
      maxResultCount?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<any> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/Activity/UserPointLog/DateAnlayse';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = {
        OrganizationUnitId: params['organizationUnitId'],
        Status: params['status'],
        UserId: params['userId'],
        Pid: params['pid'],
        Keyword: params['keyword'],
        IsActive: params['isActive'],
        Sorting: params['sorting'],
        From: params['from'],
        To: params['to'],
        SkipCount: params['skipCount'],
        MaxResultCount: params['maxResultCount']
      };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static countAnlayse(
    params: {
      /**  */
      organizationUnitId?: number;
      /**  */
      status?: number;
      /**  */
      userId?: number;
      /**  */
      pid?: number;
      /**  */
      keyword?: string;
      /**  */
      isActive?: boolean;
      /**  */
      sorting?: string;
      /**  */
      from?: string;
      /**  */
      to?: string;
      /**  */
      skipCount?: number;
      /**  */
      maxResultCount?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<any> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/Activity/UserPointLog/CountAnlayse';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = {
        OrganizationUnitId: params['organizationUnitId'],
        Status: params['status'],
        UserId: params['userId'],
        Pid: params['pid'],
        Keyword: params['keyword'],
        IsActive: params['isActive'],
        Sorting: params['sorting'],
        From: params['from'],
        To: params['to'],
        SkipCount: params['skipCount'],
        MaxResultCount: params['maxResultCount']
      };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static create(
    params: {
      /** requestBody */
      body?: UserPointLogDto;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<UserPointLogDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/Activity/UserPointLog/Create';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static update(
    params: {
      /** requestBody */
      body?: UserPointLogDto;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<UserPointLogDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/Activity/UserPointLog/Update';

      const configs: IRequestConfig = getConfigs('put', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static delete(
    params: {
      /**  */
      id?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<any> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/Activity/UserPointLog/Delete';

      const configs: IRequestConfig = getConfigs('delete', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static get(
    params: {
      /**  */
      id?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<UserPointLogDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/Activity/UserPointLog/Get';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
}

export class UserPrizeService {
  /**
   *
   */
  static check(
    params: {
      /** requestBody */
      body?: UserPrizeCheckInputDto;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<any> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/Activity/UserPrize/Check';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getAll(
    params: {
      /**  */
      organizationUnitId?: number;
      /**  */
      status?: number;
      /**  */
      userId?: number;
      /**  */
      pid?: number;
      /**  */
      keyword?: string;
      /**  */
      isActive?: boolean;
      /**  */
      sorting?: string;
      /**  */
      from?: string;
      /**  */
      to?: string;
      /**  */
      skipCount?: number;
      /**  */
      maxResultCount?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<UserPrizeDtoPagedResultDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/Activity/UserPrize/GetAll';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = {
        OrganizationUnitId: params['organizationUnitId'],
        Status: params['status'],
        UserId: params['userId'],
        Pid: params['pid'],
        Keyword: params['keyword'],
        IsActive: params['isActive'],
        Sorting: params['sorting'],
        From: params['from'],
        To: params['to'],
        SkipCount: params['skipCount'],
        MaxResultCount: params['maxResultCount']
      };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getAllMy(
    params: {
      /**  */
      organizationUnitId?: number;
      /**  */
      status?: number;
      /**  */
      userId?: number;
      /**  */
      pid?: number;
      /**  */
      keyword?: string;
      /**  */
      isActive?: boolean;
      /**  */
      sorting?: string;
      /**  */
      from?: string;
      /**  */
      to?: string;
      /**  */
      skipCount?: number;
      /**  */
      maxResultCount?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<UserPrizeDtoPagedResultDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/Activity/UserPrize/GetAllMy';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = {
        OrganizationUnitId: params['organizationUnitId'],
        Status: params['status'],
        UserId: params['userId'],
        Pid: params['pid'],
        Keyword: params['keyword'],
        IsActive: params['isActive'],
        Sorting: params['sorting'],
        From: params['from'],
        To: params['to'],
        SkipCount: params['skipCount'],
        MaxResultCount: params['maxResultCount']
      };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static setExpress(
    params: {
      /** requestBody */
      body?: UserPrizeExpressInput;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<UserPrizeDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/Activity/UserPrize/SetExpress';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getCheckQr(
    params: {
      /**  */
      id?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<string> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/Activity/UserPrize/GetCheckQr';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static exportExcel(
    params: {
      /** requestBody */
      body?: AppResultRequestDto;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<string> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/Activity/UserPrize/ExportExcel';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static create(
    params: {
      /** requestBody */
      body?: UserPrizeDto;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<UserPrizeDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/Activity/UserPrize/Create';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static update(
    params: {
      /** requestBody */
      body?: UserPrizeDto;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<UserPrizeDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/Activity/UserPrize/Update';

      const configs: IRequestConfig = getConfigs('put', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static delete(
    params: {
      /**  */
      id?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<any> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/Activity/UserPrize/Delete';

      const configs: IRequestConfig = getConfigs('delete', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getForEdit(
    params: {
      /**  */
      id?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<UserPrizeDtoGetForEditOutput> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/Activity/UserPrize/GetForEdit';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static get(
    params: {
      /**  */
      id?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<UserPrizeDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/Activity/UserPrize/Get';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
}

export class UserQuestionLogService {
  /**
   *
   */
  static postUserQuestion(
    params: {
      /** requestBody */
      body?: UserQuestionRequest;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<UserQuestionLogDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/QA/UserQuestionLog/PostUserQuestion';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static get(
    params: {
      /**  */
      id?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<UserQuestionLogDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/QA/UserQuestionLog/Get';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getUserQuestion(
    params: {
      /**  */
      id?: number;
      /**  */
      shareFrom?: string;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<UserQuestionLogDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/QA/UserQuestionLog/GetUserQuestion';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = { Id: params['id'], ShareFrom: params['shareFrom'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getPoints(
    params: {
      /**  */
      id?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<UserQuestionLogDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/QA/UserQuestionLog/GetPoints';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getAll(
    params: {
      /**  */
      organizationUnitId?: number;
      /**  */
      status?: number;
      /**  */
      userId?: number;
      /**  */
      pid?: number;
      /**  */
      keyword?: string;
      /**  */
      isActive?: boolean;
      /**  */
      sorting?: string;
      /**  */
      from?: string;
      /**  */
      to?: string;
      /**  */
      skipCount?: number;
      /**  */
      maxResultCount?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<UserQuestionLogDtoPagedResultDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/QA/UserQuestionLog/GetAll';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = {
        OrganizationUnitId: params['organizationUnitId'],
        Status: params['status'],
        UserId: params['userId'],
        Pid: params['pid'],
        Keyword: params['keyword'],
        IsActive: params['isActive'],
        Sorting: params['sorting'],
        From: params['from'],
        To: params['to'],
        SkipCount: params['skipCount'],
        MaxResultCount: params['maxResultCount']
      };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getRankList(
    params: {
      /**  */
      id?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<RankListDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/QA/UserQuestionLog/GetRankList';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static delete(
    params: {
      /**  */
      id?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<any> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/QA/UserQuestionLog/Delete';

      const configs: IRequestConfig = getConfigs('delete', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
}

export class VoteItemService {
  /**
   *
   */
  static getForEditFromPlan(
    params: {
      /**  */
      id?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<VoteItemCreateOrUpdateDtoGetForEditOutput> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/Activity/VoteItem/GetForEditFromPlan';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getForEdit(
    params: {
      /**  */
      id?: string;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<VoteItemCreateOrUpdateDtoGetForEditOutput> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/Activity/VoteItem/GetForEdit';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static update(
    params: {
      /** requestBody */
      body?: VoteItemCreateOrUpdateDto;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<VoteItemDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/Activity/VoteItem/Update';

      const configs: IRequestConfig = getConfigs('put', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static create(
    params: {
      /** requestBody */
      body?: VoteItemCreateOrUpdateDto;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<VoteItemDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/Activity/VoteItem/Create';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static startAudit(
    params: {
      /** requestBody */
      body?: GuidEntityDto;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<any> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/Activity/VoteItem/StartAudit';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static delete(
    params: {
      /**  */
      id?: string;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<any> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/Activity/VoteItem/Delete';

      const configs: IRequestConfig = getConfigs('delete', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static audit(
    params: {
      /** requestBody */
      body?: AuditUserLog;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<any> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/Activity/VoteItem/Audit';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getMyAll(
    params: {
      /**  */
      organizationUnitId?: number;
      /**  */
      status?: number;
      /**  */
      userId?: number;
      /**  */
      pid?: number;
      /**  */
      keyword?: string;
      /**  */
      isActive?: boolean;
      /**  */
      sorting?: string;
      /**  */
      from?: string;
      /**  */
      to?: string;
      /**  */
      skipCount?: number;
      /**  */
      maxResultCount?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<VoteItemDtoPagedResultDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/Activity/VoteItem/GetMyAll';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = {
        OrganizationUnitId: params['organizationUnitId'],
        Status: params['status'],
        UserId: params['userId'],
        Pid: params['pid'],
        Keyword: params['keyword'],
        IsActive: params['isActive'],
        Sorting: params['sorting'],
        From: params['from'],
        To: params['to'],
        SkipCount: params['skipCount'],
        MaxResultCount: params['maxResultCount']
      };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getLogs(
    params: {
      /**  */
      id?: string;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<AuditUserLogDto[]> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/Activity/VoteItem/GetLogs';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static get(
    params: {
      /**  */
      id?: string;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<VoteItemDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/Activity/VoteItem/Get';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getAll(
    params: {
      /**  */
      organizationUnitId?: number;
      /**  */
      status?: number;
      /**  */
      userId?: number;
      /**  */
      pid?: number;
      /**  */
      keyword?: string;
      /**  */
      isActive?: boolean;
      /**  */
      sorting?: string;
      /**  */
      from?: string;
      /**  */
      to?: string;
      /**  */
      skipCount?: number;
      /**  */
      maxResultCount?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<VoteItemDtoPagedResultDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/Activity/VoteItem/GetAll';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = {
        OrganizationUnitId: params['organizationUnitId'],
        Status: params['status'],
        UserId: params['userId'],
        Pid: params['pid'],
        Keyword: params['keyword'],
        IsActive: params['isActive'],
        Sorting: params['sorting'],
        From: params['from'],
        To: params['to'],
        SkipCount: params['skipCount'],
        MaxResultCount: params['maxResultCount']
      };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
}

export class VotePlanService {
  /**
   *
   */
  static get(
    params: {
      /**  */
      id?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<VotePlanDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/Activity/VotePlan/Get';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getForEdit(
    params: {
      /**  */
      id?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<VotePlanCreateOrUpdateDtoGetForEditOutput> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/Activity/VotePlan/GetForEdit';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static update(
    params: {
      /** requestBody */
      body?: VotePlanCreateOrUpdateDto;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<VotePlanDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/Activity/VotePlan/Update';

      const configs: IRequestConfig = getConfigs('put', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getAll(
    params: {
      /**  */
      organizationUnitId?: number;
      /**  */
      status?: number;
      /**  */
      userId?: number;
      /**  */
      pid?: number;
      /**  */
      keyword?: string;
      /**  */
      isActive?: boolean;
      /**  */
      sorting?: string;
      /**  */
      from?: string;
      /**  */
      to?: string;
      /**  */
      skipCount?: number;
      /**  */
      maxResultCount?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<VotePlanDtoPagedResultDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/Activity/VotePlan/GetAll';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = {
        OrganizationUnitId: params['organizationUnitId'],
        Status: params['status'],
        UserId: params['userId'],
        Pid: params['pid'],
        Keyword: params['keyword'],
        IsActive: params['isActive'],
        Sorting: params['sorting'],
        From: params['from'],
        To: params['to'],
        SkipCount: params['skipCount'],
        MaxResultCount: params['maxResultCount']
      };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static create(
    params: {
      /** requestBody */
      body?: VotePlanCreateOrUpdateDto;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<VotePlanDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/Activity/VotePlan/Create';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static delete(
    params: {
      /**  */
      id?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<any> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/Activity/VotePlan/Delete';

      const configs: IRequestConfig = getConfigs('delete', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
}

export class WechatUserinfoService {
  /**
   *
   */
  static getInfoByOpenid(
    params: {
      /**  */
      openid?: string;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<any> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/WechatUserinfo/GetInfoByOpenid';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = { openid: params['openid'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static sendMini(
    params: {
      /**  */
      openid?: string;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<any> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/WechatUserinfo/SendMini';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);
      configs.params = { openid: params['openid'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static runAll(
    params: {
      /**  */
      start?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<any> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/WechatUserinfo/RunAll';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);
      configs.params = { start: params['start'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static get(
    params: {
      /**  */
      id?: string;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<WechatUserinfoDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/WechatUserinfo/Get';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getAll(
    params: {
      /**  */
      organizationUnitId?: number;
      /**  */
      status?: number;
      /**  */
      userId?: number;
      /**  */
      pid?: number;
      /**  */
      keyword?: string;
      /**  */
      isActive?: boolean;
      /**  */
      sorting?: string;
      /**  */
      from?: string;
      /**  */
      to?: string;
      /**  */
      skipCount?: number;
      /**  */
      maxResultCount?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<WechatUserinfoDtoPagedResultDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/WechatUserinfo/GetAll';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = {
        OrganizationUnitId: params['organizationUnitId'],
        Status: params['status'],
        UserId: params['userId'],
        Pid: params['pid'],
        Keyword: params['keyword'],
        IsActive: params['isActive'],
        Sorting: params['sorting'],
        From: params['from'],
        To: params['to'],
        SkipCount: params['skipCount'],
        MaxResultCount: params['maxResultCount']
      };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static create(
    params: {
      /** requestBody */
      body?: WechatUserinfoDto;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<WechatUserinfoDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/WechatUserinfo/Create';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static update(
    params: {
      /** requestBody */
      body?: WechatUserinfoDto;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<WechatUserinfoDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/WechatUserinfo/Update';

      const configs: IRequestConfig = getConfigs('put', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static delete(
    params: {
      /**  */
      id?: string;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<any> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/app/WechatUserinfo/Delete';

      const configs: IRequestConfig = getConfigs('delete', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
}

export class WorkflowService {
  /**
   *
   */
  static create(
    params: {
      /** requestBody */
      body?: WorkflowDto;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<WorkflowDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/WorkFlow/Workflow/Create';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static update(
    params: {
      /** requestBody */
      body?: WorkflowDto;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<WorkflowDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/WorkFlow/Workflow/Update';

      const configs: IRequestConfig = getConfigs('put', 'application/json', url, options);

      let data = params.body;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static delete(
    params: {
      /**  */
      id?: string;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<any> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/WorkFlow/Workflow/Delete';

      const configs: IRequestConfig = getConfigs('delete', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static excute(
    params: {
      /**  */
      name?: string;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<any> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/WorkFlow/Workflow/Excute';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);
      configs.params = { name: params['name'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static excuteDsl(
    params: {
      /**  */
      json?: string;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<string> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/WorkFlow/Workflow/ExcuteDSL';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);
      configs.params = { json: params['json'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getAll(
    params: {
      /**  */
      organizationUnitId?: number;
      /**  */
      status?: number;
      /**  */
      userId?: number;
      /**  */
      pid?: number;
      /**  */
      keyword?: string;
      /**  */
      isActive?: boolean;
      /**  */
      sorting?: string;
      /**  */
      from?: string;
      /**  */
      to?: string;
      /**  */
      skipCount?: number;
      /**  */
      maxResultCount?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<WorkflowDtoPagedResultDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/WorkFlow/Workflow/GetAll';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = {
        OrganizationUnitId: params['organizationUnitId'],
        Status: params['status'],
        UserId: params['userId'],
        Pid: params['pid'],
        Keyword: params['keyword'],
        IsActive: params['isActive'],
        Sorting: params['sorting'],
        From: params['from'],
        To: params['to'],
        SkipCount: params['skipCount'],
        MaxResultCount: params['maxResultCount']
      };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static get(
    params: {
      /**  */
      id?: string;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<WorkflowDto> {
    return new Promise((resolve, reject) => {
      let url = '/api/services/WorkFlow/Workflow/Get';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = { Id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
}

export interface AbpFeatureDto {
  /**  */
  name?: string;

  /**  */
  providerName?: string;

  /**  */
  providerKey?: string;

  /**  */
  enable?: boolean;

  /**  */
  dateTimeExpired?: Date;

  /**  */
  value?: string;

  /**  */
  organizationUnit?: BasicOrganizationInfo;

  /**  */
  id?: string;
}

export interface AbpFeatureDtoGetForEditOutput {
  /**  */
  data?: AbpFeatureDto;

  /**  */
  schema?: any | null;
}

export interface AbpFeatureDtoPagedResultDto {
  /**  */
  totalCount?: number;

  /**  */
  items?: AbpFeatureDto[];
}

export interface AddFilesReqestDto {
  /**  */
  eventId?: number;

  /**  */
  data?: object;

  /**  */
  imageList?: string[];
}

export interface AddressDetail {
  /**  */
  userName?: string;

  /**  */
  postalCode?: string;

  /**  */
  provinceName?: string;

  /**  */
  cityName?: string;

  /**  */
  countyName?: string;

  /**  */
  detailInfo?: string;

  /**  */
  nationalCode?: string;

  /**  */
  telNumber?: string;
}

export interface AppCreateOrUpdateDto {
  /**  */
  name?: string;

  /**  */
  clientName?: string;

  /**  */
  providerName?: string;

  /**  */
  providerKey?: string;

  /**  */
  value?: object;

  /**  */
  id?: string;
}

export interface AppDto {
  /**  */
  name?: string;

  /**  */
  clientName?: string;

  /**  */
  clientType?: string;

  /**  */
  value?: object;

  /**  */
  providerName?: string;

  /**  */
  providerKey?: string;

  /**  */
  id?: string;
}

export interface AppDtoListResultDto {
  /**  */
  items?: AppDto[];
}

export interface AppDtoPagedResultDto {
  /**  */
  totalCount?: number;

  /**  */
  items?: AppDto[];
}

export interface ApplicationInfoDto {
  /**  */
  name?: string;

  /**  */
  version?: string;

  /**  */
  releaseDate?: Date;

  /**  */
  features?: object;
}

export interface AppResultRequestDto {
  /**  */
  organizationUnitId?: number;

  /**  */
  status?: number;

  /**  */
  userId?: number;

  /**  */
  pid?: number;

  /**  */
  keyword?: string;

  /**  */
  isActive?: boolean;

  /**  */
  sorting?: string;

  /**  */
  from?: Date;

  /**  */
  to?: Date;

  /**  */
  skipCount?: number;

  /**  */
  maxResultCount?: number;
}

export interface AuditFlowCreateOrEditDto {
  /**  */
  auditName?: string;

  /**  */
  enable?: boolean;

  /**  */
  providerName?: string;

  /**  */
  providerKey?: string;

  /**  */
  type?: AuditFlowType;

  /**  */
  auditNodes?: AuditNodeCreateOrEditDto[];

  /**  */
  id?: string;
}

export interface AuditFlowCreateOrEditDtoGetForEditOutput {
  /**  */
  data?: AuditFlowCreateOrEditDto;

  /**  */
  schema?: any | null;
}

export interface AuditFlowDto {
  /**  */
  auditName?: string;

  /**  */
  auditDisplayName?: string;

  /**  */
  enable?: boolean;

  /**  */
  providerName?: string;

  /**  */
  providerKey?: string;

  /**  */
  nodesMaxIndex?: number;

  /**  */
  auditNodes?: AuditNodeDto[];

  /**  */
  type?: AuditFlowType;

  /**  */
  creationTime?: Date;

  /**  */
  creatorUserId?: number;

  /**  */
  id?: string;
}

export interface AuditFlowDtoPagedResultDto {
  /**  */
  totalCount?: number;

  /**  */
  items?: AuditFlowDto[];
}

export interface AuditLogListDto {
  /**  */
  userId?: number;

  /**  */
  userName?: string;

  /**  */
  impersonatorTenantId?: number;

  /**  */
  impersonatorUserId?: number;

  /**  */
  serviceName?: string;

  /**  */
  methodName?: string;

  /**  */
  parameters?: string;

  /**  */
  executionTime?: Date;

  /**  */
  executionDuration?: number;

  /**  */
  clientIpAddress?: string;

  /**  */
  clientName?: string;

  /**  */
  browserInfo?: string;

  /**  */
  exception?: string;

  /**  */
  customData?: string;

  /**  */
  id?: number;
}

export interface AuditLogListDtoPagedResultDto {
  /**  */
  totalCount?: number;

  /**  */
  items?: AuditLogListDto[];
}

export interface AuditNodeCreateOrEditDto {
  /**  */
  desc?: string;

  /**  */
  userName?: string;

  /**  */
  userId?: number;

  /**  */
  index?: number;

  /**  */
  auditFlowId?: string;

  /**  */
  id?: string;
}

export interface AuditNodeDto {
  /**  */
  desc?: string;

  /**  */
  userName?: string;

  /**  */
  userId?: number;

  /**  */
  index?: number;

  /**  */
  auditFlowId?: string;

  /**  */
  iCanAudit?: boolean;

  /**  */
  creationTime?: Date;

  /**  */
  creatorUserId?: number;

  /**  */
  id?: string;
}

export interface AuditUserLog {
  /**  */
  hostId?: string;

  /**  */
  auditName?: string;

  /**  */
  auditFlowId?: string;

  /**  */
  auditNodeId?: string;

  /**  */
  status?: AuditUserLogStatus;

  /**  */
  desc?: string;

  /**  */
  creationTime?: Date;

  /**  */
  creatorUserId?: number;

  /**  */
  creatorUser?: User;

  /**  */
  tenantId?: number;

  /**  */
  auditStatus?: number;

  /**  */
  expired?: boolean;

  /**  */
  id?: number;
}

export interface AuditUserLogDto {
  /**  */
  auditName?: string;

  /**  */
  status?: AuditUserLogStatus;

  /**  */
  desc?: string;

  /**  */
  creationTime?: Date;

  /**  */
  creatorUser?: UserDtoBase;

  /**  */
  expired?: boolean;

  /**  */
  auditFlowId?: string;

  /**  */
  auditNodeId?: string;

  /**  */
  id?: number;
}

export interface AuthenticateModel {
  /**  */
  userNameOrEmailAddress?: string;

  /**  */
  password?: string;

  /**  */
  twoFactorVerificationCode?: string;

  /**  */
  rememberClient?: boolean;

  /**  */
  twoFactorRememberClientToken?: string;

  /**  */
  singleSignIn?: boolean;

  /**  */
  returnUrl?: string;

  /**  */
  captchaResponse?: string;
}

export interface AuthenticateResultModel {
  /**  */
  accessToken?: string;

  /**  */
  encryptedAccessToken?: string;

  /**  */
  expireInSeconds?: number;

  /**  */
  shouldResetPassword?: boolean;

  /**  */
  passwordResetCode?: string;

  /**  */
  userId?: number;

  /**  */
  requiresTwoFactorVerification?: boolean;

  /**  */
  twoFactorAuthProviders?: string[];

  /**  */
  twoFactorRememberClientToken?: string;

  /**  */
  returnUrl?: string;

  /**  */
  refreshToken?: string;

  /**  */
  refreshTokenExpireInSeconds?: number;
}

export interface BasicOrganizationInfo {
  /**  */
  organizationId?: number;

  /**  */
  displayName?: string;
}

export interface BoxDetail {
  /**  */
  enable?: boolean;

  /**  */
  x?: number;

  /**  */
  y?: number;

  /**  */
  width?: number;

  /**  */
  height?: number;

  /**  */
  lockAspect?: boolean;
}

export interface ChangePasswordDto {
  /**  */
  currentPassword?: string;

  /**  */
  newPassword?: string;
}

export interface ChangeUiThemeInput {
  /**  */
  theme?: string;
}

export interface ChangeUserLanguageDto {
  /**  */
  languageName?: string;
}

export interface ClientSettingEditDto {
  /**  */
  wechatMini?: boolean;
}

export interface CmsCategoryCreateOrUpdateDto {
  /**  */
  name?: string;

  /**  */
  imageUrl?: string;

  /**  */
  id?: number;
}

export interface CmsCategoryCreateOrUpdateDtoGetForEditOutput {
  /**  */
  data?: CmsCategoryCreateOrUpdateDto;

  /**  */
  schema?: any | null;
}

export interface CmsCategoryDto {
  /**  */
  name?: string;

  /**  */
  imageUrl?: string;

  /**  */
  id?: number;
}

export interface CmsCategoryDtoPagedResultDto {
  /**  */
  totalCount?: number;

  /**  */
  items?: CmsCategoryDto[];
}

export interface CmsContentCreateOrUpdateDto {
  /**  */
  categoryId?: number;

  /**  */
  title?: string;

  /**  */
  titleImageUrl?: string;

  /**  */
  linkType?: number;

  /**  */
  linkUrl?: string;

  /**  */
  content?: string;

  /**  */
  status?: CmsContentStatus;

  /**  */
  sort?: number;

  /**  */
  creationTime?: Date;

  /**  */
  id?: number;
}

export interface CmsContentCreateOrUpdateDtoGetForEditOutput {
  /**  */
  data?: CmsContentCreateOrUpdateDto;

  /**  */
  schema?: any | null;
}

export interface CmsContentDto {
  /**  */
  categoryId?: number;

  /**  */
  title?: string;

  /**  */
  titleImageUrl?: string;

  /**  */
  linkType?: CmsContentLinkType;

  /**  */
  linkUrl?: string;

  /**  */
  sort?: number;

  /**  */
  content?: string;

  /**  */
  category?: CmsCategoryDto;

  /**  */
  status?: CmsContentStatus;

  /** 创建人 */
  createUserName?: string;

  /** 创建时间 */
  creationTime?: Date;

  /**  */
  viewCount?: number;

  /** 存放用户关注等事件 */
  userEvents?: EventTypeEnum[];

  /**  */
  id?: number;
}

export interface CmsContentDtoPagedResultDto {
  /**  */
  totalCount?: number;

  /**  */
  items?: CmsContentDto[];
}

export interface CraftsmanCreateOrUpdateDto {
  /**  */
  state?: RecomandState;

  /**  */
  detail?: CraftsmanDetail;

  /**  */
  auditFlowId?: string;

  /**  */
  redpacketRecived?: boolean;

  /**  */
  redpacket?: number;

  /**  */
  id?: number;
}

export interface CraftsmanCreateOrUpdateDtoGetForEditOutput {
  /**  */
  data?: CraftsmanCreateOrUpdateDto;

  /**  */
  schema?: any | null;
}

export interface CraftsmanDetail {
  /**  */
  realname?: string;

  /**  */
  sex?: string;

  /**  */
  headImgUrl?: string;

  /**  */
  birthday?: string;

  /**  */
  nativePlace?: string;

  /**  */
  nation?: string;

  /**  */
  politicsStatus?: string;

  /**  */
  educationBackground?: string;

  /**  */
  skillLevel?: string;

  /**  */
  timeOfStartWork?: Date;

  /**  */
  timeOfStartWorkLocal?: Date;

  /**  */
  address?: string;

  /**  */
  workUnit?: string;

  /**  */
  workTitle?: string;

  /**  */
  phoneNumber?: string;

  /**  */
  personalResume?: string;

  /**  */
  mainAchievement?: string;

  /**  */
  mainEvent?: string;

  /**  */
  opinionsOfWorkUnit?: string;

  /**  */
  opinionsOfRecommandUnit?: string;

  /**  */
  opinionsOfLeadingGroup?: string;
}

export interface CraftsmanDto {
  /**  */
  state?: RecomandState;

  /**  */
  detail?: CraftsmanDetail;

  /**  */
  rejectText?: string;

  /**  */
  creatorUser?: UserDtoBase;

  /**  */
  auditFlowId?: string;

  /**  */
  audit?: number;

  /**  */
  auditStatus?: number;

  /**  */
  isAudited?: boolean;

  /**  */
  auditFlow?: AuditFlowDto;

  /**  */
  currentAuditNodes?: AuditNodeDto[];

  /**  */
  redpacketRecived?: boolean;

  /**  */
  redpacketRecivedTime?: Date;

  /**  */
  redpacket?: number;

  /**  */
  isDeleted?: boolean;

  /**  */
  deleterUserId?: number;

  /**  */
  deletionTime?: Date;

  /**  */
  lastModificationTime?: Date;

  /**  */
  lastModifierUserId?: number;

  /**  */
  creationTime?: Date;

  /**  */
  creatorUserId?: number;

  /**  */
  id?: number;
}

export interface CraftsmanDtoPagedResultDto {
  /**  */
  totalCount?: number;

  /**  */
  items?: CraftsmanDto[];
}

export interface CraftsmanRecommendCreateOrUpdateDto {
  /**  */
  detail?: CraftsmanRecommendDetail;

  /**  */
  auditFlowId?: string;

  /**  */
  id?: number;
}

export interface CraftsmanRecommendCreateOrUpdateDtoGetForEditOutput {
  /**  */
  data?: CraftsmanRecommendCreateOrUpdateDto;

  /**  */
  schema?: any | null;
}

export interface CraftsmanRecommendDetail {
  /**  */
  realname?: string;

  /**  */
  sex?: string;

  /**  */
  age?: string;

  /**  */
  politicsStatus?: string;

  /**  */
  address?: string;

  /**  */
  workUnit?: string;

  /**  */
  workTitle?: string;

  /**  */
  phoneNumber?: string;

  /**  */
  desc?: string;
}

export interface CraftsmanRecommendDto {
  /**  */
  state?: RecomandState;

  /**  */
  detail?: CraftsmanRecommendDetail;

  /**  */
  redpacketRecived?: boolean;

  /**  */
  redpacketRecivedTime?: Date;

  /**  */
  redpacket?: number;

  /**  */
  rejectText?: string;

  /**  */
  creatorUser?: UserDtoBase;

  /**  */
  auditFlowId?: string;

  /**  */
  audit?: number;

  /**  */
  auditStatus?: number;

  /**  */
  isAudited?: boolean;

  /**  */
  auditFlow?: AuditFlowDto;

  /**  */
  currentAuditNodes?: AuditNodeDto[];

  /**  */
  creationTime?: Date;

  /**  */
  creatorUserId?: number;

  /**  */
  id?: number;
}

export interface CraftsmanRecommendDtoPagedResultDto {
  /**  */
  totalCount?: number;

  /**  */
  items?: CraftsmanRecommendDto[];
}

export interface CreateOrganizationUnitInput {
  /**  */
  id?: number;

  /**  */
  parentId?: number;

  /**  */
  type?: number;

  /**  */
  displayName?: string;

  /**  */
  detail?: OrganizationUnitDetailCreateDto;

  /**  */
  formId?: string;

  /**  */
  openid?: string;
}

export interface CreateOrganizationUnitInputGetForEditOutput {
  /**  */
  data?: CreateOrganizationUnitInput;

  /**  */
  schema?: any | null;
}

export interface CreateOrUpdateUserInput {
  /**  */
  user?: UserEditDto;

  /**  */
  assignedRoleNames?: string[];

  /**  */
  organizationUnits?: number[];
}

export interface CreateRoleDto {
  /**  */
  name?: string;

  /**  */
  displayName?: string;

  /**  */
  normalizedName?: string;

  /**  */
  isDefault?: boolean;

  /**  */
  isStatic?: boolean;

  /**  */
  description?: string;

  /**  */
  grantedPermissions?: string[];

  /**  */
  id?: number;
}

export interface CreateTenantDto {
  /**  */
  tenancyName?: string;

  /**  */
  name?: string;

  /**  */
  adminEmailAddress?: string;

  /**  */
  connectionString?: string;

  /**  */
  isActive?: boolean;
}

export interface CreateUserDto {
  /**  */
  userName?: string;

  /**  */
  name?: string;

  /**  */
  surname?: string;

  /**  */
  emailAddress?: string;

  /**  */
  isActive?: boolean;

  /**  */
  roleNames?: string[];

  /**  */
  password?: string;
}

export interface CreateUserDtoGetForEditOutput {
  /**  */
  data?: CreateUserDto;

  /**  */
  schema?: any | null;
}

export interface CreatorOrUpdateOrganizationApplyDto {
  /**  */
  displayName?: string;

  /**  */
  type?: number;

  /**  */
  detail?: OrganizationUnitDetailCreateDto;

  /**  */
  auditFlowId?: string;

  /**  */
  id?: number;
}

export interface CreatorOrUpdateOrganizationApplyDtoGetForEditOutput {
  /**  */
  data?: CreatorOrUpdateOrganizationApplyDto;

  /**  */
  schema?: any | null;
}

export interface EntityPropertyChangeDto {
  /**  */
  entityChangeId?: number;

  /**  */
  newValue?: string;

  /**  */
  originalValue?: string;

  /**  */
  propertyName?: string;

  /**  */
  propertyTypeFullName?: string;

  /**  */
  tenantId?: number;

  /**  */
  id?: number;
}

export interface ExecutionErrorDto {
  /**  */
  errorTime?: Date;

  /**  */
  message?: string;
}

export interface ExternalAuthenticateModel {
  /**  */
  authProvider?: string;

  /**  */
  providerKey?: string;

  /**  */
  providerAccessCode?: string;

  /**  */
  returnUrl?: string;

  /**  */
  singleSignIn?: boolean;
}

export interface ExternalAuthenticateResultModel {
  /**  */
  accessToken?: string;

  /**  */
  encryptedAccessToken?: string;

  /**  */
  expireInSeconds?: number;

  /**  */
  waitingForActivation?: boolean;

  /**  */
  returnUrl?: string;

  /**  */
  refreshToken?: string;

  /**  */
  refreshTokenExpireInSeconds?: number;

  /**  */
  user?: any | null;

  /**  */
  roleNames?: string[];
}

export interface ExternalLoginProviderInfoModel {
  /**  */
  name?: string;

  /**  */
  clientId?: string;
}

export interface FeatureDefinition {
  /**  */
  name?: string;

  /**  */
  defaultValue?: string;

  /**  */
  providers?: string[];

  /**  */
  displayName?: ILocalizableString;
}

export interface FilePublishRequestDto {
  /**  */
  eventId?: number;

  /**  */
  state0List?: string[];

  /**  */
  state1List?: string[];
}

export interface FindOrganizationUnitUsersInput {
  /**  */
  organizationUnitId?: number;

  /**  */
  maxResultCount?: number;

  /**  */
  skipCount?: number;

  /**  */
  filter?: string;
}

export interface FlatPermissionDto {
  /**  */
  parentName?: string;

  /**  */
  name?: string;

  /**  */
  displayName?: string;

  /**  */
  description?: string;
}

export interface FontBoxDetail {
  /**  */
  fontAlign?: string;

  /**  */
  enable?: boolean;

  /**  */
  x?: number;

  /**  */
  y?: number;

  /**  */
  width?: number;

  /**  */
  height?: number;

  /**  */
  fontSize?: number;

  /**  */
  fontMaxLength?: number;

  /**  */
  fontColor?: string;
}

export interface GetCurrentLoginInformationsOutput {
  /**  */
  application?: ApplicationInfoDto;

  /**  */
  user?: UserLoginInfoDto;

  /**  */
  tenant?: TenantLoginInfoDto;

  /**  */
  organizationUnit?: OrganizationUnitDto;

  /**  */
  permissions?: string[];

  /**  */
  roles?: string[];
}

export interface GetDashboardDataOutput {
  /**  */
  newOrders?: number;

  /**  */
  newUsers?: number;

  /**  */
  totalSales?: number;

  /**  */
  checkOutOrders?: number;

  /** 机构申请数 */
  organizationApplyCount?: number;

  /** 退款订单数 */
  refundCount?: number;

  /**  */
  chatList?: any | null[];
}

export interface GetPointRequestDto {
  /**  */
  activityId?: number;

  /**  */
  shareFrom?: string;
}

export interface GetRoleForEditOutput {
  /**  */
  role?: RoleEditDto;

  /**  */
  permissions?: FlatPermissionDto[];

  /**  */
  grantedPermissionNames?: string[];
}

export interface GetUserForEditOutput {
  /**  */
  headImgUrl?: string;

  /**  */
  user?: UserEditDto;

  /**  */
  roles?: UserRoleDto[];

  /**  */
  allOrganizationUnits?: OrganizationUnitDto[];

  /**  */
  memberedOrganizationUnits?: string[];
}

export interface GuidEntityDto {
  /**  */
  id?: string;
}

export interface HostSettingsEditDto {}

export interface ILocalizableString {}

export interface Int32EntityDto {
  /**  */
  id?: number;
}

export interface Int32EntityEventDto {
  /**  */
  eventType?: number;

  /**  */
  event?: string;

  /**  */
  id?: number;
}

export interface Int64EntityDto {
  /**  */
  id?: number;
}

export interface IsTenantAvailableInput {
  /**  */
  tenancyName?: string;
}

export interface IsTenantAvailableOutput {
  /**  */
  state?: TenantAvailabilityState;

  /**  */
  tenantId?: number;
}

export interface JssdkResultDto {
  /**  */
  appId?: string;

  /**  */
  timestamp?: string;

  /**  */
  nonceStr?: string;

  /**  */
  signature?: string;
}

export interface Location {
  /**  */
  lat?: number;

  /**  */
  lng?: number;
}

export interface LuckDrawCreateOrUpdateDto {
  /**  */
  title?: string;

  /**  */
  subTitle?: string;

  /**  */
  type?: LuckDrawType;

  /**  */
  pickupWay?: PickupWay;

  /**  */
  maxDrawTimes?: number;

  /**  */
  maxWinTimes?: number;

  /**  */
  price?: number;

  /**  */
  state?: number;

  /**  */
  prizeTotal?: number;

  /**  */
  htmlContext?: string;

  /**  */
  settings?: object;

  /**  */
  datetimeStart?: Date;

  /**  */
  datetimeEnd?: Date;

  /**  */
  prizeExpiredTime?: Date;

  /**  */
  checkCodes?: string[];

  /**  */
  id?: number;
}

export interface LuckDrawCreateOrUpdateDtoGetForEditOutput {
  /**  */
  data?: LuckDrawCreateOrUpdateDto;

  /**  */
  schema?: any | null;
}

export interface LuckDrawDto {
  /**  */
  title?: string;

  /**  */
  subTitle?: string;

  /**  */
  type?: LuckDrawType;

  /**  */
  pickupWay?: PickupWay;

  /**  */
  price?: number;

  /**  */
  maxDrawTimes?: number;

  /**  */
  prizeTotal?: number;

  /**  */
  prizeCount?: number;

  /**  */
  state?: number;

  /**  */
  htmlContext?: string;

  /**  */
  settings?: object;

  /**  */
  datetimeStart?: Date;

  /**  */
  datetimeEnd?: Date;

  /**  */
  luckDrawPrizes?: LuckDrawPrizeDto[];

  /**  */
  luckTimes?: number;

  /**  */
  creationTime?: Date;

  /**  */
  creatorUserId?: number;

  /**  */
  id?: number;
}

export interface LuckDrawDtoPagedResultDto {
  /**  */
  totalCount?: number;

  /**  */
  items?: LuckDrawDto[];
}

export interface LuckDrawPrizeCreateOrUpdateDto {
  /**  */
  name?: string;

  /**  */
  imageUrl?: string;

  /**  */
  totalCount?: number;

  /**  */
  stockCount?: number;

  /**  */
  pickupWay?: PickupWay;

  /**  */
  luckDrawId?: number;

  /**  */
  id?: number;
}

export interface LuckDrawPrizeCreateOrUpdateDtoGetForEditOutput {
  /**  */
  data?: LuckDrawPrizeCreateOrUpdateDto;

  /**  */
  schema?: any | null;
}

export interface LuckDrawPrizeDto {
  /**  */
  name?: string;

  /**  */
  imageUrl?: string;

  /**  */
  totalCount?: number;

  /**  */
  stockCount?: number;

  /**  */
  pickupWay?: PickupWay;

  /**  */
  checkedCount?: number;

  /**  */
  luckDrawId?: number;

  /**  */
  luckDrawTitle?: string;

  /**  */
  id?: number;
}

export interface LuckDrawPrizeDtoPagedResultDto {
  /**  */
  totalCount?: number;

  /**  */
  items?: LuckDrawPrizeDto[];
}

export interface LuckDrawPrizeMessageInput {
  /**  */
  prizeId?: number;

  /**  */
  text?: string;
}

export interface MoveOrganizationUnitInput {
  /**  */
  id?: number;

  /**  */
  newParentId?: number;
}

export interface NameValueDto {
  /**  */
  name?: string;

  /**  */
  value?: string;
}

export interface NameValueDtoPagedResultDto {
  /**  */
  totalCount?: number;

  /**  */
  items?: NameValueDto[];
}

export interface NotifySettingEditDto {
  /**  */
  phone?: string;

  /**  */
  newOrderSendStatus?: boolean;

  /**  */
  refundSendStatus?: boolean;

  /**  */
  adminOpenid?: string;
}

export interface OrganizationApplyDto {
  /**  */
  displayName?: string;

  /**  */
  detail?: OrganizationUnitDetailDto;

  /**  */
  type?: number;

  /**  */
  organizationUnitId?: number;

  /**  */
  auditFlowId?: string;

  /**  */
  audit?: number;

  /**  */
  auditStatus?: number;

  /**  */
  isAudited?: boolean;

  /**  */
  auditFlow?: AuditFlowDto;

  /**  */
  currentAuditNodes?: AuditNodeDto[];

  /**  */
  creationTime?: Date;

  /**  */
  creatorUserId?: number;

  /**  */
  refuseText?: string;

  /**  */
  id?: number;
}

export interface OrganizationApplyDtoPagedResultDto {
  /**  */
  totalCount?: number;

  /**  */
  items?: OrganizationApplyDto[];
}

export interface OrganizationUnitDetailCreateDto {
  /**  */
  desc?: string;

  /**  */
  logoImgUrl?: string;

  /**  */
  address?: string;

  /**  */
  headmanRealName?: string;

  /**  */
  headmanPhone?: string;

  /**  */
  province?: string;

  /**  */
  city?: string;

  /**  */
  cityId?: number;

  /**  */
  district?: string;

  /**  */
  lng?: number;

  /**  */
  lat?: number;

  /**  */
  unionid?: string;

  /**  */
  extensionData?: string;
}

export interface OrganizationUnitDetailDto {
  /**  */
  desc?: string;

  /**  */
  logoImgUrl?: string;

  /**  */
  address?: string;

  /**  */
  headmanRealName?: string;

  /**  */
  headmanPhone?: string;

  /**  */
  province?: string;

  /**  */
  city?: string;

  /**  */
  cityId?: number;

  /**  */
  district?: string;

  /**  */
  lng?: number;

  /**  */
  lat?: number;
}

export interface OrganizationUnitDto {
  /**  */
  parentId?: number;

  /**  */
  code?: string;

  /**  */
  status?: number;

  /**  */
  type?: number;

  /**  */
  displayName?: string;

  /**  */
  memberCount?: number;

  /**  */
  refuseContent?: string;

  /**  */
  detail?: OrganizationUnitDetailDto;

  /**  */
  userEvents?: number[];

  /**  */
  lastModificationTime?: Date;

  /**  */
  lastModifierUserId?: number;

  /**  */
  creationTime?: Date;

  /**  */
  creatorUserId?: number;

  /**  */
  id?: number;
}

export interface OrganizationUnitDtoBase {
  /**  */
  displayName?: string;

  /**  */
  id?: number;
}

export interface OrganizationUnitDtoBaseListResultDto {
  /**  */
  items?: OrganizationUnitDtoBase[];
}

export interface OrganizationUnitDtoListResultDto {
  /**  */
  items?: OrganizationUnitDto[];
}

export interface OrganizationUnitUserListDto {
  /**  */
  name?: string;

  /**  */
  surname?: string;

  /**  */
  userName?: string;

  /**  */
  emailAddress?: string;

  /**  */
  phoneNumber?: string;

  /**  */
  profilePictureId?: string;

  /**  */
  addedTime?: Date;

  /**  */
  id?: number;
}

export interface OrganizationUnitUserListDtoPagedResultDto {
  /**  */
  totalCount?: number;

  /**  */
  items?: OrganizationUnitUserListDto[];
}

export interface OssSettingEditDto {
  /**  */
  bucketName?: string;

  /**  */
  userName?: string;

  /**  */
  password?: string;

  /**  */
  domainHost?: string;
}

export interface PayOrderDto {
  /**  */
  body?: string;

  /** 单位:分 */
  totalPrice?: number;

  /**  */
  billNo?: string;

  /**  */
  type?: OrderType;

  /**  */
  orderId?: number;

  /**  */
  payType?: PayType;

  /**  */
  isSuccessPay?: boolean;

  /**  */
  successPayTime?: Date;

  /**  */
  creationTime?: Date;

  /**  */
  isRefund?: boolean;

  /**  */
  organizationUnitId?: number;

  /**  */
  refundComplateTime?: Date;

  /**  */
  fromUserId?: number;

  /**  */
  fromUser?: UserDtoBase;

  /**  */
  id?: number;
}

export interface PayOrderDtoPagedResultDto {
  /**  */
  totalCount?: number;

  /**  */
  items?: PayOrderDto[];
}

export interface PermissionDto {
  /**  */
  name?: string;

  /**  */
  displayName?: string;

  /**  */
  description?: string;

  /**  */
  id?: number;
}

export interface PermissionDtoListResultDto {
  /**  */
  items?: PermissionDto[];
}

export interface PersistedExecutionPointer {
  /**  */
  workflowId?: string;

  /**  */
  stepId?: number;

  /**  */
  active?: boolean;

  /**  */
  sleepUntil?: Date;

  /**  */
  persistenceData?: string;

  /**  */
  startTime?: Date;

  /**  */
  endTime?: Date;

  /**  */
  eventName?: string;

  /**  */
  eventKey?: string;

  /**  */
  eventPublished?: boolean;

  /**  */
  eventData?: string;

  /**  */
  stepName?: string;

  /**  */
  retryCount?: number;

  /**  */
  children?: string;

  /**  */
  contextItem?: string;

  /**  */
  predecessorId?: string;

  /**  */
  outcome?: string;

  /**  */
  status?: PointerStatus;

  /**  */
  scope?: string;

  /**  */
  extensionAttributes?: PersistedExtensionAttribute[];

  /**  */
  workflow?: PersistedWorkflow;

  /**  */
  id?: string;
}

export interface PersistedExtensionAttribute {
  /**  */
  executionPointerId?: string;

  /**  */
  attributeKey?: string;

  /**  */
  attributeValue?: string;

  /**  */
  executionPointer?: PersistedExecutionPointer;

  /**  */
  id?: string;
}

export interface PersistedWorkflow {
  /**  */
  workflowDefinitionId?: string;

  /**  */
  version?: number;

  /**  */
  description?: string;

  /**  */
  reference?: string;

  /**  */
  executionPointers?: PersistedExecutionPointer[];

  /**  */
  nextExecution?: number;

  /**  */
  data?: string;

  /**  */
  createTime?: Date;

  /**  */
  completeTime?: Date;

  /**  */
  status?: WorkflowStatus;

  /**  */
  createUserIdentityName?: string;

  /**  */
  id?: string;
}

export interface Place {
  /**  */
  id?: string;

  /**  */
  uid?: string;

  /**  */
  name?: string;

  /**  */
  city?: string;

  /**  */
  cityId?: number;

  /**  */
  district?: string;

  /**  */
  province?: string;

  /**  */
  lat?: number;

  /**  */
  lng?: number;

  /**  */
  location?: Location;
}

export interface PointActivityCreateOrUpdateDto {
  /**  */
  titleImageUrl?: string;

  /**  */
  title?: string;

  /**  */
  subTitle?: string;

  /**  */
  state?: number;

  /**  */
  htmlContext?: string;

  /**  */
  viewCount?: number;

  /**  */
  helpPerDay?: number;

  /**  */
  settings?: object;

  /**  */
  datetimeStart?: Date;

  /**  */
  datetimeEnd?: Date;

  /**  */
  timeStart?: string;

  /**  */
  timeEnd?: string;

  /**  */
  minPoint?: number;

  /**  */
  maxPoint?: number;

  /**  */
  id?: number;
}

export interface PointActivityCreateOrUpdateDtoGetForEditOutput {
  /**  */
  data?: PointActivityCreateOrUpdateDto;

  /**  */
  schema?: any | null;
}

export interface PointActivityDto {
  /**  */
  titleImageUrl?: string;

  /**  */
  title?: string;

  /**  */
  subTitle?: string;

  /**  */
  state?: number;

  /**  */
  htmlContext?: string;

  /**  */
  viewCount?: number;

  /**  */
  helpPerDay?: number;

  /**  */
  settings?: object;

  /**  */
  datetimeStart?: Date;

  /**  */
  datetimeEnd?: Date;

  /**  */
  timeStart?: string;

  /**  */
  timeEnd?: string;

  /**  */
  minPoint?: number;

  /**  */
  maxPoint?: number;

  /**  */
  isEnd?: boolean;

  /**  */
  id?: number;
}

export interface PointActivityDtoPagedResultDto {
  /**  */
  totalCount?: number;

  /**  */
  items?: PointActivityDto[];
}

export interface PointActivityUserLogDto {
  /**  */
  activityId?: number;

  /**  */
  helperNum?: number;

  /**  */
  point?: number;

  /**  */
  state?: PointActivityUserState;

  /**  */
  sharedFrom?: number;

  /**  */
  id?: number;
}

export interface PointRule {
  /**  */
  count?: number;

  /**  */
  points?: number;

  /**  */
  luckTime?: number;
}

export interface PosterDto {
  /**  */
  url?: string;

  /**  */
  bgImageUrl?: string;

  /**  */
  bgWidth?: number;

  /**  */
  bgHeight?: number;

  /**  */
  mainImage?: BoxDetail;

  /**  */
  qrImage?: BoxDetail;

  /**  */
  headImage?: BoxDetail;

  /**  */
  titleText?: FontBoxDetail;

  /**  */
  descText?: FontBoxDetail;

  /**  */
  id?: number;
}

export interface PosterDtoGetForEditOutput {
  /**  */
  data?: PosterDto;

  /**  */
  schema?: any | null;
}

export interface PosterDtoPagedResultDto {
  /**  */
  totalCount?: number;

  /**  */
  items?: PosterDto[];
}

export interface ProjectNameOrganizationUnitDtoBase {
  /**  */
  displayName?: string;

  /**  */
  logoImgUrl?: string;

  /**  */
  address?: string;

  /**  */
  desc?: string;

  /**  */
  status?: number;

  /**  */
  type?: number;

  /**  */
  phone?: string;

  /**  */
  id?: number;
}

export interface ProjectNameOrganizationUnitDtoBasePagedResultDto {
  /**  */
  totalCount?: number;

  /**  */
  items?: ProjectNameOrganizationUnitDtoBase[];
}

export interface QAPlanCreateOrUpdateDto {
  /**  */
  titleImageUrl?: string;

  /**  */
  title?: string;

  /**  */
  subTitle?: string;

  /**  */
  state?: QAPlanState;

  /**  */
  type?: QAPlanType;

  /**  */
  luckDrawId?: number;

  /**  */
  htmlContext?: string;

  /**  */
  viewCount?: number;

  /**  */
  answerPerDay?: number;

  /**  */
  helpPerDay?: number;

  /**  */
  sharePoints?: number;

  /**  */
  settings?: any | null;

  /**  */
  pointRules?: PointRule[];

  /**  */
  questionCount?: number;

  /**  */
  datetimeStart?: Date;

  /**  */
  datetimeEnd?: Date;

  /**  */
  timeStart?: string;

  /**  */
  timeEnd?: string;

  /**  */
  id?: number;
}

export interface QAPlanCreateOrUpdateDtoGetForEditOutput {
  /**  */
  data?: QAPlanCreateOrUpdateDto;

  /**  */
  schema?: any | null;
}

export interface QAPlanDto {
  /**  */
  titleImageUrl?: string;

  /**  */
  title?: string;

  /**  */
  subTitle?: string;

  /**  */
  state?: QAPlanState;

  /**  */
  type?: QAPlanType;

  /**  */
  luckDrawId?: number;

  /**  */
  htmlContext?: string;

  /**  */
  viewCount?: number;

  /**  */
  answerPerDay?: number;

  /**  */
  helpPerDay?: number;

  /**  */
  sharePoints?: number;

  /**  */
  settings?: any | null;

  /**  */
  pointRules?: PointRule[];

  /**  */
  questionCount?: number;

  /**  */
  datetimeStart?: Date;

  /**  */
  datetimeEnd?: Date;

  /**  */
  timeStart?: string;

  /**  */
  timeEnd?: string;

  /**  */
  isDeleted?: boolean;

  /**  */
  deleterUserId?: number;

  /**  */
  deletionTime?: Date;

  /**  */
  lastModificationTime?: Date;

  /**  */
  lastModifierUserId?: number;

  /**  */
  creationTime?: Date;

  /**  */
  creatorUserId?: number;

  /**  */
  id?: number;
}

export interface QAPlanDtoPagedResultDto {
  /**  */
  totalCount?: number;

  /**  */
  items?: QAPlanDto[];
}

export interface QAQuestionCreateOrUpdateDto {
  /**  */
  planId?: number;

  /**  */
  title?: string;

  /**  */
  state?: number;

  /**  */
  answerIndex?: number;

  /**  */
  answerList?: string[];

  /**  */
  id?: string;
}

export interface QAQuestionCreateOrUpdateDtoGetForEditOutput {
  /**  */
  data?: QAQuestionCreateOrUpdateDto;

  /**  */
  schema?: any | null;
}

export interface QAQuestionDto {
  /**  */
  title?: string;

  /**  */
  state?: number;

  /**  */
  planId?: number;

  /**  */
  answerIndex?: number;

  /**  */
  answerList?: string[];

  /**  */
  id?: string;
}

export interface QAQuestionDtoPagedResultDto {
  /**  */
  totalCount?: number;

  /**  */
  items?: QAQuestionDto[];
}

export interface QAUserRequestDto {
  /**  */
  surname?: string;

  /**  */
  town?: string;

  /**  */
  phoneNumber?: string;
}

export interface QuestionItem {
  /**  */
  id?: string;

  /**  */
  title?: string;

  /**  */
  answerIndex?: number;

  /**  */
  answerList?: string[];

  /**  */
  userSelectIndex?: number;

  /**  */
  finishTime?: Date;
}

export interface RankListDto {
  /**  */
  creationTime?: Date;

  /**  */
  items?: RankListItem[];

  /**  */
  shareItems?: RankShareListItem[];

  /**  */
  total?: number;
}

export interface RankListItem {
  /**  */
  imageUrl?: string;

  /**  */
  name?: string;

  /**  */
  phoneNumber?: string;

  /**  */
  town?: string;

  /**  */
  userId?: number;

  /**  */
  rightCount?: number;

  /**  */
  pointCount?: number;

  /**  */
  spendTime?: number;
}

export interface RankShareListItem {
  /**  */
  imageUrl?: string;

  /**  */
  name?: string;

  /**  */
  phoneNumber?: string;

  /**  */
  town?: string;

  /**  */
  userId?: number;

  /**  */
  count?: number;
}

export interface RefundDetailDto {
  /**  */
  user?: UserDtoBase;

  /**  */
  wechatUserinfo?: WechatUserinfoDto;

  /**  */
  payOrder?: PayOrderDto;

  /**  */
  refundLog?: RefundLogDto;

  /**  */
  organizationUnit?: ProjectNameOrganizationUnitDtoBase;
}

export interface RefundLog {
  /**  */
  billNo?: string;

  /**  */
  price?: number;

  /**  */
  isSuccess?: boolean;

  /**  */
  successTime?: Date;

  /**  */
  creationTime?: Date;

  /**  */
  lastModificationTime?: Date;

  /**  */
  lastModifierUserId?: number;

  /**  */
  lastModifierUser?: User;

  /**  */
  creatorUserId?: number;

  /**  */
  creatorUser?: User;

  /**  */
  tenantId?: number;

  /**  */
  auditFlowId?: string;

  /**  */
  audit?: number;

  /**  */
  auditStatus?: number;

  /**  */
  isAudited?: boolean;

  /**  */
  id?: number;
}

export interface RefundLogDto {
  /**  */
  billNo?: string;

  /**  */
  price?: number;

  /**  */
  isSuccess?: boolean;

  /**  */
  successTime?: Date;

  /**  */
  creationTime?: Date;

  /**  */
  creatorUser?: UserDtoBase;

  /**  */
  auditFlowId?: string;

  /**  */
  audit?: number;

  /**  */
  auditStatus?: number;

  /**  */
  isAudited?: boolean;

  /**  */
  auditFlow?: AuditFlowDto;

  /**  */
  currentAuditNodes?: AuditNodeDto[];

  /**  */
  id?: number;
}

export interface RefundLogDtoGetForEditOutput {
  /**  */
  data?: RefundLogDto;

  /**  */
  schema?: any | null;
}

export interface RefundLogDtoPagedResultDto {
  /**  */
  totalCount?: number;

  /**  */
  items?: RefundLogDto[];
}

export interface RefundLogPagedResultDto {
  /**  */
  totalCount?: number;

  /**  */
  items?: RefundLog[];
}

export interface RegisterInput {
  /**  */
  name?: string;

  /**  */
  surname?: string;

  /**  */
  phoneNumber?: string;

  /**  */
  userName?: string;

  /**  */
  emailAddress?: string;

  /**  */
  password?: string;

  /**  */
  captchaResponse?: string;
}

export interface RegisterOutput {
  /**  */
  canLogin?: boolean;
}

export interface ResetPasswordDto {
  /**  */
  adminPassword?: string;

  /**  */
  userId?: number;

  /**  */
  newPassword?: string;
}

export interface RoleDto {
  /**  */
  name?: string;

  /**  */
  displayName?: string;

  /**  */
  isDefault?: boolean;

  /**  */
  isStatic?: boolean;

  /**  */
  normalizedName?: string;

  /**  */
  description?: string;

  /**  */
  grantedPermissions?: string[];

  /**  */
  id?: number;
}

export interface RoleDtoListResultDto {
  /**  */
  items?: RoleDto[];
}

export interface RoleDtoPagedResultDto {
  /**  */
  totalCount?: number;

  /**  */
  items?: RoleDto[];
}

export interface RoleEditDto {
  /**  */
  name?: string;

  /**  */
  displayName?: string;

  /**  */
  description?: string;

  /**  */
  isStatic?: boolean;

  /**  */
  isDefault?: boolean;

  /**  */
  id?: number;
}

export interface RoleListDto {
  /**  */
  name?: string;

  /**  */
  displayName?: string;

  /**  */
  isStatic?: boolean;

  /**  */
  isDefault?: boolean;

  /**  */
  creationTime?: Date;

  /**  */
  id?: number;
}

export interface RoleListDtoListResultDto {
  /**  */
  items?: RoleListDto[];
}

export interface Setting {
  /**  */
  tenantId?: number;

  /**  */
  userId?: number;

  /**  */
  name?: string;

  /**  */
  value?: string;

  /**  */
  lastModificationTime?: Date;

  /**  */
  lastModifierUserId?: number;

  /**  */
  creationTime?: Date;

  /**  */
  creatorUserId?: number;

  /**  */
  id?: number;
}

export interface StsResultDto {
  /**  */
  accessKeyId?: string;

  /**  */
  accessKeySecret?: string;

  /**  */
  securityToken?: string;

  /**  */
  bucket?: string;

  /**  */
  region?: string;
}

export interface SwiperDto {
  /**  */
  groupId?: number;

  /**  */
  swiperType?: number;

  /**  */
  imagePath?: string;

  /**  */
  title?: string;

  /**  */
  url?: string;

  /**  */
  index?: number;

  /**  */
  organizationUnitId?: number;

  /**  */
  status?: number;

  /**  */
  id?: number;
}

export interface SwiperDtoGetForEditOutput {
  /**  */
  data?: SwiperDto;

  /**  */
  schema?: any | null;
}

export interface SwiperDtoPagedResultDto {
  /**  */
  totalCount?: number;

  /**  */
  items?: SwiperDto[];
}

export interface TenantDto {
  /**  */
  tenancyName?: string;

  /**  */
  name?: string;

  /**  */
  isActive?: boolean;

  /**  */
  id?: number;
}

export interface TenantDtoPagedResultDto {
  /**  */
  totalCount?: number;

  /**  */
  items?: TenantDto[];
}

export interface TenantLoginInfoDto {
  /**  */
  tenancyName?: string;

  /**  */
  name?: string;

  /**  */
  id?: number;
}

export interface TenantSettingsEditDto {
  /**  */
  weixin?: WeixinSettingsEditDto;

  /**  */
  oss?: OssSettingEditDto;

  /**  */
  client?: ClientSettingEditDto;

  /**  */
  notify?: NotifySettingEditDto;
}

export interface TenPayNotifyXml {
  /**  */
  appid?: string;

  /**  */
  mch_id?: string;

  /**  */
  device_info?: string;

  /**  */
  nonce_str?: string;

  /**  */
  sign?: string;

  /**  */
  result_code?: string;

  /**  */
  err_code?: string;

  /**  */
  err_code_des?: string;

  /** 交易类型 */
  trade_type?: string;

  /** 付款银行 ,银行类型，采用字符串类型的银行标识，银行类型见银行列表 */
  bank_type?: string;

  /** is_subscribe 用户是否关注公众账号，Y-关注，N-未关注 */
  is_subscribe?: string;

  /**  */
  openid?: string;

  /**  */
  total_fee?: string;

  /**  */
  settlement_total_fee?: number;

  /**  */
  fee_type?: string;

  /**  */
  cash_fee?: string;

  /**  */
  cash_fee_type?: string;

  /** 微信支付订单号 */
  transaction_id?: string;

  /** 商户订单号 */
  out_trade_no?: string;

  /** 支付完成时间 */
  time_end?: string;

  /**  */
  return_code?: string;

  /**  */
  return_msg?: string;
}

export interface TimelineCategoryCreateOrUpdateDto {
  /**  */
  name?: string;

  /**  */
  imageUrl?: string;

  /**  */
  id?: number;
}

export interface TimelineCategoryCreateOrUpdateDtoGetForEditOutput {
  /**  */
  data?: TimelineCategoryCreateOrUpdateDto;

  /**  */
  schema?: any | null;
}

export interface TimelineCategoryDto {
  /**  */
  name?: string;

  /**  */
  imageUrl?: string;

  /**  */
  id?: number;
}

export interface TimelineCategoryDtoPagedResultDto {
  /**  */
  totalCount?: number;

  /**  */
  items?: TimelineCategoryDto[];
}

export interface TimelineEventCreateOrUpdateDto {
  /**  */
  categoryId?: number;

  /**  */
  titleImageUrl?: string;

  /**  */
  title?: string;

  /**  */
  subTitle?: string;

  /**  */
  state?: TimelineEventState;

  /**  */
  type?: number;

  /**  */
  htmlContext?: string;

  /**  */
  datetimeStart?: Date;

  /**  */
  datetimeEnd?: Date;

  /**  */
  settings?: any | null;

  /**  */
  auditFlowId?: string;

  /**  */
  id?: number;
}

export interface TimelineEventCreateOrUpdateDtoGetForEditOutput {
  /**  */
  data?: TimelineEventCreateOrUpdateDto;

  /**  */
  schema?: any | null;
}

export interface TimelineEventDto {
  /**  */
  categoryId?: number;

  /**  */
  timelineCategory?: TimelineCategoryDto;

  /**  */
  titleImageUrl?: string;

  /**  */
  title?: string;

  /**  */
  subTitle?: string;

  /**  */
  state?: TimelineEventState;

  /**  */
  type?: number;

  /**  */
  htmlContext?: string;

  /**  */
  datetimeStart?: Date;

  /**  */
  datetimeEnd?: Date;

  /**  */
  settings?: any | null;

  /**  */
  auditFlowId?: string;

  /**  */
  audit?: number;

  /**  */
  auditStatus?: number;

  /**  */
  isAudited?: boolean;

  /**  */
  auditFlow?: AuditFlowDto;

  /**  */
  currentAuditNodes?: AuditNodeDto[];

  /**  */
  isDeleted?: boolean;

  /**  */
  deleterUserId?: number;

  /**  */
  deletionTime?: Date;

  /**  */
  lastModificationTime?: Date;

  /**  */
  lastModifierUserId?: number;

  /**  */
  creationTime?: Date;

  /**  */
  creatorUserId?: number;

  /**  */
  id?: number;
}

export interface TimelineEventDtoPagedResultDto {
  /**  */
  totalCount?: number;

  /**  */
  items?: TimelineEventDto[];
}

export interface TimelineFileCreateOrUpdateDto {
  /**  */
  eventId?: number;

  /**  */
  type?: TimelineFileType;

  /**  */
  state?: number;

  /**  */
  sort?: number;

  /**  */
  url?: string;

  /**  */
  fileName?: string;

  /**  */
  mimeType?: string;

  /**  */
  desc?: string;

  /**  */
  id?: string;
}

export interface TimelineFileDto {
  /**  */
  eventId?: number;

  /**  */
  type?: TimelineFileType;

  /**  */
  state?: number;

  /**  */
  sort?: number;

  /**  */
  url?: string;

  /**  */
  fileName?: string;

  /**  */
  mimeType?: string;

  /**  */
  desc?: string;

  /**  */
  data?: any | null;

  /**  */
  creationTime?: Date;

  /**  */
  creatorUserId?: number;

  /**  */
  id?: string;
}

export interface TimelineFileDtoPagedResultDto {
  /**  */
  totalCount?: number;

  /**  */
  items?: TimelineFileDto[];
}

export interface UpdateOrganizationUnitInput {
  /**  */
  id?: number;

  /**  */
  displayName?: string;

  /**  */
  type?: number;

  /**  */
  detail?: OrganizationUnitDetailCreateDto;
}

export interface User {
  /**  */
  headImgUrl?: string;

  /**  */
  town?: string;

  /**  */
  jf?: number;

  /**  */
  fromClient?: number;

  /**  */
  organizationUnits?: UserOrganizationUnit[];

  /**  */
  signInTokenExpireTimeUtc?: Date;

  /**  */
  signInToken?: string;

  /**  */
  normalizedUserName?: string;

  /**  */
  normalizedEmailAddress?: string;

  /**  */
  concurrencyStamp?: string;

  /**  */
  tokens?: UserToken[];

  /**  */
  deleterUser?: User;

  /**  */
  creatorUser?: User;

  /**  */
  lastModifierUser?: User;

  /**  */
  authenticationSource?: string;

  /**  */
  userName?: string;

  /**  */
  tenantId?: number;

  /**  */
  emailAddress?: string;

  /**  */
  name?: string;

  /**  */
  surname?: string;

  /**  */
  fullName?: string;

  /**  */
  password?: string;

  /**  */
  emailConfirmationCode?: string;

  /**  */
  passwordResetCode?: string;

  /**  */
  lockoutEndDateUtc?: Date;

  /**  */
  accessFailedCount?: number;

  /**  */
  isLockoutEnabled?: boolean;

  /**  */
  phoneNumber?: string;

  /**  */
  isPhoneNumberConfirmed?: boolean;

  /**  */
  securityStamp?: string;

  /**  */
  isTwoFactorEnabled?: boolean;

  /**  */
  logins?: UserLogin[];

  /**  */
  roles?: UserRole[];

  /**  */
  claims?: UserClaim[];

  /**  */
  permissions?: UserPermissionSetting[];

  /**  */
  settings?: Setting[];

  /**  */
  isEmailConfirmed?: boolean;

  /**  */
  isActive?: boolean;

  /**  */
  isDeleted?: boolean;

  /**  */
  deleterUserId?: number;

  /**  */
  deletionTime?: Date;

  /**  */
  lastModificationTime?: Date;

  /**  */
  lastModifierUserId?: number;

  /**  */
  creationTime?: Date;

  /**  */
  creatorUserId?: number;

  /**  */
  id?: number;
}

export interface UserClaim {
  /**  */
  tenantId?: number;

  /**  */
  userId?: number;

  /**  */
  claimType?: string;

  /**  */
  claimValue?: string;

  /**  */
  creationTime?: Date;

  /**  */
  creatorUserId?: number;

  /**  */
  id?: number;
}

export interface UserDto {
  /**  */
  userName?: string;

  /**  */
  name?: string;

  /**  */
  surname?: string;

  /**  */
  emailAddress?: string;

  /**  */
  isActive?: boolean;

  /**  */
  fullName?: string;

  /**  */
  lastLoginTime?: Date;

  /**  */
  creationTime?: Date;

  /**  */
  roleNames?: string[];

  /**  */
  phoneNumber?: string;

  /**  */
  headImgUrl?: string;

  /**  */
  fromClient?: number;

  /**  */
  id?: number;
}

export interface UserDtoBase {
  /**  */
  userName?: string;

  /**  */
  name?: string;

  /**  */
  phoneNumber?: string;

  /**  */
  surname?: string;

  /**  */
  town?: string;

  /**  */
  id?: number;
}

export interface UserDtoPagedResultDto {
  /**  */
  totalCount?: number;

  /**  */
  items?: UserDto[];
}

export interface UserEditDto {
  /** Set null to create a new user. Set user's Id to update a user */
  id?: number;

  /**  */
  userName?: string;

  /**  */
  emailAddress?: string;

  /**  */
  name?: string;

  /**  */
  surname?: string;

  /**  */
  headImgUrl?: string;

  /**  */
  phoneNumber?: string;

  /**  */
  password?: string;

  /**  */
  isActive?: boolean;
}

export interface UserEventDto {
  /** 1 扫码访问 */
  eventType?: number;

  /**  */
  value?: object;

  /**  */
  creationTime?: Date;

  /**  */
  creatorUser?: UserDtoBase;

  /**  */
  fromUser?: UserDtoBase;

  /**  */
  id?: number;
}

export interface UserEventDtoPagedResultDto {
  /**  */
  totalCount?: number;

  /**  */
  items?: UserEventDto[];
}

export interface UserLogin {
  /**  */
  tenantId?: number;

  /**  */
  userId?: number;

  /**  */
  loginProvider?: string;

  /**  */
  providerKey?: string;

  /**  */
  id?: number;
}

export interface UserLoginInfoDto {
  /**  */
  name?: string;

  /**  */
  surname?: string;

  /**  */
  userName?: string;

  /**  */
  emailAddress?: string;

  /**  */
  headImgUrl?: string;

  /**  */
  jf?: number;

  /**  */
  isSubscribe?: boolean;

  /**  */
  phoneNumber?: string;

  /**  */
  town?: string;

  /**  */
  id?: number;
}

export interface UserOrganizationUnit {
  /**  */
  tenantId?: number;

  /**  */
  userId?: number;

  /**  */
  organizationUnitId?: number;

  /**  */
  isDeleted?: boolean;

  /**  */
  creationTime?: Date;

  /**  */
  creatorUserId?: number;

  /**  */
  id?: number;
}

export interface UserPermissionSetting {
  /**  */
  userId?: number;

  /**  */
  tenantId?: number;

  /**  */
  name?: string;

  /**  */
  isGranted?: boolean;

  /**  */
  creationTime?: Date;

  /**  */
  creatorUserId?: number;

  /**  */
  id?: number;
}

export interface UserPointLog {
  /**  */
  userId?: number;

  /**  */
  eventType?: UserPointEventType;

  /**  */
  eventId?: string;

  /**  */
  afterPoints?: number;

  /**  */
  beforePoints?: number;

  /**  */
  changePoints?: number;

  /**  */
  desc?: string;

  /**  */
  tenantId?: number;

  /**  */
  creationTime?: Date;

  /**  */
  creatorUserId?: number;

  /**  */
  id?: number;
}

export interface UserPointLogDto {
  /**  */
  userId?: number;

  /**  */
  user?: UserDtoBase;

  /**  */
  eventType?: UserPointEventType;

  /**  */
  eventId?: string;

  /**  */
  afterPoints?: number;

  /**  */
  beforePoints?: number;

  /**  */
  changePoints?: number;

  /**  */
  desc?: string;

  /**  */
  tenantId?: number;

  /**  */
  creationTime?: Date;

  /**  */
  id?: number;
}

export interface UserPointLogDtoPagedResultDto {
  /**  */
  totalCount?: number;

  /**  */
  items?: UserPointLogDto[];
}

export interface UserPointLogListResultDto {
  /**  */
  items?: UserPointLog[];
}

export interface UserPrizeCheckInputDto {
  /**  */
  id?: number;

  /**  */
  code?: string;
}

export interface UserPrizeDto {
  /**  */
  prizeId?: number;

  /**  */
  luckDrawId?: number;

  /**  */
  pickupWay?: PickupWay;

  /**  */
  luckDrawTitle?: string;

  /**  */
  name?: string;

  /**  */
  imageUrl?: string;

  /**  */
  count?: number;

  /**  */
  phoneNumber?: string;

  /**  */
  state?: number;

  /**  */
  qrUrl?: string;

  /**  */
  luckDraw?: LuckDrawDto;

  /**  */
  checkTime?: Date;

  /**  */
  checkCode?: string;

  /**  */
  checkUserId?: number;

  /**  */
  checkPhoneNumber?: string;

  /**  */
  expiredTime?: Date;

  /**  */
  address?: AddressDetail;

  /**  */
  creationTime?: Date;

  /**  */
  creatorUserId?: number;

  /**  */
  id?: number;
}

export interface UserPrizeDtoGetForEditOutput {
  /**  */
  data?: UserPrizeDto;

  /**  */
  schema?: any | null;
}

export interface UserPrizeDtoPagedResultDto {
  /**  */
  totalCount?: number;

  /**  */
  items?: UserPrizeDto[];
}

export interface UserPrizeExpressInput {
  /**  */
  id?: number;

  /**  */
  address?: AddressDetail;
}

export interface UserQuestionLogDto {
  /**  */
  planId?: number;

  /**  */
  state?: UserQuestionLogEnum;

  /**  */
  rightCount?: number;

  /**  */
  points?: number;

  /**  */
  userLuckTimeId?: number;

  /**  */
  shareFrom?: number;

  /**  */
  questionItems?: QuestionItem[];

  /**  */
  spendTime?: number;

  /**  */
  creationTime?: Date;

  /**  */
  creatorUserId?: number;

  /**  */
  id?: number;
}

export interface UserQuestionLogDtoPagedResultDto {
  /**  */
  totalCount?: number;

  /**  */
  items?: UserQuestionLogDto[];
}

export interface UserQuestionRequest {
  /**  */
  id?: number;

  /**  */
  questionIndex?: number;

  /**  */
  answerIndex?: number;
}

export interface UserRole {
  /**  */
  tenantId?: number;

  /**  */
  userId?: number;

  /**  */
  roleId?: number;

  /**  */
  creationTime?: Date;

  /**  */
  creatorUserId?: number;

  /**  */
  id?: number;
}

export interface UserRoleDto {
  /**  */
  roleId?: number;

  /**  */
  roleName?: string;

  /**  */
  roleDisplayName?: string;

  /**  */
  isAssigned?: boolean;
}

export interface UsersToOrganizationUnitInput {
  /**  */
  userIds?: number[];

  /**  */
  organizationUnitId?: number;
}

export interface UserToken {
  /**  */
  tenantId?: number;

  /**  */
  userId?: number;

  /**  */
  loginProvider?: string;

  /**  */
  name?: string;

  /**  */
  value?: string;

  /**  */
  expireDate?: Date;

  /**  */
  id?: number;
}

export interface VoteItemCreateOrUpdateDto {
  /**  */
  votePlanId?: number;

  /**  */
  state?: number;

  /**  */
  sort?: number;

  /**  */
  form?: any | null;

  /**  */
  auditFlowId?: string;

  /**  */
  id?: string;
}

export interface VoteItemCreateOrUpdateDtoGetForEditOutput {
  /**  */
  data?: VoteItemCreateOrUpdateDto;

  /**  */
  schema?: any | null;
}

export interface VoteItemDto {
  /**  */
  votePlanId?: number;

  /**  */
  votePlan?: VotePlanDto;

  /**  */
  state?: ListState;

  /**  */
  sort?: number;

  /**  */
  form?: any | null;

  /**  */
  auditFlowId?: string;

  /**  */
  audit?: number;

  /**  */
  auditStatus?: number;

  /**  */
  isAudited?: boolean;

  /**  */
  auditFlow?: AuditFlowDto;

  /**  */
  currentAuditNodes?: AuditNodeDto[];

  /**  */
  rejectText?: string;

  /**  */
  id?: string;
}

export interface VoteItemDtoPagedResultDto {
  /**  */
  totalCount?: number;

  /**  */
  items?: VoteItemDto[];
}

export interface VotePlanCreateOrUpdateDto {
  /**  */
  titleImageUrl?: string;

  /**  */
  title?: string;

  /**  */
  subTitle?: string;

  /**  */
  state?: VotePlanState;

  /**  */
  type?: number;

  /**  */
  htmlContext?: string;

  /**  */
  isUserUpload?: boolean;

  /**  */
  uploadStartTime?: Date;

  /**  */
  uploadEndTime?: Date;

  /**  */
  voteStartTime?: Date;

  /**  */
  voteEndTime?: Date;

  /**  */
  settings?: any | null;

  /**  */
  id?: number;
}

export interface VotePlanCreateOrUpdateDtoGetForEditOutput {
  /**  */
  data?: VotePlanCreateOrUpdateDto;

  /**  */
  schema?: any | null;
}

export interface VotePlanDto {
  /**  */
  titleImageUrl?: string;

  /**  */
  title?: string;

  /**  */
  subTitle?: string;

  /**  */
  state?: VotePlanState;

  /**  */
  type?: number;

  /**  */
  htmlContext?: string;

  /**  */
  viewCount?: number;

  /**  */
  isUserUpload?: boolean;

  /**  */
  uploadStartTime?: Date;

  /**  */
  uploadEndTime?: Date;

  /**  */
  voteStartTime?: Date;

  /**  */
  voteEndTime?: Date;

  /**  */
  settings?: any | null;

  /**  */
  id?: number;
}

export interface VotePlanDtoPagedResultDto {
  /**  */
  totalCount?: number;

  /**  */
  items?: VotePlanDto[];
}

export interface WeChatMiniProgramAuthenticateModel {
  /**  */
  code?: string;

  /**  */
  encryptedData?: string;

  /**  */
  iv?: string;

  /**  */
  session_key?: string;

  /**  */
  openid?: string;

  /**  */
  unionid?: string;

  /**  */
  appid?: string;
}

export interface WechatUserinfoDto {
  /**  */
  openid?: string;

  /**  */
  unionid?: string;

  /**  */
  nickname?: string;

  /**  */
  headimgurl?: string;

  /**  */
  city?: string;

  /**  */
  province?: string;

  /**  */
  country?: string;

  /**  */
  sex?: number;

  /**  */
  fromClient?: ClientTypeEnum;

  /**  */
  appName?: string;

  /**  */
  id?: string;
}

export interface WechatUserinfoDtoPagedResultDto {
  /**  */
  totalCount?: number;

  /**  */
  items?: WechatUserinfoDto[];
}

export interface WeixinSettingsEditDto {
  /**  */
  appId?: string;

  /**  */
  appSecret?: string;

  /**  */
  mini_AppId?: string;

  /**  */
  mini_AppSecret?: string;

  /**  */
  pay_MchId?: string;

  /**  */
  pay_Key?: string;

  /**  */
  pay_Notify?: string;

  /**  */
  tenPay_AppId?: string;

  /**  */
  tenPay_AppSecret?: string;

  /**  */
  tenPay_RefundAccount?: string;
}

export interface WorkflowDto {
  /**  */
  executionErrors?: ExecutionErrorDto[];

  /**  */
  workflowDefinitionId?: string;

  /**  */
  version?: number;

  /**  */
  description?: string;

  /**  */
  reference?: string;

  /**  */
  executionPointers?: PersistedExecutionPointer[];

  /**  */
  nextExecution?: number;

  /**  */
  nextExecutionTime?: Date;

  /**  */
  data?: string;

  /**  */
  createTime?: Date;

  /**  */
  completeTime?: Date;

  /**  */
  status?: WorkflowStatus;

  /**  */
  createUserIdentityName?: string;

  /**  */
  id?: string;
}

export interface WorkflowDtoPagedResultDto {
  /**  */
  totalCount?: number;

  /**  */
  items?: WorkflowDto[];
}

export enum AuditFlowType {
  'AudtitOne' = 'AudtitOne',
  'AuditAll' = 'AuditAll'
}

export enum AuditUserLogStatus {
  'Reject' = 'Reject',
  'Pass' = 'Pass',
  'Back' = 'Back',
  'Continue' = 'Continue'
}

export enum ClientTypeEnum {
  'System' = 'System',
  'WeixinMini' = 'WeixinMini',
  'Weixin' = 'Weixin'
}

export enum CmsContentLinkType {
  '不跳转' = '不跳转',
  '跳转到小程序内容' = '跳转到小程序内容',
  '跳转到小程序路径' = '跳转到小程序路径',
  '公众号图文消息' = '公众号图文消息',
  'H5' = 'H5'
}

export enum CmsContentStatus {
  'Draft' = 'Draft',
  'Published' = 'Published'
}

export enum EventTypeEnum {
  'Favorite' = 'Favorite',
  'Share' = 'Share',
  'Zan' = 'Zan'
}

export enum ListState {
  '草稿' = '草稿',
  '退回' = '退回',
  '已提交/待审核' = '已提交/待审核',
  '审核通过' = '审核通过',
  '不通过' = '不通过'
}

export enum LuckDrawType {
  'Default' = 'Default',
  'Points' = 'Points',
  'UserLuckyTimes' = 'UserLuckyTimes'
}

export enum OrderType {
  'Default' = 'Default'
}

export enum PayType {
  '微信' = '微信',
  '微信扫码' = '微信扫码',
  '支付宝' = '支付宝',
  '银联' = '银联',
  '用户余额' = '用户余额'
}

export enum PickupWay {
  'Qr' = 'Qr',
  'Express' = 'Express'
}

export enum PointActivityUserState {
  '未完成' = '未完成',
  '已发放' = '已发放'
}

export enum PointerStatus {
  'Legacy' = 'Legacy',
  'Pending' = 'Pending',
  'Running' = 'Running',
  'Complete' = 'Complete',
  'Sleeping' = 'Sleeping',
  'WaitingForEvent' = 'WaitingForEvent',
  'Failed' = 'Failed',
  'Compensated' = 'Compensated',
  'Cancelled' = 'Cancelled',
  'PendingPredecessor' = 'PendingPredecessor'
}

export enum QAPlanState {
  '关闭' = '关闭',
  '开启' = '开启'
}

export enum QAPlanType {
  '未设置' = '未设置',
  '积分奖励' = '积分奖励',
  '抽奖' = '抽奖'
}

export enum RecomandState {
  '已提交' = '已提交',
  '审核中' = '审核中',
  '推荐成功' = '推荐成功',
  '推荐失败' = '推荐失败'
}

export enum SalesSummaryDatePeriod {
  'Daily' = 'Daily',
  'Weekly' = 'Weekly',
  'Monthly' = 'Monthly'
}

export enum TenantAvailabilityState {
  'Available' = 'Available',
  'InActive' = 'InActive',
  'NotFound' = 'NotFound'
}

export enum TimelineEventState {
  '草稿' = '草稿',
  '已发布' = '已发布',
  '置顶' = '置顶'
}

export enum TimelineFileType {
  'Unknown' = 'Unknown',
  'Image' = 'Image',
  'Doc' = 'Doc',
  'Video' = 'Video'
}

export enum UserPointEventType {
  'Normal' = 'Normal',
  'Mall' = 'Mall',
  'Activity' = 'Activity',
  'Register' = 'Register',
  'PointQr' = 'PointQr',
  'QA' = 'QA'
}

export enum UserQuestionLogEnum {
  '未完成' = '未完成',
  '完成答题' = '完成答题',
  '已领奖' = '已领奖'
}

export enum VotePlanState {
  '关闭' = '关闭',
  '开启' = '开启'
}

export enum WorkflowStatus {
  'Runnable' = 'Runnable',
  'Suspended' = 'Suspended',
  'Complete' = 'Complete',
  'Terminated' = 'Terminated'
}
