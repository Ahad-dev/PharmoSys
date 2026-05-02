-- =========================================
-- PharmoSys - PostgreSQL Seed Data Script
-- =========================================

-- 1. Insert Roles
INSERT INTO "Roles" ("RoleName") 
VALUES 
    ('Admin'),
    ('Cashier'),
    ('Manager');

-- 2. Insert Users
-- Note: The PasswordHash below is the BCrypt hash for 'admin123'
INSERT INTO "Users" ("Username", "PasswordHash", "FullName", "Phone", "RoleId", "CreatedAt") 
VALUES 
    ('admin', '$2a$11$D7oD9n.7/F5O5X0p0E2L/Oo6tEaP7R2Y1F4p8s9B7L5Q6E4p8s9B7', 'System Administrator', '1234567890', 1, CURRENT_TIMESTAMP),
    ('cashier1', '$2a$11$D7oD9n.7/F5O5X0p0E2L/Oo6tEaP7R2Y1F4p8s9B7L5Q6E4p8s9B7', 'John Cashier', '0987654321', 2, CURRENT_TIMESTAMP),
    ('manager1', '$2a$11$D7oD9n.7/F5O5X0p0E2L/Oo6tEaP7R2Y1F4p8s9B7L5Q6E4p8s9B7', 'Alice Manager', '1122334455', 3, CURRENT_TIMESTAMP);

-- 3. Insert Suppliers
INSERT INTO "Suppliers" ("SupplierName", "Contact", "Address") 
VALUES 
    ('PharmaCorp Inc.', 'contact@pharmacorp.com', '123 Health Ave, New York'),
    ('MediSupply Ltd.', 'sales@medisupply.com', '456 Wellness Blvd, California');

-- 4. Insert Products
INSERT INTO "Products" ("Name", "Category", "Price", "StockQuantity", "ExpiryDate", "SupplierId", "CreatedAt") 
VALUES 
    ('Panadol 500mg', 'Painkiller', 5.99, 100, '2028-12-31', 1, CURRENT_TIMESTAMP),
    ('Amoxicillin 250mg', 'Antibiotic', 12.50, 50, '2026-06-30', 2, CURRENT_TIMESTAMP),
    ('Vitamin C 1000mg', 'Supplement', 8.99, 200, '2027-01-01', 1, CURRENT_TIMESTAMP);
