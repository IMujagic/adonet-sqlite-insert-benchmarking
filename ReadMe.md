# .NET Sqlite Insert Benchmarking   

**NOTE THAT THIS REPOSITORY IS STILL WORK IN PROGRESS**

Purpose of this project is to show different approaches for inserting huge number of rows in Sqlite database using ADO.NET provider.

Default approach where each insert is packed in own transaction has bad performance. That was a motivation to explore different approaches and measure 
which one has the best performance. 

1. Default approach where each insert is commited separately is implemented in repository `RepositoryWithoutTransaction`  class
2. First improvement step is to wrap all inserts inside one transaction =>  `RepositoryWithTransaction`  class

