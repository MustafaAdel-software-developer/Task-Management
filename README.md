# Task Management System

A comprehensive full-stack task management application built with ASP.NET Core Web API backend and Vue.js frontend, featuring real-time updates, user authentication, and advanced task management capabilities.

## 📋 Project Overview

This project is a full-featured task management system that allows users to create, assign, and track tasks with real-time notifications. It consists of a robust .NET Core Web API backend and a modern Vue.js frontend, designed to demonstrate modern full-stack development practices including authentication, real-time communication, and responsive UI design.

## 🚀 Key Features

- **User Authentication & Authorization** - JWT-based authentication with role management
- **Task Management** - Complete CRUD operations for tasks with categories and assignments
- **Real-time Updates** - Live notifications using SignalR for task changes
- **Task Assignment** - Assign tasks to team members with due date tracking
- **Advanced Filtering** - Search and filter tasks by various criteria
- **Comment System** - Collaborative features with task comments
- **Statistics & Reporting** - Analytics and reporting capabilities
- **Email Notifications** - Automated email alerts for important events

## 🛠️ Technology Stack

### Backend
- **API**: ASP.NET Core 8.0 Web API
- **Database**: SQL Server with Entity Framework Core
- **Real-time**: SignalR for live updates
- **Authentication**: JWT (JSON Web Tokens)
- **Documentation**: Swagger/OpenAPI
- **Testing**: xUnit for unit and integration tests
- **Caching**: Redis (optional)
- **Email**: SMTP integration

### Frontend
- **Framework**: Vue.js 3 with Composition API
- **Build Tool**: Vite
- **UI Library**: Vuetify 3 (Material Design)
- **State Management**: Pinia
- **HTTP Client**: Axios
- **Real-time**: SignalR Client
- **Routing**: Vue Router
- **Form Validation**: VeeValidate
- **Charts**: Chart.js
- **Testing**: Vitest + Vue Test Utils

## 📁 Project Structure

```
TaskManagementSystem/
├── backend/              # ASP.NET Core Web API
│   ├── Controllers/      # API Controllers
│   ├── Models/          # Data Models and DTOs
│   ├── Services/        # Business Logic Services
│   ├── Data/            # Entity Framework DbContext
│   ├── Migrations/      # Database Migrations
│   ├── Middleware/      # Custom Middleware
│   ├── Hubs/            # SignalR Hubs
│   ├── Extensions/      # Extension Methods
│   ├── Configuration/   # App Settings
│   └── Tests/           # Unit and Integration Tests
└── frontend/            # Vue.js Application
    ├── src/
    │   ├── components/  # Reusable Vue Components
    │   ├── views/       # Page Components
    │   ├── stores/      # Pinia State Management
    │   ├── services/    # API Services
    │   ├── router/      # Vue Router Configuration
    │   ├── plugins/     # Vuetify and other plugins
    │   ├── utils/       # Utility Functions
    │   └── assets/      # Static Assets
    ├── public/          # Public Assets
    └── tests/           # Frontend Tests
```

## 🎯 Development Phases

### Phase 1: Foundation (Critical - Start Here)
- [ ] **Backend Project Setup** - ASP.NET Core Web API with proper folder structure
- [ ] **Frontend Project Setup** - Vue.js 3 project with Vite, Vuetify, and Pinia
- [ ] **Database Setup** - Entity Framework Core with SQL Server and create initial DbContext
- [ ] **User Models** - Create User and Authentication models (User, Role, UserRole)
- [ ] **Task Models** - Create Task-related models (Task, TaskCategory, TaskAssignment, TaskComment)
- [ ] **Database Migrations** - Create and run initial database migrations
- [ ] **Frontend Routing** - Set up Vue Router with authentication guards

### Phase 2: Authentication & Security (Essential)
- [ ] **JWT Authentication** - Implement JWT authentication and authorization middleware
- [ ] **User Registration** - Create user registration endpoint with password hashing
- [ ] **User Login** - Create user login endpoint with JWT token generation
- [ ] **Login/Register UI** - Create Vue.js login and registration forms with validation
- [ ] **Auth Store** - Set up Pinia store for authentication state management
- [ ] **Token Management** - Implement JWT token storage and refresh logic

### Phase 3: Core Task Features (Primary Functionality)
- [ ] **Task CRUD** - Implement basic Task CRUD operations (Create, Read, Update, Delete)
- [ ] **Task Assignment** - Implement task assignment functionality to users
- [ ] **Due Date Tracking** - Add due date tracking and validation to tasks
- [ ] **Task Categories** - Implement task category management (CRUD operations)
- [ ] **Task List UI** - Create Vue.js task list component with data table
- [ ] **Task Form UI** - Create task creation/editing form with validation
- [ ] **Task Store** - Set up Pinia store for task state management
- [ ] **API Service** - Create Axios service for task API calls

### Phase 4: Real-time Features (Enhanced UX)
- [ ] **SignalR Setup** - Set up SignalR for real-time communication
- [ ] **Real-time Notifications** - Implement real-time notifications for task updates and assignments
- [ ] **SignalR Client** - Set up SignalR client in Vue.js for real-time updates
- [ ] **Notification UI** - Create notification components and toast messages
- [ ] **Real-time Updates** - Implement live task list updates without page refresh

### Phase 5: Advanced Features (User Experience)
- [ ] **Task Filtering** - Add filtering and search functionality for tasks
- [ ] **Task Pagination** - Implement pagination for task lists
- [ ] **Task Comments** - Add comment system for tasks
- [ ] **User Profile** - Create user profile management endpoints
- [ ] **Search & Filter UI** - Create Vue.js search and filter components
- [ ] **Pagination UI** - Implement pagination controls in task list
- [ ] **Comment System UI** - Create comment components and thread display
- [ ] **User Profile UI** - Create user profile management interface
- [ ] **Dashboard UI** - Create main dashboard with task overview

### Phase 6: Analytics & Reporting (Business Value)
- [ ] **Task Statistics** - Implement task statistics and reporting endpoints
- [ ] **Email Notifications** - Add email notifications for task assignments and due dates
- [ ] **Analytics Dashboard** - Create Vue.js dashboard with charts and statistics
- [ ] **Report Generation** - Implement report generation and export functionality
- [ ] **Chart Components** - Create Chart.js components for data visualization

### Phase 7: Quality & Documentation (Production Ready)
- [ ] **API Documentation** - Add Swagger/OpenAPI documentation
- [ ] **Error Handling** - Implement global error handling and logging
- [ ] **Data Validation** - Add comprehensive data validation and model validation
- [ ] **Unit Tests** - Write unit tests for core business logic
- [ ] **Integration Tests** - Write integration tests for API endpoints
- [ ] **Frontend Error Handling** - Implement global error handling in Vue.js
- [ ] **Frontend Testing** - Write unit tests for Vue components and stores
- [ ] **Responsive Design** - Ensure mobile-friendly responsive design
- [ ] **Performance Optimization** - Optimize bundle size and loading performance

## 🚀 Getting Started

### Prerequisites
- .NET 8.0 SDK
- Node.js 18+ and npm/yarn
- SQL Server (LocalDB or full instance)
- Visual Studio 2022 or VS Code
- Git

### Installation

#### Backend Setup
1. Navigate to `backend/` directory
2. Restore NuGet packages: `dotnet restore`
3. Update connection strings in `appsettings.json`
4. Run database migrations: `dotnet ef database update`
5. Start the API: `dotnet run`

#### Frontend Setup
1. Navigate to `frontend/` directory
2. Install dependencies: `npm install` or `yarn install`
3. Update API base URL in environment variables
4. Start the development server: `npm run dev` or `yarn dev`
5. Open browser to `http://localhost:3000`

### API Endpoints
- `POST /api/auth/register` - User registration
- `POST /api/auth/login` - User login
- `GET /api/tasks` - Get all tasks
- `POST /api/tasks` - Create new task
- `PUT /api/tasks/{id}` - Update task
- `DELETE /api/tasks/{id}` - Delete task

### Frontend Features
- **Responsive Dashboard** - Modern Material Design interface
- **Task Management** - Intuitive task creation, editing, and organization
- **Real-time Updates** - Live notifications and task updates
- **User Authentication** - Secure login/logout with JWT tokens
- **Search & Filtering** - Advanced task filtering and search capabilities
- **Data Visualization** - Charts and analytics for task statistics
- **Mobile Support** - Fully responsive design for all devices

## 📊 Database Schema

### Core Entities
- **Users** - User accounts and profiles
- **Tasks** - Task information and metadata
- **TaskCategories** - Task categorization
- **TaskAssignments** - Task-to-user assignments
- **TaskComments** - Task discussion threads

## 🔧 Configuration

### Environment Variables
- `ConnectionStrings__DefaultConnection` - Database connection string
- `JwtSettings__SecretKey` - JWT signing key
- `JwtSettings__Issuer` - JWT issuer
- `JwtSettings__Audience` - JWT audience
- `EmailSettings__SmtpServer` - SMTP server configuration

## 🧪 Testing

Run tests using:
```bash
dotnet test
```

## 📝 API Documentation

Once the project is running, visit `/swagger` to view the interactive API documentation.

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests for new functionality
5. Submit a pull request

## 📄 License

This project is licensed under the MIT License.

## 🎓 Learning Objectives

This project demonstrates:

### Backend Skills
- Modern ASP.NET Core development patterns
- JWT authentication and authorization
- Entity Framework Core with migrations
- SignalR for real-time communication
- RESTful API design principles
- Clean architecture and separation of concerns
- Unit and integration testing
- API documentation with Swagger

### Frontend Skills
- Vue.js 3 with Composition API
- Modern JavaScript/TypeScript development
- State management with Pinia
- Material Design with Vuetify
- Real-time communication with SignalR
- Form validation and error handling
- Responsive web design
- Component-based architecture
- HTTP client integration with Axios
- Chart.js for data visualization

---

**Happy Coding! 🚀**
