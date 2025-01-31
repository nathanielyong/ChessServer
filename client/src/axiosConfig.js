import axios from 'axios'
import store from './store'

const instance = axios.create({
    baseUrl: ''
})

instance.interceptors.request.use(
    config => {
        const token = store.getters.getToken;
        if (token) {
            config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
    },
    error => {
        return Promise.reject(error);
    }
)

export default instance