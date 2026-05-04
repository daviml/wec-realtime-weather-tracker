<script setup lang="ts">
import { onMounted, computed } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { useWeatherStore } from '@/stores/weatherStore'
import WeatherWidget from '@/components/WeatherWidget.vue'

const route = useRoute()
const router = useRouter()
const store = useWeatherStore()

const circuitId = route.params.id as string

const circuit = computed(() => store.circuits.find(c => c.id === circuitId))
const currentSnapshot = computed(() => store.snapshots[circuitId])
const history = computed(() => store.history[circuitId] || [])

onMounted(async () => {
  if (store.circuits.length === 0) {
    await store.fetchCircuits()
  }
  await store.fetchLatestWeather()
  await store.fetchHistory(circuitId)
})

const goBack = () => {
  router.push('/')
}

const formatDate = (dateStr: string) => {
  return new Date(dateStr).toLocaleString([], { 
    day: '2-digit', 
    month: '2-digit',
    hour: '2-digit', 
    minute: '2-digit' 
  })
}
</script>

<template>
  <div class="detail-view container">
    <header class="detail-header">
      <button @click="goBack" class="back-button">
        <span class="icon">←</span> Dashboard
      </button>
      <div v-if="circuit" class="circuit-title">
        <h1>{{ circuit.name }}</h1>
        <p class="location">{{ circuit.location }}, {{ circuit.country }}</p>
      </div>
    </header>

    <div v-if="currentSnapshot" class="detail-content">
      <section class="main-section">
        <h2 class="section-title">Condições Atuais</h2>
        <WeatherWidget :snapshot="currentSnapshot" />
      </section>

      <section class="history-section">
        <h2 class="section-title">Histórico de Leituras</h2>
        <div class="history-list glass">
          <div class="history-header">
            <span>Data/Hora</span>
            <span>Temp</span>
            <span>Condição</span>
            <span class="desktop-only">Vento</span>
          </div>
          <div v-for="item in history" :key="item.id" class="history-item">
            <span class="time">{{ formatDate(item.recordedAt) }}</span>
            <span class="temp">{{ Math.round(item.temperature) }}°C</span>
            <span class="cond">{{ item.condition }}</span>
            <span class="wind desktop-only">{{ Math.round(item.windSpeed) }} km/h</span>
          </div>
          <div v-if="history.length === 0" class="no-history">
            Carregando histórico...
          </div>
        </div>
      </section>
    </div>

    <div v-else class="loading-state">
      <p>Carregando detalhes do circuito...</p>
    </div>
  </div>
</template>

<style scoped>
.detail-view {
  padding-top: 3rem;
  padding-bottom: 5rem;
}

.detail-header {
  margin-bottom: 3rem;
  display: flex;
  flex-direction: column;
  gap: 1.5rem;
}

.back-button {
  background: none;
  border: none;
  color: var(--color-primary);
  font-weight: 700;
  text-transform: uppercase;
  letter-spacing: 1px;
  cursor: pointer;
  display: flex;
  align-items: center;
  gap: 0.5rem;
  padding: 0;
  width: fit-content;
  transition: var(--transition-smooth);
}

.back-button:hover {
  transform: translateX(-5px);
  color: #fff;
}

.circuit-title h1 {
  margin: 0;
  font-size: 3rem;
  background: linear-gradient(135deg, #fff 0%, var(--color-primary) 100%);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
}

.location {
  font-size: 1.1rem;
  opacity: 0.6;
  margin: 0.5rem 0 0 0;
}

.section-title {
  font-size: 1.2rem;
  margin-bottom: 1.5rem;
  color: var(--color-text-muted);
  border-left: 3px solid var(--color-primary);
  padding-left: 1rem;
}

.detail-content {
  display: grid;
  grid-template-columns: 1fr;
  gap: 4rem;
}

.history-list {
  border-radius: var(--border-radius-lg);
  overflow: hidden;
}

.history-header {
  display: grid;
  grid-template-columns: 2fr 1fr 2fr 1fr;
  padding: 1.25rem 2rem;
  background: rgba(255, 255, 255, 0.05);
  font-weight: 700;
  text-transform: uppercase;
  font-size: 0.75rem;
  letter-spacing: 1px;
  color: var(--color-text-dim);
}

.history-item {
  display: grid;
  grid-template-columns: 2fr 1fr 2fr 1fr;
  padding: 1.25rem 2rem;
  border-bottom: 1px solid rgba(255, 255, 255, 0.03);
  transition: var(--transition-smooth);
}

.history-item:hover {
  background: rgba(255, 255, 255, 0.02);
}

.history-item .temp {
  font-weight: 700;
  color: var(--color-primary);
}

.no-history {
  padding: 3rem;
  text-align: center;
  opacity: 0.5;
}

@media (max-width: 768px) {
  .history-header, .history-item {
    grid-template-columns: 2fr 1fr 2fr;
  }
  .desktop-only {
    display: none;
  }
  .circuit-title h1 {
    font-size: 2rem;
  }
}
</style>
