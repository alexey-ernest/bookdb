# BookDb
Single page web application with in-memory lock-free database for book management.

## Lock-free
The heart of database is non-locking technique proposed by Jeff Richter. 

[AsyncOneManyLock](/BookDb/BookDb/Infrastructure/Threading/AsyncOneManyLock.cs) - asynchronous thread synchronization construct which allows the database to be maximum performant and scalable: 

## Single Page Application
UI is implemented as a single page application powered by Angular.js

![](Assets/ss.png?raw=true)
