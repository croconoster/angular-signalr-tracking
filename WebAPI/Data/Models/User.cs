using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Infrastructure;

namespace WebAPI.Data.Models
{
    public class User : IdentityUser, IUser
    {
        internal User(string email) : base(email) => Email = email;

        internal User(string firstName, string lastName, string email) : this(email)
        {
            FirstName = firstName;
            LastName = lastName;
        }

        [PersonalData]
        public string FirstName { get; set; }

        [PersonalData]
        public string LastName { get; set; }

    }

    public class IdentityConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = Guid.Parse("48490e8c-a349-4be8-9218-0917d1fd3b68").ToString(),
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Id = Guid.Parse("57228876-bea2-4233-91dc-1bd216790015").ToString(),
                    Name = "User",
                    NormalizedName = "USER"
                }
            });
        }
    }

}
