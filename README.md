# EaseUrOrder

EaseUrOrder is a subscription-based, multi-tenant QR and takeaway ordering platform for cafes and restaurants. It is being built with ASP.NET Core, Entity Framework Core, and SQL Server.

## Domain Configuration

**Current Domain:** `easeurorder.con`

- **Public Landing Page:** `https://easeurorder.con`
- **Tenant QR Orders:** `https://{tenantSubdomain}.easeurorder.con/qr/{qrToken}`
- **Tenant Admin Panel:** `https://{tenantSubdomain}.easeurorder.con/admin`

## Product vision

- Customers scan a table QR code and order from the correct outlet menu.
- Multiple customers at the same table can place separate orders from the same QR code.
- Each QR code points to `https://{tenantSubdomain}.easeurorder.con/qr/{qrToken}`.
- Every dine-in order references the same table and QR code when it comes from the same table QR; duplicate table rows should not be created.
- Customers can also search restaurants from the public landing page and place pay-first takeaway orders.
- Razorpay will be used for online payments through a provider abstraction.
- Tenants pay a cloud subscription only. There is no sales commission and no QR-code limit.
- Tenant admins can add cost price and selling price so dashboards can calculate estimated gross profit.

## Architecture summary

The app uses two data boundaries:

1. **Platform database** for EaseUrOrder SaaS data: tenants, subscription status, tenant database names, subdomains, and platform administration.
2. **Tenant database** for each restaurant business: outlets, tables, QR codes, menus, orders, payments, and reports.

See the full architecture blueprint in [`docs/ARCHITECTURE.md`](docs/ARCHITECTURE.md).

## Roles

- **SuperAdmin:** manages tenant registration, deregistration, subscriptions, and provisioning.
- **TenantAdmin:** manages the restaurant business and can add multiple outlets.
- **OutletAdmin:** manages one outlet, including menu, QR codes, tables, and reports.
- **Receptionist:** manages incoming orders and item availability.
- **Customer:** scans QR codes or places takeaway orders from the landing page.

## Current technical foundation

- ASP.NET Core Razor Pages project.
- Entity Framework Core with SQL Server.
- Platform models for tenants and subscriptions.
- Tenant models for outlets, tables, QR codes, menu categories, menu items, orders, order items, and payments.

## Configuration Files

### Development
- `appsettings.json` - Base development configuration
- `appsettings.Development.json` - Development-specific settings
- Launch profiles available: `http`, `https`, and `domain`

### Production
- `appsettings.Production.json` - Production configuration with easeurorder.con domain
- Update SQL Server credentials before deployment

## Recommended build sequence

1. Tenant resolution from `tenantSubdomain.easeurorder.con`.
2. Platform tenant provisioning and tenant database migration.
3. Authentication and authorization policies for all roles.
4. Outlet, table, and QR-code management.
5. Customer QR menu and dine-in ordering flow.
6. Receptionist order dashboard.
7. Landing-page restaurant search and pay-first takeaway flow.
8. Razorpay payment provider and webhook handling.
9. Sales, best-selling item, and gross-profit dashboards.

## Setup Instructions

### Local Development
1. Update your local `hosts` file to map `easeurorder.con` and test subdomains:
   ```
   127.0.0.1 easeurorder.con
   127.0.0.1 test.easeurorder.con
   127.0.0.1 restaurant1.easeurorder.con
   ```

2. Configure SQL Server connection strings in `appsettings.Development.json`

3. Run migrations to create platform and tenant databases

4. Select the `domain` launch profile to test with the domain name

### Production Deployment
1. Update `appsettings.Production.json` with your SQL Server and domain settings
2. Configure DNS A records:
   - `easeurorder.con` → Your server IP
   - `*.easeurorder.con` → Your server IP
3. Set up SSL/TLS certificates for the domain and wildcard
4. Deploy with `ASPNETCORE_ENVIRONMENT=Production`
