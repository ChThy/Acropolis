
# Adding migrations:
To add migrations, adapt and run the following command:
`dotnet ef migrations add InitialMigration -p "Acropolis.Infrastructure.EfCore" -c "AppDbContext" --output-dir "Messenger/Migrations"`