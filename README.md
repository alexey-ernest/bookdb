# BookDb
Single page web application with in-memory lock-free database for book management.

The heart of database is non-locking technique proposed by Jeff Richter - asynchronous thread synchronization construct which allows the database to be maximum performant and scalable: https://github.com/alexey-ernest/bookdb/blob/master/BookDb/BookDb/Infrastructure/Threading/AsyncOneManyLock.cs

![](Assets/ss.png?raw=true)
