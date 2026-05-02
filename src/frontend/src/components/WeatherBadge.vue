<script setup lang="ts">
import { computed } from 'vue'

const props = defineProps<{
  condition: string
  weatherCode: number
}>()

// Mapeamento simples de Weather Code (WMO) para ícones/emojis
const weatherConfig = computed(() => {
  const code = props.weatherCode
  if (code === 0) return { icon: '☀️', color: '#ffcc00', label: 'Céu Limpo' }
  if (code === 1 || code === 2 || code === 3) return { icon: '⛅', color: '#a0a0a0', label: 'Parcialmente Nublado' }
  if (code >= 45 && code <= 48) return { icon: '🌫️', color: '#cccccc', label: 'Nevoeiro' }
  if (code >= 51 && code <= 67) return { icon: '🌧️', color: '#0066ff', label: 'Chuva' }
  if (code >= 71 && code <= 77) return { icon: '❄️', color: '#ffffff', label: 'Neve' }
  if (code >= 80 && code <= 82) return { icon: '🌦️', color: '#0099ff', label: 'Pancadas de Chuva' }
  if (code >= 95) return { icon: '⛈️', color: '#6600cc', label: 'Tempestade' }
  
  return { icon: '☁️', color: '#888888', label: props.condition }
})
</script>

<template>
  <div class="weather-badge" :style="{ backgroundColor: weatherConfig.color + '22', borderColor: weatherConfig.color }">
    <span class="icon">{{ weatherConfig.icon }}</span>
    <span class="label">{{ weatherConfig.label }}</span>
  </div>
</template>

<style scoped>
.weather-badge {
  display: inline-flex;
  align-items: center;
  gap: 0.5rem;
  padding: 0.25rem 0.75rem;
  border-radius: 2rem;
  border: 1px solid;
  font-size: 0.875rem;
  font-weight: 600;
  backdrop-filter: blur(4px);
}

.icon {
  font-size: 1.1rem;
}

.label {
  color: var(--color-text);
}
</style>
