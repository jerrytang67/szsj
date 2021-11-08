import utils from "./utils";

let host = process.env.VUE_APP_BASE_API;
// host = 'http://192.168.3.50:21031';
// host = 'http://gitlab.somall.top:21031';

const getRequest = utils.httpsPromisify(uni.request);

const request = (
    method: 'OPTIONS' | 'GET' | 'HEAD' | 'POST' | 'PUT' | 'DELETE' | 'TRACE' | 'CONNECT',
    url: string,
    data?: string | object | ArrayBuffer | undefined) => {

    uni.showLoading({});
    uni.showNavigationBarLoading();

    const _url = (url.startsWith("http") ? url : host + url);

    // method为请求方法，url为接口路径，data为传参
    return getRequest({
        url: _url,
        data: data,
        method: method,
        timeout: 3000000,
        header: {
            // @ts-ignore
            // "Abp.TenantId": window.$tenantId,
            "Abp.TenantId": 1,
            "content-type": "application/json",
            "Authorization": `Bearer ${uni.getStorageSync("token") || ''}`,

            // #ifdef MP
            "AppName": "SZMLSJ_MINI",
            // #endif

            // #ifdef H5
            // @ts-ignore
            "AppName": "SZMLSJ_H5"
            // #endif
        }
    });
};

export default {

    getSetting: () => request("GET", "/api/services/LaborUnion/Client/GetSettings"),


    //小程序
    client_miniAuth: (data: any) => request("POST", `/api/TokenAuth/WeixinMiniAuthenticate`, data),
    phoneAuth: (data: any) => request("POST", `/api/TokenAuth/WeixinMiniPhoneAuthenticate`, data),
    getPhone: (data: any) => request("POST", `/api/app/weixin/getPhone`, data),
    code2session: (data: any) => request("GET", `/api/services/app/Client/minicode2session`, data),


    //H5
    tokenAuth: (data: any) => request('POST', `/api/TokenAuth/Authenticate`, data),

    // 公众号H5
    getJssdk: (data: any) => request('GET', `/api/services/app/Client/GetJssdk`, data),
    oAuth: (data: any) => request('GET', `/api/TokenAuth/WeixinAuthenticate`, data),

    // user 
    checkLogin: () => request("GET", `/api/services/app/Session/GetCurrentLoginInformations`),
    public_updateUserProfile: (data: any) => request('POST', `/api/app/public/updateUserProfile`, data),

    //swiper
    swiper_getList: (data: any) => request("GET", `/api/services/app/Swiper/GetByGroupId`, data),

    //cms
    getAllCmsCategory: () => request("GET", `/api/services/app/CmsCategory/GetAll`),
    getAllCmsContent: (data: any) => request("GET", `/api/services/app/CmsContent/GetAllPublish`, data),
    getCmsCategory: (data: any) => request("GET", `/api/services/app/CmsCategory/Get`, data),
    getCmsContent: (data: any) => request("GET", `/api/services/app/CmsContent/Get`, data),


    //craftsman
    getEditCraftsmanRecommand: (data: any) => request("GET", `/api/services/LaborUnion/CraftsmanRecommend/GetForEdit`, data),
    createCraftsmanRecommand: (data: any) => request("POST", `/api/services/LaborUnion/CraftsmanRecommend/Create`, data),
    updateCraftsmanRecommand: (data: any) => request("PUT", `/api/services/LaborUnion/CraftsmanRecommend/Update`, data),
    getAllMyCraftsmanRecommand: (data: any) => request("GET", `/api/services/LaborUnion/CraftsmanRecommend/GetAllMy`, data),
    getRecommendRedPacket: (data: any) => request("GET", `/api/services/LaborUnion/CraftsmanRecommend/GetRedpacket`, data),
    getCraftsmanRedPacket: (data: any) => request("GET", `/api/services/LaborUnion/Craftsman/GetRedpacket`, data),

    getEditCraftsman: (data: any) => request("GET", `/api/services/LaborUnion/Craftsman/GetForEdit`, data),
    createCraftsman: (data: any) => request("POST", `/api/services/LaborUnion/Craftsman/Create`, data),
    updateCraftsman: (data: any) => request("PUT", `/api/services/LaborUnion/Craftsman/Update`, data),


    //activity
    getPointActivity: (data: any) => request("GET", `/api/services/Activity/PointActivity/Get`, data),
    getPoint: (data: any) => request("POST", `/api/services/Activity/PointActivity/PostGetPoint`, data),

    //userPointLog
    getMyLogs: (data: any) => request("GET", `/api/services/Activity/UserPointLog/GetMyLogs`, data),

    //luckDraw
    getLuckDraw: (data: any) => request("GET", `/api/services/Activity/LuckDraw/Get`, data),
    luckDraw: (data: any) => request("GET", `/api/services/Activity/LuckDraw/LuckDraw`, data),

    getUserPrize: (data: any) => request("GET", `/api/services/Activity/UserPrize/Get`, data),
    getAllUserPrize: (data: any) => request("GET", `/api/services/Activity/UserPrize/GetAll`, data),
    getAllMyUserPrize: (data: any) => request("GET", `/api/services/Activity/UserPrize/GetAllMy`, data),
    getCheckQr: (data: any) => request("GET", `/api/services/Activity/UserPrize/GetCheckQr`, data),
    userPrizeSetExpress :(data: any) => request("POST", `/api/services/Activity/UserPrize/SetExpress`, data),

    //vote
    getVotePlan: (data: any) => request("GET", `/api/services/Activity/VotePlan/Get`, data),
    getForEditFromPlan: (data: any) => request("GET", `/api/services/Activity/VoteItem/GetForEditFromPlan`, data),
    createVoteItem: (data: any) => request("POST", `/api/services/Activity/VoteItem/Create`, data),
    updateVoteItem: (data: any) => request("PUT", `/api/services/Activity/VoteItem/Update`, data),

    //qa
    getQAPlan: (data: any) => request("GET", `/api/services/QA/QAPlan/Get`, data),
    checkUserInfo: () => request("GET", `/api/services/QA/QAPlan/CheckUserInfo`),
    postUserInfo: (data: any) => request("POST", `/api/services/QA/QAPlan/PostUserInfo`, data),

    getUserQuestionLog: (data: any) => request("GET", `/api/services/QA/UserQuestionLog/Get`, data),
    getUserQuestion: (data: any) => request("GET", `/api/services/QA/UserQuestionLog/GetUserQuestion`, data),
    postUserQuestion: (data: any) => request("POST", `/api/services/QA/UserQuestionLog/PostUserQuestion`, data),
    getPoints: (data: any) => request("GET", `/api/services/QA/UserQuestionLog/GetPoints`, data),

    getRankList: (data: any) => request("GET", `/api/services/QA/UserQuestionLog/GetRankList`, data),


    //timeline
    getAllEvent: (data: any) => request("GET", `/api/services/Timeline/TimelineEvent/GetAll`, data),
    getEvent: (data: any) => request("GET", `/api/services/Timeline/TimelineEvent/Get`, data),

    getAllFile: (data: any) => request("GET", `/api/services/Timeline/TimelineFile/GetAll`, data),
    postAddFiles: (data: any) => request("POST", `/api/services/Timeline/TimelineFile/PostAddFiles`, data),





    userPriceCheck: (data: any) => request("POST", `/api/services/Activity/UserPrize/Check`, data),

};