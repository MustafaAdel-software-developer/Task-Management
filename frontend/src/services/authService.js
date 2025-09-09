import api from './api'

const authService = {
  // Login user
  async login(credentials) {
    const response = await api.post('/auth/login', credentials)
    return response.data
  },

  // Register user
  async register(userData) {
    const response = await api.post('/auth/register', userData)
    return response.data
  },

  // Get current user profile
  async getProfile() {
    const response = await api.get('/auth/profile')
    return response.data
  },

  // Update user profile
  async updateProfile(profileData) {
    const response = await api.put('/auth/profile', profileData)
    return response.data
  },

  // Change password
  async changePassword(passwordData) {
    const response = await api.put('/auth/change-password', passwordData)
    return response.data
  },

  // Refresh token
  async refreshToken() {
    const response = await api.post('/auth/refresh')
    return response.data
  },

  // Logout (optional - usually handled client-side)
  async logout() {
    const response = await api.post('/auth/logout')
    return response.data
  }
}

export default authService
