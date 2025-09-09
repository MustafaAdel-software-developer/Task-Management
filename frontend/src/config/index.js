// Application configuration
export const config = {
  // API Configuration
  api: {
    baseURL: import.meta.env.VITE_API_BASE_URL || 'http://localhost:5000/api',
    timeout: 10000,
  },

  // App Configuration
  app: {
    name: import.meta.env.VITE_APP_NAME || 'Task Management System',
    version: import.meta.env.VITE_APP_VERSION || '1.0.0',
    debug: import.meta.env.VITE_DEBUG === 'true',
  },

  // Feature Flags
  features: {
    signalR: import.meta.env.VITE_ENABLE_SIGNALR !== 'false',
    analytics: import.meta.env.VITE_ENABLE_ANALYTICS !== 'false',
    notifications: import.meta.env.VITE_ENABLE_NOTIFICATIONS !== 'false',
  },

  // UI Configuration
  ui: {
    theme: {
      primary: '#1976D2',
      secondary: '#424242',
      accent: '#82B1FF',
      error: '#FF5252',
      info: '#2196F3',
      success: '#4CAF50',
      warning: '#FFC107',
    },
    pagination: {
      defaultPageSize: 10,
      pageSizeOptions: [5, 10, 25, 50],
    },
  },

  // Task Configuration
  tasks: {
    priorities: [
      { value: 'low', label: 'Low', color: 'success' },
      { value: 'medium', label: 'Medium', color: 'info' },
      { value: 'high', label: 'High', color: 'warning' },
      { value: 'urgent', label: 'Urgent', color: 'error' },
    ],
    statuses: [
      { value: 'pending', label: 'Pending', color: 'warning' },
      { value: 'inProgress', label: 'In Progress', color: 'info' },
      { value: 'completed', label: 'Completed', color: 'success' },
      { value: 'cancelled', label: 'Cancelled', color: 'error' },
    ],
  },

  // Date/Time Configuration
  dateTime: {
    format: 'YYYY-MM-DD',
    displayFormat: 'MMM DD, YYYY',
    timeFormat: 'HH:mm',
  },
}

export default config
