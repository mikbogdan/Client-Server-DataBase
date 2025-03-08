# News Parser Desktop App

Проект для парсинга новостей и поиска статей по сущностям. Приложение использует Electron для создания десктопного интерфейса.

## Содержание
- [Описание](#описание)
- [Технологии](#технологии)
- [Установка](#установка)
- [Использование](#использование)

## Описание
Этот проект позволяет:
- Парсить новости с сайтов.
- Сохранять их в базу данных.
- Искать статьи по сущностям.
- Запускать десктопное приложение через Electron.

## Технологии
- Backend: ASP.NET Core
- Frontend: Angular
- Десктоп: Electron
- База данных: MySQL

## Установка
1. Клонируйте репозиторий:
   ```bash
   git clone https://github.com/your-username/news-parser.git
  Установите зависимости:
   BackEnd:
     -cd Backend
     -dotnet restore
   NewsClient:
   -cd NewsClient
   -npm install
  Запустите приложение:
    Backend: dotnet run
    NewsClient: ng build, npm run electron
    
