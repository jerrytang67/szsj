import axios from 'axios'
import { Message } from 'element-ui'
import { UserModule } from '@/store/modules/user'
import { getOu } from './cookies';

const service = axios.create({
  baseURL: process.env.VUE_APP_BASE_API,
  timeout: 1200000
})

// Request interceptors
service.interceptors.request.use((config: any) => {
  config.headers['Abp.OrganizationUnitId'] = UserModule.ouId || getOu();
  config.headers["Authorization"] = `Bearer ${UserModule.getToken}`;
  config.headers["Content-Type"] = 'application/json';
  config.headers['.AspNetCore.Culture'] = 'c=zh-Hans|uic=zh-Hans'
  config.headers['Abp.TenantId'] = 1
  return config
},
  (error: any) => {
    Promise.reject(error)
  }
)

// Response interceptors
service.interceptors.response.use(
  (response: any) => {
    if (isAbpResponse(response)) {
      return doAbpResponse(response).then(abpRes => {
        return abpRes
      }, error => {
        Message({
          message: error,
          type: 'error',
          duration: 5 * 1000
        })
        return error
      })
    } else {
      return response;
    }
  },
  (err: any) => {
    // var axiosConfig = err.response.config;
    // if (err.response.status === 401) {
    //   console.log("axios error status is 401");
    //   // if already refreshing don't refresh
    //   if (!refreshing) {
    //     refreshing = true;
    //     console.log("to refresh the token");
    //     return auth.signinSilent().then(user => {
    //       console.log(user);
    //       if (user) {
    //         axios.defaults.headers.common["Authorization"] = `Bearer ${user.access_token}`;
    //         axiosConfig.headers["Authorization"] = `Bearer ${user.access_token}`;
    //       }
    //       refreshing = false;
    //       // retry the http request
    //       return axios(axiosConfig);
    //     }, (error: any) => {
    //       console.log(error);
    //       refreshing = false;
    //     });
    //   }
    // }
    console.log('%chttp response error', 'color:red;')
    console.log(err)
    if (isAbpResponse(err.response)) {
      // if (error.response.data.unAuthorizedRequest === true) {
      //   store.dispatch("user/logout").then(() => {
      //     return Promise.reject(error.response.data.error)
      //   });
      // }
      // if (err.response.data.unAuthorizedRequest) {
      //   auth.logout();
      //   return;
      // }
      if (err.response.data.error.validationErrors && err.response.data.error.validationErrors.length > 0) {
        const message = [];
        message.push(`<h4>请求参数未能通过验证</h4>`);
        message.push('<ul style="padding-left:20px">');
        err.response.data.error.validationErrors.forEach((errItem: any) => message.push(`<li style="line-height: 16px;">${errItem.message}</li>`));
        message.push('</ul>');
        Message({
          message: message.join('\n'),
          type: 'error',
          dangerouslyUseHTMLString: true,
          duration: 5 * 1000
        })
      } else {
        Message({
          message: err.response.data.error.details || err.response.data.error.message,
          type: 'error',
          duration: 5 * 1000
        })
      }


      return Promise.reject(err.response.data.error)
    } else {
      Message({
        message: err.message,
        type: 'error',
        duration: 5 * 1000
      })
      return Promise.reject(err)
    }
  },
)


function isAbpResponse(response: any) {
  return response && response.data && response.data.__abp
}

function doAbpResponse(response: any) {
  return new Promise((resolve, reject) => {
    if (response.data.success === true) {
      let _response = response;
      _response.data = response.data.result
      resolve(_response)
    } else {
      //todo:处理ABP错误
      reject(response.data.error.message)
    }
  })
}


export default service
