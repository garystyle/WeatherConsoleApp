- 發布 exe 檔案
  `dotnet publish -c Release -o bin\Release\netcoreapp3.1\publish`
  `dotnet publish -c Development -o bin\Development\netcoreapp3.1\publish`

#### appsettings.json

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "ServiceSetting": {
    "LineNotify": {
      "ClientID": "",
      "ClientSecret": "",
      "CallbackURL": "",
      "Token": ""
    },
    "Authorization": "CWB-..."
  }
}
```
