# Garage 3

# SeedData
If you don't have any registered users when loading the project the SeedData function will add
- 1 Admin user
- 1 Normal user
- 5 VehicleTypes
- 1 Vehicle of each type
- 5 Parkingspots, 2 filled and 1 partly filled

The login information for the users are username/passwords
- Admin - admin@group6.com / Group6!
- User - user@group6.com / Group6!
 
If you don't have an empty database but would like to run the SeedData anyway, You need to uncomment the lines in the `Extensions/ApplicationBuilderExtensions.cs`
```
// await context.Database.EnsureDeletedAsync();
// await context.Database.MigrateAsync();
```
Don't forget to comment these out once the SeedData has run for quicker project startup times.
