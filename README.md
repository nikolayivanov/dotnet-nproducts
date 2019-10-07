[![Board Status](https://dev.azure.com/NikolaiIvanov/60e1a9d0-a257-4f63-bd89-5f6a465ffef4/ffaca637-bdb9-4d7f-b6b8-3b1ecdbd4dbe/_apis/work/boardbadge/d0ec62d6-da3e-42fd-b948-255cf3e1f9b1)](https://dev.azure.com/NikolaiIvanov/60e1a9d0-a257-4f63-bd89-5f6a465ffef4/_boards/board/t/ffaca637-bdb9-4d7f-b6b8-3b1ecdbd4dbe/Microsoft.RequirementCategory)
# dotnet-nproducts
Web application and Web API for Northwind products management.

## База данных
Для работы с проектом и для его запуска нужна БД Northwind. 
Создаем ее локально просто запуском [sql script](https://raw.githubusercontent.com/microsoft/sql-server-samples/master/samples/databases/northwind-pubs/instnwnd.sql)

## Описание проектов
* NProducts.Data - базовый проект с классами моделями БД, интерфейсами
* NProducts.DAL - слой доступа к данным. Код репозиториев, код unitofwork.
* NProducts.BLL - 
* NProducts.Web - asp.net core web app сайт для просмотра категорий и продуктов. Используем IUnitOfWork для доступа к данным. Содержит свои DTO и используем [automapper](https://metanit.com/sharp/mvc5/23.4.php)  для маппинга DTO в db модели
* NProducts.WebApi - ASP.NET Core WebAPI с RESTful сервисом для Products
* NProducts.Tests - проект с тестами для обоих веб приложений Web and WebApi
