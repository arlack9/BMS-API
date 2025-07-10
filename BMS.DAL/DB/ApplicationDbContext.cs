using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMS.Models.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace BMS.DAL.DB;

public class ApplicationDbContext : IdentityDbContext

{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }
    public DbSet<Book> Books { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Seed initial data for the Book table
        modelBuilder.Entity<Book>().HasData(
         new Book()
         {
             Id = 1,
             Title = "Sherlock Holmes",
             Author = "Doyle",
             PublishedYear = 1979
         },
         new Book()
         {
             Id = 2,
             Title = "Tom Holland",
             Author = "Tom",
             PublishedYear = 2001
         },
         new Book()
         {
             Id = 3,
             Title = "Tarzan",
             Author = "Rich Burroughs",
             PublishedYear = 2000
         }

            );
    }



}
