# 氣象預報服務

## 專案簡介
使用中央氣象局開放API，取得所需資料整理成自己想看的格式，並透過Line Notify發送給訂閱者，每日排程定時執行。

## 技術運用
- .NET Core 3.1 Console Application
- 使用 windows 工作排程器定時執行

## 發布 exe 檔案
  - `dotnet publish -c Release -o bin\Release\netcoreapp3.1\publish`
  - `dotnet publish -c Development -o bin\Development\netcoreapp3.1\publish`
