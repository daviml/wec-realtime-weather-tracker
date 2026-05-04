<script setup lang="ts">
import type { WeatherSnapshot } from '@/stores/weatherStore'
import WeatherBadge from './WeatherBadge.vue'

defineProps<{
  snapshot: WeatherSnapshot
}>()

const formatTime = (dateStr: string) => {
  return new Date(dateStr).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })
}
</script>

<template>
  <div class="weather-widget glass">
    <div class="widget-header">
      <div class="main-info">
        <div class="temperature">
          <span class="value">{{ Math.round(snapshot.temperature) }}</span>
          <span class="unit">°C</span>
        </div>
        <div class="condition-wrapper">
          <WeatherBadge :condition="snapshot.condition" :weatherCode="snapshot.weatherCode" />
          <span class="feels-like">Sensação: {{ Math.round(snapshot.feelsLike) }}°C</span>
        </div>
      </div>
    </div>

    <div class="stats-grid">
      <div class="stat-card">
        <span class="stat-label">Umidade</span>
        <div class="stat-value">
          <span class="icon">💧</span>
          {{ snapshot.humidity }}%
        </div>
        <div class="stat-bar">
          <div class="bar-fill" :style="{ width: snapshot.humidity + '%' }"></div>
        </div>
      </div>

      <div class="stat-card">
        <span class="stat-label">Vento</span>
        <div class="stat-value">
          <span class="icon">💨</span>
          {{ Math.round(snapshot.windSpeed) }} <small>km/h</small>
        </div>
        <div class="stat-sub">Direção: {{ snapshot.windDirection }}°</div>
      </div>
    </div>

    <div class="widget-footer">
      <span class="last-update">Última leitura: {{ formatTime(snapshot.recordedAt) }}</span>
    </div>
  </div>
</template>

<style scoped>
.weather-widget {
  padding: 2rem;
  border-radius: var(--border-radius-lg);
  display: flex;
  flex-direction: column;
  gap: 2rem;
}

.widget-header {
  border-bottom: 1px solid rgba(255, 255, 255, 0.05);
  padding-bottom: 1.5rem;
}

.main-info {
  display: flex;
  align-items: center;
  gap: 2.5rem;
}

.temperature {
  display: flex;
  align-items: flex-start;
}

.temperature .value {
  font-size: 5rem;
  font-weight: 800;
  line-height: 1;
  font-family: var(--font-heading);
  background: linear-gradient(180deg, #fff 0%, #aaa 100%);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
}

.temperature .unit {
  font-size: 2rem;
  font-weight: 600;
  color: var(--color-primary);
  margin-top: 0.5rem;
}

.condition-wrapper {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}

.feels-like {
  font-size: 0.9rem;
  color: var(--color-text-muted);
}

.stats-grid {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 1.5rem;
}

.stat-card {
  background: rgba(255, 255, 255, 0.03);
  padding: 1.25rem;
  border-radius: var(--border-radius-md);
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}

.stat-label {
  font-size: 0.75rem;
  text-transform: uppercase;
  letter-spacing: 1px;
  color: var(--color-text-dim);
  font-weight: 700;
}

.stat-value {
  font-size: 1.5rem;
  font-weight: 700;
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.stat-value small {
  font-size: 0.8rem;
  color: var(--color-text-dim);
}

.stat-bar {
  height: 4px;
  background: rgba(255, 255, 255, 0.05);
  border-radius: 2px;
  margin-top: 0.5rem;
  overflow: hidden;
}

.bar-fill {
  height: 100%;
  background: var(--color-primary);
  box-shadow: 0 0 10px var(--color-primary);
}

.stat-sub {
  font-size: 0.8rem;
  color: var(--color-text-muted);
}

.widget-footer {
  text-align: right;
}

.last-update {
  font-size: 0.8rem;
  color: var(--color-text-dim);
}

@media (max-width: 600px) {
  .main-info {
    flex-direction: column;
    align-items: flex-start;
    gap: 1rem;
  }
  
  .stats-grid {
    grid-template-columns: 1fr;
  }
}
</style>
