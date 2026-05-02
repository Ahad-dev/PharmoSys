# 🏥 PharmoSys - Project Documentation

## 🧱 Project Overview
PharmoSys is a Medical Store / Pharmacy Management System desktop application. It functions as a complete Point of Sale (POS) system integrated with inventory management, built using WPF, MVVM, and Entity Framework Core.

## 👥 Role-Based Access Control
The system enforces security and access control through three distinct roles:

1. **👑 Admin**
   - Full system access.
   - Manage users (add/edit/delete).
   - Manage products and inventory.
   - View all system reports.
   - Monitor stock levels and stock history.

2. **💰 Cashier**
   - Access to POS (billing system) only.
   - Create sales invoices.
   - Search for products.
   - Cannot view overarching reports or manage users.

3. **📊 Manager**
   - View reports (Sales, top products, summaries).
   - View sales analytics.
   - View stock status.
   - Cannot modify products, users, or perform POS transactions.

## 📄 Core Features & Pages (WPF Views)

### 🔐 Authentication
- **LoginView**: Secure entry point validating user credentials and roles.

### 🏠 Main Shell
- **MainWindow**: The main container using a `ContentControl` for seamless navigation, featuring a dynamic Sidebar based on user roles.

### 📊 Dashboard
- **DashboardView**: Overview of daily metrics.
  - Total sales today.
  - Low stock alerts (e.g., Stock < 10).
  - Expiring medicines alerts.

### 🛒 POS (Billing System)
- **POSView**: The cashier interface.
  - Product search functionality.
  - Cart system (add/remove items, adjust quantities).
  - Checkout process calculating total, discount, and tax.
  - Invoice generation.

### 📦 Product Management
- **ProductListView**: Data grid of all inventory.
- **ProductFormView**: Add/Edit product details (Name, Price, Category, Supplier, Expiry).

### 👤 User Management
- **UserView**: Admin interface to manage staff access and roles.

### 📊 Reports
- **ReportsView**: Detailed analytics.
  - Sales reports.
  - Top selling products.
  - Daily/monthly financial summaries.

### 📉 Stock Management
- **StockView**: 
  - Current stock levels.
  - Historical stock changes (additions/deductions).

## 🧱 Architecture Design

The application follows a **Clean Architecture** combined with the **MVVM (Model-View-ViewModel)** pattern:

*   **Data Layer (EF Core):** 
    *   `Entities`: Database schema representations (`ProductEntity`, `UserEntity`, `SaleEntity`).
    *   `DbContext`: Entity Framework context for database communication.
    *   `Repositories`: Data access abstraction.
*   **Core Layer (Business Logic):**
    *   `Models`: Application domain models encapsulating business rules (e.g., `Product`, `Sale`, `CartItem`).
    *   `DTOs`: Data Transfer Objects for moving data between layers.
    *   `Interfaces`: Contracts for Services and Repositories.
    *   `Mappers`: Translating between Entities and Models.
*   **UI Layer (WPF):**
    *   `ViewModels`: Presentation logic and state management (`POSViewModel`, `DashboardViewModel`).
    *   `Views`: XAML UI definitions.
    *   `Commands`: `RelayCommand` for user interactions (button clicks, etc.).
*   **Services Layer:** Business use cases (`AuthService`, `SalesService`, `ProductService`).

## 🧠 Future Technical Considerations
*   **Dependency Injection:** Use `Microsoft.Extensions.DependencyInjection` to decouple ViewModels, Services, and Repositories.
*   **Navigation Service:** Implement an `INavigationService` to control the `ContentControl` in `MainWindow`.
*   **Concurrency Handling:** Implement robust database locking or optimistic concurrency for stock deduction during checkout.
*   **UI Responsiveness:** All database operations must be asynchronous (`async/await`) to prevent WPF UI thread blocking.
*   **Validation:** Use `INotifyDataErrorInfo` in ViewModels for robust user input validation.
