<template>
   <div :class="className" :style="{height: height, width: width}" />
</template>

<script lang="ts">
import echarts, { EChartOption } from "echarts";
import { Component, Prop, Watch } from "vue-property-decorator";
import { mixins } from "vue-class-component";
import ResizeMixin from "@/components/Charts/mixins/resize";

@Component({
   name: "LineChart",
})
export default class extends mixins(ResizeMixin) {
   @Prop({ required: true }) private chartData!: any;
   @Prop({ default: "" }) private title!: string;
   @Prop({ default: "" }) private subText!: string;
   @Prop({ default: "chart" }) private className!: string;
   @Prop({ default: "100%" }) private width!: string;
   @Prop({ default: "550px" }) private height!: string;

   @Watch("chartData", { deep: true })
   private onChartDataChange(value: any) {
      this.setOptions(value);
   }

   mounted() {
      this.$nextTick(() => {
         this.initChart();
      });
   }

   beforeDestroy() {
      if (!this.chart) {
         return;
      }
      this.chart.dispose();
      this.chart = null;
   }

   private initChart() {
      this.chart = echarts.init(this.$el as HTMLDivElement, "macarons");
      this.setOptions(this.chartData);
   }

   private setOptions(chartData: any[]) {
      if (this.chart) {
         this.chart.setOption({
            title: [
               {
                  text: this.subText,
                  subtext: this.subText,
                  left: "center",
               },
            ],
            tooltip: {
               trigger: "item",
               formatter: "{a} <br/>{b} : {c} ({d}%)",
            },
            series: [
               {
                  name: this.subText,
                  type: "pie",
                  radius: "65%",
                  center: ["50%", "60%"],
                  data: this.chartData,
                  emphasis: {
                     itemStyle: {
                        shadowBlur: 10,
                        shadowOffsetX: 0,
                        shadowColor: "rgba(0, 0, 0, 0.5)",
                     },
                  },
               },
            ],
         } as EChartOption<EChartOption.SeriesPie>);
      }
   }
}
</script>
