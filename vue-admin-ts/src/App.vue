<template>
   <div id="app">
      <router-view />
   </div>
</template>

<script lang='ts'>
import { Component, Vue } from "vue-property-decorator";
import router, { asyncRouter } from "./router/index";

import { UserModule } from "@/store/modules/user";
import { AppModule } from "@/store/modules/app";
import api from "./api";

@Component({
   name: "App",
})
export default class extends Vue {
   signedIn: boolean = true;

   async beforeCreate() {
      await UserModule.GetUserInfo();
   }
}
</script>
