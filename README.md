# BookDb
Single page web application with in-memory lock-free database for book management.

The heart of database is non-locking technique proposed by Jeff Richter. [AsyncOneManyLock](/blob/master/BookDb/BookDb/Infrastructure/Threading/AsyncOneManyLock.cs) - asynchronous thread synchronization construct which allows the database to be maximum performant and scalable: 

![](Assets/ss.png?raw=true)
