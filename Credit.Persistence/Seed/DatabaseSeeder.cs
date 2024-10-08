using Credit.Domain.Entities;
using Credit.Domain.Enums;
using Credit.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace Credit.Infrastructure.Seed
{
    public static class DatabaseSeeder
    {
        public static void Seed(ServiceProvider context)
        {
            SeedPersons(context.GetService<MainDbContext>());
        }


        //seed person data
        private static void SeedPersons(MainDbContext context)
        {
            //if not, seed the database
            if (!context.Persons.Any())
            {
                context.Persons.Add(new PersonEntity
                {
                    FirstName = "admin",
                    LastName = "admin",
                    PersonalNumber = "12345678911",
                    Email = "admin@gmail.com",
                    PasswordHash = "$2a$11$kudNAZ2vJsyTmqwFYXgPAuB1.TqvMco8p5EugOtApm2jki4btZJN6",
                    Role = RoleEnum.Admin,
                    DateOfBirth = DateOnly.MinValue,
                    CreatedAt = DateTime.Now,
                });

                context.Persons.Add(new PersonEntity
                {
                    FirstName = "test",
                    LastName = "personal",
                    PersonalNumber = "45678912345",
                    Email = "test@gmail.com",
                    PasswordHash = "$2a$11$kudNAZ2vJsyTmqwFYXgPAuB1.TqvMco8p5EugOtApm2jki4btZJN6",
                    Role = RoleEnum.Person,
                    DateOfBirth = DateOnly.MinValue,
                    CreatedAt = DateTime.Now,
                });

                context.SaveChanges();
            }
        }

    }
}
