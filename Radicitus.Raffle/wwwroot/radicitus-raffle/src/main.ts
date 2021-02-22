import Vue from "vue";
import App from "./App.vue";
import router from "./router";
import store from "./store";
import Buefy from "buefy";
import "bulma/css/bulma.css";
import '@fortawesome/fontawesome-free/css/all.css'
import VeeValidate from "vee-validate";
import VueCookies from "vue-cookies";
import axios from "axios";

Vue.use(Buefy, {
  defaultIconPack: "fas"
});
Vue.use(VueCookies);
Vue.use(VeeValidate, {
  classes: true,
  classNames: {
    valid: "is-success",
    invalid: "is-danger"
  }
});

Vue.filter("formatDate", (value: any) => {
  if (value) {
    const d = new Date(value);
    console.log(d);
    return `${d.getMonth() + 1}/${d.getDate()}/${d.getFullYear()}`;
  }
});

const axiosConfig = axios.create({
  headers: {
    "content-type": "application/json"
  }
});

Vue.prototype.$http = axiosConfig;

Vue.config.productionTip = false;
new Vue({
  router,
  store,
  render: h => h(App)
}).$mount("#app");
