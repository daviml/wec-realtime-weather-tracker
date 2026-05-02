<script setup lang="ts">
import { ref, watch } from 'vue'
import type { Circuit, WeatherSnapshot } from '@/stores/weatherStore'
import WeatherBadge from './WeatherBadge.vue'

const props = defineProps<{
  circuit: Circuit
  snapshot?: WeatherSnapshot
}>()

const isUpdating = ref(false)

// Animação de "ping" quando o snapshot muda
watch(() => props.snapshot, () => {
  isUpdating.value = true
  setTimeout(() => {
    isUpdating.value = false
  }, 1000)
}, { deep: true })
</script>

<template>
  <div class="circuit-card" :class="{ 'is-updating': isUpdating }">
    <div class="header">
      <div class="circuit-info">
        <h3>{{ circuit.name }}</h3>
        <p class="location">{{ circuit.location }}, {{ circuit.country }}</p>
      </div>
      <div class="update-indicator" v-if="isUpdating"></div>
    </div>

    <div class="content" v-if="snapshot">
      <div class="main-stats">
        <div class="temp">
          <span class="value">{{ Math.round(snapshot.temperature) }}</span>
          <span class="unit">°C</span>
        </div>
        <WeatherBadge :condition="snapshot.condition" :weatherCode="snapshot.weatherCode" />
      </div>

      <div class="details">
        <div class="detail-item">
          <span class="icon">💧</span>
          <span class="value">{{ snapshot.humidity }}%</span>
          <span class="label">Umidade</span>
        </div>
        <div class="detail-item">
          <span class="icon">💨</span>
          <span class="value">{{ snapshot.windSpeed }} <small>km/h</small></span>
          <span class="label">Vento</span>
        </div>
      </div>
    </div>
    <div class="content loading" v-else>
      <p>Carregando clima...</p>
    </div>

    <div class="footer" v-if="snapshot">
      <span class="time">Atualizado em {{ new Date(snapshot.recordedAt).toLocaleTimeString() }}</span>
    </div>
  </div>
</template>

<style scoped>
.circuit-card {
  background: rgba(255, 255, 255, 0.05);
  border: 1px solid rgba(255, 255, 255, 0.1);
  border-radius: 1rem;
  padding: 1.5rem;
  transition: all 0.3s ease;
  position: relative;
  overflow: hidden;
}

.circuit-card:hover {
  transform: translateY(-4px);
  background: rgba(255, 255, 255, 0.08);
  border-color: rgba(255, 0, 0, 0.3);
}

.is-updating {
  box-shadow: 0 0 20px rgba(255, 0, 0, 0.2);
  border-color: rgba(255, 0, 0, 0.5);
}

.header {
  display: flex;
  justify-content: space-between;
  margin-bottom: 1.5rem;
}

h3 {
  margin: 0;
  font-size: 1.25rem;
  color: #fff;
}

.location {
  margin: 0;
  font-size: 0.875rem;
  opacity: 0.6;
}

.update-indicator {
  width: 8px;
  height: 8px;
  background: #ff0000;
  border-radius: 50%;
  box-shadow: 0 0 10px #ff0000;
  animation: pulse 1s infinite;
}

.main-stats {
  display: flex;
  align-items: center;
  justify-content: space-between;
  margin-bottom: 1.5rem;
}

.temp .value {
  font-size: 3rem;
  font-weight: 700;
  color: #fff;
}

.temp .unit {
  font-size: 1.5rem;
  opacity: 0.6;
}

.details {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 1rem;
  padding-top: 1rem;
  border-top: 1px solid rgba(255, 255, 255, 0.1);
}

.detail-item {
  display: flex;
  flex-direction: column;
}

.detail-item .value {
  font-weight: 600;
  font-size: 1.1rem;
}

.detail-item .label {
  font-size: 0.75rem;
  opacity: 0.5;
  text-transform: uppercase;
}

.footer {
  margin-top: 1rem;
  font-size: 0.7rem;
  opacity: 0.4;
  text-align: right;
}

@keyframes pulse {
  0% { transform: scale(1); opacity: 1; }
  50% { transform: scale(1.5); opacity: 0.5; }
  100% { transform: scale(1); opacity: 1; }
}
</style>
