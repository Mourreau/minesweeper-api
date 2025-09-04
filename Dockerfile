# ---------- STAGE 1: build ----------
# Образ со SDK .NET 8 — здесь есть компилятор и инструменты для сборки
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копируем всё исходники внутрь контейнера
# (на реальных проектах сначала копят csproj, делают restore, потом COPY . — ради кеша;
#  здесь без усложнений, но можно оптимизировать позже)
COPY . .

# Собираем веб-проект в Release в папку /app
# ВАЖНО: указываем путь к .csproj веб-проекта
RUN dotnet publish Minesweeper.Presentation/Minesweeper.Presentation.csproj -c Release -o /app


# ---------- STAGE 2: runtime ----------
# Лёгкий рантайм-образ без SDK — меньше вес и быстрее старт
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Копируем только собранные артефакты из предыдущего этапа
COPY --from=build /app .

# Render прокидывает порт в переменную PORT.
# Kestrel должен слушать на всех интерфейсах (http://+:<port>), иначе снаружи не будет доступен.
ENV ASPNETCORE_URLS=http://+:${PORT}

# Официально «декларируем» порт (для локальных запусков/докеров это полезно; на Render не обязательно)
EXPOSE 8080

# Точка входа: запускаем твоё веб-приложение
# Имя DLL = имя проекта (.csproj)
ENTRYPOINT ["dotnet", "Minesweeper.Presentation.dll"]
