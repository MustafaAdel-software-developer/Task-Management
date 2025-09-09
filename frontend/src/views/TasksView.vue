<template>
  <v-container fluid>
    <!-- Header -->
    <v-row class="mb-6">
      <v-col cols="12">
        <v-card class="pa-6">
          <v-row align="center">
            <v-col cols="12" md="8">
              <h1 class="text-h4 font-weight-bold mb-2">Tasks</h1>
              <p class="text-body-1 text-medium-emphasis">
                Manage and track all your tasks in one place
              </p>
            </v-col>
            <v-col cols="12" md="4" class="text-right">
              <v-btn
                color="primary"
                size="large"
                prepend-icon="mdi-plus"
                @click="showCreateDialog = true"
              >
                Create Task
              </v-btn>
            </v-col>
          </v-row>
        </v-card>
      </v-col>
    </v-row>

    <!-- Filters and Search -->
    <v-row class="mb-4">
      <v-col cols="12">
        <v-card>
          <v-card-text>
            <v-row>
              <v-col cols="12" md="4">
                <v-text-field
                  v-model="filters.search"
                  label="Search tasks..."
                  prepend-inner-icon="mdi-magnify"
                  variant="outlined"
                  density="compact"
                  clearable
                  @input="updateFilters"
                ></v-text-field>
              </v-col>
              
              <v-col cols="12" md="2">
                <v-select
                  v-model="filters.status"
                  label="Status"
                  :items="statusOptions"
                  variant="outlined"
                  density="compact"
                  clearable
                  @update:model-value="updateFilters"
                ></v-select>
              </v-col>
              
              <v-col cols="12" md="2">
                <v-select
                  v-model="filters.priority"
                  label="Priority"
                  :items="priorityOptions"
                  variant="outlined"
                  density="compact"
                  clearable
                  @update:model-value="updateFilters"
                ></v-select>
              </v-col>
              
              <v-col cols="12" md="2">
                <v-select
                  v-model="filters.category"
                  label="Category"
                  :items="categoryOptions"
                  variant="outlined"
                  density="compact"
                  clearable
                  @update:model-value="updateFilters"
                ></v-select>
              </v-col>
              
              <v-col cols="12" md="2">
                <v-btn
                  variant="outlined"
                  @click="clearFilters"
                  class="mt-1"
                >
                  Clear Filters
                </v-btn>
              </v-col>
            </v-row>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>

    <!-- Tasks Table -->
    <v-row>
      <v-col cols="12">
        <v-card>
          <v-data-table
            :headers="headers"
            :items="filteredTasks"
            :loading="loading"
            :items-per-page="pagination.pageSize"
            :page="pagination.page"
            class="elevation-1"
            @update:page="updatePage"
            @update:items-per-page="updatePageSize"
          >
            <!-- Task Title Column -->
            <template v-slot:item.title="{ item }">
              <div class="d-flex align-center">
                <v-icon
                  :icon="getStatusIcon(item.status)"
                  :color="getStatusColor(item.status)"
                  class="me-2"
                ></v-icon>
                <div>
                  <div class="font-weight-medium">{{ item.title }}</div>
                  <div class="text-caption text-medium-emphasis">
                    {{ item.description?.substring(0, 50) }}{{ item.description?.length > 50 ? '...' : '' }}
                  </div>
                </div>
              </div>
            </template>

            <!-- Status Column -->
            <template v-slot:item.status="{ item }">
              <v-chip
                :color="getStatusColor(item.status)"
                size="small"
                variant="tonal"
              >
                {{ item.status }}
              </v-chip>
            </template>

            <!-- Priority Column -->
            <template v-slot:item.priority="{ item }">
              <v-chip
                :color="getPriorityColor(item.priority)"
                size="small"
                variant="tonal"
                :prepend-icon="getPriorityIcon(item.priority)"
              >
                {{ item.priority }}
              </v-chip>
            </template>

            <!-- Due Date Column -->
            <template v-slot:item.dueDate="{ item }">
              <div class="d-flex align-center">
                <v-icon
                  :color="isOverdue(item.dueDate, item.status) ? 'error' : 'default'"
                  icon="mdi-calendar"
                  size="small"
                  class="me-1"
                ></v-icon>
                <span :class="{ 'text-error': isOverdue(item.dueDate, item.status) }">
                  {{ formatDate(item.dueDate) }}
                </span>
              </div>
            </template>

            <!-- Assigned To Column -->
            <template v-slot:item.assignedTo="{ item }">
              <div v-if="item.assignedTo" class="d-flex align-center">
                <v-avatar size="24" class="me-2">
                  <v-img :src="item.assignedTo.avatar || 'https://randomuser.me/api/portraits/men/1.jpg'"></v-img>
                </v-avatar>
                <span class="text-body-2">{{ item.assignedTo.name }}</span>
              </div>
              <span v-else class="text-medium-emphasis">Unassigned</span>
            </template>

            <!-- Actions Column -->
            <template v-slot:item.actions="{ item }">
              <v-btn
                icon="mdi-eye"
                size="small"
                variant="text"
                @click="viewTask(item)"
              ></v-btn>
              <v-btn
                icon="mdi-pencil"
                size="small"
                variant="text"
                @click="editTask(item)"
              ></v-btn>
              <v-btn
                icon="mdi-delete"
                size="small"
                variant="text"
                color="error"
                @click="deleteTask(item)"
              ></v-btn>
            </template>
          </v-data-table>
        </v-card>
      </v-col>
    </v-row>

    <!-- Create/Edit Task Dialog -->
    <v-dialog v-model="showCreateDialog" max-width="800" persistent>
      <v-card>
        <v-card-title>
          {{ editingTask ? 'Edit Task' : 'Create New Task' }}
        </v-card-title>
        
        <v-card-text>
          <v-form ref="taskForm" @submit.prevent="saveTask">
            <v-row>
              <v-col cols="12" md="8">
                <v-text-field
                  v-model="taskForm.title"
                  label="Task Title"
                  :rules="[v => !!v || 'Title is required']"
                  required
                  variant="outlined"
                  class="mb-4"
                ></v-text-field>
              </v-col>
              
              <v-col cols="12" md="4">
                <v-select
                  v-model="taskForm.priority"
                  label="Priority"
                  :items="priorityOptions"
                  :rules="[v => !!v || 'Priority is required']"
                  required
                  variant="outlined"
                  class="mb-4"
                ></v-select>
              </v-col>
            </v-row>
            
            <v-textarea
              v-model="taskForm.description"
              label="Description"
              variant="outlined"
              rows="3"
              class="mb-4"
            ></v-textarea>
            
            <v-row>
              <v-col cols="12" md="6">
                <v-select
                  v-model="taskForm.categoryId"
                  label="Category"
                  :items="categoryOptions"
                  variant="outlined"
                  class="mb-4"
                ></v-select>
              </v-col>
              
              <v-col cols="12" md="6">
                <v-text-field
                  v-model="taskForm.dueDate"
                  label="Due Date"
                  type="date"
                  variant="outlined"
                  class="mb-4"
                ></v-text-field>
              </v-col>
            </v-row>
            
            <v-select
              v-model="taskForm.assignedToId"
              label="Assign To"
              :items="userOptions"
              variant="outlined"
              class="mb-4"
            ></v-select>
          </v-form>
        </v-card-text>
        
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn @click="closeDialog">Cancel</v-btn>
          <v-btn
            color="primary"
            @click="saveTask"
            :loading="loading"
          >
            {{ editingTask ? 'Update' : 'Create' }}
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <!-- Delete Confirmation Dialog -->
    <v-dialog v-model="showDeleteDialog" max-width="400">
      <v-card>
        <v-card-title>Delete Task</v-card-title>
        <v-card-text>
          Are you sure you want to delete "{{ taskToDelete?.title }}"? This action cannot be undone.
        </v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn @click="showDeleteDialog = false">Cancel</v-btn>
          <v-btn color="error" @click="confirmDelete" :loading="loading">
            Delete
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </v-container>
</template>

<script setup>
import { ref, computed, onMounted, watch } from 'vue'
import { useRouter } from 'vue-router'
import { useTaskStore } from '@/stores/tasks'

const router = useRouter()
const taskStore = useTaskStore()

// Reactive data
const showCreateDialog = ref(false)
const showDeleteDialog = ref(false)
const editingTask = ref(null)
const taskToDelete = ref(null)
const taskForm = ref({
  title: '',
  description: '',
  priority: 'medium',
  categoryId: '',
  dueDate: '',
  assignedToId: ''
})

// Table headers
const headers = [
  { title: 'Task', key: 'title', sortable: true },
  { title: 'Status', key: 'status', sortable: true },
  { title: 'Priority', key: 'priority', sortable: true },
  { title: 'Due Date', key: 'dueDate', sortable: true },
  { title: 'Assigned To', key: 'assignedTo', sortable: false },
  { title: 'Actions', key: 'actions', sortable: false }
]

// Options for filters and forms
const statusOptions = [
  { title: 'Pending', value: 'pending' },
  { title: 'In Progress', value: 'inProgress' },
  { title: 'Completed', value: 'completed' },
  { title: 'Cancelled', value: 'cancelled' }
]

const priorityOptions = [
  { title: 'Low', value: 'low' },
  { title: 'Medium', value: 'medium' },
  { title: 'High', value: 'high' },
  { title: 'Urgent', value: 'urgent' }
]

const categoryOptions = computed(() => 
  taskStore.categories.map(cat => ({
    title: cat.name,
    value: cat.id
  }))
)

const userOptions = [
  { title: 'John Doe', value: '1' },
  { title: 'Jane Smith', value: '2' },
  { title: 'Bob Johnson', value: '3' }
]

// Computed properties
const loading = computed(() => taskStore.loading)
const tasks = computed(() => taskStore.tasks)
const filteredTasks = computed(() => taskStore.filteredTasks)
const pagination = computed(() => taskStore.pagination)
const filters = computed(() => taskStore.filters)

// Methods
const updateFilters = () => {
  taskStore.updateFilters(filters.value)
}

const clearFilters = () => {
  taskStore.clearFilters()
}

const updatePage = (page) => {
  taskStore.pagination.page = page
  taskStore.fetchTasks()
}

const updatePageSize = (pageSize) => {
  taskStore.pagination.pageSize = pageSize
  taskStore.fetchTasks()
}

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

const isOverdue = (dueDate, status) => {
  if (status === 'completed') return false
  const today = new Date()
  const due = new Date(dueDate)
  return due < today
}

const formatDate = (dateString) => {
  return new Date(dateString).toLocaleDateString()
}

const viewTask = (task) => {
  router.push({ name: 'TaskDetail', params: { id: task.id } })
}

const editTask = (task) => {
  editingTask.value = task
  taskForm.value = {
    title: task.title,
    description: task.description,
    priority: task.priority,
    categoryId: task.categoryId,
    dueDate: task.dueDate,
    assignedToId: task.assignedToId
  }
  showCreateDialog.value = true
}

const deleteTask = (task) => {
  taskToDelete.value = task
  showDeleteDialog.value = true
}

const confirmDelete = async () => {
  try {
    await taskStore.deleteTask(taskToDelete.value.id)
    showDeleteDialog.value = false
    taskToDelete.value = null
  } catch (error) {
    console.error('Failed to delete task:', error)
  }
}

const saveTask = async () => {
  try {
    if (editingTask.value) {
      await taskStore.updateTask(editingTask.value.id, taskForm.value)
    } else {
      await taskStore.createTask(taskForm.value)
    }
    closeDialog()
  } catch (error) {
    console.error('Failed to save task:', error)
  }
}

const closeDialog = () => {
  showCreateDialog.value = false
  editingTask.value = null
  taskForm.value = {
    title: '',
    description: '',
    priority: 'medium',
    categoryId: '',
    dueDate: '',
    assignedToId: ''
  }
}

// Lifecycle
onMounted(async () => {
  try {
    await Promise.all([
      taskStore.fetchTasks(),
      taskStore.fetchCategories()
    ])
  } catch (error) {
    console.error('Failed to load data:', error)
  }
})
</script>

<style scoped>
.v-data-table {
  border-radius: 8px;
}

.v-card {
  border-radius: 12px;
}
</style>
