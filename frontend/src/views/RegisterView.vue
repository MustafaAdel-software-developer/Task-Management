<template>
  <v-container fluid class="fill-height">
    <v-row justify="center" align="center" class="fill-height">
      <v-col cols="12" sm="8" md="6" lg="4">
        <v-card class="elevation-12">
          <v-toolbar color="primary" dark flat>
            <v-toolbar-title>Create Account</v-toolbar-title>
          </v-toolbar>
          
          <v-card-text>
            <v-form @submit.prevent="handleRegister" ref="registerForm">
              <v-text-field
                v-model="form.name"
                label="Full Name"
                prepend-icon="mdi-account"
                :rules="nameRules"
                required
                variant="outlined"
                class="mb-4"
              ></v-text-field>

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

              <v-text-field
                v-model="form.confirmPassword"
                label="Confirm Password"
                type="password"
                prepend-icon="mdi-lock-check"
                :rules="confirmPasswordRules"
                required
                variant="outlined"
                class="mb-4"
              ></v-text-field>

              <v-checkbox
                v-model="form.agreeToTerms"
                :rules="[v => !!v || 'You must agree to the terms']"
                required
                color="primary"
              >
                <template v-slot:label>
                  I agree to the
                  <a href="#" class="text-primary text-decoration-none">Terms of Service</a>
                  and
                  <a href="#" class="text-primary text-decoration-none">Privacy Policy</a>
                </template>
              </v-checkbox>
            </v-form>
          </v-card-text>

          <v-card-actions class="px-6 pb-6">
            <v-btn
              color="primary"
              block
              size="large"
              @click="handleRegister"
              :loading="loading"
              :disabled="!isFormValid"
            >
              Create Account
            </v-btn>
          </v-card-actions>

          <v-divider></v-divider>

          <v-card-text class="text-center">
            <p class="text-body-2">
              Already have an account?
              <router-link to="/login" class="text-primary text-decoration-none">
                Sign in here
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
  name: '',
  email: '',
  password: '',
  confirmPassword: '',
  agreeToTerms: false
})

// Form validation rules
const nameRules = [
  v => !!v || 'Name is required',
  v => v.length >= 2 || 'Name must be at least 2 characters'
]

const emailRules = [
  v => !!v || 'Email is required',
  v => /.+@.+\..+/.test(v) || 'Email must be valid'
]

const passwordRules = [
  v => !!v || 'Password is required',
  v => v.length >= 8 || 'Password must be at least 8 characters',
  v => /(?=.*[a-z])/.test(v) || 'Password must contain at least one lowercase letter',
  v => /(?=.*[A-Z])/.test(v) || 'Password must contain at least one uppercase letter',
  v => /(?=.*\d)/.test(v) || 'Password must contain at least one number'
]

const confirmPasswordRules = [
  v => !!v || 'Please confirm your password',
  v => v === form.value.password || 'Passwords do not match'
]

// Computed properties
const loading = computed(() => authStore.loading)
const error = computed(() => authStore.error)
const isFormValid = computed(() => {
  return form.value.name &&
         form.value.email &&
         form.value.password &&
         form.value.confirmPassword &&
         form.value.agreeToTerms &&
         /.+@.+\..+/.test(form.value.email) &&
         form.value.password.length >= 8 &&
         form.value.password === form.value.confirmPassword
})

// Methods
const handleRegister = async () => {
  try {
    await authStore.register({
      name: form.value.name,
      email: form.value.email,
      password: form.value.password
    })
    
    // Redirect to dashboard on successful registration
    router.push('/dashboard')
  } catch (error) {
    console.error('Registration failed:', error)
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
