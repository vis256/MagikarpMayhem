# Magikarp Mayhem

## How to Run

### Prerequisites
- Docker installed on your machine

### Steps to Run with Docker

1. **Build the Docker image:**
   ```sh
   docker build -t magikarp-mayhem-app .
   ```

2. **Run the Docker container:**
   ```sh
   docker run -d -p 8080:8080 --name magikarp-mayhem-container magikarp-mayhem-app
   ```

The application will be accessible at `http://localhost:8080`.