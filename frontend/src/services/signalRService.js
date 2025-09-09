import * as signalR from '@microsoft/signalr'

class SignalRService {
  constructor() {
    this.connection = null
    this.isConnected = false
    this.reconnectAttempts = 0
    this.maxReconnectAttempts = 5
  }

  // Initialize SignalR connection
  async initialize() {
    try {
      const token = localStorage.getItem('token')
      
      this.connection = new signalR.HubConnectionBuilder()
        .withUrl(`${import.meta.env.VITE_API_BASE_URL || 'http://localhost:5000'}/taskHub`, {
          accessTokenFactory: () => token,
          skipNegotiation: true,
          transport: signalR.HttpTransportType.WebSockets
        })
        .withAutomaticReconnect([0, 2000, 10000, 30000])
        .configureLogging(signalR.LogLevel.Information)
        .build()

      // Connection event handlers
      this.connection.onclose((error) => {
        this.isConnected = false
        console.log('SignalR connection closed:', error)
        this.handleReconnection()
      })

      this.connection.onreconnecting((error) => {
        this.isConnected = false
        console.log('SignalR reconnecting:', error)
      })

      this.connection.onreconnected((connectionId) => {
        this.isConnected = true
        this.reconnectAttempts = 0
        console.log('SignalR reconnected:', connectionId)
      })

      // Start connection
      await this.connection.start()
      this.isConnected = true
      console.log('SignalR connected successfully')
      
      return this.connection
    } catch (error) {
      console.error('SignalR connection failed:', error)
      this.handleReconnection()
      throw error
    }
  }

  // Handle reconnection logic
  handleReconnection() {
    if (this.reconnectAttempts < this.maxReconnectAttempts) {
      this.reconnectAttempts++
      setTimeout(() => {
        this.initialize()
      }, 5000 * this.reconnectAttempts)
    }
  }

  // Register event handlers
  registerEventHandlers(handlers) {
    if (!this.connection) {
      console.error('SignalR connection not initialized')
      return
    }

    // Task events
    if (handlers.onTaskCreated) {
      this.connection.on('TaskCreated', handlers.onTaskCreated)
    }
    
    if (handlers.onTaskUpdated) {
      this.connection.on('TaskUpdated', handlers.onTaskUpdated)
    }
    
    if (handlers.onTaskDeleted) {
      this.connection.on('TaskDeleted', handlers.onTaskDeleted)
    }
    
    if (handlers.onTaskAssigned) {
      this.connection.on('TaskAssigned', handlers.onTaskAssigned)
    }

    // Comment events
    if (handlers.onCommentAdded) {
      this.connection.on('CommentAdded', handlers.onCommentAdded)
    }
    
    if (handlers.onCommentUpdated) {
      this.connection.on('CommentUpdated', handlers.onCommentUpdated)
    }
    
    if (handlers.onCommentDeleted) {
      this.connection.on('CommentDeleted', handlers.onCommentDeleted)
    }

    // Notification events
    if (handlers.onNotificationReceived) {
      this.connection.on('NotificationReceived', handlers.onNotificationReceived)
    }
  }

  // Unregister event handlers
  unregisterEventHandlers(events) {
    if (!this.connection) return

    events.forEach(event => {
      this.connection.off(event)
    })
  }

  // Join task group for real-time updates
  async joinTaskGroup(taskId) {
    if (!this.connection || !this.isConnected) {
      console.error('SignalR connection not available')
      return
    }

    try {
      await this.connection.invoke('JoinTaskGroup', taskId)
      console.log(`Joined task group: ${taskId}`)
    } catch (error) {
      console.error('Failed to join task group:', error)
    }
  }

  // Leave task group
  async leaveTaskGroup(taskId) {
    if (!this.connection || !this.isConnected) {
      console.error('SignalR connection not available')
      return
    }

    try {
      await this.connection.invoke('LeaveTaskGroup', taskId)
      console.log(`Left task group: ${taskId}`)
    } catch (error) {
      console.error('Failed to leave task group:', error)
    }
  }

  // Send message to hub
  async sendMessage(method, ...args) {
    if (!this.connection || !this.isConnected) {
      console.error('SignalR connection not available')
      return
    }

    try {
      await this.connection.invoke(method, ...args)
    } catch (error) {
      console.error('Failed to send message:', error)
    }
  }

  // Disconnect
  async disconnect() {
    if (this.connection) {
      await this.connection.stop()
      this.isConnected = false
      console.log('SignalR disconnected')
    }
  }

  // Get connection state
  getConnectionState() {
    return this.connection?.state || signalR.HubConnectionState.Disconnected
  }
}

// Create singleton instance
const signalRService = new SignalRService()

export default signalRService
