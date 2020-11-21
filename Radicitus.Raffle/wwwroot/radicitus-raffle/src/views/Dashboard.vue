<template>
  <div class="section"></div>
</template>
<script lang="ts">
import RadRaffle from "@/models/raffle.model";
import Vue from "vue";
import Component from "vue-class-component";
@Component
export default class Dashboard extends Vue {
    raffles!: RadRaffle[];
}
</script>
