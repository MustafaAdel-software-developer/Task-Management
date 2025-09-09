<template>
  <v-container fluid>
    <!-- Header -->
    <v-row class="mb-6">
      <v-col cols="12">
        <v-card class="pa-6">
          <v-row align="center">
            <v-col cols="12" md="8">
              <h1 class="text-h4 font-weight-bold mb-2">Profile</h1>
              <p class="text-body-1 text-medium-emphasis">
                Manage your account settings and preferences
              </p>
            </v-col>
          </v-row>
        </v-card>
      </v-col>
    </v-row>

    <v-row>
      <!-- Profile Information -->
      <v-col cols="12" lg="8">
        <v-card class="mb-6">
          <v-card-title class="d-flex align-center">
            <v-icon icon="mdi-account" class="me-2"></v-icon>
            Personal Information
          </v-card-title>
          <v-card-text>
            <v-form @submit.prevent="updateProfile">
              <v-row>
                <v-col cols="12" md="6">
                  <v-text-field
                    v-model="profileForm.name"
                    label="Full Name"
                    variant="outlined"
                    :rules="[v => !!v || 'Name is required']"
                    required
                  ></v-text-field>
                </v-col>
                
                <v-col cols="12" md="6">
                  <v-text-field
                    v-model="profileForm.email"
                    label="Email"
                    type="email"
                    variant="outlined"
                    :rules="emailRules"
                    required
                  ></v-text-field>
                </v-col>
              </v-row>
              
              <v-row>
                <v-col cols="12" md="6">
                  <v-text-field
                    v-model="profileForm.phone"
                    label="Phone Number"
                    variant="outlined"
                  ></v-text-field>
                </v-col>
                
                <v-col cols="12" md="6">
                  <v-text-field
                    v-model="profileForm.department"
                    label="Department"
                    variant="outlined"
                  ></v-text-field>
                </v-col>
              </v-row>
              
              <v-textarea
                v-model="profileForm.bio"
                label="Bio"
                variant="outlined"
                rows="3"
                placeholder="Tell us about yourself..."
              ></v-textarea>
              
              <v-btn
                color="primary"
                type="submit"
                :loading="loading"
                class="mt-4"
              >
                Update Profile
              </v-btn>
            </v-form>
          </v-card-text>
        </v-card>

        <!-- Change Password -->
        <v-card>
          <v-card-title class="d-flex align-center">
            <v-icon icon="mdi-lock" class="me-2"></v-icon>
            Change Password
          </v-card-title>
          <v-card-text>
            <v-form @submit.prevent="changePassword">
              <v-text-field
                v-model="passwordForm.currentPassword"
                label="Current Password"
                type="password"
                variant="outlined"
                :rules="[v => !!v || 'Current password is required']"
                required
                class="mb-4"
              ></v-text-field>
              
              <v-text-field
                v-model="passwordForm.newPassword"
                label="New Password"
                type="password"
                variant="outlined"
                :rules="passwordRules"
                required
                class="mb-4"
              ></v-text-field>
              
              <v-text-field
                v-model="passwordForm.confirmPassword"
                label="Confirm New Password"
                type="password"
                variant="outlined"
                :rules="confirmPasswordRules"
                required
                class="mb-4"
              ></v-text-field>
              
              <v-btn
                color="primary"
                type="submit"
                :loading="loading"
              >
                Change Password
              </v-btn>
            </v-form>
          </v-card-text>
        </v-card>
      </v-col>

      <!-- Sidebar -->
      <v-col cols="12" lg="4">
        <!-- Profile Picture -->
        <v-card class="mb-6">
          <v-card-title class="d-flex align-center">
            <v-icon icon="mdi-account-circle" class="me-2"></v-icon>
            Profile Picture
          </v-card-title>
          <v-card-text class="text-center">
            <v-avatar size="120" class="mb-4">
              <v-img :src="profileForm.avatar || 'https://randomuser.me/api/portraits/men/85.jpg'"></v-img>
            </v-avatar>
            <v-btn
              color="primary"
              variant="outlined"
              @click="uploadAvatar"
            >
              Change Picture
            </v-btn>
          </v-card-text>
        </v-card>

        <!-- Account Statistics -->
        <v-card class="mb-6">
          <v-card-title class="d-flex align-center">
            <v-icon icon="mdi-chart-line" class="me-2"></v-icon>
            Account Statistics
          </v-card-title>
          <v-card-text>
            <v-list>
              <v-list-item>
                <template v-slot:prepend>
                  <v-icon icon="mdi-format-list-checks" color="primary"></v-icon>
                </template>
                <v-list-item-title>Total Tasks</v-list-item-title>
                <template v-slot:append>
                  <v-chip color="primary" variant="tonal">24</v-chip>
                </template>
              </v-list-item>
              
              <v-list-item>
                <template v-slot:prepend>
                  <v-icon icon="mdi-check-circle" color="success"></v-icon>
                </template>
                <v-list-item-title>Completed Tasks</v-list-item-title>
                <template v-slot:append>
                  <v-chip color="success" variant="tonal">18</v-chip>
                </template>
              </v-list-item>
              
              <v-list-item>
                <template v-slot:prepend>
                  <v-icon icon="mdi-progress-clock" color="info"></v-icon>
                </template>
                <v-list-item-title>In Progress</v-list-item-title>
                <template v-slot:append>
                  <v-chip color="info" variant="tonal">4</v-chip>
                </template>
              </v-list-item>
              
              <v-list-item>
                <template v-slot:prepend>
                  <v-icon icon="mdi-comment" color="warning"></v-icon>
                </template>
                <v-list-item-title>Comments Made</v-list-item-title>
                <template v-slot:append>
                  <v-chip color="warning" variant="tonal">32</v-chip>
                </template>
              </v-list-item>
            </v-list>
          </v-card-text>
        </v-card>

        <!-- Preferences -->
        <v-card>
          <v-card-title class="d-flex align-center">
            <v-icon icon="mdi-cog" class="me-2"></v-icon>
            Preferences
          </v-card-title>
          <v-card-text>
            <v-list>
              <v-list-item>
                <v-list-item-title>Email Notifications</v-list-item-title>
                <template v-slot:append>
                  <v-switch
                    v-model="preferences.emailNotifications"
                    color="primary"
                    @change="updatePreferences"
                  ></v-switch>
                </template>
              </v-list-item>
              
              <v-list-item>
                <v-list-item-title>Push Notifications</v-list-item-title>
                <template v-slot:append>
                  <v-switch
                    v-model="preferences.pushNotifications"
                    color="primary"
                    @change="updatePreferences"
                  ></v-switch>
                </template>
              </v-list-item>
              
              <v-list-item>
                <v-list-item-title>Dark Mode</v-list-item-title>
                <template v-slot:append>
                  <v-switch
                    v-model="preferences.darkMode"
                    color="primary"
                    @change="updatePreferences"
                  ></v-switch>
                </template>
              </v-list-item>
            </v-list>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>

    <!-- Success/Error Messages -->
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
  </v-container>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue'
import { useAuthStore } from '@/stores/auth'

const authStore = useAuthStore()

// Reactive data
const profileForm = ref({
  name: '',
  email: '',
  phone: '',
  department: '',
  bio: '',
  avatar: ''
})

const passwordForm = ref({
  currentPassword: '',
  newPassword: '',
  confirmPassword: ''
})

const preferences = ref({
  emailNotifications: true,
  pushNotifications: true,
  darkMode: false
})

const snackbar = ref({
  show: false,
  message: '',
  color: 'success',
  timeout: 3000
})

// Form validation rules
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
  v => v === passwordForm.value.newPassword || 'Passwords do not match'
]

// Computed properties
const loading = computed(() => authStore.loading)
const user = computed(() => authStore.user)

// Methods
const updateProfile = async () => {
  try {
    await authStore.updateProfile(profileForm.value)
    showSnackbar('Profile updated successfully!', 'success')
  } catch (error) {
    showSnackbar('Failed to update profile', 'error')
  }
}

const changePassword = async () => {
  try {
    await authStore.changePassword(passwordForm.value)
    showSnackbar('Password changed successfully!', 'success')
    passwordForm.value = {
      currentPassword: '',
      newPassword: '',
      confirmPassword: ''
    }
  } catch (error) {
    showSnackbar('Failed to change password', 'error')
  }
}

const uploadAvatar = () => {
  // Mock avatar upload
  showSnackbar('Avatar upload feature coming soon!', 'info')
}

const updatePreferences = () => {
  // Mock preferences update
  showSnackbar('Preferences updated!', 'success')
}

const showSnackbar = (message, color = 'success') => {
  snackbar.value = {
    show: true,
    message,
    color,
    timeout: 3000
  }
}

// Lifecycle
onMounted(() => {
  if (user.value) {
    profileForm.value = {
      name: user.value.name || '',
      email: user.value.email || '',
      phone: user.value.phone || '',
      department: user.value.department || '',
      bio: user.value.bio || '',
      avatar: user.value.avatar || ''
    }
  }
})
</script>

<style scoped>
.v-card {
  border-radius: 12px;
}

.v-list-item {
  border-radius: 8px;
  margin-bottom: 4px;
}
</style>
