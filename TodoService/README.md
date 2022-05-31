# TodoService
https://localhost:44331/api/TodoItems/
https://localhost:44331/swagger/v1/swagger.json
https://localhost:44331/swagger/

{
	"id": 1,
	"name": "Item1",
	"isComplete": false
}

dotnet tool install --global dotnet-ef --version 3.1.0
dotnet ef migrations add Initial
dotnet ef database update
