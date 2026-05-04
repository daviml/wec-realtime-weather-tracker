<script setup lang="ts">
import { onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { useWeatherStore } from '@/stores/weatherStore'
import { useSignalR } from '@/composables/useSignalR'
import CircuitCard from '@/components/CircuitCard.vue'

const router = useRouter()

const store = useWeatherStore()
const { isConnected } = useSignalR()

onMounted(async () => {
  await store.fetchCircuits()
  await store.fetchLatestWeather()
})

const goToDetail = (id: string) => {
  router.push(`/circuit/${id}`)
}
</script>

<template>
  <div class="dashboard container">
    <header class="dashboard-header">
      <div class="title-section">
        <div class="logo-badge">WEC</div>
        <h1>Weather Tracker</h1>
        <p>Monitoramento em tempo real das condições climáticas do FIA WEC</p>
      </div>
      <div class="status-badge glass" :class="{ 'is-connected': isConnected }">
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
        @click="goToDetail(circuit.id)"
      />
    </div>
  </div>
</template>

<style scoped>
.dashboard {
  padding-top: 3rem;
  padding-bottom: 5rem;
}

.dashboard-header {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
  margin-bottom: 4rem;
}

.logo-badge {
  background: var(--color-primary);
  color: white;
  padding: 0.2rem 0.6rem;
  font-weight: 900;
  font-size: 0.8rem;
  width: fit-content;
  border-radius: 4px;
  margin-bottom: 0.5rem;
  letter-spacing: 1px;
}

h1 {
  font-size: 3.5rem;
  line-height: 1;
  margin: 0;
  background: linear-gradient(135deg, #fff 30%, var(--color-primary) 100%);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
}

.title-section p {
  color: var(--color-text-muted);
  font-size: 1.1rem;
  margin: 1rem 0 0 0;
}

.status-badge {
  display: flex;
  align-items: center;
  gap: 0.75rem;
  padding: 0.6rem 1.25rem;
  border-radius: 2rem;
  font-size: 0.8rem;
  font-weight: 800;
  text-transform: uppercase;
  letter-spacing: 1.5px;
}

.status-badge .dot {
  width: 10px;
  height: 10px;
  background: #333;
  border-radius: 50%;
  transition: var(--transition-smooth);
}

.status-badge.is-connected .dot {
  background: #00ff00;
  box-shadow: 0 0 12px rgba(0, 255, 0, 0.6);
}

.circuits-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(340px, 1fr));
  gap: 2.5rem;
}

.loading-state, .error-state {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  min-height: 400px;
  gap: 2rem;
}

@media (max-width: 768px) {
  .dashboard-header {
    flex-direction: column;
    gap: 2rem;
  }
  h1 {
    font-size: 2.5rem;
  }
}
</style>
