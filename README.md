Project Overview
The STOCK API project is a Proof of Concept (POC) designed to demonstrate a robust, scalable, and secure stock management system. It leverages a range of modern technologies to ensure high performance, security, and maintainability.
Technologies Used

	API Gateway: Ocelot

	Stock API: .NET 8

	Identity Server 4: For authentication using client credentials and resource owner password flows

	AutoMapper: For object mapping between DTOs and models

	Entity Framework: For ORM and database operations

	Redis: For caching

	RabbitMQ: For message queuing

	SQL Server: For persistent data storage


Key Features
- Stock API: Performs all CRUD operations (Create, Read, Update, Delete) on stock data.
- Authentication: Utilizes Identity Server 4 for secure authentication and authorization.
- Caching: Implements Redis for caching frequently accessed data to enhance performance.
- Message Queue: Uses RabbitMQ for handling asynchronous tasks and communication between services.
- Mapping: Employs AutoMapper for efficient data mapping.
- Database: Utilizes Entity Framework for interaction with SQL Server.

Architecture Diagram
Below is a simplified architecture diagram of the STOCK API project:



 
![Architecture diagram1 drawio](https://github.com/MuhammadHassam01209/StockManagement/assets/174413713/af9ec2e1-87a9-430a-abbb-6948ca0bb99a)

