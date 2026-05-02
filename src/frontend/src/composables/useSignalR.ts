import { ref, onMounted, onUnmounted } from 'vue'
import { HubConnectionBuilder, HubConnection, LogLevel } from '@microsoft/signalr'
import { useWeatherStore, type WeatherSnapshot } from '@/stores/weatherStore'

export function useSignalR() {
  const store = useWeatherStore()
  const connection = ref<HubConnection | null>(null)
  const isConnected = ref(false)

  const startConnection = async () => {
    const hubUrl = `${import.meta.env.VITE_API_URL || 'http://localhost:5000'}/hubs/weather`
    
    connection.value = new HubConnectionBuilder()
      .withUrl(hubUrl)
      .withAutomaticReconnect()
      .configureLogging(LogLevel.Information)
      .build()

    connection.value.on('WeatherUpdated', (snapshot: WeatherSnapshot) => {
      console.log('Update real-time recebido:', snapshot)
      store.updateSnapshot(snapshot)
    })

    try {
      await connection.value.start()
      isConnected.value = true
      console.log('Conectado ao SignalR Hub')
      
      // Ao conectar, se já tivermos circuitos, podemos entrar nos grupos
      // No dashboard, queremos atualizações de todos os circuitos
      if (store.circuits.length > 0) {
        await joinAllCircuits()
      }
    } catch (err) {
      console.error('Erro ao conectar ao SignalR:', err)
      isConnected.value = false
    }
  }

  const joinAllCircuits = async () => {
    if (connection.value && isConnected.value) {
      const joinTasks = store.circuits.map(c => 
        connection.value!.invoke('JoinCircuit', c.id)
      )
      await Promise.all(joinTasks)
      console.log('Inscrito em todos os grupos de circuitos')
    }
  }

  const joinCircuit = async (circuitId: string) => {
    if (connection.value && isConnected.value) {
      await connection.value.invoke('JoinCircuit', circuitId)
    }
  }

  const leaveCircuit = async (circuitId: string) => {
    if (connection.value && isConnected.value) {
      await connection.value.invoke('LeaveCircuit', circuitId)
    }
  }

  onMounted(() => {
    startConnection()
  })

  onUnmounted(async () => {
    if (connection.value) {
      await connection.value.stop()
    }
  })

  return {
    isConnected,
    joinCircuit,
    leaveCircuit,
    joinAllCircuits
  }
}
