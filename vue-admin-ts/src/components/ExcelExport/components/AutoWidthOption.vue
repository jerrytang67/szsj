<template>
  <div style="display:inline-block;">
    <label class="radio-label">单元格自适应宽度: </label>
    <!-- <el-radio-group v-model="autoWidth">
      <el-radio :label="true" border>
        是
      </el-radio>
      <el-radio :label="false" border>
        否
      </el-radio>
    </el-radio-group> -->

    <el-switch v-model="autoWidth" active-value="true" inactive-value="false">
    </el-switch>

  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from "vue-property-decorator";

@Component({
  name: "AutoWidthOption"
})
export default class extends Vue {
  @Prop({ required: true }) private value!: boolean;

  get autoWidth() {
    return this.value;
  }

  set autoWidth(value) {
    this.$emit("input", value);
  }
}
</script>
