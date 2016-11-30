import * as Vuex from 'vuex'
import mutations from './mutations'
import * as _ from 'lodash'
import { Identity } from './identity'

export default new Vuex.Store({
  state: {
    authorized: false,
    
    identity: <Identity>{
      name: '',
      fullName: '',
      roles: [],
    }

  },
  mutations: {
    [mutations.AUTHORIZE] (state, authorized) {
      state.authorized = authorized
    },

    [mutations.IDENTITY] (state, identity: Identity) {
      _.assign(state.identity, identity);
    }
  }
})