# Azure Blob Storage Explorer - ASP.NET Core MVC

This project is a web-based **Azure Blob Storage Explorer** developed in **ASP.NET Core MVC**. The application allows you to:
- Connect to Azure Blob Storage via a connection string.
- Browse containers and folders (virtual directories) like a file explorer.
- List and perform CRUD operations (create, read, update, delete) on blob items.
- View blob metadata such as size and modification dates in a human-readable format.

---

## Features
- List and browse Azure Blob containers and folders.
- Display blob details (name, size, created date, modified date).
- Support for CRUD operations on blobs.
- Human-readable date and size formatting.

---

## Prerequisites

Before you begin, ensure you have the following tools installed:

- **.NET 8 SDK**: To build and run the application locally.
- **Docker**: To containerize the application.
- **Kubernetes**: To deploy the containerized app to a Kubernetes cluster.

---

## Setup Instructions

### Clone the repository
```bash
git clone https://github.com/your-username/azure-blob-explorer.git
cd azure-blob-explorer
```

### Restore the project dependencies
```
dotnet restore
```
### Run the application locally (Optional)
#### To test the app locally, run the following command:
```
dotnet run
```
This will start the application at `http://localhost:5000`.

------

## Docker Setup
### 1. Dockerfile
This project includes a Dockerfile for building and running the application in a container.
### 2. Build the Docker image
From the root directory of your project, run:
```
docker build -t azure-blob-explorer .
```
### 3. Run the Docker container
Run the container in detached mode and map port 8080 of the container to port 80 on your local machine:
```
docker run -d -p 8080:80 --name azure-blob-explorer-container azure-blob-explorer
```
### 4. Access the application
Open your browser and go to `http://localhost:8080` to access the app.

---------

## Kubernetes Deployment

### Prerequisites

Before deploying to Kubernetes, ensure you have the following tools installed:

- **kubectl**: The Kubernetes command-line tool.
- **Docker**: To build and push Docker images.
- **A Kubernetes Cluster**: You can use **AKS (Azure Kubernetes Service)** or any other cluster.

### 1. Build and Push Docker Image to Container Registry

To deploy the application to Kubernetes, you need to push the Docker image to a container registry (e.g., **Azure Container Registry (ACR)** or **Docker Hub**).

1. **Log in to Azure Container Registry** (if using Azure):
```
   az acr login --name <your-acr-name>
```

2. **Tag the Docker image** for the registry:
```
docker tag azure-blob-explorer <your-acr-name>.azurecr.io/azure-blob-explorer:v1
```
3. **Push the image** to the container registry:
```
docker push <your-acr-name>.azurecr.io/azure-blob-explorer:v1
```

### 2. Create Kubernetes Deployment YAML
modify azure-blob-explorer-deployment.yaml file for the Kubernetes deployment:

### 3. Apply the Deployment
Deploy the application to your Kubernetes cluster using `kubectl`:
```
kubectl apply -f azure-blob-explorer-deployment.yaml
```

### 4. Access the Application
Once the deployment is complete, use the following command to get the external IP of the service:
```
kubectl get svc azure-blob-explorer-service
```
