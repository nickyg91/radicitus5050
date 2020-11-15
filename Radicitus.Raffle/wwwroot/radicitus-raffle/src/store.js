import Vue from "vue";
import Vuex from "vuex";
Vue.use(Vuex);
export default new Vuex.Store({
    state: {
        currentUser: "",
        selectedRaffle: {}
    },
    getters: {
        currentUser: state => {
            return state.currentUser;
        },
        selectedRaffle: state => {
            return state.selectedRaffle;
        }
    },
    mutations: {
        setUser: (state, userName) => {
            state.currentUser = userName;
        },
        setSelectedRaffle: (state, selectedRaffle) => {
            state.selectedRaffle = selectedRaffle;
        }
    },
    actions: {}
});
//# sourceMappingURL=store.js.map