﻿using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;

namespace MessageHub.Web.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
	public class ApplicationUser : IdentityUser<long, CustomIdentityUserLogin, CustomIdentityUserRole, CustomIdentityUserClaim>
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool Sex { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser, long> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

	public class ApplicationDbContext : IdentityDbContext<ApplicationUser, CustomIdentityRole, long, CustomIdentityUserLogin, CustomIdentityUserRole, CustomIdentityUserClaim>
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }

	public class CustomIdentityRole : IdentityRole<long, CustomIdentityUserRole>
	{	
	}

	public class CustomIdentityUserRole : IdentityUserRole<long>
	{
	}

	public class CustomIdentityUserLogin : IdentityUserLogin<long>
	{	
	}

	public class CustomIdentityUserClaim : IdentityUserClaim<long>
	{	
	}
}