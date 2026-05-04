<script setup lang="ts">
import { RouterView } from 'vue-router'
</script>

<template>
  <div class="app-container">
    <RouterView v-slot="{ Component }">
      <transition name="fade" mode="out-in">
        <component :is="Component" />
      </transition>
    </RouterView>
  </div>
</template>

<style>
/* Os estilos base estão no main.css */
.app-container {
  min-height: 100vh;
  background-image: 
    radial-gradient(circle at 0% 0%, rgba(0, 102, 255, 0.05) 0%, transparent 40%),
    radial-gradient(circle at 100% 100%, rgba(0, 102, 255, 0.05) 0%, transparent 40%);
}
</style>
