using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageHub.Lib.Entity
{
	public class MessageHubDbContext : DbContext
	{
		public MessageHubDbContext()
            : base("name=MessageHubDBEntities")
        {
        }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Category>()
				.HasMany(e => e.Categories1)
				.WithOptional(e => e.Category1)
				.HasForeignKey(e => e.ParentId);

			modelBuilder.Entity<Category>()
				.HasMany(e => e.Messages)
				.WithRequired(e => e.Category)
				.HasForeignKey(e => e.SubCategoryId)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<Message>()
				.Property(e => e.Title)
				.IsUnicode(false);

			modelBuilder.Entity<Message>()
				.HasMany(e => e.Comments)
				.WithRequired(e => e.Message)
				.WillCascadeOnDelete(false);
		}
    
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
	}
}
