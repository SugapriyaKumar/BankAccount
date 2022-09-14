# BankAccount
TemUs assignment to create user accounts in a bank

TemUs Assessment Preparation and evaluation

Assignment done by,
Sugapriya Kumar
UID: 196740
5th Batch - .NET
.NET Framework Version - 4.7.2
MVC - 5.2.9.0

Given Problem Statement:

The task is to create a basic web-based tool for Bank accounts. This will require:
•            An application form [Assumed to use web forms]
•            A result page [Assumed to show created account details as result page]
•            A small database [MS SQL – 1 DB - 3 tables]
•            Writing logic to depending on Account Type [Logic in MVC Controller]

A society is mimicking a bank and is providing 3 types of accounts. 
Normal Account, Parent Account and Child Account all three are also considered as Savings Account. [Use Enums]
We can say all types of Accounts inherit from Savings Account. [In case of API implementation to handle the business case]
Use case 1: 
The application form will have fields to capture [Page 1]
1. First Name
2. Last Name
3. Username
4. Account Type
All types of account need to call the same CreateAccount() method but Child account needs to capture the following data additionally [Page 2]
5. Parent Account Number
6. Can WithDraw
7. WithDrawal Limit
Validations to implement in the application.
a. AccountType is mandatory [Implement as Drop Down & always selected 1 default value]
b. If Child Account then Parent Account is mandatory and must exist in the system.  Display message stating "No Parent Account found with the given account number" [Error Message]

Solution:
Implemented MVC 5 in .NET 4.7.2 using Enity Framework,
Followed Code First Approach,
Implemented logging using log4net,
Handled exceptions,
Written Unit tests using MSTest
