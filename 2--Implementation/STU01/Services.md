# Backend Services Overview

## Identity Service
- Handles user authentication and authorization.
- Manages tokens for secure communication between services.

## User Service
- Manages user profiles, preferences, and roles.
- Provides user-related information to other services.

## Customer Service
- Manages customer-specific data, such as loyalty, history, and contact details.
- Works closely with the User Service to retrieve customer-related user details.

## Product Service
- Handles the product catalog, including descriptions, pricing, and availability.
- Fetches stock information from the Inventory Service.

## Inventory Service
- Tracks inventory levels and stock movements.
- Provides stock availability data to the Product Service.

## Order Service
- Manages the lifecycle of orders (creation, tracking, and fulfillment).
- Retrieves customer details from the Customer Service and product information from the Product Service.
- Handles payment workflows through the Payment Service (optional in your context).
