<template>
  <v-container fluid class="fill-height">
    <v-row justify="center" align="center" class="fill-height">
      <v-col cols="12" sm="8" md="6" lg="4">
        <v-card class="elevation-12">
          <v-toolbar color="primary" dark flat>
            <v-toolbar-title>Login</v-toolbar-title>
          </v-toolbar>
          
          <v-card-text>
            <v-form @submit.prevent="handleLogin" ref="loginForm">
              <v-text-field
                v-model="form.email"
                label="Email"
                type="email"
                prepend-icon="mdi-email"
                :rules="emailRules"
                required
                variant="outlined"
                class="mb-4"
              ></v-text-field>

              <v-text-field
                v-model="form.password"
                label="Password"
                type="password"
                prepend-icon="mdi-lock"
                :rules="passwordRules"
                required
                variant="outlined"
                class="mb-4"
              ></v-text-field>

              <v-checkbox
                v-model="form.rememberMe"
                label="Remember me"
                color="primary"
              ></v-checkbox>
            </v-form>
          </v-card-text>

          <v-card-actions class="px-6 pb-6">
            <v-btn
              color="primary"
              block
              size="large"
              @click="handleLogin"
              :loading="loading"
              :disabled="!isFormValid"
            >
              Login
            </v-btn>
          </v-card-actions>

          <v-divider></v-divider>

          <v-card-text class="text-center">
            <p class="text-body-2">
              Don't have an account?
              <router-link to="/register" class="text-primary text-decoration-none">
                Sign up here
              </router-link>
            </p>
          </v-card-text>
        </v-card>

        <!-- Error Alert -->
        <v-alert
          v-if="error"
          type="error"
          variant="tonal"
          class="mt-4"
          closable
          @click:close="clearError"
        >
          {{ error }}
        </v-alert>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup>
import { ref, computed } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/auth'

const router = useRouter()
const authStore = useAuthStore()

// Form data
const form = ref({
  email: '',
  password: '',
  rememberMe: false
})

// Form validation rules
const emailRules = [
  v => !!v || 'Email is required',
  v => /.+@.+\..+/.test(v) || 'Email must be valid'
]

const passwordRules = [
  v => !!v || 'Password is required',
  v => v.length >= 6 || 'Password must be at least 6 characters'
]

// Computed properties
const loading = computed(() => authStore.loading)
const error = computed(() => authStore.error)
const isFormValid = computed(() => {
  return form.value.email && 
         form.value.password && 
         /.+@.+\..+/.test(form.value.email) &&
         form.value.password.length >= 6
})

// Methods
const handleLogin = async () => {
  try {
    await authStore.login({
      email: form.value.email,
      password: form.value.password,
      rememberMe: form.value.rememberMe
    })
    
    // Redirect to dashboard on successful login
    router.push('/dashboard')
  } catch (error) {
    console.error('Login failed:', error)
  }
}

const clearError = () => {
  authStore.clearError()
}
</script>

<style scoped>
.fill-height {
  min-height: 100vh;
}

.v-card {
  border-radius: 12px;
}

.v-toolbar {
  border-radius: 12px 12px 0 0;
}
</style>
