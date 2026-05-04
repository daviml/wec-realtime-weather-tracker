import { defineStore } from 'pinia'
import { ref, computed } from 'vue'

export interface Circuit {
  id: string
  name: string
  location: string
  country: string
  latitude: number
  longitude: number
}

export interface WeatherSnapshot {
  id: string
  circuitId: string
  circuitName: string
  temperature: number
  feelsLike: number
  humidity: number
  windSpeed: number
  windDirection: number
  condition: string
  weatherCode: number
  recordedAt: string
}

export const useWeatherStore = defineStore('weather', () => {
  const circuits = ref<Circuit[]>([])
  const snapshots = ref<Record<string, WeatherSnapshot>>({})
  const isLoading = ref(false)
  const error = ref<string | null>(null)
  const history = ref<Record<string, WeatherSnapshot[]>>({})

  const sortedCircuits = computed(() => {
    return [...circuits.value].sort((a, b) => a.name.localeCompare(b.name))
  })

  async function fetchCircuits() {
    isLoading.value = true
    try {
      const response = await fetch(`${import.meta.env.VITE_API_URL || 'http://localhost:5000'}/api/circuits`)
      if (!response.ok) throw new Error('Falha ao carregar circuitos')
      circuits.value = await response.json()
    } catch (err: any) {
      error.value = err.message
    } finally {
      isLoading.value = false
    }
  }

  async function fetchLatestWeather() {
    try {
      const response = await fetch(`${import.meta.env.VITE_API_URL || 'http://localhost:5000'}/api/weather/latest`)
      if (!response.ok) throw new Error('Falha ao carregar clima atual')
      const data: WeatherSnapshot[] = await response.json()
      
      data.forEach(snapshot => {
        snapshots.value[snapshot.circuitId] = snapshot
      })
    } catch (err: any) {
      console.error('Erro ao buscar clima:', err)
    }
  }

  async function fetchHistory(circuitId: string) {
    try {
      const response = await fetch(`${import.meta.env.VITE_API_URL || 'http://localhost:5000'}/api/weather/${circuitId}/history`)
      if (!response.ok) throw new Error('Falha ao carregar histórico')
      history.value[circuitId] = await response.json()
    } catch (err: any) {
      console.error(`Erro ao buscar histórico para ${circuitId}:`, err)
    }
  }

  function updateSnapshot(snapshot: WeatherSnapshot) {
    snapshots.value[snapshot.circuitId] = snapshot
  }

  return {
    circuits,
    snapshots,
    history,
    isLoading,
    error,
    sortedCircuits,
    fetchCircuits,
    fetchLatestWeather,
    fetchHistory,
    updateSnapshot
  }
})
