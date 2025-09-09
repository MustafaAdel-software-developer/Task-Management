# Task Management Frontend

A modern Vue.js 3 frontend application for the Task Management System, built with Vuetify 3 and featuring real-time updates, responsive design, and comprehensive task management capabilities.

## 🚀 Features

- **Modern UI/UX** - Built with Vue.js 3, Vuetify 3, and Material Design
- **Real-time Updates** - SignalR integration for live task updates
- **Responsive Design** - Mobile-first approach with adaptive layouts
- **State Management** - Pinia for efficient state management
- **Authentication** - JWT-based authentication with route guards
- **Task Management** - Complete CRUD operations with filtering and search
- **Analytics Dashboard** - Charts and statistics using Chart.js
- **Form Validation** - Comprehensive validation with VeeValidate
- **Notifications** - Toast notifications and real-time alerts

## 🛠️ Technology Stack

- **Vue.js 3** - Progressive JavaScript framework with Composition API
- **Vite** - Fast build tool and development server
- **Vuetify 3** - Material Design component framework
- **Pinia** - State management library
- **Vue Router 4** - Official router for Vue.js
- **Axios** - HTTP client for API communication
- **SignalR** - Real-time communication
- **Chart.js** - Data visualization
- **VeeValidate** - Form validation
- **Vitest** - Unit testing framework

## 📁 Project Structure

```
src/
├── components/          # Reusable Vue components
├── views/              # Page components
│   ├── LoginView.vue
│   ├── RegisterView.vue
│   ├── DashboardView.vue
│   ├── TasksView.vue
│   ├── TaskDetailView.vue
│   ├── ProfileView.vue
│   └── AnalyticsView.vue
├── stores/             # Pinia stores
│   ├── auth.js
│   └── tasks.js
├── services/           # API services
│   ├── api.js
│   ├── authService.js
│   ├── taskService.js
│   └── signalRService.js
├── router/             # Vue Router configuration
│   └── index.js
├── config/             # Application configuration
│   └── index.js
├── utils/              # Utility functions
├── assets/             # Static assets
└── main.js             # Application entry point
```

## 🚀 Getting Started

### Prerequisites

- Node.js 18+ and npm/yarn
- Backend API running on `http://localhost:5000`

### Installation

1. **Install dependencies:**
   ```bash
   npm install
   ```

2. **Configure environment:**
   Create a `.env` file in the root directory:
   ```env
   VITE_API_BASE_URL=http://localhost:5000/api
   VITE_APP_NAME=Task Management System
   VITE_DEBUG=true
   ```

3. **Start development server:**
   ```bash
   npm run dev
   ```

4. **Open your browser:**
   Navigate to `http://localhost:3000`

### Available Scripts

- `npm run dev` - Start development server
- `npm run build` - Build for production
- `npm run preview` - Preview production build
- `npm run test` - Run unit tests

## 🎨 UI Components

### Main Layout
- **Navigation Drawer** - Collapsible sidebar with navigation
- **App Bar** - Top navigation with user menu and notifications
- **Main Content** - Router view for page content

### Key Views
- **Dashboard** - Overview with statistics and recent tasks
- **Tasks** - Comprehensive task list with filtering and CRUD operations
- **Task Detail** - Detailed task view with comments and history
- **Profile** - User profile management and preferences
- **Analytics** - Charts and performance metrics

### Components
- **Data Tables** - Sortable, filterable task lists
- **Forms** - Validated forms for task creation and editing
- **Charts** - Interactive charts for analytics
- **Notifications** - Toast messages and real-time alerts

## 🔧 Configuration

### API Configuration
The application connects to the backend API through the `api.js` service, which includes:
- Automatic JWT token attachment
- Request/response interceptors
- Error handling and token refresh

### State Management
Pinia stores manage application state:
- **Auth Store** - User authentication and profile
- **Task Store** - Task data, filtering, and CRUD operations

### Real-time Features
SignalR integration provides:
- Live task updates
- Real-time notifications
- Collaborative features

## 🎯 Key Features

### Authentication
- JWT-based authentication
- Route guards for protected pages
- Automatic token refresh
- User profile management

### Task Management
- Create, read, update, delete tasks
- Advanced filtering and search
- Task assignment and status tracking
- Due date management
- Priority levels

### Real-time Updates
- Live task notifications
- Real-time collaboration
- Instant updates across users
- SignalR connection management

### Analytics
- Task completion trends
- Performance metrics
- Category-based analytics
- Export functionality

## 🧪 Testing

The project includes unit tests using Vitest:

```bash
# Run tests
npm run test

# Run tests in watch mode
npm run test:watch

# Generate coverage report
npm run test:coverage
```

## 📱 Responsive Design

The application is fully responsive and works on:
- Desktop computers
- Tablets
- Mobile phones
- Various screen sizes

## 🔒 Security

- JWT token management
- Secure API communication
- Input validation and sanitization
- XSS protection
- CSRF protection

## 🚀 Deployment

### Production Build
```bash
npm run build
```

### Environment Variables
Set the following environment variables for production:
- `VITE_API_BASE_URL` - Backend API URL
- `VITE_APP_NAME` - Application name
- `VITE_DEBUG` - Debug mode (false for production)

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests for new functionality
5. Submit a pull request

## 📄 License

This project is licensed under the MIT License.

---

**Happy Coding! 🚀**