import { VuexModule, Module, Mutation, Action, getModule } from 'vuex-module-decorators'
import store from '@/store'
import api from '@/utils/api';

@Module({ dynamic: true, store, name: 'organization' })
class organization extends VuexModule {

    private organizationList: any[] = []

    private organization: any = null;

    get List() { return this.organizationList }

    get current() { return this.organization; }

    @Mutation
    private SET_LIST(payload: any[]) {
        this.organizationList = payload;
    }

    @Mutation
    private SET_CURRENT(payload: any) {
        this.organization = payload;
    }

    @Action
    public async GetAll(param: any) {
        // await api.ou_getAll(param).then((res: any) => {
        //     this.SET_LIST(res.items);
        // })
    }

    @Action
    public async Get(id: number) {
        // await api.ou_get({ id }).then((res: any) => {
        //     this.SET_CURRENT(res);
        // })
    }
}

export const OrganizationModule = getModule(organization)
