# EaseUrOrder Architecture Blueprint

EaseUrOrder is a subscription-based, multi-tenant QR and takeaway ordering platform for cafes and restaurants. The product supports dine-in ordering from table QR codes, pay-first takeaway ordering from the public landing page, outlet-level operations, Razorpay payment integration, and profit reporting through optional cost-price capture.

## Core product decisions

- **Public domain:** `easeurorder.com`.
- **Tenant URL format:** `https://{tenantSubdomain}.easeurorder.com/qr/{qrToken}`.
- **Tenant isolation:** every tenant gets a separate operational database.
- **Revenue model:** subscription only; no commission on restaurant sales.
- **QR policy:** no QR limit. Tenants/outlets can create as many active table QR codes as needed.
- **Payment provider:** outsource online payments to Razorpay through a payment-provider abstraction.
- **Dine-in concurrency:** multiple customers at the same table may place separate orders from the same QR code. This creates multiple `Orders` rows that reference the same `TableId` and `QrCodeId`; it must not create duplicate table records.
- **Takeaway rule:** public takeaway orders must be paid first before the order is sent to the outlet queue.
- **Profit feature:** admins can add cost price and selling price for menu items so dashboards can calculate estimated gross profit.

## System boundaries

EaseUrOrder has two database boundaries.

### Platform database

The platform database belongs to EaseUrOrder and stores SaaS-level data:

- tenants
- tenant subscription status
- tenant subdomain and custom domain mapping
- tenant database name/connection metadata
- super-admin users and platform audit logs
- tenant onboarding/provisioning logs

The current project already has `PlatformDbContext` and `Tenant`, so this layer should remain separate from outlet/order data.

### Tenant database

Each tenant database stores restaurant operations data:

- outlets
- outlet staff
- tables
- QR codes
- menus
- dine-in orders
- takeaway orders
- payments
- reports and summaries

Tenant data must never be mixed in the platform database, except for subscription/provisioning metadata.

## Roles and permissions

### SuperAdmin

Platform role owned by EaseUrOrder.

Responsibilities:

- create/register tenants
- deactivate/deregister tenants
- manage tenant subscriptions
- provision tenant databases
- view platform-level health and audit logs

### TenantAdmin

Business owner role for a tenant.

Responsibilities:

- manage tenant profile
- add multiple outlets
- create outlet admins and receptionists
- configure subscription billing information
- view tenant-wide reports

### OutletAdmin

Store admin role for exactly one outlet, unless explicitly assigned to more outlets later.

Responsibilities:

- manage outlet details
- manage tables and QR codes
- manage menus, categories, prices, cost prices, and item availability
- view outlet reports
- configure Razorpay account/settings for the outlet or tenant

### Receptionist

Operational role for an outlet.

Responsibilities:

- monitor incoming dine-in and takeaway orders
- accept/reject orders
- change order status
- update item availability
- confirm manual payment methods if those are enabled later

### Customer

Usually anonymous.

Responsibilities:

- scan table QR and place dine-in orders
- search restaurants from landing page
- place pay-first takeaway orders
- track basic order status

## Main user journeys

### Dine-in QR ordering

1. Customer scans `https://tenant1.easeurorder.com/qr/{qrToken}`.
2. Tenant is resolved from subdomain.
3. QR token is resolved inside the tenant database.
4. The app loads the linked outlet and table.
5. Customer selects menu items.
6. Customer places an order.
7. The order stores the same `TableId` and `QrCodeId` as the QR code.
8. If another customer at the same table orders separately, a new order row is created with the same `TableId` and `QrCodeId`.
9. Receptionist receives the order in the outlet order dashboard.

### Takeaway ordering from landing page

1. Customer visits `https://easeurorder.com`.
2. Customer searches for a restaurant/outlet.
3. Customer opens the outlet menu.
4. Customer selects takeaway items and pickup time if enabled.
5. Customer must pay through Razorpay before the order enters the outlet queue.
6. On payment success, the order becomes `Placed` and `Paid`.
7. Receptionist prepares the order for pickup.

### Tenant onboarding

1. SuperAdmin creates tenant and subscription record.
2. Platform generates tenant database name.
3. Provisioning service creates the tenant database.
4. Tenant migrations are applied.
5. TenantAdmin account is invited/created.
6. TenantAdmin creates first outlet.

## Recommended backend modules

```text
Platform
  Tenant management
  Subscription management
  Tenant provisioning
  Platform audit logs

Tenant resolution
  Subdomain resolver
  Tenant context
  Tenant DbContext factory

Identity and access control
  SuperAdmin policies
  TenantAdmin policies
  OutletAdmin policies
  Receptionist policies

Outlet operations
  Outlet management
  Table management
  QR code management
  Menu management
  Availability management

Ordering
  Dine-in QR ordering
  Pay-first takeaway ordering
  Order status workflow
  Reception dashboard

Payments
  Razorpay provider
  Payment webhook processing
  Payment reconciliation

Reporting
  Sales dashboard
  Best-selling items
  Gross profit reports
  Order trend reports
```

## Tenant resolution approach

Use a request-scoped tenant resolver:

```text
Host: tenant1.easeurorder.com
Route: /qr/{qrToken}
```

Resolution steps:

1. Extract `tenant1` from the host.
2. Query the platform database for active tenant with `Subdomain = tenant1`.
3. Build tenant connection string from `TenantConnection` and `Tenant.DatabaseName`.
4. Create `ApplicationDbContext` for the tenant database.
5. Resolve `qrToken` inside the tenant database.

This avoids putting database identifiers in URLs and keeps QR codes simple.

## Data model blueprint

### Platform database

```text
Tenants
- Id
- Name
- Subdomain
- CustomDomain
- DatabaseName
- IsActive
- SubscriptionStatus
- SubscriptionStartedAt
- SubscriptionExpiresAt
- CreatedAt

TenantSubscriptions
- Id
- TenantId
- PlanName
- BillingCycle
- Amount
- Currency
- Status
- StartedAt
- EndsAt
- RazorpayCustomerId
- RazorpaySubscriptionId
```

### Tenant database

```text
Outlets
- Id
- Name
- Slug
- Address
- Phone
- IsActive
- CreatedAt

CafeTables
- Id
- OutletId
- TableNumber
- DisplayName
- IsActive

QrCodes
- Id
- OutletId
- CafeTableId
- Token
- IsActive
- CreatedAt
- RevokedAt

MenuCategories
- Id
- OutletId
- Name
- SortOrder
- IsActive

MenuItems
- Id
- OutletId
- MenuCategoryId
- Name
- Description
- ImageUrl
- CostPrice
- SellingPrice
- IsAvailable
- IsActive

Orders
- Id
- OutletId
- CafeTableId nullable
- QrCodeId nullable
- OrderNumber
- OrderType: DineIn or Takeaway
- Status
- PaymentStatus
- Subtotal
- TaxTotal
- DiscountTotal
- GrandTotal
- CustomerName
- CustomerPhone
- CustomerNote
- PickupTime nullable
- CreatedAt
- AcceptedAt nullable
- CompletedAt nullable

OrderItems
- Id
- OrderId
- MenuItemId
- NameSnapshot
- UnitPriceSnapshot
- CostPriceSnapshot
- Quantity
- LineTotal
- Notes

Payments
- Id
- OrderId
- Provider
- Method
- Status
- Amount
- Currency
- ProviderOrderId
- ProviderPaymentId
- ProviderSignature
- PaidAt nullable
- CreatedAt
```

## Order status workflow

```text
Draft -> PaymentPending -> Placed -> Accepted -> Preparing -> Ready -> Served/Completed
                       \-> PaymentFailed
Placed/Accepted/Preparing -> Cancelled
Placed -> Rejected
```

For dine-in, payment can be optional depending on outlet settings. For takeaway, payment must be successful before the order becomes visible as a normal placed order.

## Razorpay integration approach

Use a payment abstraction so Razorpay can be outsourced without locking the whole codebase to Razorpay classes.

```text
IPaymentProvider
  CreateOrderAsync
  VerifyPaymentAsync
  HandleWebhookAsync

RazorpayPaymentProvider
  implements IPaymentProvider
```

Store Razorpay identifiers in `Payments`, but keep app order state in `Orders`. Webhooks should be idempotent so duplicate Razorpay webhook delivery does not duplicate payments or orders.

## Reporting blueprint

Start with live calculations from orders:

- today sales
- weekly sales
- monthly sales
- best-selling items by quantity
- best-selling items by revenue
- estimated gross profit
- cancelled/rejected order count
- dine-in vs takeaway revenue

Estimated gross profit:

```text
GrossProfit = Sum((UnitPriceSnapshot - CostPriceSnapshot) * Quantity)
```

Later add summary tables for performance:

```text
DailyOutletSalesSummaries
DailyMenuItemSalesSummaries
```

## Build sequence

### Phase 1: Foundation

- rename the business concept from restaurant-first to outlet-first
- add tenant resolver and tenant context
- add platform subscription model
- add tenant provisioning service
- add identity roles and authorization policies

### Phase 2: Core QR MVP

- outlet CRUD
- table CRUD
- QR code generation
- customer QR menu page
- dine-in order creation
- receptionist order dashboard

### Phase 3: Takeaway MVP

- public landing page search
- outlet public menu
- takeaway cart
- Razorpay pay-first checkout
- paid takeaway order queue

### Phase 4: Reporting

- sales charts
- most-selling item chart
- gross profit chart
- increase/decrease trend cards

### Phase 5: Operational polish

- SignalR live order updates
- kitchen display mode
- receipt printing
- audit logs
- tenant subscription renewal reminders
