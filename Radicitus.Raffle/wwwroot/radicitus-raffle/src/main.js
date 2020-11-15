import Vue from "vue";
import App from "./App.vue";
import router from "./router";
import store from "./store";
import Buefy from "buefy";
import "bulma/css/bulma.css";
import '@fortawesome/fontawesome-free/css/all.css';
import VeeValidate from "vee-validate";
import VueCookies from "vue-cookies";
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
Vue.filter("formatDate", (value) => {
    if (value) {
        const d = new Date(value);
        return `${d.getDay()}/${d.getMonth()}/${d.getFullYear()}`;
    }
});
Vue.config.productionTip = false;
new Vue({
    router,
    store,
    render: h => h(App)
}).$mount("#app");
//# sourceMappingURL=main.js.map