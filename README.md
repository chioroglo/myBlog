# MyBlog

# Used Technologies:

Microsoft SQL Server 15.0.4153.1,  
ASP.NET Core WEB API  
Entity Framework  
React(TypeScript) (Material UI,Formik,react-router-dom,Axios,React Redux)


# Contribution guide
1. Create new branch (git branch "name")
2. Switch to this branch (git checkout "name")
3. Make changes
4. Push branch to remote origin (git push)
5. Make pull request using UI GitHub Interface from your branch to origin/slave

# Getting Started
1. Install Docker, set up local MS SQL Server Database for development using this instruction (guide [9/9/2023] => https://www.youtube.com/watch?v=TPbCKUwJ_hE). Command for quick installation:
```shell
docker run -e "ACCEPT_EULA=Y" -e 'SA_PASSWORD=P@ssword!' -p 1433:1433 -d mcr.microsoft.com/mssql/server:2022-latest
```
2. Install an IDE for development, set up environment to develop .NET Applications (Install .NET 6 SDK (!!DOTNET SIX, NOT HIGHER!!))
3. Then, install Dotnet Entity Framework 6 Core Tools
https://learn.microsoft.com/en-us/ef/core/cli/dotnet
This is necessary to manage database migrations.
While you've installed CLI (Command Line Interface) for Entity Framework, type in following commands
```shell
# To view the migartions list
    dotnet ef migrations list -s WebApi
# To apply migrations to your brand new database
    dotnet ef database update -s WebApi
```
That should do well, after you lanch WebApi, it'll be populated with initial mock values. Launch the API!

4. Set up React application.
4.1 Install NODE.JS LTS version https://nodejs.org/en
4.2 Open react section of this project in console ("React/my-blog/")
4.3 Install all dependencies.
```shell
    npm install
```
4.4 Run the application
```shell
    npm start
```