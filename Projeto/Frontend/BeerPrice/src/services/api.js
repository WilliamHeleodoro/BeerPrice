import axios from "axios";

const api = axios.create({
  //baseURL: 'https://beerprice.azurewebsites.net/'
  baseURL: "http://192.168.2.77:3000",
});

export default api;
