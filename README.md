# Rodrigo Cezar Leao
Posterr is a lightweight social feed where users can pick a profile, publish short posts, repost existing content, filter by keywords, and browse the timeline sorted by date or trend with infinite scrolling.

## Frontend App

The frontend is a Vite + React SPA that handles login selection, filtering, post creation, repost confirmation, and infinite scrolling. It consumes the REST API via a small service layer.

**Tech stack**
- React 19.0.0
- Vite 6.1.0
- TypeScript 5.7.2
- React Router DOM 7.1.5
- MUI 6.4.3
- Styled Components 6.1.15

## Backend App

The backend is a .NET 7 REST API following a DDD-style layering (API, Application, Domain, Infrastructure). It exposes endpoints for posts, reposts, users, and records, and uses EF Core with PostgreSQL.

**Tech stack**
- .NET 7.0
- ASP.NET Core (Web API)
- Entity Framework Core 7.x
- Npgsql.EntityFrameworkCore.PostgreSQL 7.0.18
- AutoMapper 13.0.1
- Swashbuckle.AspNetCore 7.2.0
- Swashbuckle.AspNetCore.Filters 8.0.2


### Instructions to run project

- The database is containerized and can be built by running this command in the terminal (notice the path in the terminal; it must be the project root path)

```
cp .env.example .env
docker-compose up -d

// removing containers and volume
docker-compose down
docker volume rm <volume_name>
```

- The API will create the tables and seed default users on the first run.
- Set the API connection string via environment variable:

```
ConnectionStrings__DefaultConnection=Host=localhost;Port=5432;Database=posterr;Username=postgres;Password=posterr123;
```

- To run API, open it on Visual Studio, define PosterrAPI as starter project and execute.. or open terminal, go to path `/PosterrBackend` and run command `dotnet build`, then navigate to `/PosterrBackend/PosterrAPI` and run command `dotnet run`. It will display the local link to open the API, adding `/swagger` at the end. (must have installed dotnet SDK for dotnet 7.0)

```
// navigate to /PosterrBackend
dotnet build

// navigate to /PosterrBackend/PosterrAPI
dotnet run

// open api docs in browser
http://localhost:5225/swagger
```

- To run UI, open terminal and go to path `/PosterrFrontend`, then run command `npm install`, wait until finish and then run `npm run dev`. It will display the link to open the UI in browser.

```
// navigate to /PosterrFrontend
cp .env.example .env
npm install
npm run dev

// open api docs in browser
http://localhost:5173
```

### Deploy frontend to GitHub Pages

- This repo includes a GitHub Actions workflow at `.github/workflows/deploy-frontend.yml` that builds the Vite app and publishes `PosterrFrontend/dist` to GitHub Pages.

#### Required repository settings

1) GitHub Pages
   - Go to Settings -> Pages.
   - Under "Source", select "GitHub Actions".

2) Actions secret
   - Go to Settings -> Secrets and variables -> Actions.
   - Create a repository secret named `VITE_API_URL` with the public API URL, for example:
     - `https://your-api.onrender.com/api`

#### How it works

- On every push to `master`, the workflow builds the frontend with:
  - `VITE_API_URL` from GitHub Actions secrets.
  - `VITE_BASE` set to `/<repo-name>/` so the app loads correctly under GitHub Pages.

### Deploy API to Render (free sleeping tier)

#### 1) Create the Render service

1) Go to https://render.com and create a new **Web Service** from this GitHub repo.
2) Select **Environment**: `Docker`.
3) Render will use the `Dockerfile` at the repo root. No build/start command fields are needed.
4) **Auto-Deploy**: enabled (optional).

#### 2) Add environment variables

In the Render service settings, add these environment variables:

- `ASPNETCORE_URLS`:
  ```
  http://0.0.0.0:$PORT
  ```
- `ConnectionStrings__DefaultConnection`:
  ```
  Host=<neon-host>;Port=5432;Database=<db-name>;Username=<user>;Password=<password>;SSL Mode=Require;Trust Server Certificate=true
  ```
- `Cors__AllowedOrigins__0`:
  ```
  https://<your-github-username>.github.io/<repo-name>
  ```

After the first deploy, open the Render service URL and add `/swagger` to confirm the API is running.

### Create the database on Neon (free tier)

#### 1) Create a Neon project

1) Go to https://neon.tech and create a free account.
2) Create a new project (free tier).
3) In the project dashboard, copy the **connection string**.

#### 2) Use the connection string in Render

Replace the placeholders and paste the full string into:

```
ConnectionStrings__DefaultConnection
```

Example format (use your real values):

```
Host=ep-example-123456.us-east-2.aws.neon.tech;Port=5432;Database=posterr;Username=posterr_owner;Password=your_password;SSL Mode=Require;Trust Server Certificate=true
```

#### 3) Verify database creation

- The API will create the tables and seed default users on its first startup.
- If you want to check data, use the Neon SQL editor in the dashboard.


---

### Project details

##### Dotnet 7.0
- There was a need to use this version due to platform compatibility issues. I had to transfer the project to a MacBook for development, and to avoid prolonged downtime, this was the most viable option.




##### Database
- PostgreSQL: Database used in the project.

- Connection String
```
Host: localhost
Port: 5432
Database: posterr
Username: postgres
Password: posterr123
```

![schema](/general-assets/database-schema.png)

 

##### Back-end

- The goal was to create a simple REST API following the DDD (Domain-Driven Design) pattern. The architecture was structured as follows.

```
PosterrAPI Solution
│
├── PosterrAPI (API)
│   ├── Controllers
│   ├── Services
│   ├── DTOs
│   ├── Configuration
│   ├── Program.cs (Entry Point)
│   ├── appsettings.json
│
├── Application
│   ├── Interfaces
│   ├── UseCases
│
├── Domain
│   ├── Entities
│   ├── ValueObjects
│   ├── Interfaces
│   ├── Services (Business Rules)
│
├── Infrastructure
│   ├── Data
│   │   ├── Migrations
│   │   ├── Repositories
│   ├── Context (DbContext - Entity Framework)
│
├── PosterrAPITests (Tests)
│   ├── UnitTests
│   ├── IntegrationTests
│
└── Solution Configuration
    ├── Debug | Any CPU
    ├── Release | Any CPU

```



- Used Packages
```
### API
Swashbuckle.AspNetCore
Swashbuckle.AspNetCore.Annotations
Swashbuckle.AspNetCore.Filters
Microsoft.EntityFrameworkCore.SqlServer
Microsoft.EntityFrameworkCore.Tools
AutoMapper

    //Execute commands in API path
    //create migration
        dotnet ef migrations add MigrationName
    //update database
        dotnet ef database update
    //remove migration
        ef migrations remove


### Infrastructure
Microsoft.EntityFrameworkCore

### Application
AutoMapper
AutoMapper.Extensions.Microsoft.DependencyInjection

### Tests
Microsoft.EntityFrameworkCore.InMemory
AutoMapper
AutoMapper.Extensions.Microsoft.DependencyInjection
```
- Dotnet 7.0
- EF: 7.0.16


##### Front-end

- The UI was designed with simplicity in mind, while ensuring all the requested features were implemented. The architecture was structured as follows.

```
/my-react-app
│── /src
│   │── /components
│   │   │── ConfirmRespostDialog
│   │   │── Content
│   │   │── FilterInput
│   │   │── PosterrCard
│   │   │── PostInput
│   │── /screens
│   │   │── Home
│   │   │── Login
│   │── /services
│   │   │── PostService
│   │   │── RepostService
│   │   │── RecordService
│   │   │── UserService
│   │── /helpers
│   │   │── helper.ts
│   │── /models
│   │   │── Record.ts
│   │   │── User.ts
│   │── App.tsx
│   │── index.tsx
│   │── main.tsx
│   │── routes.tsx
│── package.json
│── tsconfig.json
│── vite.config.ts
│── .env
│── README.md

```


- Used Packages
```
npm create vite@latest .

npm install 
npm install react-router-dom
npm install styled-components
npm install @mui/icons-material
npm install @emotion/react @emotion/styled
npm install @mui/material

npm run dev
```


---



## Critique


#### Considerations

- Posterr is a very interesting project that I really enjoyed developing. The business rules are engaging, and it involves a variety of different skills, exactly the kind of challenge a full-stack developer appreciates.

- I started by structuring the database schema, considering which fields and columns would be needed for the specified operations. After drafting the design in Excalidraw, I finalized it and included the proper diagram in the database section of this README. Initially, I considered two different approaches. One involved using a single table for both posts and reposts, with additional columns to distinguish them, such as allowing the text to be null for reposts. However, this approach didn’t seem ideal. In the end, I decided to separate Posts and Reposts into distinct tables, ensuring a clearer representation of the entities and making operations more understandable and accurate.

- Then, I used Docker and created a script to initialize the database, ensuring it was ready to use as soon as it was set up.
    - If I had more time, I would have containerized not just the database but also the API and UI, so the entire project would be managed within Docker Compose. This would make it easier to run everything with a single command. Additionally, I would set up CI/CD pipelines to build both the frontend and backend, run automated tests, and deploy the Docker image to a registry like Docker Hub.


- Although the login section was not a requirement, I wanted to make it easy for the reviewer to switch users. So, I added a screen displaying all users, allowing selection of the one to use at that moment. This made testing the implementation and features much simpler, especially those related to reposts, which involve more business rules.
    - A key point here is the middleware that checks if the user exists before processing requests in endpoints that require a user. This was very helpful in avoiding repetitive validation in each endpoint, following the DRY principle.

- For the backend, I used the architecture I’m most familiar with and regularly work with, following the DDD pattern. The structure includes API, Infra, Domain, and Application layers. I also implemented automated tests for both repositories and services.
    - Here’s one point: if I had more time, I would have improved the variable names in the tests. For the tests related to services, I would have pre populated the database with the proper data beforehand, instead of filling the data within the tests themselves.

- In the API, I tried to stick closely to the design principles, ensuring that controllers, services, and repositories had well defined responsibilities. I also made sure to place the code in the right files, as it’s common to have the code working correctly but not in the most appropriate place, especially when you’re in a hurry to deliver a feature or fix a bug. Additionally, I focused on returning the proper HTTP status code for each case and using objects to return custom error messages, along with custom exceptions to handle all business rule scenarios.
    - One point to mention is that if I had more time, I would have improved the "retrieve proper error to the user" part. This would include not only the message but also an error status code, and especially using a better dialog or toaster to display error cases in the UI. The dialog for repost confirmation and the simple alerts used to show errors weren’t the best presentation, but they were the quickest way to handle it at the time.
    - The helper service/repository was designed to handle features that required a combination of queries from both the Post/Repost tables and the Record entity. This approach aimed to make the Post/Repost concept more generic and provide the frontend with a better way to display data, as the core features and data manipulation were handled by the API.

- I wanted the documentation with Swagger to be well structured, so I provided clear explanations for each endpoint, including all possible status codes, example payloads, and instructions on how to use the API. While there were some scenarios that would not occur in the API (such as a repost being linked to a nonexistent post, since the UI would only display existing posts and the repost button would be attached to them), I aimed to make the API independent and concise. This was to ensure that any HTTP requests not sent by the UI would not mess the system, maintaining data integrity.


- In the UI, the focus was on implementing the specified features rather than on colors or design. I added the top bar with the username and logout icon to make the login process work properly and to simplify testing the features. The filter input and post input might not be ideal in terms of UX, but they are understandable when considering the icons. Since the UI was the last step, after spending most of the time on the API, design was not the main priority.
    - If I had more time, I would have improved the colors, edges, borders, and especially the positioning of the top part of the page (header, filter input, and post input). Also, as mentioned before, better visibility for errors and warnings, perhaps with a dialog or toaster, would have been useful. To be more complete, I could have implemented tests (maybe with Jest) for the frontend functions and operations, as well as for component rendering, to ensure everything behaves as expected. Additionally, documenting the components with Storybook would have been a nice touch.


#### Scaling

- I think the first issue that would arise as the project grows, with more users and posts, would be the screen scrolling and loading. This was already the most challenging part for me due to the different scenarios (ordering by date, ordering by trend, and filtering). Additionally, the number of components loaded on the screen could slow things down significantly with heavy usage, leading to slower rendering times.

- Then, the login needed to be implemented properly to allow users to access the system securely and prevent data or post leaks. This could be done by generating a JWT with an expiration time, using SHA-256 for encryption, implementing OAuth 2.0, or even integrating a Single Sign-On (SSO) service.

- The test suites should cover a wider range of scenarios to minimize bugs and ensure the best performance.

- The entire database infrastructure should also be analyzed in this way. Considering the amount of data in these three columns, it’s important to discuss optimizing resources, improving the architecture or schema, and possibly using stored procedures. Implementing indexes on tables or exploring a database better suited for handling data over time or for the specific needs of the project might also be necessary.

- The backend monolith seems fine for now and should work as the system grows. However, using cloud resources like load balancers and auto-scaling could enhance the infrastructure as the system’s usage increases. If it grows to the point where the architecture needs to be adjusted, we can consider transitioning to microservices to better manage responsibilities and resources. With tools like Kubernetes, we can manage different pods, scale them easily, and implement communication through message queues like ServiceBus or RabbitMQ. Additionally, we could use monitoring tools like Prometheus and Grafana to create dashboards for real-time system health monitoring and prevent unexpected crashes.

- Finally, one feature that could have been implemented in the test version is logging. Logs are crucial for tracking events and making debugging easier. Each operation (post, repost) should record a log, and the history should be accessible for troubleshooting. Instead of using a simple .txt file, we could use a reliable service like Logz.io, which provides an interface with Kibana for easier log queries, searching, and even building dashboards to monitor the application.
