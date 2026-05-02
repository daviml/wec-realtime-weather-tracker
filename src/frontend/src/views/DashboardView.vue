<script setup lang="ts">
import { onMounted } from 'vue'
import { useWeatherStore } from '@/stores/weatherStore'
import { useSignalR } from '@/composables/useSignalR'
import CircuitCard from '@/components/CircuitCard.vue'

const store = useWeatherStore()
const { isConnected } = useSignalR()

onMounted(async () => {
  await store.fetchCircuits()
  await store.fetchLatestWeather()
})
</script>

<template>
  <div class="dashboard">
    <header class="dashboard-header">
      <div class="title-section">
        <h1>WEC Weather Tracker</h1>
        <p>Acompanhamento em tempo real das condições climáticas do FIA WEC</p>
      </div>
      <div class="status-badge" :class="{ 'is-connected': isConnected }">
        <span class="dot"></span>
        {{ isConnected ? 'Live' : 'Conectando...' }}
      </div>
    </header>

    <div v-if="store.isLoading" class="loading-state">
      <p>Carregando circuitos...</p>
    </div>

    <div v-else-if="store.error" class="error-state">
      <p>{{ store.error }}</p>
      <button @click="store.fetchCircuits">Tentar novamente</button>
    </div>

    <div v-else class="circuits-grid">
      <CircuitCard
        v-for="circuit in store.sortedCircuits"
        :key="circuit.id"
        :circuit="circuit"
        :snapshot="store.snapshots[circuit.id]"
      />
    </div>
  </div>
</template>

<style scoped>
.dashboard {
  padding: 2rem;
  max-width: 1400px;
  margin: 0 auto;
}

.dashboard-header {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
  margin-bottom: 3rem;
}

h1 {
  font-size: 2.5rem;
  font-weight: 800;
  margin: 0;
  background: linear-gradient(135deg, #fff 0%, #ff0000 100%);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
}

.title-section p {
  opacity: 0.6;
  margin: 0.5rem 0 0 0;
}

.status-badge {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  padding: 0.5rem 1rem;
  background: rgba(255, 255, 255, 0.05);
  border-radius: 2rem;
  font-size: 0.8rem;
  font-weight: 700;
  text-transform: uppercase;
  letter-spacing: 1px;
}

.status-badge .dot {
  width: 8px;
  height: 8px;
  background: #666;
  border-radius: 50%;
}

.status-badge.is-connected .dot {
  background: #00ff00;
  box-shadow: 0 0 10px #00ff00;
}

.circuits-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(320px, 1fr));
  gap: 2rem;
}

.loading-state, .error-state {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  min-height: 400px;
}

@media (max-width: 768px) {
  .dashboard-header {
    flex-direction: column;
    gap: 1.5rem;
  }
}
</style>
