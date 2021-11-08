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
   @Prop({ required: true }) private xKey!: string;
   @Prop({ required: true }) private yKey!: string;
   @Prop({ required: false, default: ["标题"] }) private title!: any[];
   @Prop({ default: "chart" }) private className!: string;
   @Prop({ default: "100%" }) private width!: string;
   @Prop({ default: "350px" }) private height!: string;
   @Prop({ default: "#3888fa" }) private lineColor!: string;
   @Prop({ default: "#f3f8ff" }) private areaColor!: string;

   @Prop({ default: false }) private smooth!: boolean;

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
      console.log(chartData, this.xKey, this.yKey);
      if (this.chart) {
         this.chart.setOption({
            xAxis: {
               data: chartData.map((x: any) => x[this.yKey]),
               boundaryGap: false,
               axisTick: {
                  show: false,
               },
            },
            grid: {
               left: 10,
               right: 10,
               bottom: 20,
               top: 30,
               containLabel: true,
            },
            tooltip: {
               trigger: "axis",
               axisPointer: {
                  type: "cross",
               },
               padding: 8,
            },
            yAxis: {
               axisTick: {
                  show: true,
               },
            },
            legend: {
               data: this.title,
            },
            series: [
               {
                  name: this.title[0],
                  smooth: this.smooth,
                  type: "line",
                  itemStyle: {
                     normal: {
                        label: {
                           show: true,
                        },
                        color: this.lineColor,
                        lineStyle: {
                           color: this.lineColor,
                           width: 2,
                        },
                        areaStyle: {
                           color: this.areaColor,
                        },
                     },
                  },
                  data: chartData.map((x: any) => x[this.xKey]),
                  animationDuration: 1000,
                  animationEasing: "quadraticOut",
               },
            ],
         } as EChartOption<EChartOption.SeriesLine>);
      }
   }
}
</script>
