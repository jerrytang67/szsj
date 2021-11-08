import Vue from 'vue'
import Vuex from 'vuex'
Vue.use(Vuex)

interface StoreType {
    app: any;
    user: any;
    permission: any;
    settings: any;
    session: any;
}

// Declare empty store first, dynamically register all modules later.
export default new Vuex.Store<StoreType>({})
