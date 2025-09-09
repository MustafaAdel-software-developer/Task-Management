import api from './api'

const taskService = {
  // Get all tasks with optional filters and pagination
  async getTasks(params = {}) {
    const response = await api.get('/tasks', { params })
    return response.data
  },

  // Get single task by ID
  async getTask(id) {
    const response = await api.get(`/tasks/${id}`)
    return response.data
  },

  // Create new task
  async createTask(taskData) {
    const response = await api.post('/tasks', taskData)
    return response.data
  },

  // Update task
  async updateTask(id, taskData) {
    const response = await api.put(`/tasks/${id}`, taskData)
    return response.data
  },

  // Delete task
  async deleteTask(id) {
    const response = await api.delete(`/tasks/${id}`)
    return response.data
  },

  // Assign task to user
  async assignTask(id, userId) {
    const response = await api.post(`/tasks/${id}/assign`, { userId })
    return response.data
  },

  // Update task status
  async updateTaskStatus(id, status) {
    const response = await api.patch(`/tasks/${id}/status`, { status })
    return response.data
  },

  // Get task categories
  async getCategories() {
    const response = await api.get('/categories')
    return response.data
  },

  // Create category
  async createCategory(categoryData) {
    const response = await api.post('/categories', categoryData)
    return response.data
  },

  // Update category
  async updateCategory(id, categoryData) {
    const response = await api.put(`/categories/${id}`, categoryData)
    return response.data
  },

  // Delete category
  async deleteCategory(id) {
    const response = await api.delete(`/categories/${id}`)
    return response.data
  },

  // Get task comments
  async getTaskComments(taskId) {
    const response = await api.get(`/tasks/${taskId}/comments`)
    return response.data
  },

  // Add comment to task
  async addComment(taskId, commentData) {
    const response = await api.post(`/tasks/${taskId}/comments`, commentData)
    return response.data
  },

  // Update comment
  async updateComment(taskId, commentId, commentData) {
    const response = await api.put(`/tasks/${taskId}/comments/${commentId}`, commentData)
    return response.data
  },

  // Delete comment
  async deleteComment(taskId, commentId) {
    const response = await api.delete(`/tasks/${taskId}/comments/${commentId}`)
    return response.data
  },

  // Get task statistics
  async getTaskStatistics(params = {}) {
    const response = await api.get('/tasks/statistics', { params })
    return response.data
  },

  // Get user tasks
  async getUserTasks(userId, params = {}) {
    const response = await api.get(`/users/${userId}/tasks`, { params })
    return response.data
  }
}

export default taskService
