### Инструкция по запуску
1. **Клонируйте репозиторий:**
   ```sh
   git clone https://github.com/cofheim/ProfitTest.git
   ```
2. **Перейдите в корневую папку:**
   ```sh
   cd ProfitTest
   ```

3. **Запустите Docker-контейнеры:**
   Запустите Docker Desktop и выполните команду в корневой папке проекта (в ProfitTest):
   ```sh
   docker-compose up -d --build
   ```
   Эта команда соберёт образы и запустит контейнеры для API, PostgreSQL, Kafka и Zookeeper.

4. **Запустите клиентское приложение:**
   - Откройте файл `ProfitTest.sln` в Visual Studio.
   - Установите проект `ProfitTest` в качестве запускаемого.
   - Запустите приложение.

Для удобного тестирования и взаимодействия с backend API в проекте настроен Swagger и Swagger UI.

После запуска Docker-контейнеров, Swagger будет доступен в браузере по адресу:
[http://localhost:5006/swagger/index.html](http://localhost:5006/swagger/index.html)

Kafka UI доступен по http://localhost:8081/