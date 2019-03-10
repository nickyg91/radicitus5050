<template>
    <div @click="clicked" class="box">
        <div>
            <div class="has-text-centered">
                {{num}}
            </div>
            <div class="has-text-centered">
                {{name.length === 0 ? '&nbsp;' : name}}
            </div>
        </div>
    </div>
</template>
<script lang="ts">
import { Component, Prop, Vue, Model } from 'vue-property-decorator';
@Component
export default class Square extends Vue {
    @Prop(Number) public squareNumber!: number;
    public name = '';
    private _squareTakenBy!: string;
    private _squareNumber!: number;
    private clicked() {
        this.$emit('square-clicked', this);
    }
    get squareName() {
        return this.name;
    }
    set squareName(val: string) {
        this.name = val;
    }
    get num() {
        return this.squareNumber;
    }
    set num(val: number) {
        this.squareNumber = val;
    }
}
</script>