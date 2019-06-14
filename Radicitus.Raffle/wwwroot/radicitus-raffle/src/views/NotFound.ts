import Vue from 'vue';
import Component from 'vue-class-component';
@Component({
    template: '@/views/NotFound.vue'
})
export default class NotFound extends Vue {
    public goHome() {
        this.$router.push('raffles');
    }
}
