<template>
  <v-container fluid v-if="task">
    <!-- Header -->
    <v-row class="mb-6">
      <v-col cols="12">
        <v-card class="pa-6">
          <v-row align="center">
            <v-col cols="12" md="8">
              <div class="d-flex align-center mb-2">
                <v-icon
                  :icon="getStatusIcon(task.status)"
                  :color="getStatusColor(task.status)"
                  size="large"
                  class="me-3"
                ></v-icon>
                <h1 class="text-h4 font-weight-bold">{{ task.title }}</h1>
              </div>
              <p class="text-body-1 text-medium-emphasis">{{ task.description }}</p>
            </v-col>
            <v-col cols="12" md="4" class="text-right">
              <v-btn
                color="primary"
                prepend-icon="mdi-pencil"
                @click="editMode = true"
                class="me-2"
              >
                Edit
              </v-btn>
              <v-btn
                variant="outlined"
                prepend-icon="mdi-arrow-left"
                @click="$router.push({ name: 'Tasks' })"
              >
                Back to Tasks
              </v-btn>
            </v-col>
          </v-row>
        </v-card>
      </v-col>
    </v-row>

    <v-row>
      <!-- Task Details -->
      <v-col cols="12" lg="8">
        <!-- Task Information -->
        <v-card class="mb-6">
          <v-card-title class="d-flex align-center">
            <v-icon icon="mdi-information" class="me-2"></v-icon>
            Task Information
          </v-card-title>
          <v-card-text>
            <v-row>
              <v-col cols="12" md="6">
                <div class="mb-4">
                  <div class="text-caption text-medium-emphasis mb-1">Status</div>
                  <v-chip
                    :color="getStatusColor(task.status)"
                    size="large"
                    variant="tonal"
                  >
                    {{ task.status }}
                  </v-chip>
                </div>
              </v-col>
              
              <v-col cols="12" md="6">
                <div class="mb-4">
                  <div class="text-caption text-medium-emphasis mb-1">Priority</div>
                  <v-chip
                    :color="getPriorityColor(task.priority)"
                    size="large"
                    variant="tonal"
                    :prepend-icon="getPriorityIcon(task.priority)"
                  >
                    {{ task.priority }}
                  </v-chip>
                </div>
              </v-col>
              
              <v-col cols="12" md="6">
                <div class="mb-4">
                  <div class="text-caption text-medium-emphasis mb-1">Due Date</div>
                  <div class="d-flex align-center">
                    <v-icon
                      :color="isOverdue ? 'error' : 'default'"
                      icon="mdi-calendar"
                      class="me-2"
                    ></v-icon>
                    <span :class="{ 'text-error': isOverdue }">
                      {{ formatDate(task.dueDate) }}
                    </span>
                  </div>
                </div>
              </v-col>
              
              <v-col cols="12" md="6">
                <div class="mb-4">
                  <div class="text-caption text-medium-emphasis mb-1">Created</div>
                  <div class="d-flex align-center">
                    <v-icon icon="mdi-calendar-plus" class="me-2"></v-icon>
                    <span>{{ formatDate(task.createdAt) }}</span>
                  </div>
                </div>
              </v-col>
              
              <v-col cols="12" md="6">
                <div class="mb-4">
                  <div class="text-caption text-medium-emphasis mb-1">Assigned To</div>
                  <div v-if="task.assignedTo" class="d-flex align-center">
                    <v-avatar size="32" class="me-2">
                      <v-img :src="task.assignedTo.avatar || 'https://randomuser.me/api/portraits/men/1.jpg'"></v-img>
                    </v-avatar>
                    <span>{{ task.assignedTo.name }}</span>
                  </div>
                  <span v-else class="text-medium-emphasis">Unassigned</span>
                </div>
              </v-col>
              
              <v-col cols="12" md="6">
                <div class="mb-4">
                  <div class="text-caption text-medium-emphasis mb-1">Category</div>
                  <v-chip
                    v-if="task.category"
                    color="primary"
                    variant="tonal"
                  >
                    {{ task.category.name }}
                  </v-chip>
                  <span v-else class="text-medium-emphasis">No category</span>
                </div>
              </v-col>
            </v-row>
          </v-card-text>
        </v-card>

        <!-- Comments Section -->
        <v-card>
          <v-card-title class="d-flex align-center">
            <v-icon icon="mdi-comment" class="me-2"></v-icon>
            Comments
            <v-spacer></v-spacer>
            <v-btn
              color="primary"
              prepend-icon="mdi-plus"
              @click="showAddComment = true"
            >
              Add Comment
            </v-btn>
          </v-card-title>
          
          <v-card-text>
            <v-list v-if="comments.length > 0">
              <v-list-item
                v-for="comment in comments"
                :key="comment.id"
                class="mb-4"
              >
                <template v-slot:prepend>
                  <v-avatar>
                    <v-img :src="comment.author.avatar || 'https://randomuser.me/api/portraits/men/1.jpg'"></v-img>
                  </v-avatar>
                </template>
                
                <v-list-item-title>{{ comment.author.name }}</v-list-item-title>
                <v-list-item-subtitle>
                  {{ formatDate(comment.createdAt) }}
                </v-list-item-subtitle>
                
                <template v-slot:append>
                  <v-btn
                    icon="mdi-delete"
                    size="small"
                    variant="text"
                    color="error"
                    @click="deleteComment(comment.id)"
                  ></v-btn>
                </template>
                
                <v-list-item-text class="mt-2">
                  {{ comment.content }}
                </v-list-item-text>
              </v-list-item>
            </v-list>
            
            <v-empty-state
              v-else
              title="No comments yet"
              text="Be the first to add a comment!"
              icon="mdi-comment-outline"
            ></v-empty-state>
          </v-card-text>
        </v-card>
      </v-col>

      <!-- Sidebar -->
      <v-col cols="12" lg="4">
        <!-- Quick Actions -->
        <v-card class="mb-6">
          <v-card-title class="d-flex align-center">
            <v-icon icon="mdi-lightning-bolt" class="me-2"></v-icon>
            Quick Actions
          </v-card-title>
          <v-card-text>
            <v-list>
              <v-list-item
                prepend-icon="mdi-check-circle"
                title="Mark as Completed"
                @click="updateStatus('completed')"
                v-if="task.status !== 'completed'"
              ></v-list-item>
              
              <v-list-item
                prepend-icon="mdi-progress-clock"
                title="Start Task"
                @click="updateStatus('inProgress')"
                v-if="task.status === 'pending'"
              ></v-list-item>
              
              <v-list-item
                prepend-icon="mdi-account-plus"
                title="Assign Task"
                @click="showAssignDialog = true"
              ></v-list-item>
              
              <v-list-item
                prepend-icon="mdi-calendar-edit"
                title="Change Due Date"
                @click="showDueDateDialog = true"
              ></v-list-item>
            </v-list>
          </v-card-text>
        </v-card>

        <!-- Task History -->
        <v-card>
          <v-card-title class="d-flex align-center">
            <v-icon icon="mdi-history" class="me-2"></v-icon>
            Task History
          </v-card-title>
          <v-card-text>
            <v-timeline density="compact" align="start">
              <v-timeline-item
                v-for="activity in taskHistory"
                :key="activity.id"
                :dot-color="getActivityColor(activity.type)"
                size="small"
              >
                <div class="text-body-2">{{ activity.description }}</div>
                <div class="text-caption text-medium-emphasis">
                  {{ formatDate(activity.createdAt) }}
                </div>
              </v-timeline-item>
            </v-timeline>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>

    <!-- Add Comment Dialog -->
    <v-dialog v-model="showAddComment" max-width="600">
      <v-card>
        <v-card-title>Add Comment</v-card-title>
        <v-card-text>
          <v-textarea
            v-model="newComment"
            label="Comment"
            rows="4"
            variant="outlined"
            :rules="[v => !!v || 'Comment is required']"
          ></v-textarea>
        </v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn @click="showAddComment = false">Cancel</v-btn>
          <v-btn color="primary" @click="addComment" :loading="loading">
            Add Comment
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <!-- Assign Task Dialog -->
    <v-dialog v-model="showAssignDialog" max-width="400">
      <v-card>
        <v-card-title>Assign Task</v-card-title>
        <v-card-text>
          <v-select
            v-model="selectedAssignee"
            label="Assign to"
            :items="userOptions"
            variant="outlined"
          ></v-select>
        </v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn @click="showAssignDialog = false">Cancel</v-btn>
          <v-btn color="primary" @click="assignTask" :loading="loading">
            Assign
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <!-- Due Date Dialog -->
    <v-dialog v-model="showDueDateDialog" max-width="400">
      <v-card>
        <v-card-title>Change Due Date</v-card-title>
        <v-card-text>
          <v-text-field
            v-model="newDueDate"
            label="Due Date"
            type="date"
            variant="outlined"
          ></v-text-field>
        </v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn @click="showDueDateDialog = false">Cancel</v-btn>
          <v-btn color="primary" @click="updateDueDate" :loading="loading">
            Update
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </v-container>

  <!-- Loading State -->
  <v-container v-else-if="loading" class="fill-height d-flex align-center justify-center">
    <v-progress-circular indeterminate size="64"></v-progress-circular>
  </v-container>

  <!-- Error State -->
  <v-container v-else class="fill-height d-flex align-center justify-center">
    <v-card>
      <v-card-text class="text-center">
        <v-icon icon="mdi-alert-circle" size="64" color="error" class="mb-4"></v-icon>
        <h2 class="text-h5 mb-2">Task Not Found</h2>
        <p class="text-body-1 mb-4">The task you're looking for doesn't exist or has been deleted.</p>
        <v-btn color="primary" @click="$router.push({ name: 'Tasks' })">
          Back to Tasks
        </v-btn>
      </v-card-text>
    </v-card>
  </v-container>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { useTaskStore } from '@/stores/tasks'

const route = useRoute()
const router = useRouter()
const taskStore = useTaskStore()

// Reactive data
const editMode = ref(false)
const showAddComment = ref(false)
const showAssignDialog = ref(false)
const showDueDateDialog = ref(false)
const newComment = ref('')
const selectedAssignee = ref('')
const newDueDate = ref('')

const userOptions = [
  { title: 'John Doe', value: '1' },
  { title: 'Jane Smith', value: '2' },
  { title: 'Bob Johnson', value: '3' }
]

// Mock data
const comments = ref([
  {
    id: 1,
    content: 'This task is progressing well. I should be able to complete it by the due date.',
    author: { name: 'John Doe', avatar: 'https://randomuser.me/api/portraits/men/1.jpg' },
    createdAt: '2024-01-15T10:30:00Z'
  },
  {
    id: 2,
    content: 'Great work! Let me know if you need any help.',
    author: { name: 'Jane Smith', avatar: 'https://randomuser.me/api/portraits/women/1.jpg' },
    createdAt: '2024-01-15T14:20:00Z'
  }
])

const taskHistory = ref([
  {
    id: 1,
    type: 'created',
    description: 'Task created',
    createdAt: '2024-01-10T09:00:00Z'
  },
  {
    id: 2,
    type: 'assigned',
    description: 'Assigned to John Doe',
    createdAt: '2024-01-10T09:15:00Z'
  },
  {
    id: 3,
    type: 'status',
    description: 'Status changed to In Progress',
    createdAt: '2024-01-12T14:30:00Z'
  }
])

// Computed properties
const loading = computed(() => taskStore.loading)
const task = computed(() => taskStore.currentTask)
const isOverdue = computed(() => {
  if (!task.value || task.value.status === 'completed') return false
  const today = new Date()
  const due = new Date(task.value.dueDate)
  return due < today
})

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

const getActivityColor = (type) => {
  const colors = {
    created: 'success',
    assigned: 'info',
    status: 'warning',
    comment: 'primary'
  }
  return colors[type] || 'grey'
}

const formatDate = (dateString) => {
  return new Date(dateString).toLocaleDateString()
}

const updateStatus = async (status) => {
  try {
    await taskStore.updateTask(task.value.id, { status })
  } catch (error) {
    console.error('Failed to update status:', error)
  }
}

const addComment = async () => {
  try {
    // Mock comment addition
    const comment = {
      id: Date.now(),
      content: newComment.value,
      author: { name: 'Current User', avatar: 'https://randomuser.me/api/portraits/men/1.jpg' },
      createdAt: new Date().toISOString()
    }
    comments.value.unshift(comment)
    newComment.value = ''
    showAddComment.value = false
  } catch (error) {
    console.error('Failed to add comment:', error)
  }
}

const deleteComment = async (commentId) => {
  try {
    comments.value = comments.value.filter(c => c.id !== commentId)
  } catch (error) {
    console.error('Failed to delete comment:', error)
  }
}

const assignTask = async () => {
  try {
    await taskStore.updateTask(task.value.id, { assignedToId: selectedAssignee.value })
    showAssignDialog.value = false
    selectedAssignee.value = ''
  } catch (error) {
    console.error('Failed to assign task:', error)
  }
}

const updateDueDate = async () => {
  try {
    await taskStore.updateTask(task.value.id, { dueDate: newDueDate.value })
    showDueDateDialog.value = false
    newDueDate.value = ''
  } catch (error) {
    console.error('Failed to update due date:', error)
  }
}

// Lifecycle
onMounted(async () => {
  try {
    await taskStore.fetchTask(route.params.id)
  } catch (error) {
    console.error('Failed to fetch task:', error)
  }
})
</script>

<style scoped>
.v-timeline-item {
  padding-bottom: 16px;
}

.v-card {
  border-radius: 12px;
}
</style>
