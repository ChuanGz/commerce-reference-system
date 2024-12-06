# How to Run the Project

This repository contains the source code for a full-stack application with multiple microservices and frontend applications.

## Prerequisites
1. Install Docker: [https://www.docker.com/](https://www.docker.com/)
2. Install Node.js (for frontend): [https://nodejs.org/](https://nodejs.org/)
3. Install .NET 7.0 SDK: [https://dotnet.microsoft.com/](https://dotnet.microsoft.com/)

## Running Locally with Docker
1. Clone the repository:
   ```bash
   git clone https://github.com/your-repo-name.git
   cd your-repo-name
   ```

2. Start all services and frontend applications:
   ```bash
   docker-compose up --build
   ```

3. Access applications:
   - **Storefront**: [http://localhost:3000](http://localhost:3000)
   - **Backoffice**: [http://localhost:4200](http://localhost:4200)
   - **Product Service**: [http://localhost:5000](http://localhost:5000)
   - **Order Service**: [http://localhost:5001](http://localhost:5001)

## Running Frontend Applications Individually
### Storefront
1. Navigate to the storefront directory:
   ```bash
   cd frontend/storefront
   ```
2. Install dependencies:
   ```bash
   npm install
   ```
3. Start the development server:
   ```bash
   npm start
   ```

### Backoffice
1. Navigate to the backoffice directory:
   ```bash
   cd frontend/backoffice
   ```
2. Install dependencies:
   ```bash
   npm install
   ```
3. Start the development server:
   ```bash
   npm start
   ```

## Running Backend Services Individually
1. Navigate to the service directory:
   ```bash
   cd services/product-service
   ```
2. Run the service:
   ```bash
   dotnet run
   ```

## CI/CD Pipelines
- Pipelines are defined in the `.github/workflows` folder.
- To trigger a pipeline for a specific service, make changes in its folder and push the code.

## Notes
- This project is for study purposes and uses free services for hosting and CI/CD.
