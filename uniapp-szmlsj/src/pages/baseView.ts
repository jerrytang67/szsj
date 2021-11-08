import { Vue, Component, Prop, Watch } from "vue-property-decorator";
import { UserModule } from '@/store/modules/user';
import { Tips } from '@/utils/tips';
import api from '@/utils/api';
import { AppModule } from '@/store/modules/app';

const tabPages = [
    "pages/index/index",
    "pages/index/my",
]

interface IMenu {
    width: number,
    height: number,
    top: number,
    right: number,
    bottom: number,
    left: number
}

@Component
export class BaseView extends Vue {
    public needLogin = false;

    public chkAgree = true;

    public modalName: string = "";

    public loginBtnText: string = "登录";

    // get token() {
    //     return UserModule.getToken;
    // }

    get openid() {
        return UserModule.getOpenid;
    }

    get darkMode() {
        return AppModule.darkMode;
    }

    get isLogin() {
        return !!UserModule.getToken
    }


    get setting() {
        return AppModule.getSetting;
    }


    isTabPage(page: string) {
        let _result = false;
        tabPages
            .map(x => x.toLowerCase())
            .forEach(x => {
                if (page.indexOf(x) > -1)
                    _result = true;
            });
        return _result;
    }



    get userinfo() {
        return UserModule.getUserInfo;
    }


    phoneCall(number: string, successFunc?: Function, errorFunc?: Function, completeFunc?: Function) {
        uni.makePhoneCall({
            phoneNumber: number,
            success: (res) => {
                if (successFunc)
                    successFunc(res);
            },
            fail: (res) => {
                if (errorFunc)
                    errorFunc(res);
            },
            complete: (res) => {
                if (completeFunc)
                    completeFunc(res);
            }
        })
    }

    menu: IMenu | null = null;

    created() {
        console.log("baseview created");
        this.menu = uni.getMenuButtonBoundingClientRect();

    }

    onShow() {
        // 判断当前页是否需要登录
        console.log("baseView onShow,needLogin:", this.needLogin)
        this.checkLogin();
    }

    public checkLogin(forceCheck = false, backHome = true) {
        if ((this.needLogin && !UserModule.getToken) || forceCheck === true) {
            uni.showModal({
                content: "需要登录后才能继续",
                success: e => {
                    if (e.confirm) {
                        let pages: any[] = getCurrentPages() || [];
                        setTimeout(() => {
                            this.toLogin(pages[pages.length - 1].route, pages[pages.length - 1].options);
                        }, 200);
                    }
                    else {
                        if (backHome)
                            this.toHome();
                        // uni.navigateBack({});
                    }
                }
            });
            return false
        }
        return true;
    }

    toggleDark() {
        AppModule.ToggleDarkMode();
    }


    initUser() {

    }

    bindGetUserInfo(back: boolean = false) {
        if (!this.chkAgree) {
            Tips.info("请先勾选同意后再进行登录");
            return;
        }
        //用户按了允许授权按钮
        UserModule.Login()
            .then((res: any) => {
                console.log("getUserInfo", res);
                if (back) { uni.navigateBack({}) }
            });
    };


    getphonenumber(e: any, back: boolean) {
        if (e.mp.detail.errMsg === "getPhoneNumber:ok") {
            UserModule.PhonLogin({
                iv: e.mp.detail.iv,
                encryptedData: e.mp.detail.encryptedData
            }).then(res => {
                if (back) { uni.navigateBack({}) }
            });

        } else if (e.mp.detail.errMsg === "getPhoneNumber:fail user deny") {
            uni.showToast({
                // icon: "error",
                title: "手机登录失败",
            });
        } else {
            uni.showToast({
                icon: "none",
                title: e.mp.detail.errMsg,
            });
        }
    }

    toLogin(url: string, options: any) {
        let param = "?"
        if (options)
            Object.keys(options).forEach(key => {
                param += `${key}=${options[key]}&`
            })
        let redirectUrl = encodeURIComponent(`${url}${param.substring(0, param.length - 1)}`)
        // uni.removeStorageSync("redirectUrl");
        uni.navigateTo({ url: `/pages/index/login?url=${redirectUrl}` })
    }

    toBack() {
        uni.navigateBack({});
    }

    navTo(page: string) {
        if (page) {
            if (this.isTabPage(page))
                uni.switchTab({ url: page });
            else
                uni.navigateTo({ url: page });
        }
        else {
            Tips.info("暂未开放")
        }
    }

    toHome() {
        uni.switchTab({ url: "/pages/index/index" })
    }

    toMy() {
        uni.switchTab({ url: "/pages/index/my" })
    }

    showModal(e: any) {
        console.log(e);
        this.modalName = e.currentTarget.dataset.target
    }

    hideModal() {
        this.modalName = ""
    }

    showPolicy(type: number) {
        api.getSetting().then((res: any) => {
            if (type === 1)
                uni.showModal({
                    title: "用户使用协议",
                    content: res.userAgreement,
                    showCancel: false,
                    confirmText: "同意",
                });
            else if (type === 2)
                uni.showModal({
                    title: "隐私政策",
                    content: res.privacyPolicy,
                    showCancel: false,
                    confirmText: "同意",
                });
        });
    }


    cmsGo(c: any) {
        console.log("cmsGo", c)
        if (c.linkType === "公众号图文消息") {
            // #ifdef MP-WEIXIN
            uni.navigateTo({ url: `/pages/index/webView?id=${c.id}` });
            // #endif
            // #ifdef H5
            window.open(c.linkUrl, "_blank");
            // #endif
        }
        if (c.linkType === "H5") {
            // #ifdef MP-WEIXIN
            uni.navigateTo({ url: `/pages/index/h5?id=${c.id}` });
            // #endif
            // #ifdef H5
            window.open(c.linkUrl, "_blank");
            // #endif
        }
        if (c.linkType === "跳转到小程序内容") {
            if (c.id > 0)
                uni.navigateTo({ url: `/pages/cms/content?id=${c.id}` });
        }
        if (c.linkType === "跳转到小程序路径") {
            if (c.linkUrl)
                uni.navigateTo({ url: `${c.linkUrl}` });
        }

    }

    cmsMore(pid: number) {
        console.log("cmsMore", pid)
        uni.setStorageSync("pid", pid);
        this.navTo(`/pages/cms/category?id=${pid}`)
        // uni.navigateTo({ url: `/pages/cms/category?id=${pid}` });
    }
}

export default {
    BaseView
}