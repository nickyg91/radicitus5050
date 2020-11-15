import Vue from "vue";
import Router from "vue-router";
import RaffleView from "@/views/Raffle.vue";
import CreateRaffle from "@/views/CreateRaffle.vue";
import Raffles from "@/views/Raffles.vue";
import ChooseWinner from "@/views/ChooseWinner.vue";
import NotFound from "@/views/NotFound.vue";
Vue.use(Router);
export default new Router({
    mode: "history",
    base: process.env.BASE_URL,
    routes: [
        {
            path: "/raffle/:raffleId",
            name: "raffle",
            component: RaffleView
        },
        {
            path: "/raffle/guild/create",
            name: "create",
            component: CreateRaffle
        },
        {
            path: "/raffles",
            name: "raffles",
            component: Raffles
        },
        {
            path: "/choose/winner",
            name: "choosewinner",
            component: ChooseWinner
        },
        {
            path: "/",
            redirect: "/raffles"
        },
        {
            path: "/where",
            component: NotFound
        },
        {
            path: "*",
            redirect: "/where"
        }
    ]
});
//# sourceMappingURL=router.js.map