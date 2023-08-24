import axios from "axios";

const api = axios.create({
    baseURL: 'https://beerprice.azurewebsites.net/'
})

export default api;