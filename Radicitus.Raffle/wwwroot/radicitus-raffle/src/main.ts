import Vue from 'vue';
import App from './App.vue';
import router from './router';
import store from './store';
import Buefy from 'buefy';
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
import { library } from '@fortawesome/fontawesome-svg-core';
import {
  faExclamationCircle,
  faCheck,
  faTimes,
  faCalendar,
  faCalendarDay,
  faAngleLeft,
  faAngleRight,
  faUsers } from '@fortawesome/free-solid-svg-icons';
import 'bulma/css/bulma.css';
import VeeValidate from 'vee-validate';
import VueCookies from 'vue-cookies';

library.add(faExclamationCircle, faCheck, faTimes, faCalendar, faCalendarDay, faAngleLeft, faAngleRight, faUsers);
Vue.component('font-awesome-icon', FontAwesomeIcon);
Vue.use(Buefy, {
  defaultIconComponent: FontAwesomeIcon,
  defaultIconPack: 'fas',
});
Vue.use(VueCookies);
Vue.use(VeeValidate, {
  classes: true,
  classNames: {
    valid: 'is-success',
    invalid: 'is-danger',
  },
});

Vue.filter('formatDate', (value: any) => {
  if (value) {
    const d = new Date(value);
    return `${d.getDay()}/${d.getMonth()}/${d.getFullYear()}`;
  }
});

Vue.config.productionTip = false;
new Vue({
  router,
  store,
  render: (h) => h(App),
}).$mount('#app');
