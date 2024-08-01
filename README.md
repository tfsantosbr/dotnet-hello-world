# .NET 8 Project README

This README provides instructions on how to run the .NET 8 project locally and with Docker Compose.

## Pre Requisites

Before running the project, ensure that you have the following installed on your machine:

- .NET 8 SDK
- Docker
- Docker Compose

## Running Locally

To run the project locally, follow these steps:

1. Clone the repository to your local machine.
2. Open a terminal and navigate to the project directory.
3. Run the following command to restore the project dependencies:

   ```bash
   dotnet restore
   ```

4. Build the project using the following command:

   ```bash
   dotnet build
   ```

5. Run the project using the following command:

   ```bash
   dotnet run --project src/TestApi
   ```

6. Open your web browser and navigate to `http://localhost:5193` to access the application.

## Running with Docker Compose

To run the project with Docker Compose, follow these steps:

1. Clone the repository to your local machine.
2. Open a terminal and navigate to the project directory.
3. Build the Docker image using the following command:

   ```bash
   docker-compose build
   ```

4. Start the Docker containers using the following command:

   ```bash
   docker-compose up
   ```

5. Open your web browser and navigate to `http://localhost:5000` to access the application.

That's it! You should now be able to run the .NET 8 project locally and with Docker Compose.
