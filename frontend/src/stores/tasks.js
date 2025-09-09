import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import taskService from '@/services/taskService'

export const useTaskStore = defineStore('tasks', () => {
  // State
  const tasks = ref([])
  const currentTask = ref(null)
  const categories = ref([])
  const loading = ref(false)
  const error = ref(null)
  const filters = ref({
    search: '',
    category: '',
    status: '',
    assignedTo: '',
    dueDate: ''
  })
  const pagination = ref({
    page: 1,
    pageSize: 10,
    total: 0
  })

  // Getters
  const filteredTasks = computed(() => {
    let filtered = tasks.value

    if (filters.value.search) {
      const search = filters.value.search.toLowerCase()
      filtered = filtered.filter(task => 
        task.title.toLowerCase().includes(search) ||
        task.description.toLowerCase().includes(search)
      )
    }

    if (filters.value.category) {
      filtered = filtered.filter(task => task.categoryId === filters.value.category)
    }

    if (filters.value.status) {
      filtered = filtered.filter(task => task.status === filters.value.status)
    }

    if (filters.value.assignedTo) {
      filtered = filtered.filter(task => task.assignedToId === filters.value.assignedTo)
    }

    if (filters.value.dueDate) {
      const today = new Date()
      const filterDate = new Date(filters.value.dueDate)
      
      filtered = filtered.filter(task => {
        const taskDueDate = new Date(task.dueDate)
        switch (filters.value.dueDate) {
          case 'today':
            return taskDueDate.toDateString() === today.toDateString()
          case 'overdue':
            return taskDueDate < today && task.status !== 'completed'
          case 'thisWeek':
            const weekEnd = new Date(today.getTime() + 7 * 24 * 60 * 60 * 1000)
            return taskDueDate >= today && taskDueDate <= weekEnd
          default:
            return true
        }
      })
    }

    return filtered
  })

  const tasksByStatus = computed(() => {
    return {
      pending: tasks.value.filter(task => task.status === 'pending'),
      inProgress: tasks.value.filter(task => task.status === 'inProgress'),
      completed: tasks.value.filter(task => task.status === 'completed'),
      overdue: tasks.value.filter(task => {
        const today = new Date()
        const dueDate = new Date(task.dueDate)
        return dueDate < today && task.status !== 'completed'
      })
    }
  })

  // Actions
  const fetchTasks = async (params = {}) => {
    loading.value = true
    error.value = null
    
    try {
      const response = await taskService.getTasks(params)
      tasks.value = response.data
      pagination.value = {
        page: response.page,
        pageSize: response.pageSize,
        total: response.total
      }
      return response
    } catch (err) {
      error.value = err.response?.data?.message || 'Failed to fetch tasks'
      throw err
    } finally {
      loading.value = false
    }
  }

  const fetchTask = async (id) => {
    loading.value = true
    error.value = null
    
    try {
      const response = await taskService.getTask(id)
      currentTask.value = response.data
      return response
    } catch (err) {
      error.value = err.response?.data?.message || 'Failed to fetch task'
      throw err
    } finally {
      loading.value = false
    }
  }

  const createTask = async (taskData) => {
    loading.value = true
    error.value = null
    
    try {
      const response = await taskService.createTask(taskData)
      tasks.value.unshift(response.data)
      return response
    } catch (err) {
      error.value = err.response?.data?.message || 'Failed to create task'
      throw err
    } finally {
      loading.value = false
    }
  }

  const updateTask = async (id, taskData) => {
    loading.value = true
    error.value = null
    
    try {
      const response = await taskService.updateTask(id, taskData)
      const index = tasks.value.findIndex(task => task.id === id)
      if (index !== -1) {
        tasks.value[index] = response.data
      }
      if (currentTask.value?.id === id) {
        currentTask.value = response.data
      }
      return response
    } catch (err) {
      error.value = err.response?.data?.message || 'Failed to update task'
      throw err
    } finally {
      loading.value = false
    }
  }

  const deleteTask = async (id) => {
    loading.value = true
    error.value = null
    
    try {
      await taskService.deleteTask(id)
      tasks.value = tasks.value.filter(task => task.id !== id)
      if (currentTask.value?.id === id) {
        currentTask.value = null
      }
    } catch (err) {
      error.value = err.response?.data?.message || 'Failed to delete task'
      throw err
    } finally {
      loading.value = false
    }
  }

  const fetchCategories = async () => {
    try {
      const response = await taskService.getCategories()
      categories.value = response.data
      return response
    } catch (err) {
      error.value = err.response?.data?.message || 'Failed to fetch categories'
      throw err
    }
  }

  const createCategory = async (categoryData) => {
    try {
      const response = await taskService.createCategory(categoryData)
      categories.value.push(response.data)
      return response
    } catch (err) {
      error.value = err.response?.data?.message || 'Failed to create category'
      throw err
    }
  }

  const updateFilters = (newFilters) => {
    filters.value = { ...filters.value, ...newFilters }
  }

  const clearFilters = () => {
    filters.value = {
      search: '',
      category: '',
      status: '',
      assignedTo: '',
      dueDate: ''
    }
  }

  const clearError = () => {
    error.value = null
  }

  return {
    // State
    tasks,
    currentTask,
    categories,
    loading,
    error,
    filters,
    pagination,
    
    // Getters
    filteredTasks,
    tasksByStatus,
    
    // Actions
    fetchTasks,
    fetchTask,
    createTask,
    updateTask,
    deleteTask,
    fetchCategories,
    createCategory,
    updateFilters,
    clearFilters,
    clearError
  }
})
