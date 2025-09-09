<template>
  <v-container fluid>
    <!-- Welcome Section -->
    <v-row class="mb-6">
      <v-col cols="12">
        <v-card class="pa-6" color="primary" dark>
          <v-row align="center">
            <v-col cols="12" md="8">
              <h1 class="text-h4 font-weight-bold mb-2">
                Welcome back, {{ user?.name || 'User' }}!
              </h1>
              <p class="text-h6 opacity-90">
                Here's what's happening with your tasks today.
              </p>
            </v-col>
            <v-col cols="12" md="4" class="text-right">
              <v-btn
                color="white"
                variant="outlined"
                size="large"
                prepend-icon="mdi-plus"
                @click="$router.push({ name: 'Tasks' })"
              >
                Create Task
              </v-btn>
            </v-col>
          </v-row>
        </v-card>
      </v-col>
    </v-row>

    <!-- Statistics Cards -->
    <v-row class="mb-6">
      <v-col cols="12" sm="6" md="3" v-for="stat in statistics" :key="stat.title">
        <v-card class="pa-4" :color="stat.color" dark>
          <v-row align="center">
            <v-col cols="8">
              <div class="text-h4 font-weight-bold">{{ stat.value }}</div>
              <div class="text-body-1 opacity-90">{{ stat.title }}</div>
            </v-col>
            <v-col cols="4" class="text-right">
              <v-icon :icon="stat.icon" size="48" class="opacity-70"></v-icon>
            </v-col>
          </v-row>
        </v-card>
      </v-col>
    </v-row>

    <!-- Recent Tasks and Quick Actions -->
    <v-row>
      <!-- Recent Tasks -->
      <v-col cols="12" lg="8">
        <v-card>
          <v-card-title class="d-flex align-center">
            <v-icon icon="mdi-format-list-checks" class="me-2"></v-icon>
            Recent Tasks
            <v-spacer></v-spacer>
            <v-btn
              variant="text"
              color="primary"
              @click="$router.push({ name: 'Tasks' })"
            >
              View All
            </v-btn>
          </v-card-title>
          
          <v-card-text>
            <v-list v-if="recentTasks.length > 0">
              <v-list-item
                v-for="task in recentTasks"
                :key="task.id"
                :title="task.title"
                :subtitle="task.description"
                :prepend-icon="getStatusIcon(task.status)"
                :append-icon="getPriorityIcon(task.priority)"
                @click="$router.push({ name: 'TaskDetail', params: { id: task.id } })"
              >
                <template v-slot:subtitle>
                  <div class="d-flex align-center">
                    <v-chip
                      :color="getStatusColor(task.status)"
                      size="small"
                      class="me-2"
                    >
                      {{ task.status }}
                    </v-chip>
                    <span class="text-caption">
                      Due: {{ formatDate(task.dueDate) }}
                    </span>
                  </div>
                </template>
              </v-list-item>
            </v-list>
            
            <v-empty-state
              v-else
              title="No recent tasks"
              text="Create your first task to get started!"
              icon="mdi-format-list-checks"
            >
              <v-btn
                color="primary"
                @click="$router.push({ name: 'Tasks' })"
              >
                Create Task
              </v-btn>
            </v-empty-state>
          </v-card-text>
        </v-card>
      </v-col>

      <!-- Quick Actions -->
      <v-col cols="12" lg="4">
        <v-card>
          <v-card-title class="d-flex align-center">
            <v-icon icon="mdi-lightning-bolt" class="me-2"></v-icon>
            Quick Actions
          </v-card-title>
          
          <v-card-text>
            <v-list>
              <v-list-item
                prepend-icon="mdi-plus"
                title="Create New Task"
                subtitle="Add a new task to your list"
                @click="showCreateTaskDialog = true"
              ></v-list-item>
              
              <v-list-item
                prepend-icon="mdi-account-plus"
                title="Invite Team Member"
                subtitle="Add someone to your team"
                @click="showInviteDialog = true"
              ></v-list-item>
              
              <v-list-item
                prepend-icon="mdi-chart-line"
                title="View Analytics"
                subtitle="Check your productivity stats"
                @click="$router.push({ name: 'Analytics' })"
              ></v-list-item>
              
              <v-list-item
                prepend-icon="mdi-cog"
                title="Settings"
                subtitle="Manage your preferences"
                @click="$router.push({ name: 'Profile' })"
              ></v-list-item>
            </v-list>
          </v-card-text>
        </v-card>

        <!-- Upcoming Deadlines -->
        <v-card class="mt-4">
          <v-card-title class="d-flex align-center">
            <v-icon icon="mdi-clock-alert" class="me-2"></v-icon>
            Upcoming Deadlines
          </v-card-title>
          
          <v-card-text>
            <v-list v-if="upcomingTasks.length > 0">
              <v-list-item
                v-for="task in upcomingTasks"
                :key="task.id"
                :title="task.title"
                :subtitle="`Due: ${formatDate(task.dueDate)}`"
                :prepend-icon="getPriorityIcon(task.priority)"
              >
                <template v-slot:prepend>
                  <v-icon
                    :color="getPriorityColor(task.priority)"
                    :icon="getPriorityIcon(task.priority)"
                  ></v-icon>
                </template>
              </v-list-item>
            </v-list>
            
            <v-empty-state
              v-else
              title="No upcoming deadlines"
              text="You're all caught up!"
              icon="mdi-check-circle"
            ></v-empty-state>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>

    <!-- Create Task Dialog -->
    <v-dialog v-model="showCreateTaskDialog" max-width="600">
      <v-card>
        <v-card-title>Create New Task</v-card-title>
        <v-card-text>
          <v-form @submit.prevent="createTask">
            <v-text-field
              v-model="newTask.title"
              label="Task Title"
              required
              variant="outlined"
              class="mb-4"
            ></v-text-field>
            
            <v-textarea
              v-model="newTask.description"
              label="Description"
              variant="outlined"
              class="mb-4"
            ></v-textarea>
            
            <v-select
              v-model="newTask.priority"
              label="Priority"
              :items="priorityOptions"
              variant="outlined"
              class="mb-4"
            ></v-select>
            
            <v-text-field
              v-model="newTask.dueDate"
              label="Due Date"
              type="date"
              variant="outlined"
            ></v-text-field>
          </v-form>
        </v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn @click="showCreateTaskDialog = false">Cancel</v-btn>
          <v-btn color="primary" @click="createTask">Create</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </v-container>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue'
import { useAuthStore } from '@/stores/auth'
import { useTaskStore } from '@/stores/tasks'

const authStore = useAuthStore()
const taskStore = useTaskStore()

// Reactive data
const showCreateTaskDialog = ref(false)
const showInviteDialog = ref(false)
const newTask = ref({
  title: '',
  description: '',
  priority: 'medium',
  dueDate: ''
})

const priorityOptions = [
  { title: 'Low', value: 'low' },
  { title: 'Medium', value: 'medium' },
  { title: 'High', value: 'high' },
  { title: 'Urgent', value: 'urgent' }
]

// Computed properties
const user = computed(() => authStore.user)
const recentTasks = computed(() => taskStore.tasks.slice(0, 5))
const upcomingTasks = computed(() => {
  const today = new Date()
  const nextWeek = new Date(today.getTime() + 7 * 24 * 60 * 60 * 1000)
  
  return taskStore.tasks.filter(task => {
    const dueDate = new Date(task.dueDate)
    return dueDate >= today && dueDate <= nextWeek && task.status !== 'completed'
  }).slice(0, 3)
})

const statistics = computed(() => [
  {
    title: 'Total Tasks',
    value: taskStore.tasks.length,
    icon: 'mdi-format-list-checks',
    color: 'primary'
  },
  {
    title: 'In Progress',
    value: taskStore.tasksByStatus.inProgress.length,
    icon: 'mdi-progress-clock',
    color: 'info'
  },
  {
    title: 'Completed',
    value: taskStore.tasksByStatus.completed.length,
    icon: 'mdi-check-circle',
    color: 'success'
  },
  {
    title: 'Overdue',
    value: taskStore.tasksByStatus.overdue.length,
    icon: 'mdi-alert-circle',
    color: 'error'
  }
])

// Methods
const getStatusIcon = (status) => {
  const icons = {
    pending: 'mdi-clock-outline',
    inProgress: 'mdi-progress-clock',
    completed: 'mdi-check-circle',
    cancelled: 'mdi-cancel'
  }
  return icons[status] || 'mdi-help-circle'
}

const getStatusColor = (status) => {
  const colors = {
    pending: 'warning',
    inProgress: 'info',
    completed: 'success',
    cancelled: 'error'
  }
  return colors[status] || 'grey'
}

const getPriorityIcon = (priority) => {
  const icons = {
    low: 'mdi-arrow-down',
    medium: 'mdi-minus',
    high: 'mdi-arrow-up',
    urgent: 'mdi-alert'
  }
  return icons[priority] || 'mdi-minus'
}

const getPriorityColor = (priority) => {
  const colors = {
    low: 'success',
    medium: 'info',
    high: 'warning',
    urgent: 'error'
  }
  return colors[priority] || 'grey'
}

const formatDate = (dateString) => {
  return new Date(dateString).toLocaleDateString()
}

const createTask = async () => {
  try {
    await taskStore.createTask(newTask.value)
    showCreateTaskDialog.value = false
    newTask.value = {
      title: '',
      description: '',
      priority: 'medium',
      dueDate: ''
    }
  } catch (error) {
    console.error('Failed to create task:', error)
  }
}

// Lifecycle
onMounted(async () => {
  try {
    await taskStore.fetchTasks()
  } catch (error) {
    console.error('Failed to fetch tasks:', error)
  }
})
</script>

<style scoped>
.v-card {
  border-radius: 12px;
}

.v-list-item {
  border-radius: 8px;
  margin-bottom: 4px;
}

.v-list-item:hover {
  background-color: rgba(0, 0, 0, 0.04);
}
</style>
