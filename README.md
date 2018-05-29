#TheProject（VideoStore）

#How to run this application

1. Double click VideoStore.sln to open the solution in Visual Studio
2. Create database needed：
2.1. Visual Studio-->"Tools" menu-->SQL Server-->New Query:
Link to Local-->MSSQLLocalDB
Enter "create database Video", execute (if it already exists, please drop it beforehead)
Enter "create database Bank", execute (if it already exists, please drop it beforehead)
Enter "create database Deliveries", execute (if it already exists, please drop it beforehead)

2.2. Open: VideoStore.Business.Entities.VideoStoreEntityModel.edmx.sql, execute it to the [Video] database
2.3. Open: Bank.Business.Entities.BankEntityModel.edmx.sql, execute it to the [Bank] database
2.4. Open: DeliverCo.Business.Entities.DeliveryEntityModel.edmx.sql, execute it to the [Deliveries] database

3. Right click on VideoStoreMessageBus, Debug->Start new instance.
4. In VideoStore.Application, right click on VideoStore.Process, Debug->Start new instance.
5. In Bank.Application, right click on Bank.Process, Debug->Start new instance.
6. In DeliveryCo.Application, right click on DeliverCo.Process, Debug->Start new instance.
7. In EmailService.Application, right click on EmailService.Process, Debug->Start new instance.
8. Now, after all the processes are started, on VideoStore.Presentation, right click on VideoStore.WebClient, Debug->Start new instance.
9. In the opened browser, login with username 'Customer' and password 'COMP5348' and start shopping.
10. After checkout, various messages will be shown on different consoles of different processes.