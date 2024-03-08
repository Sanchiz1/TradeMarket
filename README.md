# Trade Market


## Domain description

Supermarkets sell goods of various categories. The customers can shop anonymously or by logging in. When buying, a receipt is created with a list of goods purchased in a particular market.

## Trade Market - DAL

Data Access Layer (DAL) for an electronic system **"Trade Market"** with Three-Layer Architecture in dynamic library form named “Data”.

![Data Entities](/Data/DataEntities_Scheme.jpeg)

The structure of the DAL project in the final form:
- The folder **Entities** contains classes of entities. Every entity inherits **BaseEntity { int Id }**.
- The folder **Interfaces** contains repository interfaces of entities and the interface of their merge point.
- The folder **Repositories** contains repository classes that implement interfaces from the folder **Interfaces**.
- The root folder of the project contains **MarketDBContext.cs** file for project entity context.
- The root folder of the project contains **UnitOfWork.cs** file. This class is entry point for all repositories to get access to DAL from the business logic.
- The folder **Migrations** contains project database migration files.  
Instructions on **how to create migrations** can be found in file **_Data / Migrations_Overview.md_.**


## Trade Market - BLL

Business Logic Layer (BLL) for the electronic system **"Trade Market"** with a three-layered architecture in the form of dynamic library called “Business”. Data Access Layer (DAL) is used from the Trade Market task – DAL.

The structure of the BLL project in the final form:
- The folder **Models** contains classes of logic’s models.
- The folder **Interfaces** contains BLL service interfaces.
- The folder **Services** contains service classes that implement interfaces from the folder **Interfaces**.
- The root folder of the project contains **AutomapperProfile.cs** file to display DAL entities in the BLL model and opposite.
```
Product <-> ProductModel
Customer <-> CustomerModel
Receipt <-> ReceiptModel
ReceiptDetail <-> ReceiptDetailModel,
ProductCategory <-> ProductCategoryModel
```
- The folder **Validation** which contains **MarketException.cs** file – the class of user exception **MarketException**.

![Business Entities](/Business/BusinessModels_Scheme.jpeg)


## Trade Market - BPL

Web Application (Presentation Layer, PL) for the electronic system **"Trade Market"** with a Three-Layer Architecture as an asp.net application named WebAPI with functionality according route list bellow. Data Access Layer (DAL) is used from the Trade Market task – DAL, Business Logic Layer (BLL) is used from the Trade Market task – BLL.
The structure of the PL project in the final form:

- The folder **Controllers** contains Web API classes of controllers.
- The project root folder contains file **appsettings.Development.json**.
- The project root folder contains **Startup.cs** file.
- Using **Swagger** to self-document the Web Api.


Route List:
```
GET/api/products – all products
GET/api/products/{id} – a selected products
GET/api/products?categoryId=1&minPrice=20&maxPrice=50 – search for products by filter, ffor example, goods of category with Id = 1 and price less then maxPrice=50 and more then minPrice = 20
POST/api/products – add a product
PUT/api/products/{id} – change a product
DELETE/api/products/{id} – delete a product
GET/api/products/categories - get all categories
POST/api/products/categories - add a category 
PUT/api/products/categories{id} - update a category
DELETE/api/products/categories/{id} - delete a category

GET/api/customers – all customers
GET/api/customers/products/{id} - all customers who bought specified product
GET/api/customers/{id} – a selected customer
POST/api/customers – add a customer
PUT/api/customers/{id} – change a customer
DELETE/api/customers/{id} – delete a customer

GET/api/receipts – all receipts
GET/api/receipts/{id} – a selected receipt
GET/api/receipts/{id}/details - all details 
GET/api/receipts/{id}/sum – a selected receipt sum
GET/api/receipts/period?startDate=2021-12-1&endDate=2020-12-31 – all receipts by period, for example, from 2021-12-1 to 2020-12-31
POST/api/receipts – add a receipt
PUT/api/receipts/{id} – change a receipt
PUT/api/receipts/{id}/products/add/{productId}/{quantity} – add a product to a receipt
PUT/api/receipts/{id}/products/remove/{productId}/{quantity} – remove a product from a receipt
PUT/api/receipts/{id}/checkout – change a receipt check out value to true
DELETE/api/receipts/{id} – delete a receipt

GET/api/statistic/popularProducts?productCount=2 - Gets most popular products
GET/api/statisic/customer/{id}/{productCount} - Gets the concrete number of most favourite products of customer
GET/api/statistic/activity/{customerCount}?startDate= 2020-7-21&endDate= 2020-7-22 – Gets the most active customers in a period of time, for example, from 2020-7-21 to 2020-7-22
GET/api/statistic/income/{categoryId}?startDate= 2020-7-21&endDate= 2020-7-22 – Gets the income of category in a period of time, for example, from 2020-7-21 to 2020-7-22

```
