<template>
  <v-container fluid>
    <!-- Header -->
    <v-row class="mb-6">
      <v-col cols="12">
        <v-card class="pa-6">
          <v-row align="center">
            <v-col cols="12" md="8">
              <h1 class="text-h4 font-weight-bold mb-2">Analytics</h1>
              <p class="text-body-1 text-medium-emphasis">
                Track your productivity and task performance
              </p>
            </v-col>
            <v-col cols="12" md="4" class="text-right">
              <v-select
                v-model="selectedPeriod"
                :items="periodOptions"
                label="Time Period"
                variant="outlined"
                density="compact"
                @update:model-value="updateAnalytics"
              ></v-select>
            </v-col>
          </v-row>
        </v-card>
      </v-col>
    </v-row>

    <!-- Key Metrics -->
    <v-row class="mb-6">
      <v-col cols="12" sm="6" md="3" v-for="metric in keyMetrics" :key="metric.title">
        <v-card class="pa-4" :color="metric.color" dark>
          <v-row align="center">
            <v-col cols="8">
              <div class="text-h4 font-weight-bold">{{ metric.value }}</div>
              <div class="text-body-1 opacity-90">{{ metric.title }}</div>
              <div class="text-caption opacity-70">
                <v-icon :icon="metric.trend > 0 ? 'mdi-trending-up' : 'mdi-trending-down'" size="small" class="me-1"></v-icon>
                {{ Math.abs(metric.trend) }}% from last period
              </div>
            </v-col>
            <v-col cols="4" class="text-right">
              <v-icon :icon="metric.icon" size="48" class="opacity-70"></v-icon>
            </v-col>
          </v-row>
        </v-card>
      </v-col>
    </v-row>

    <!-- Charts Row -->
    <v-row class="mb-6">
      <!-- Task Completion Chart -->
      <v-col cols="12" lg="8">
        <v-card>
          <v-card-title class="d-flex align-center">
            <v-icon icon="mdi-chart-line" class="me-2"></v-icon>
            Task Completion Trend
          </v-card-title>
          <v-card-text>
            <div class="chart-container">
              <canvas ref="completionChart"></canvas>
            </div>
          </v-card-text>
        </v-card>
      </v-col>

      <!-- Task Status Distribution -->
      <v-col cols="12" lg="4">
        <v-card>
          <v-card-title class="d-flex align-center">
            <v-icon icon="mdi-chart-pie" class="me-2"></v-icon>
            Task Status Distribution
          </v-card-title>
          <v-card-text>
            <div class="chart-container">
              <canvas ref="statusChart"></canvas>
            </div>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>

    <!-- Performance Metrics -->
    <v-row class="mb-6">
      <!-- Productivity Score -->
      <v-col cols="12" md="6">
        <v-card>
          <v-card-title class="d-flex align-center">
            <v-icon icon="mdi-speedometer" class="me-2"></v-icon>
            Productivity Score
          </v-card-title>
          <v-card-text>
            <div class="text-center">
              <div class="text-h2 font-weight-bold text-primary mb-2">87%</div>
              <v-progress-linear
                :model-value="87"
                color="primary"
                height="8"
                rounded
                class="mb-4"
              ></v-progress-linear>
              <p class="text-body-2 text-medium-emphasis">
                You're performing above average this month!
              </p>
            </div>
          </v-card-text>
        </v-card>
      </v-col>

      <!-- Average Completion Time -->
      <v-col cols="12" md="6">
        <v-card>
          <v-card-title class="d-flex align-center">
            <v-icon icon="mdi-clock" class="me-2"></v-icon>
            Average Completion Time
          </v-card-title>
          <v-card-text>
            <div class="text-center">
              <div class="text-h2 font-weight-bold text-info mb-2">2.3</div>
              <p class="text-h6 mb-2">Days</p>
              <p class="text-body-2 text-medium-emphasis">
                Average time to complete a task
              </p>
            </div>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>

    <!-- Detailed Analytics -->
    <v-row>
      <!-- Task Categories Performance -->
      <v-col cols="12" lg="6">
        <v-card>
          <v-card-title class="d-flex align-center">
            <v-icon icon="mdi-folder" class="me-2"></v-icon>
            Performance by Category
          </v-card-title>
          <v-card-text>
            <v-list>
              <v-list-item
                v-for="category in categoryPerformance"
                :key="category.name"
              >
                <template v-slot:prepend>
                  <v-avatar :color="category.color" size="32">
                    <v-icon :icon="category.icon" color="white"></v-icon>
                  </v-avatar>
                </template>
                
                <v-list-item-title>{{ category.name }}</v-list-item-title>
                <v-list-item-subtitle>
                  {{ category.completed }}/{{ category.total }} tasks completed
                </v-list-item-subtitle>
                
                <template v-slot:append>
                  <div class="text-right">
                    <div class="text-h6 font-weight-bold">{{ category.percentage }}%</div>
                    <v-progress-linear
                      :model-value="category.percentage"
                      :color="category.color"
                      height="4"
                      rounded
                    ></v-progress-linear>
                  </div>
                </template>
              </v-list-item>
            </v-list>
          </v-card-text>
        </v-card>
      </v-col>

      <!-- Recent Activity -->
      <v-col cols="12" lg="6">
        <v-card>
          <v-card-title class="d-flex align-center">
            <v-icon icon="mdi-history" class="me-2"></v-icon>
            Recent Activity
          </v-card-title>
          <v-card-text>
            <v-timeline density="compact" align="start">
              <v-timeline-item
                v-for="activity in recentActivity"
                :key="activity.id"
                :dot-color="getActivityColor(activity.type)"
                size="small"
              >
                <div class="text-body-2">{{ activity.description }}</div>
                <div class="text-caption text-medium-emphasis">
                  {{ formatTimeAgo(activity.timestamp) }}
                </div>
              </v-timeline-item>
            </v-timeline>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>

    <!-- Export Options -->
    <v-row class="mt-6">
      <v-col cols="12">
        <v-card>
          <v-card-title class="d-flex align-center">
            <v-icon icon="mdi-download" class="me-2"></v-icon>
            Export Analytics
          </v-card-title>
          <v-card-text>
            <v-row>
              <v-col cols="12" md="4">
                <v-btn
                  color="primary"
                  variant="outlined"
                  block
                  prepend-icon="mdi-file-pdf"
                  @click="exportPDF"
                >
                  Export as PDF
                </v-btn>
              </v-col>
              <v-col cols="12" md="4">
                <v-btn
                  color="success"
                  variant="outlined"
                  block
                  prepend-icon="mdi-file-excel"
                  @click="exportExcel"
                >
                  Export as Excel
                </v-btn>
              </v-col>
              <v-col cols="12" md="4">
                <v-btn
                  color="info"
                  variant="outlined"
                  block
                  prepend-icon="mdi-file-chart"
                  @click="exportCSV"
                >
                  Export as CSV
                </v-btn>
              </v-col>
            </v-row>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup>
import { ref, computed, onMounted, nextTick } from 'vue'
import { Chart, registerables } from 'chart.js'

// Register Chart.js components
Chart.register(...registerables)

// Reactive data
const selectedPeriod = ref('30days')
const completionChart = ref(null)
const statusChart = ref(null)

const periodOptions = [
  { title: 'Last 7 days', value: '7days' },
  { title: 'Last 30 days', value: '30days' },
  { title: 'Last 3 months', value: '3months' },
  { title: 'Last year', value: '1year' }
]

const keyMetrics = computed(() => [
  {
    title: 'Tasks Completed',
    value: '24',
    icon: 'mdi-check-circle',
    color: 'success',
    trend: 12
  },
  {
    title: 'Productivity Score',
    value: '87%',
    icon: 'mdi-speedometer',
    color: 'primary',
    trend: 5
  },
  {
    title: 'Avg. Completion Time',
    value: '2.3d',
    icon: 'mdi-clock',
    color: 'info',
    trend: -8
  },
  {
    title: 'Team Collaboration',
    value: '15',
    icon: 'mdi-account-group',
    color: 'warning',
    trend: 3
  }
])

const categoryPerformance = ref([
  {
    name: 'Development',
    completed: 12,
    total: 15,
    percentage: 80,
    color: 'primary',
    icon: 'mdi-code-tags'
  },
  {
    name: 'Design',
    completed: 8,
    total: 10,
    percentage: 80,
    color: 'success',
    icon: 'mdi-palette'
  },
  {
    name: 'Testing',
    completed: 6,
    total: 8,
    percentage: 75,
    color: 'info',
    icon: 'mdi-bug'
  },
  {
    name: 'Documentation',
    completed: 4,
    total: 6,
    percentage: 67,
    color: 'warning',
    icon: 'mdi-file-document'
  }
])

const recentActivity = ref([
  {
    id: 1,
    type: 'completed',
    description: 'Completed task "Implement user authentication"',
    timestamp: new Date(Date.now() - 2 * 60 * 60 * 1000) // 2 hours ago
  },
  {
    id: 2,
    type: 'created',
    description: 'Created new task "Update API documentation"',
    timestamp: new Date(Date.now() - 4 * 60 * 60 * 1000) // 4 hours ago
  },
  {
    id: 3,
    type: 'assigned',
    description: 'Assigned task "Fix login bug" to John Doe',
    timestamp: new Date(Date.now() - 6 * 60 * 60 * 1000) // 6 hours ago
  },
  {
    id: 4,
    type: 'commented',
    description: 'Added comment to "Database optimization"',
    timestamp: new Date(Date.now() - 8 * 60 * 60 * 1000) // 8 hours ago
  }
])

// Methods
const getActivityColor = (type) => {
  const colors = {
    completed: 'success',
    created: 'primary',
    assigned: 'info',
    commented: 'warning'
  }
  return colors[type] || 'grey'
}

const formatTimeAgo = (timestamp) => {
  const now = new Date()
  const diff = now - timestamp
  const hours = Math.floor(diff / (1000 * 60 * 60))
  const days = Math.floor(hours / 24)
  
  if (days > 0) return `${days} day${days > 1 ? 's' : ''} ago`
  if (hours > 0) return `${hours} hour${hours > 1 ? 's' : ''} ago`
  return 'Just now'
}

const updateAnalytics = () => {
  // Mock analytics update
  console.log('Updating analytics for period:', selectedPeriod.value)
  // In a real app, this would fetch new data from the API
}

const exportPDF = () => {
  // Mock PDF export
  console.log('Exporting analytics as PDF')
}

const exportExcel = () => {
  // Mock Excel export
  console.log('Exporting analytics as Excel')
}

const exportCSV = () => {
  // Mock CSV export
  console.log('Exporting analytics as CSV')
}

const createCompletionChart = () => {
  if (!completionChart.value) return
  
  const ctx = completionChart.value.getContext('2d')
  new Chart(ctx, {
    type: 'line',
    data: {
      labels: ['Week 1', 'Week 2', 'Week 3', 'Week 4'],
      datasets: [{
        label: 'Tasks Completed',
        data: [5, 8, 12, 15],
        borderColor: 'rgb(25, 118, 210)',
        backgroundColor: 'rgba(25, 118, 210, 0.1)',
        tension: 0.4,
        fill: true
      }]
    },
    options: {
      responsive: true,
      maintainAspectRatio: false,
      plugins: {
        legend: {
          display: false
        }
      },
      scales: {
        y: {
          beginAtZero: true
        }
      }
    }
  })
}

const createStatusChart = () => {
  if (!statusChart.value) return
  
  const ctx = statusChart.value.getContext('2d')
  new Chart(ctx, {
    type: 'doughnut',
    data: {
      labels: ['Completed', 'In Progress', 'Pending', 'Overdue'],
      datasets: [{
        data: [18, 4, 2, 1],
        backgroundColor: [
          'rgb(76, 175, 80)',
          'rgb(33, 150, 243)',
          'rgb(255, 193, 7)',
          'rgb(244, 67, 54)'
        ]
      }]
    },
    options: {
      responsive: true,
      maintainAspectRatio: false,
      plugins: {
        legend: {
          position: 'bottom'
        }
      }
    }
  })
}

// Lifecycle
onMounted(async () => {
  await nextTick()
  createCompletionChart()
  createStatusChart()
})
</script>

<style scoped>
.chart-container {
  position: relative;
  height: 300px;
  width: 100%;
}

.v-card {
  border-radius: 12px;
}

.v-timeline-item {
  padding-bottom: 16px;
}
</style>
