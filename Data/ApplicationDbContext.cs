﻿using CSToDoList.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CSToDoList.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ToDoItem> ToDoItem { get; set; } = default!;
        public virtual DbSet<Accessory> Accessories { get; set; } = default!;
        
    }
}