# Interact with Azure Cosmos Db using web api, and consume it from a React application 

React sample show how use azure cosmos db with web api
 
About this sample

**Scenario**

1-The react app  call to the orders api service.  
2-The web api, call via http to azure cosmos db.  

![alt text](https://github.com/ntiseira/WebApi-AzureCosmos-ReactApp/blob/master/ReadmeFiles/azureCosmosDb.png)



Once you've started the OrdersApi, you can run the React application, and then view items in the order list.



**React Application**


![alt text](https://github.com/ntiseira/WebApi-AzureCosmos-ReactApp/blob/master/ReadmeFiles/reactApp.jpg)



**Web api**

![alt text](https://github.com/ntiseira/WebApi-AzureCosmos-ReactApp/blob/master/ReadmeFiles/webApi.jpg)



**How to run this sample To run this sample, you'll need:**

-Visual Studio 2017  
-Sql express 2017  
-NodeJs  
-An Internet connection  
-Create a database account - >Create a resource > Databases > Azure Cosmos DB and create a database.  
-Create a collection of azure cosmos db  

**Step 1: Clone or download this repository**

git clone https://github.com/ntiseira/WebApi-AzureCosmos-ReactApp.git


**Step 2: Run the sample**

Web api

Clean the solution, rebuild the solution, and run it. You might want to go into the solution properties and set both projects as startup projects, with the service project starting first.

Set the next keys in web.config:

-database: name of database created  
-collection: name of collection created  
-endpoint: This url can be found in keys option, inside it appear as "URI"  
-authKey:  This authKey can be found in keys option, inside it appear as "PRIMARY KEY"   


React app

-Open windows command line, or node console   
-cd FE\ -npm install -npm start  


**Support**

Community Help and Support Use Stack Overflow to get support from the community. Ask your questions on Stack Overflow first and browse existing issues to see if someone has asked your question before.

If you find a bug in the sample, please raise the issue on GitHub Issues.

**Contributing**

If you wish to make code changes to samples, or contribute something new, please follow the GitHub Forks / Pull requests model: Fork the sample repo, make the change and propose it back by submitting a pull request.


