namespace MessageHub.Lib.DAL
{
	using System;
	using System.Data.Entity;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Linq;

	public partial class Model1 : DbContext
	{
		public Model1()
			: base("name=Model1")
		{
		}

		public virtual DbSet<Category> Categories { get; set; }
		public virtual DbSet<Comment> Comments { get; set; }
		public virtual DbSet<Message> Messages { get; set; }

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
	}
}
