<template>
  <v-app class="full-height">
    <!-- Navigation Drawer -->
    <v-navigation-drawer
      v-model="drawer"
      :rail="rail"
      permanent
      @click="rail = false"
      v-if="isAuthenticated"
    >
      <v-list-item
        prepend-avatar="https://randomuser.me/api/portraits/men/85.jpg"
        :title="user?.name || 'User'"
        :subtitle="user?.email"
        nav
      >
        <template v-slot:append>
          <v-btn
            variant="text"
            icon="mdi-chevron-left"
            @click.stop="rail = !rail"
          ></v-btn>
        </template>
      </v-list-item>

      <v-divider></v-divider>

      <v-list density="compact" nav>
        <v-list-item
          prepend-icon="mdi-view-dashboard"
          title="Dashboard"
          value="dashboard"
          :to="{ name: 'Dashboard' }"
        ></v-list-item>

        <v-list-item
          prepend-icon="mdi-format-list-checks"
          title="Tasks"
          value="tasks"
          :to="{ name: 'Tasks' }"
        ></v-list-item>

        <v-list-item
          prepend-icon="mdi-chart-line"
          title="Analytics"
          value="analytics"
          :to="{ name: 'Analytics' }"
        ></v-list-item>

        <v-list-item
          prepend-icon="mdi-account"
          title="Profile"
          value="profile"
          :to="{ name: 'Profile' }"
        ></v-list-item>
      </v-list>
    </v-navigation-drawer>

    <!-- App Bar -->
    <v-app-bar
      :elevation="2"
      v-if="isAuthenticated"
    >
      <v-app-bar-nav-icon @click="drawer = !drawer"></v-app-bar-nav-icon>
      
      <v-toolbar-title>Task Management System</v-toolbar-title>

      <v-spacer></v-spacer>

      <!-- Notifications -->
      <v-btn icon @click="showNotifications = !showNotifications">
        <v-badge
          :content="notificationCount"
          :model-value="notificationCount > 0"
          color="error"
        >
          <v-icon>mdi-bell</v-icon>
        </v-badge>
      </v-btn>

      <!-- User Menu -->
      <v-menu>
        <template v-slot:activator="{ props }">
          <v-btn
            icon
            v-bind="props"
          >
            <v-avatar size="32">
              <v-img src="https://randomuser.me/api/portraits/men/85.jpg"></v-img>
            </v-avatar>
          </v-btn>
        </template>

        <v-list>
          <v-list-item
            prepend-icon="mdi-account"
            title="Profile"
            :to="{ name: 'Profile' }"
          ></v-list-item>
          <v-list-item
            prepend-icon="mdi-logout"
            title="Logout"
            @click="handleLogout"
          ></v-list-item>
        </v-list>
      </v-menu>
    </v-app-bar>

    <!-- Main Content -->
    <v-main>
      <router-view />
    </v-main>

    <!-- Notifications Panel -->
    <v-navigation-drawer
      v-model="showNotifications"
      location="right"
      temporary
      width="400"
    >
      <v-toolbar>
        <v-toolbar-title>Notifications</v-toolbar-title>
        <v-spacer></v-spacer>
        <v-btn icon @click="showNotifications = false">
          <v-icon>mdi-close</v-icon>
        </v-btn>
      </v-toolbar>
      
      <v-list>
        <v-list-item
          v-for="notification in notifications"
          :key="notification.id"
          :title="notification.title"
          :subtitle="notification.message"
          :prepend-icon="getNotificationIcon(notification.type)"
        >
        </v-list-item>
      </v-list>
    </v-navigation-drawer>

    <!-- Global Snackbar for notifications -->
    <v-snackbar
      v-model="snackbar.show"
      :color="snackbar.color"
      :timeout="snackbar.timeout"
    >
      {{ snackbar.message }}
      <template v-slot:actions>
        <v-btn
          color="white"
          variant="text"
          @click="snackbar.show = false"
        >
          Close
        </v-btn>
      </template>
    </v-snackbar>
  </v-app>
</template>

<script setup>
import { ref, computed, onMounted, onUnmounted } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/auth'
import signalRService from '@/services/signalRService'

const router = useRouter()
const authStore = useAuthStore()

// Reactive data
const drawer = ref(true)
const rail = ref(false)
const showNotifications = ref(false)
const notifications = ref([])
const snackbar = ref({
  show: false,
  message: '',
  color: 'success',
  timeout: 3000
})

// Computed properties
const isAuthenticated = computed(() => authStore.isAuthenticated)
const user = computed(() => authStore.user)
const notificationCount = computed(() => notifications.value.length)

// Methods
const handleLogout = async () => {
  try {
    await signalRService.disconnect()
    authStore.logout()
    router.push('/login')
  } catch (error) {
    console.error('Logout error:', error)
  }
}

const getNotificationIcon = (type) => {
  const icons = {
    task: 'mdi-format-list-checks',
    comment: 'mdi-comment',
    assignment: 'mdi-account-plus',
    due: 'mdi-clock-alert',
    default: 'mdi-information'
  }
  return icons[type] || icons.default
}

const showSnackbar = (message, color = 'success') => {
  snackbar.value = {
    show: true,
    message,
    color,
    timeout: 3000
  }
}

// SignalR event handlers
const setupSignalRHandlers = () => {
  const handlers = {
    onTaskCreated: (task) => {
      showSnackbar(`New task created: ${task.title}`, 'info')
    },
    onTaskUpdated: (task) => {
      showSnackbar(`Task updated: ${task.title}`, 'info')
    },
    onTaskAssigned: (data) => {
      showSnackbar(`Task assigned: ${data.taskTitle}`, 'info')
    },
    onCommentAdded: (comment) => {
      showSnackbar(`New comment added`, 'info')
    },
    onNotificationReceived: (notification) => {
      notifications.value.unshift(notification)
      showSnackbar(notification.message, 'info')
    }
  }

  signalRService.registerEventHandlers(handlers)
}

// Lifecycle hooks
onMounted(async () => {
  // Initialize auth
  authStore.initializeAuth()
  
  // Initialize SignalR if authenticated
  if (isAuthenticated.value) {
    try {
      await signalRService.initialize()
      setupSignalRHandlers()
    } catch (error) {
      console.error('SignalR initialization failed:', error)
    }
  }
})

onUnmounted(async () => {
  await signalRService.disconnect()
})
</script>

<style scoped>
.v-navigation-drawer {
  border-right: 1px solid rgba(0, 0, 0, 0.12);
}

.v-app-bar {
  border-bottom: 1px solid rgba(0, 0, 0, 0.12);
}

.full-height {
  height: 100vh !important;
  width: 100% !important;
}
</style>

<style>
/* Global styles for full-width application */
html, body {
  margin: 0 !important;
  padding: 0 !important;
  width: 100% !important;
  height: 100% !important;
  overflow-x: hidden;
}

#app {
  width: 100% !important;
  height: 100vh !important;
  margin: 0 !important;
  padding: 0 !important;
}

.v-application {
  width: 100% !important;
  height: 100vh !important;
}

.v-main {
  width: 100% !important;
  min-height: 100vh !important;
}

.v-container {
  max-width: none !important;
  width: 100% !important;
  padding: 16px !important;
}

/* Ensure cards and content take appropriate width */
.v-card {
  width: 100%;
}

/* Responsive container adjustments */
@media (min-width: 1264px) {
  .v-container {
    max-width: 100% !important;
  }
}

@media (min-width: 960px) {
  .v-container {
    max-width: 100% !important;
  }
}

@media (min-width: 600px) {
  .v-container {
    max-width: 100% !important;
  }
}
</style>
