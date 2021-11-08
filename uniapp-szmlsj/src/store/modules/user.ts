import { VuexModule, Module, Mutation, Action, getModule } from 'vuex-module-decorators'
import store from '@/store'
import api from '@/utils/api';

export interface IUser {
    id?: number;
    isAuthenticated?: boolean;
    userName?: string;
    name?: string;
    surname?: string;
    headImgUrl?: string;
    phoneNumber?: string;
    phoneNumberConfirmed?: boolean;
    emailAddress?: string;
    tenantId?: string;
    roles?: string[];
}


export interface IUserInfo {
    avatarUrl?: string;
    city?: string;
    country?: string;
    gender?: number;
    language?: string;
    nickName?: string;
    openid?: string;
    province?: string;
    unionid?: string;
}

export interface IShopMember {
    Id?: number;
    Balance?: number;
    DateTimeCreate?: Date;
    Name?: string;
    OrderCount?: number;
    StoreId?: number;
    Telphone?: string;
    nickname?: string;
    openid?: string;
    unionid?: string;
}

@Module({ dynamic: true, store, name: 'user' })
class User extends VuexModule {

    private user: IUser = {}

    private roles: string[] = []

    private openid: string = uni.getStorageSync("openid")

    private unionid: string = uni.getStorageSync("unionid")

    private userInfo: IUserInfo = uni.getStorageSync("userInfo") || {
        openid: "",
        unionid: ""
    };
    private token: string = uni.getStorageSync("token") || "";
    private sessionKey: string = uni.getStorageSync("sessionKey") || "";
    private phone: string = uni.getStorageSync("phone") || "";

    get getOpenid() {
        return this.openid;
    }

    get getUnionid() {
        return this.unionid;
    }

    get getUser() {
        return this.user;
    }

    get getRoles() {
        return this.roles;
    }

    get isLogin() {
        return (this.user!.id! > 0) ? true : false
    }

    get getUserInfo() {
        return this.userInfo;
    }

    get getPhone() {
        return this.phone;
    }

    get getToken() {
        return this.token;
    }

    get getSessionKey() {
        return this.sessionKey;
    }

    @Mutation
    private SET_OPENID(payload: string) {
        uni.setStorageSync("openid", payload);
        this.openid = payload
    }
    @Mutation
    private SET_UNIONID(payload: string) {
        uni.setStorageSync("unionid", payload);
        this.unionid = payload
    }

    @Mutation
    private SET_USER(payload: IUser) {
        uni.setStorageSync("userid", payload.id);
        this.user = payload
    }

    @Mutation
    private SET_USERINFO(v: IUserInfo) {
        uni.setStorageSync("userInfo", v);
        if (v.openid)
            uni.setStorageSync("openid", v.openid);
        if (v.unionid)
            uni.setStorageSync("unionid", v.unionid);
        this.userInfo = v;
    }

    @Mutation
    private SET_SESSIONKEY(v: string) {
        uni.setStorageSync("sessionKey", v);
        this.sessionKey = v;
    }
    @Mutation
    private SET_TOKEN(v: string) {
        uni.setStorageSync("token", v);
        this.token = v;
    }

    @Mutation
    private SET_PHONE(payload: string) {
        uni.setStorageSync("phone", payload);
        this.phone = payload;
    }

    @Mutation
    private SET_ROLES(roles: string[]) {
        this.roles = roles;
    }

    @Mutation
    private LOGOUT() {
        console.log("mutaction:LOGOUT")
        uni.removeStorageSync("token");
        uni.removeStorageSync("userInfo");
        uni.removeStorageSync("userid");
        this.token = "";
        this.userInfo = {
            openid: "",
            unionid: ""
        };
        this.user = {};
    }

    @Action
    public PhonLogin(data: { iv: string, encryptedData: string }) {
        return new Promise(async (resolve, reject) => {

            // var session = UserModule.getSessionKey;
            // if (!session) {
            await UserModule.Code2Session();
            // }
            console.log("PhoneLogin")

            await api.phoneAuth({
                openid: UserModule.getOpenid,
                unionid: UserModule.getUnionid,
                iv: data.iv,
                encryptedData: data.encryptedData,
                session_key: UserModule.getSessionKey,
            }).then(async (res: any) => {
                if (res.accessToken) {
                    await this.SET_TOKEN(res.accessToken)

                    if (res.user) {
                        await this.SET_USER(res.user);
                        if (res.user.phoneNumber)
                            await this.SET_PHONE(res.user.phoneNumber);
                    }

                    if (res.user && res.user.weChatUserLogin) {
                        await this.SET_USERINFO(res.user.weChatUserLogin)
                    }
                    if (res.roleNames) {
                        await this.SET_ROLES(res.roleNames)
                    }

                    await this.CheckLogin();
                    return resolve(res);
                } else {
                    return reject("获取登录失败");
                }
            }, err => {
                return reject(err)
            });
        });
    }


    @Action
    public Login(v: any = null) {
        return new Promise(async (resolve, reject) => {
            wx.getUserProfile({
                desc: "用于完善会员资料", // 声明获取用户个人信息后的用途，后续会展示在弹窗中，请谨慎填写
                success: async (res: any) => {
                    await UserModule.Code2Session();
                    console.log(res);
                    api.client_miniAuth({
                        openid: UserModule.getOpenid,
                        unionid: UserModule.getUnionid,
                        session_key: UserModule.getSessionKey,
                        iv: res.iv,
                        encryptedData: res.encryptedData,
                    }).then(async (res: any) => {
                        if (res.accessToken) {
                            await this.SET_TOKEN(res.accessToken)

                            if (res.user) {
                                await this.SET_USER(res.user);
                                if (res.user.phone)
                                    await this.SET_PHONE(res.user.phone);
                            }

                            if (res.user && res.user.weChatUserLogin) {
                                await this.SET_USERINFO(res.user.weChatUserLogin)
                            }
                            if (res.roleNames) {
                                await this.SET_ROLES(res.roleNames)
                            }

                            await this.CheckLogin();
                            return resolve(res);
                        } else {
                            return reject("获取登录失败");
                        }
                    });
                },
            });
        });
    }

    @Action
    public async Code2Auth(code: string) {
        return new Promise(async (resolve, reject) => {
            await api.oAuth({ code }).then((res: any) => {
                if (res.accessToken) {
                    UserModule.SET_TOKEN(res.accessToken);
                    if (res.user) {
                        this.SET_USER(res.user);
                        if (res.user.phone)
                            this.SET_PHONE(res.user.phone);
                    }
                    if (res.user && res.user.weChatUserLogin) {
                        this.SET_USERINFO(res.user.weChatUserLogin)
                        this.SET_SESSIONKEY(res.user.weChatUserLogin.session_key)
                    }
                    if (res.roleNames) {
                        this.SET_ROLES(res.roleNames)
                    }
                    return resolve(res.user);
                }
                return reject();
            });
        })
    }

    @Action
    public async PasswordLogin(data: { username: string, password: string }) {
        return new Promise(async (resolve, reject) => {

            await api.tokenAuth({ userNameOrEmailAddress: data.username, password: data.password }).then(async (res: any) => {
                if (res.accessToken) {
                    await UserModule.SET_TOKEN(res.accessToken);

                    await this.CheckLogin();
                    return resolve(this.user);
                }
                return reject();
            });
        })
    }

    // Logout
    @Action
    public Logout() {
        this.LOGOUT();
    }

    @Action
    public Code2Session() {
        uni.login({
            provider: "weixin",
            success: (loginRes) => {
                console.log("loginRes:", loginRes);
                if (loginRes.errMsg === "login:ok" && loginRes.code) {
                    console.log("code", loginRes.code);
                    api.code2session({ code: loginRes.code }).then((res: any) => {
                        console.log("code2session:", res)
                        if (res.openid)
                            this.SET_OPENID(res.openid)
                        if (res.session_key)
                            this.SET_SESSIONKEY(res.session_key)
                        if (res.unionid)
                            this.SET_UNIONID(res.unionid)
                    });
                }
            },
        });
        console.log("Code2Session complate")
    }

    @Action
    public async CheckLogin() {
        await api.checkLogin().then((res: any) => {
            if (res && res.user) {
                this.SET_USER(res.user);
                if (res.user.phoneNumber && res.user.phoneNumberConfirmed)
                    this.SET_PHONE(res.user.phoneNumber);

                if (res.roles) {
                    this.SET_ROLES(res.roles)
                }
            }
            else {
                console.log("notlogin... to logout")
                console.log(res)
                this.LOGOUT();
            }
        }).catch(() => {
            console.log("notlogin")
            this.LOGOUT();
        })
    }

    @Action
    SetPhone(phone: string) {
        this.SET_PHONE(phone);
    }
}

export const UserModule = getModule(User)
