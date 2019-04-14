import Vue from 'vue';
import Router from 'vue-router';
import Raffle from '@/views/Raffle.vue';
import CreateRaffle from '@/views/CreateRaffle.vue';
Vue.use(Router);

export default new Router({
  mode: 'history',
  base: process.env.BASE_URL,
  routes: [
    {
      path: '/raffle/:guid',
      name: 'raffle',
      component: Raffle,
    },
    {
      path: '/raffle/guild/create',
      name: 'create',
      component: CreateRaffle,
    },
    {
      path: '/raffles',
      name: 'raffle',
      component: CreateRaffle
    }
  ],
});
