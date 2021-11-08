import { VuexModule, Module, Mutation, Action, getModule } from 'vuex-module-decorators'
import store from '@/store'

@Module({ dynamic: true, store, name: 'session' })
class session extends VuexModule {


    @Mutation
    private LOGIN(payload: any) {
        console.log("SESSION:LOGIN", payload)
    }

    @Action
    public login() {
        console.log("session/login");
        this.LOGIN({ openid: "123" })
    }
}

export const SessionModule = getModule(session)
