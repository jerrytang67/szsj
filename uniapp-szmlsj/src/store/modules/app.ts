import { VuexModule, Module, Mutation, Action, getModule } from 'vuex-module-decorators'
import store from '@/store'
import api from '@/utils/api'

const DARKMODE = "darkMode";

@Module({ dynamic: true, store, name: 'app' })
class App extends VuexModule {

    public darkMode = uni.getStorageSync(DARKMODE) || false;

    public loading = false;
    get getLoading() { return this.loading }


    private setting: any = { data: {} }

    get getDarkMode() {
        return this.darkMode;
    }

    get getSetting() {
        return this.setting || { data: {} }
    }

    @Mutation
    async TOGGLE_DARKMODE() {
        this.darkMode = !this.darkMode;
        await uni.setStorageSync(DARKMODE, this.darkMode);
    }


    @Mutation
    SET_SETTING(payload: any) {
        this.setting = Object.assign({}, payload);
    }

    @Action
    public ToggleDarkMode() {
        this.TOGGLE_DARKMODE();
    }

    @Action
    public async GetSetting() {
        await api.getSetting().then(res => {
            this.SET_SETTING(res);
        })
    }

    @Action
    public async Init(data: any = { "maxResultCount": 50 }) {
        // await api.init(data).then(async (res: any) => {
        //     console.log(res);
        // });

    }
}

export const AppModule = getModule(App)
