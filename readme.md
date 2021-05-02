# Breakdown of Time Spent

|     | Task                                    | Percentage |
| ---:| --------------------------------------- | ----------:|
| 1.  | Understand requirement                  | 10%        |
| 2.  | Create DB schema                        | 10%        |
| 3.  | Implement APIs                          | 30%        |
| 4.  | Test APIs                               | 10%        |
| 5.  | Implement connector removal suggestions | 10%        |
| 6.  | Refactor to add layers                  | 10%        |
| 7.  | Create unit tests                       | 5%         |
| 8.  | Create integration tests                | 5%         |
| 9.  | Misc. (sln, repo, readme)               | 10%        |

---

# Suggestions

+ Dapper can be used as the ORM in place of Entity Framework Core since Dapper is known to perform better at insert and select queries. It also has an easier learning curve. Yet another advantage with Dapper is a lack of strong coupling between database schema and code like in Entity Framework. It allows for modification of schema without having to change code that queries the affected tables. 

+ A service can be created to handle generation of domain objects such as Group, Charge Stations and Connectors. Design patterns such as Factory, Builder can be used to implement the service. It enables isolation of logic involved in creation of resources. For example, the validation rules mentioned in assignment. 

+ More unit and integration tests are required to ensure both happy and unhappy flows work as expected. 

+ It is considered a security best practice to not expose sequential surrogate keys via api. Here, a numeric id is used as surrogate key to uniquely identify the resources group, charge station and connector. It would be preferable to involve some form of random number generation scheme such as UUID.

+ An api for the resource Charge Station and CRUD action methods for all mentioned resources is required.

+ Logging could be added to track application execution, errors, warnings and information that could help with monitoring/debugging. 

+ Exception handling middleware is required to enable graceful execution under unexpected circumstances. 

+ Data needs to be persisted in a disk-based database. 

+ Application configuration such as database connection string should be stored outside the codebase like in the runtime environment or a configuration file. 



