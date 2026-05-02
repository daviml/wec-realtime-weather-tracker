<script setup lang="ts">
import { RouterView } from 'vue-router'
</script>

<template>
  <div class="app-container">
    <RouterView />
  </div>
</template>

<style>
:root {
  --color-background: #0a0a0a;
  --color-surface: #1a1a1a;
  --color-primary: #ff0000;
  --color-text: #ffffff;
  --color-text-muted: rgba(255, 255, 255, 0.6);
}

body {
  margin: 0;
  padding: 0;
  background-color: var(--color-background);
  color: var(--color-text);
  font-family: 'Inter', -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, Oxygen, Ubuntu,
    Cantarell, 'Open Sans', 'Helvetica Neue', sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
}

.app-container {
  min-height: 100vh;
  background-image: 
    radial-gradient(circle at 0% 0%, rgba(255, 0, 0, 0.05) 0%, transparent 50%),
    radial-gradient(circle at 100% 100%, rgba(255, 0, 0, 0.05) 0%, transparent 50%);
}

* {
  box-sizing: border-box;
}
</style>
