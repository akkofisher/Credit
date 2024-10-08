# Credit  API

## Features


- **Error Handling:** Middleware for error logging and handling. 
- **Data Validation:** Ensures data integrity and structure.
- **Repository Pattern:** Abstracts the data access layer, making it easy to swap out the data source.
- **Unit of Work Pattern:** Manages database transactions efficiently.
- **RabbitMQ:** Using for order credit queue.
- **Serilog & Seq:** logging data request and errors.


## Getting Started

1 Clone the repository:
- git clone https://github.com/akkofisher/Credit.API

2 Set up the database:
- Update the connection string in appsettings.json to point to your SQL Server instance.
- Go to Credit.Infrastructure and Run the following command to apply migrations and create the database:
  database-update

3 Run the application 
   At start program will seed dumb data automatically 

  