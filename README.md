
# Template Architect

**Template Architect** is a comprehensive repository designed to streamline the development of web-based applications. This project includes both **frontend applications** (Storefront and Backoffice) and **backend microservices**, providing a robust and modular architecture for developers.

## Table of Contents

- [Template Architect](#template-architect)
  - [Table of Contents](#table-of-contents)
  - [Overview](#overview)
  - [Features](#features)
  - [Tech Stack](#tech-stack)
    - [Frontend:](#frontend)
    - [Backend:](#backend)
    - [DevOps:](#devops)
  - [Project Structure](#project-structure)
  - [Getting Started](#getting-started)
    - [Prerequisites](#prerequisites)
    - [Installation](#installation)
    - [Running the Project](#running-the-project)
  - [Frontend Applications](#frontend-applications)
    - [Storefront](#storefront)
    - [Backoffice](#backoffice)
  - [Backend Microservices](#backend-microservices)
    - [Identity Service](#identity-service)
    - [User Service](#user-service)
    - [Customer Service](#customer-service)
    - [Product Service](#product-service)
    - [Inventory Service](#inventory-service)
    - [Order Service](#order-service)
  - [Deployment](#deployment)
    - [Frontend Deployment](#frontend-deployment)
    - [Backend Deployment](#backend-deployment)
  - [Contributing](#contributing)
  - [License](#license)
  - [Contact](#contact)

---

## Overview

Template Architect is an example architecture for full-stack applications, designed to demonstrate best practices in development, deployment, and scalability. The repository provides:

- **A storefront React application** for customer interactions.
- **A backoffice Angular application** for administrative workflows.
- **6 backend microservices** implementing various business logic layers.

The project is highly modular, enabling developers to extend or modify individual components without affecting the entire system.

---

## Features

- **Frontend Applications**:
  - Responsive and accessible user interfaces.
  - Modularized components and state management.
- **Backend Microservices**:
  - Authentication and authorization (Identity Service).
  - RESTful APIs for managing users, customers, products, orders, and inventory.
  - Scalable and isolated services.
- **Modern DevOps Integration**:
  - Docker Compose for local development and container orchestration.
  - GitHub Actions for CI/CD pipelines.
- **Easy Deployment**:
  - Host frontend applications on GitHub Pages.
  - Containerized backend services for seamless deployment.

---

## Tech Stack

### Frontend:
- **React** (Storefront)
- **Angular** (Backoffice)
- **TypeScript** and **JavaScript**
- **React Router** and **Angular Router**

### Backend:
- **Node.js** and **Express**
- **Docker** for containerization

### DevOps:
- **GitHub Actions** for CI/CD
- **Docker Compose** for orchestration
- **GitHub Pages** for hosting static files

---

## Project Structure

The repository is organized as follows:

```
template-architect/
│
├── 2--Implementation/STU01/src/frontend/
│   ├── storefront/       # React Storefront application
│   ├── backoffice/       # Angular Backoffice application
│
├── 2--Implementation/STU01/src/backend/
│   ├── identity-service/ # Authentication and authorization
│   ├── user-service/     # User profile management
│   ├── customer-service/ # Customer-related data
│   ├── product-service/  # Product catalog
│   ├── inventory-service/# Inventory and stock tracking
│   ├── order-service/    # Order lifecycle management
│
├── docker-compose.yaml   # Service orchestration file
├── README.md             # Project documentation
└── CONTRIBUTING.md       # Contribution guidelines
```

---

## Getting Started

### Prerequisites

- **Node.js** (>= 16.0)
- **npm** or **yarn**
- **Angular CLI** (for backoffice)
- **Docker** and **Docker Compose**

### Installation

Clone the repository to your local machine:

```bash
git clone https://github.com/chuangz/template-architect.git
cd template-architect
```

### Running the Project

1. **Frontend Applications**:
   - Navigate to the desired app directory (e.g., `storefront`):
     ```bash
     cd 2--Implementation/STU01/src/frontend/storefront
     npm install
     npm start
     ```
   - For the Angular backoffice:
     ```bash
     cd 2--Implementation/STU01/src/frontend/backoffice
     npm install
     ng serve
     ```

2. **Backend Services**:
   - Start all backend services using Docker Compose:
     ```bash
     docker-compose up --build
     ```

---

## Frontend Applications

### Storefront
- React-based customer-facing application.
- URL: `https://chuangz.github.io/template-architect/storefront`.

### Backoffice
- Angular-based administrative application.
- URL: `https://chuangz.github.io/template-architect/backoffice`.

---

## Backend Microservices

### Identity Service
- Handles authentication and authorization.

### User Service
- Manages user profiles and preferences.

### Customer Service
- Provides customer-related data.

### Product Service
- Manages product catalog and pricing.

### Inventory Service
- Tracks stock levels and inventory.

### Order Service
- Handles order creation, updates, and fulfillment.

---

## Deployment

### Frontend Deployment
- Use `gh-pages` for `storefront`:
  ```bash
  npm run deploy
  ```
- Use `angular-cli-ghpages` for `backoffice`:
  ```bash
  npx angular-cli-ghpages --dir=dist/backoffice
  ```

### Backend Deployment
- Deploy backend services using Docker Compose in production mode:
  ```bash
  docker-compose -f docker-compose.prod.yaml up -d
  ```

---

## Contributing

We welcome contributions! Check out our [CONTRIBUTING.md](CONTRIBUTING.md) for guidelines.

---

## License

This project is licensed under the [MIT License](LICENSE).

---

## Contact

For any questions or suggestions, please contact:
- **GitHub**: [chuangz](https://github.com/chuangz)
- **Email**: example@example.com
