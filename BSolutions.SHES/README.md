# Setup Development Environment

## Database

In this project a SQLite database is used as a basis. This is configured and created via Entity Framework Core migrations.

### Add new Migration

Open the ***Package Manager Console*** in Visual Studio and add a new Database Migration with the following command:

```
PM> Add-Migration <MIGRATION_NAME>
```

Then the existing database must be updated or a new database must be created:

```
PM> Update-Databasè
```
