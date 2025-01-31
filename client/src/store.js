import { createStore } from 'vuex';
import { jwtDecode } from 'jwt-decode';

export default createStore({
    state: {
        jwtToken: localStorage.getItem('jwtToken') || null,
        isTokenValid: false,
    },
    mutations: {
        setToken(state, token) {
            state.jwtToken = token;
            localStorage.setItem('jwtToken', token);
            state.isTokenValid = true;
        },
        invalidateToken(state) {
            state.jwtToken = null;
            localStorage.removeItem('jwtToken');
            state.isTokenValid = false;
        },
    },
    actions: {
        login({ commit }, token) {
            commit('setToken', token);
        },
        logout({ commit }) {
            commit('invalidateToken');
        },
        validateToken({ commit, getters }) {
            const token = getters.getToken; 
            if (!token) {
                commit('invalidateToken');
                return false;
            }
            const { exp } = jwtDecode(token);
            const currentTime = Date.now() / 1000;

            if (exp < currentTime) {
                commit('invalidateToken');
                return false;
            }
            commit('setToken', token)
            return true;
        },
        initialize({ dispatch }) {
            dispatch('validateToken'); 
        },
    },
    getters: {
        isAuthenticated: (state) => state.isTokenValid,
        getToken: (state) => state.jwtToken,
    },
})