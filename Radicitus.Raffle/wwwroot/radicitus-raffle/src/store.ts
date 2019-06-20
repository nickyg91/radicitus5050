import Vue from "vue";
import Vuex from "vuex";
import RadRaffle from "./models/raffle.model";

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
    setUser: (state, userName: string) => {
      state.currentUser = userName;
    },
    setSelectedRaffle: (state, selectedRaffle: RadRaffle) => {
      state.selectedRaffle = selectedRaffle;
    }
  },
  actions: {}
});
