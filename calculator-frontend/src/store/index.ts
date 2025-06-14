import { createStore } from "vuex";

export interface State {
  token: string | null;
}

export default createStore<State>({
  state: {
    token: localStorage.getItem("token"),
  },
  mutations: {
    setToken(state, token: string | null) {
      state.token = token;
      if (token) {
        localStorage.setItem("token", token);
      } else {
        localStorage.removeItem("token");
      }
    },
  },
  actions: {},
  modules: {},
});
