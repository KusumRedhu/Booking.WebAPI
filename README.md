# Booking.WebAPI

Booking Web Api has post endpoint to allow the users to book the settlement.

Solution is built in dotnet 8, and is using in-memory database with Entity Framework, unit test coverage for controller and service.

Zip folder can be extracted to run the solution.

How to test the Endpoint : 
1. Extract the zip folder and build the solution. The Build should be succeeded.
2. Run the Web API and it should redirec to swagger url.
3. You can find the API documentation what the endpoint accepts and return, and all possible response types expected if something goes wrong
   Swagger URL - https://localhost:7029/swagger/index.html
4. You can test the solution via postman
5. Create the POST request and enter - http://localhost:5200/api/booking
    In the body, add the following json  - 
    {
        "bookingTime": "09:30",
        "name":"John Smith"
    }
 6. If the booking slot is available, you should get the Guid and the 200 OK response status.
 7. Add the out of office hours - 08:00, you should get 400 Bad Request with error message.
 8. When invalid data passed, Controller validates the model, and return 400.
 9. When try to book 5 booking at given time, if it has 4 slots allocated, then return 409 with error message.
