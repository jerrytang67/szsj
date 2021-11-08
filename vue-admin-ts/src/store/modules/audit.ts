import { VuexModule, Module, Mutation, Action, getModule } from 'vuex-module-decorators'
import store from '@/store'
import elementVariables from '@/styles/element-variables.scss'
import defaultSettings from '@/settings'

interface IPanelDetail {
    name: string, count: number
}


export interface IAuditState {
    PanelDetail: {}
}

@Module({ dynamic: true, store, name: 'audits' })
class Audits extends VuexModule implements IAuditState {

    public PanelDetail: any = {
        "craftsmanRecommendAuditPanel": { name: "craftsmanRecommendAuditPanel", count: 0 },
        "craftsmanAuditPanel": { name: "craftsmanAuditPanel", count: 0 },

    };

    get getPanelDetail() {
        return this.PanelDetail;
    }

    @Mutation
    private SET_PANEL(payload: IPanelDetail) {
        this.PanelDetail[payload.name] = payload;
    }

    @Action
    public SetPanel(payload: IPanelDetail) {
        this.SET_PANEL(payload)
    }
}

export const AuditsModule = getModule(Audits)
