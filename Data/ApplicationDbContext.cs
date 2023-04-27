using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyPersonalProject.Models;

namespace MyPersonalProject.Data
{
    public class ApplicationDbContext:IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options) 
        {
            
        }


        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        public DbSet<LocalUser> LocalUsers { get; set; }

        public DbSet<Contact> Contacts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<Contact>().HasData(
            //    new Contact()
            //    {
            //        Id = 1,
            //        FirstName = "Test",
            //        LastName = "Test",
            //        Email = "Test",
            //        PhoneNumber = "Test",
            //        Address = "Test",
            //        ImageUrl="test",
            //        CreatedDate = DateTime.Now
            //    },
            //    new Contact()
            //    {
            //        Id = 2,
            //        FirstName = "John",
            //        LastName = "Doe",
            //        Email = "johndoe@example.com",
            //        PhoneNumber = "555-555-5555",
            //        Address = "123 Main St",
            //        ImageUrl = "https://example.com/johndoe.jpg",
            //        CreatedDate = DateTime.Now
            //    },
            //        new Contact()
            //        {
                    //    Id = 3,
                    //    FirstName = "Jane",
                    //    LastName = "Smith",
                    //    Email = "janesmith@example.com",
                    //    PhoneNumber = "555-555-5555",
                    //    Address = "456 Elm St",
                    //    ImageUrl = "https://example.com/janesmith.jpg",
                    //    CreatedDate = DateTime.Now
                    //},
                    //new Contact()
                    //{
                    //    Id = 4,
                    //    FirstName = "Bob",
                    //    LastName = "Johnson",
                    //    Email = "bobjohnson@example.com",
                    //    PhoneNumber = "555-555-5555",
                    //    Address = "789 Oak St",
                    //    ImageUrl = "https://example.com/bobjohnson.jpg",
                    //    CreatedDate = DateTime.Now
                    //},
                    //new Contact()
                    //{
                    //    Id = 5,
                    //    FirstName = "Alice",
                    //    LastName = "Lee",
                    //    Email = "alicelee@example.com",
                    //    PhoneNumber = "555-555-5555",
                    //    Address = "1011 Pine St",
                    //    ImageUrl = "https://example.com/alicelee.jpg",
                    //    CreatedDate = DateTime.Now
                    //},
                    //new Contact()
                    //{
                    //    Id = 6,
                    //    FirstName = "David",
                    //    LastName = "Wang",
                    //    Email = "davidwang@example.com",
                    //    PhoneNumber = "555-555-5555",
                    //    Address = "1213 Cedar St",
                    //    ImageUrl = "https://example.com/davidwang.jpg",
                    //    CreatedDate = DateTime.Now
                    //},
                    //new Contact()
                    //{
                    //    Id = 7,
                    //    FirstName = "Emily",
                    //    LastName = "Chen",
                    //    Email = "emilychen@example.com",
                    //    PhoneNumber = "555-555-5555",
                    //    Address = "1415 Maple St",
                    //    ImageUrl = "https://example.com/emilychen.jpg",
                    //    CreatedDate = DateTime.Now
                //    },
                //    new Contact()
                //    {
                //        Id = 8,
                //        FirstName = "Michael",
                //        LastName = "Liu",
                //        Email = "michaelliu@example.com",
                //        PhoneNumber = "555-555-5555",
                //        Address = "1617 Birch St",
                //        ImageUrl = "https://example.com/michaelliu.jpg",
                //        CreatedDate = DateTime.Now
                //    }
                //);
        }
    }
}
