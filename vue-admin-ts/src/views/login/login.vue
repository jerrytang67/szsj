<template>
   <div class="wrap">
      <div class="card grid grid-cols-1 gap-4">
         <div class="formAvatar text-sky-400 justify-self-center">
            <img src="saas.png" class="w-24 h-24" />
         </div>
         <label class="black">
            <span class="sr-only">用户名</span>
            <input type="text" class="w-full" placeholder="用户名" required>
         </label>
         <label class="black">
            <span class="sr-only">密码</span>
            <input type="password" class="w-full" placeholder="密码" required>
         </label>
         <label class="black">
            <input type="checkbox" checked />
            <span class="">记住密码</span>
         </label>
         <button class="btn justify-self-start" type="button">登录</button>
         <a href="#" class="text-sm text-gray-400">忘记密码</a>
      </div>
   </div>
</template>
 
<script lang="ts">
import { Component, Vue, Prop, Watch, Ref } from "vue-property-decorator";
import { Route } from "vue-router";
import { Dictionary } from "vue-router/types/router";
import api from "@/api";

@Component
export default class LoginPage extends Vue {
   created() {}
}
</script>

<style lang="scss" scoped>
[type="text"],
[type="password"] {
   @apply p-2;
}

[type="text"],
[type="password"],
[type="button"],
[type="checkbox"],
[type="radio"],
textarea,
select {
   @apply rounded-md border-gray-100 border shadow-sm bg-gray-100 focus:border-sky-300 focus:ring focus:ring-sky-200 focus:ring-opacity-50;
   &:focus {
      @apply outline-none;
   }
}

[type="checkbox"],
[type="radio"] {
   @apply text-sky-400 m-1 p-1 bg-sky-400;
   &:checked {
      @apply text-sky-400 m-1 p-1 bg-sky-400;
   }
}

.wrap {
   @apply bg-gray-100 text-gray-900 flex justify-center items-center h-screen;
}
.card {
   @apply border rounded-lg p-10 m-10 bg-white shadow-sm w-full max-w-sm;
}

.btn {
   @apply rounded border px-6 py-2 bg-sky-400 text-gray-100 border-sky-400 shadow-sm;
   @apply hover:bg-blue-500;
}
</style>
