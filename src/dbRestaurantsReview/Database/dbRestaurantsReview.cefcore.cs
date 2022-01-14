//************************************************************
// Generated using CalystoEFCoreFluentGenerator for MSSQL
//************************************************************
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Calysto.EntityFrameworkCore;
using Calysto.Data.Direct;
using Calysto.Common;
using Calysto.Common.Extensions;

namespace RestaurantsReview.Database
{
	/// <summary>
	/// CalystoEFCoreFluent for MSSQL
	/// </summary>
	public partial class dbRestaurantsReviewDataContext : DbContext
	{
		#region Context constructors
		
		public dbRestaurantsReviewDataContext(bool useLazyLoadingProxies)
			: base(new DbContextOptionsBuilder<dbRestaurantsReviewDataContext>()
				.UsingThis(builder => { if (useLazyLoadingProxies) builder.UseLazyLoadingProxies(); })
				.Options)
		{
			OnCreated();
		}
		
		public dbRestaurantsReviewDataContext(Action<DbContextOptionsBuilder<dbRestaurantsReviewDataContext>> configure)
			: base(new DbContextOptionsBuilder<dbRestaurantsReviewDataContext>()
				.UsingThis(builder => configure(builder))
				.Options)
		{
			OnCreated();
		}

		public dbRestaurantsReviewDataContext(DbContextOptions<dbRestaurantsReviewDataContext> options) : base(options)
		{
			OnCreated();
		}

		#endregion Context constructors

		#region OnConfiguring

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (!optionsBuilder.IsConfigured ||
				(!optionsBuilder.Options.Extensions.OfType<RelationalOptionsExtension>().Any(ext => !string.IsNullOrEmpty(ext.ConnectionString) || ext.Connection != null) &&
				!optionsBuilder.Options.Extensions.Any(ext => !(ext is RelationalOptionsExtension) && !(ext is CoreOptionsExtension))))
			{
				optionsBuilder.UseSqlServer(@"Data Source=.\MSSQL2019;Initial Catalog=dbRestaurantsReview;Integrated Security=True;Connect Timeout=30;");
			}
			CustomizeConfiguration(ref optionsBuilder);
			base.OnConfiguring(optionsBuilder);
		}

		partial void CustomizeConfiguration(ref DbContextOptionsBuilder optionsBuilder);

		#endregion OnConfiguring

		#region Context properties for tables

		public virtual DbSet<tblMember> tblMember { get; set; }
		public virtual DbSet<tblMemberRole> tblMemberRole { get; set; }
		public virtual DbSet<tblRestaurant> tblRestaurant { get; set; }
		public virtual DbSet<tblReview> tblReview { get; set; }
		public virtual DbSet<tblReviewReply> tblReviewReply { get; set; }
		public virtual DbSet<tblRole> tblRole { get; set; }

		#endregion Context properties for tables

		#region Context database functions


		#endregion Context database functions

		#region OnModelCreating

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			this.Map_tblMember(modelBuilder);
			this.Customize_tblMember(modelBuilder);

			this.Map_tblMemberRole(modelBuilder);
			this.Customize_tblMemberRole(modelBuilder);

			this.Map_tblRestaurant(modelBuilder);
			this.Customize_tblRestaurant(modelBuilder);

			this.Map_tblReview(modelBuilder);
			this.Customize_tblReview(modelBuilder);

			this.Map_tblReviewReply(modelBuilder);
			this.Customize_tblReviewReply(modelBuilder);

			this.Map_tblRole(modelBuilder);
			this.Customize_tblRole(modelBuilder);

			RelationshipsMapping(modelBuilder);
			CustomizeMapping(ref modelBuilder);
		}

		#endregion

		#region TablesMappingDetails

		#region tblMember
		private void Map_tblMember(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<tblMember>().ToTable("tblMember", "dbo");
			modelBuilder.Entity<tblMember>().HasKey(x=>x.MemberID);
			modelBuilder.Entity<tblMember>().Property(x=>x.MemberID).HasColumnName("MemberID").HasColumnType("Int").IsRequired().ValueGeneratedOnAdd();
			modelBuilder.Entity<tblMember>().Property(x=>x.CreationDate).HasColumnName("CreationDate").HasColumnType("DateTime").IsRequired().ValueGeneratedNever();
			modelBuilder.Entity<tblMember>().Property(x=>x.FirstName).HasColumnName("FirstName").HasColumnType("NVarChar(50)").ValueGeneratedNever().HasMaxLength(50);
			modelBuilder.Entity<tblMember>().Property(x=>x.LastName).HasColumnName("LastName").HasColumnType("NVarChar(50)").ValueGeneratedNever().HasMaxLength(50);
			modelBuilder.Entity<tblMember>().Property(x=>x.Email).HasColumnName("Email").HasColumnType("NVarChar(150)").IsRequired().ValueGeneratedNever().HasMaxLength(150);
			modelBuilder.Entity<tblMember>().Property(x=>x.Username).HasColumnName("Username").HasColumnType("NVarChar(100)").IsRequired().ValueGeneratedNever().HasMaxLength(100);
			modelBuilder.Entity<tblMember>().Property(x=>x.Password).HasColumnName("Password").HasColumnType("NVarChar(100)").IsRequired().ValueGeneratedNever().HasMaxLength(100);
			modelBuilder.Entity<tblMember>().Property(x=>x.LastLoginDate).HasColumnName("LastLoginDate").HasColumnType("DateTime").ValueGeneratedNever();
			modelBuilder.Entity<tblMember>().Property(x=>x.LoginsCount).HasColumnName("LoginsCount").HasColumnType("Int").IsRequired().ValueGeneratedNever();
		}

		partial void Customize_tblMember(ModelBuilder modelBuilder);
		#endregion

		#region tblMemberRole
		private void Map_tblMemberRole(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<tblMemberRole>().ToTable("tblMemberRole", "dbo");
			modelBuilder.Entity<tblMemberRole>().HasKey("MemberID", "RoleID");
			modelBuilder.Entity<tblMemberRole>().Property(x=>x.MemberID).HasColumnName("MemberID").HasColumnType("Int").IsRequired().ValueGeneratedNever();
			modelBuilder.Entity<tblMemberRole>().Property(x=>x.RoleID).HasColumnName("RoleID").HasColumnType("Int").IsRequired().ValueGeneratedNever();
		}

		partial void Customize_tblMemberRole(ModelBuilder modelBuilder);
		#endregion

		#region tblRestaurant
		private void Map_tblRestaurant(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<tblRestaurant>().ToTable("tblRestaurant", "dbo");
			modelBuilder.Entity<tblRestaurant>().HasKey(x=>x.RestaurantID);
			modelBuilder.Entity<tblRestaurant>().Property(x=>x.RestaurantID).HasColumnName("RestaurantID").HasColumnType("Int").IsRequired().ValueGeneratedOnAdd();
			modelBuilder.Entity<tblRestaurant>().Property(x=>x.OwnerMemberID).HasColumnName("OwnerMemberID").HasColumnType("Int").IsRequired().ValueGeneratedNever();
			modelBuilder.Entity<tblRestaurant>().Property(x=>x.CreationDate).HasColumnName("CreationDate").HasColumnType("DateTime").IsRequired().ValueGeneratedNever();
			modelBuilder.Entity<tblRestaurant>().Property(x=>x.Name).HasColumnName("Name").HasColumnType("NVarChar(250)").IsRequired().ValueGeneratedNever().HasMaxLength(250);
			modelBuilder.Entity<tblRestaurant>().Property(x=>x.Address).HasColumnName("Address").HasColumnType("NVarChar(250)").ValueGeneratedNever().HasMaxLength(250);
			modelBuilder.Entity<tblRestaurant>().Property(x=>x.Description).HasColumnName("Description").HasColumnType("NVarChar(2000)").ValueGeneratedNever().HasMaxLength(2000);
		}

		partial void Customize_tblRestaurant(ModelBuilder modelBuilder);
		#endregion

		#region tblReview
		private void Map_tblReview(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<tblReview>().ToTable("tblReview", "dbo");
			modelBuilder.Entity<tblReview>().HasKey(x=>x.ReviewID);
			modelBuilder.Entity<tblReview>().Property(x=>x.ReviewID).HasColumnName("ReviewID").HasColumnType("Int").IsRequired().ValueGeneratedOnAdd();
			modelBuilder.Entity<tblReview>().Property(x=>x.RestaurantID).HasColumnName("RestaurantID").HasColumnType("Int").IsRequired().ValueGeneratedNever();
			modelBuilder.Entity<tblReview>().Property(x=>x.ReviewerMemberID).HasColumnName("ReviewerMemberID").HasColumnType("Int").IsRequired().ValueGeneratedNever();
			modelBuilder.Entity<tblReview>().Property(x=>x.CreationDate).HasColumnName("CreationDate").HasColumnType("DateTime").IsRequired().ValueGeneratedNever();
			modelBuilder.Entity<tblReview>().Property(x=>x.RatingStars).HasColumnName("RatingStars").HasColumnType("Decimal(8,2)").ValueGeneratedNever().HasMaxLength(8);
			modelBuilder.Entity<tblReview>().Property(x=>x.Comment).HasColumnName("Comment").HasColumnType("NVarChar(2000)").ValueGeneratedNever().HasMaxLength(2000);
		}

		partial void Customize_tblReview(ModelBuilder modelBuilder);
		#endregion

		#region tblReviewReply
		private void Map_tblReviewReply(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<tblReviewReply>().ToTable("tblReviewReply", "dbo");
			modelBuilder.Entity<tblReviewReply>().HasKey(x=>x.ReviewID);
			modelBuilder.Entity<tblReviewReply>().Property(x=>x.ReviewID).HasColumnName("ReviewID").HasColumnType("Int").IsRequired().ValueGeneratedNever();
			modelBuilder.Entity<tblReviewReply>().Property(x=>x.ReplyedMemberID).HasColumnName("ReplyedMemberID").HasColumnType("Int").IsRequired().ValueGeneratedNever();
			modelBuilder.Entity<tblReviewReply>().Property(x=>x.CreationDate).HasColumnName("CreationDate").HasColumnType("DateTime").IsRequired().ValueGeneratedNever();
			modelBuilder.Entity<tblReviewReply>().Property(x=>x.ReplyText).HasColumnName("ReplyText").HasColumnType("NVarChar(2000)").ValueGeneratedNever().HasMaxLength(2000);
		}

		partial void Customize_tblReviewReply(ModelBuilder modelBuilder);
		#endregion

		#region tblRole
		private void Map_tblRole(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<tblRole>().ToTable("tblRole", "dbo");
			modelBuilder.Entity<tblRole>().HasKey(x=>x.RoleID);
			modelBuilder.Entity<tblRole>().Property(x=>x.RoleID).HasColumnName("RoleID").HasColumnType("Int").IsRequired().ValueGeneratedOnAdd();
			modelBuilder.Entity<tblRole>().Property(x=>x.RoleName).HasColumnName("RoleName").HasColumnType("VarChar(50)").IsRequired().ValueGeneratedNever().HasMaxLength(50);
			modelBuilder.Entity<tblRole>().Property(x=>x.CanReadReviews).HasColumnName("CanReadReviews").HasColumnType("Bit").IsRequired().ValueGeneratedNever();
			modelBuilder.Entity<tblRole>().Property(x=>x.CanWriteReview).HasColumnName("CanWriteReview").HasColumnType("Bit").IsRequired().ValueGeneratedNever();
			modelBuilder.Entity<tblRole>().Property(x=>x.CanCreateRestaurant).HasColumnName("CanCreateRestaurant").HasColumnType("Bit").IsRequired().ValueGeneratedNever();
			modelBuilder.Entity<tblRole>().Property(x=>x.CanEditRestaurant).HasColumnName("CanEditRestaurant").HasColumnType("Bit").IsRequired().ValueGeneratedNever();
			modelBuilder.Entity<tblRole>().Property(x=>x.CanReplayToRestaurantReview).HasColumnName("CanReplayToRestaurantReview").HasColumnType("Bit").IsRequired().ValueGeneratedNever();
			modelBuilder.Entity<tblRole>().Property(x=>x.CanEditMember).HasColumnName("CanEditMember").HasColumnType("Bit").IsRequired().ValueGeneratedNever();
			modelBuilder.Entity<tblRole>().Property(x=>x.CanEditRestaurantReview).HasColumnName("CanEditRestaurantReview").HasColumnType("Bit").IsRequired().ValueGeneratedNever();
			modelBuilder.Entity<tblRole>().Property(x=>x.CanEditReplayToRestaurantReview).HasColumnName("CanEditReplayToRestaurantReview").HasColumnType("Bit").IsRequired().ValueGeneratedNever();
			modelBuilder.Entity<tblRole>().Property(x=>x.CanEditRole).HasColumnName("CanEditRole").HasColumnType("Bit").IsRequired().ValueGeneratedNever();
			modelBuilder.Entity<tblRole>().Property(x=>x.CanListOwnRestaurants).HasColumnName("CanListOwnRestaurants").HasColumnType("Bit").IsRequired().ValueGeneratedNever();
			modelBuilder.Entity<tblRole>().Property(x=>x.CanListAllRestaurants).HasColumnName("CanListAllRestaurants").HasColumnType("Bit").IsRequired().ValueGeneratedNever();
		}

		partial void Customize_tblRole(ModelBuilder modelBuilder);
		#endregion

		#endregion

		#region Relationships mapping
		private void RelationshipsMapping(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<tblMember>().HasMany(x=>x.tblMemberRoleList).WithOne(x=>x.tblMember).HasForeignKey(x=>x.MemberID).IsRequired(true).OnDelete(DeleteBehavior.Cascade);
			modelBuilder.Entity<tblMember>().HasMany(x=>x.tblRestaurantList).WithOne(x=>x.tblMember).HasForeignKey(x=>x.OwnerMemberID).IsRequired(true);
			modelBuilder.Entity<tblMember>().HasMany(x=>x.tblReviewList).WithOne(x=>x.tblMember).HasForeignKey(x=>x.ReviewerMemberID).IsRequired(true).OnDelete(DeleteBehavior.Cascade);
			modelBuilder.Entity<tblMember>().HasMany(x=>x.tblReviewReplyList).WithOne(x=>x.tblMember).HasForeignKey(x=>x.ReplyedMemberID).IsRequired(true);

			modelBuilder.Entity<tblMemberRole>().HasOne(x=>x.tblMember).WithMany(x=>x.tblMemberRoleList).HasForeignKey(x=>x.MemberID).IsRequired(true).OnDelete(DeleteBehavior.Cascade);
			modelBuilder.Entity<tblMemberRole>().HasOne(x=>x.tblRole).WithMany(x=>x.tblMemberRoleList).HasForeignKey(x=>x.RoleID).IsRequired(true);

			modelBuilder.Entity<tblRestaurant>().HasOne(x=>x.tblMember).WithMany(x=>x.tblRestaurantList).HasForeignKey(x=>x.OwnerMemberID).IsRequired(true);
			modelBuilder.Entity<tblRestaurant>().HasMany(x=>x.tblReviewList).WithOne(x=>x.tblRestaurant).HasForeignKey(x=>x.RestaurantID).IsRequired(true).OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<tblReview>().HasOne(x=>x.tblMember).WithMany(x=>x.tblReviewList).HasForeignKey(x=>x.ReviewerMemberID).IsRequired(true).OnDelete(DeleteBehavior.Cascade);
			modelBuilder.Entity<tblReview>().HasOne(x=>x.tblRestaurant).WithMany(x=>x.tblReviewList).HasForeignKey(x=>x.RestaurantID).IsRequired(true).OnDelete(DeleteBehavior.Cascade);
			modelBuilder.Entity<tblReview>().HasOne(x=>x.tblReviewReply).WithOne(x=>x.tblReview).HasForeignKey<tblReviewReply>(x=>x.ReviewID).IsRequired(true).OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<tblReviewReply>().HasOne(x=>x.tblMember).WithMany(x=>x.tblReviewReplyList).HasForeignKey(x=>x.ReplyedMemberID).IsRequired(true);
			modelBuilder.Entity<tblReviewReply>().HasOne(x=>x.tblReview).WithOne(x=>x.tblReviewReply).HasForeignKey<tblReview>(x=>x.ReviewID).IsRequired(true).OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<tblRole>().HasMany(x=>x.tblMemberRoleList).WithOne(x=>x.tblRole).HasForeignKey(x=>x.RoleID).IsRequired(true);

		}
		#endregion

		#region Other details
		partial void CustomizeMapping(ref ModelBuilder modelBuilder);
		
		public bool HasChanges()
		{
		    return ChangeTracker.Entries().Any(e => e.State == Microsoft.EntityFrameworkCore.EntityState.Added || e.State == Microsoft.EntityFrameworkCore.EntityState.Modified || e.State == Microsoft.EntityFrameworkCore.EntityState.Deleted);
		}
		
		partial void OnCreated();
		#endregion

	}

	#region Entity Tables

	#region dbo.tblMember
	public partial class tblMember : EFCoreEntityBase<tblMember>
	{
		public tblMember()
		{
			OnCreated();
		}

		public tblMember(DbContext context) : base(context)
		{
			OnCreated();
		}

		#region Columns
		/// <summary>
		/// DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true, AutoSync = AutoSync.OnInsert
		/// </summary>
		public int MemberID { get; set; }

		/// <summary>
		/// DbType = "DateTime NOT NULL", DefaultValue = /*getdate()*/ DateTime.Now
		/// </summary>
		public DateTime CreationDate { get; set; } = /*getdate()*/ DateTime.Now;

		/// <summary>
		/// DbType = "NVarChar(50)"
		/// </summary>
		public string FirstName { get; set; }

		/// <summary>
		/// DbType = "NVarChar(50)"
		/// </summary>
		public string LastName { get; set; }

		/// <summary>
		/// DbType = "NVarChar(150) NOT NULL"
		/// </summary>
		public string Email { get; set; }

		/// <summary>
		/// DbType = "NVarChar(100) NOT NULL"
		/// </summary>
		public string Username { get; set; }

		/// <summary>
		/// DbType = "NVarChar(100) NOT NULL"
		/// </summary>
		public string Password { get; set; }

		/// <summary>
		/// DbType = "DateTime"
		/// </summary>
		public DateTime? LastLoginDate { get; set; }

		/// <summary>
		/// DbType = "Int NOT NULL"
		/// </summary>
		public int LoginsCount { get; set; }

		#endregion Columns

		#region Associations

		/// <summary>
		/// Association <br/>
		/// [PK][One]tblMember.MemberID --- [FK][Many]tblMemberRole.MemberID <br/>
		/// DeleteRule = Cascade <br/>
		/// </summary>
		public virtual List<tblMemberRole> tblMemberRoleList { get; set; } = new List<tblMemberRole>();

		/// <summary>
		/// Association <br/>
		/// [PK][One]tblMember.MemberID --- [FK][Many]tblRestaurant.OwnerMemberID <br/>
		/// DeleteRule = NoAction <br/>
		/// </summary>
		public virtual List<tblRestaurant> tblRestaurantList { get; set; } = new List<tblRestaurant>();

		/// <summary>
		/// Association <br/>
		/// [PK][One]tblMember.MemberID --- [FK][Many]tblReview.ReviewerMemberID <br/>
		/// DeleteRule = Cascade <br/>
		/// </summary>
		public virtual List<tblReview> tblReviewList { get; set; } = new List<tblReview>();

		/// <summary>
		/// Association <br/>
		/// [PK][One]tblMember.MemberID --- [FK][Many]tblReviewReply.ReplyedMemberID <br/>
		/// DeleteRule = NoAction <br/>
		/// </summary>
		public virtual List<tblReviewReply> tblReviewReplyList { get; set; } = new List<tblReviewReply>();
		#endregion Associations

		partial void OnCreated();
	}
	#endregion dbo.tblMember

	#region dbo.tblMemberRole
	public partial class tblMemberRole : EFCoreEntityBase<tblMemberRole>
	{
		public tblMemberRole()
		{
			OnCreated();
		}

		public tblMemberRole(DbContext context) : base(context)
		{
			OnCreated();
		}

		#region Columns
		/// <summary>
		/// DbType = "Int NOT NULL", IsPrimaryKey = true
		/// </summary>
		public int MemberID { get; set; }

		/// <summary>
		/// DbType = "Int NOT NULL", IsPrimaryKey = true
		/// </summary>
		public int RoleID { get; set; }

		#endregion Columns

		#region Associations

		/// <summary>
		/// Association <br/>
		/// [FK][Many]tblMemberRole.MemberID --- [PK][One]tblMember.MemberID <br/>
		/// DeleteOnNull = true <br/>
		/// </summary>
		public virtual tblMember tblMember { get; set; }

		/// <summary>
		/// Association <br/>
		/// [FK][Many]tblMemberRole.RoleID --- [PK][One]tblRole.RoleID <br/>
		/// </summary>
		public virtual tblRole tblRole { get; set; }
		#endregion Associations

		partial void OnCreated();
	}
	#endregion dbo.tblMemberRole

	#region dbo.tblRestaurant
	public partial class tblRestaurant : EFCoreEntityBase<tblRestaurant>
	{
		public tblRestaurant()
		{
			OnCreated();
		}

		public tblRestaurant(DbContext context) : base(context)
		{
			OnCreated();
		}

		#region Columns
		/// <summary>
		/// DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true, AutoSync = AutoSync.OnInsert
		/// </summary>
		public int RestaurantID { get; set; }

		/// <summary>
		/// DbType = "Int NOT NULL"
		/// </summary>
		public int OwnerMemberID { get; set; }

		/// <summary>
		/// DbType = "DateTime NOT NULL", DefaultValue = /*getdate()*/ DateTime.Now
		/// </summary>
		public DateTime CreationDate { get; set; } = /*getdate()*/ DateTime.Now;

		/// <summary>
		/// DbType = "NVarChar(250) NOT NULL"
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// DbType = "NVarChar(250)"
		/// </summary>
		public string Address { get; set; }

		/// <summary>
		/// DbType = "NVarChar(2000)"
		/// </summary>
		public string Description { get; set; }

		#endregion Columns

		#region Associations

		/// <summary>
		/// Association <br/>
		/// [FK][Many]tblRestaurant.OwnerMemberID --- [PK][One]tblMember.MemberID <br/>
		/// </summary>
		public virtual tblMember tblMember { get; set; }

		/// <summary>
		/// Association <br/>
		/// [PK][One]tblRestaurant.RestaurantID --- [FK][Many]tblReview.RestaurantID <br/>
		/// DeleteRule = Cascade <br/>
		/// </summary>
		public virtual List<tblReview> tblReviewList { get; set; } = new List<tblReview>();
		#endregion Associations

		partial void OnCreated();
	}
	#endregion dbo.tblRestaurant

	#region dbo.tblReview
	public partial class tblReview : EFCoreEntityBase<tblReview>
	{
		public tblReview()
		{
			OnCreated();
		}

		public tblReview(DbContext context) : base(context)
		{
			OnCreated();
		}

		#region Columns
		/// <summary>
		/// DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true, AutoSync = AutoSync.OnInsert
		/// </summary>
		public int ReviewID { get; set; }

		/// <summary>
		/// DbType = "Int NOT NULL"
		/// </summary>
		public int RestaurantID { get; set; }

		/// <summary>
		/// DbType = "Int NOT NULL"
		/// </summary>
		public int ReviewerMemberID { get; set; }

		/// <summary>
		/// DbType = "DateTime NOT NULL", DefaultValue = /*getdate()*/ DateTime.Now
		/// </summary>
		public DateTime CreationDate { get; set; } = /*getdate()*/ DateTime.Now;

		/// <summary>
		/// DbType = "Decimal(8,2)"
		/// </summary>
		public decimal? RatingStars { get; set; }

		/// <summary>
		/// DbType = "NVarChar(2000)"
		/// </summary>
		public string Comment { get; set; }

		#endregion Columns

		#region Associations

		/// <summary>
		/// Association <br/>
		/// [FK][Many]tblReview.ReviewerMemberID --- [PK][One]tblMember.MemberID <br/>
		/// DeleteOnNull = true <br/>
		/// </summary>
		public virtual tblMember tblMember { get; set; }

		/// <summary>
		/// Association <br/>
		/// [FK][Many]tblReview.RestaurantID --- [PK][One]tblRestaurant.RestaurantID <br/>
		/// DeleteOnNull = true <br/>
		/// </summary>
		public virtual tblRestaurant tblRestaurant { get; set; }

		/// <summary>
		/// Association <br/>
		/// [PK][One]tblReview.ReviewID --- [FK][One]tblReviewReply.ReviewID <br/>
		/// DeleteRule = Cascade <br/>
		/// </summary>
		public virtual tblReviewReply tblReviewReply { get; set; }
		#endregion Associations

		partial void OnCreated();
	}
	#endregion dbo.tblReview

	#region dbo.tblReviewReply
	public partial class tblReviewReply : EFCoreEntityBase<tblReviewReply>
	{
		public tblReviewReply()
		{
			OnCreated();
		}

		public tblReviewReply(DbContext context) : base(context)
		{
			OnCreated();
		}

		#region Columns
		/// <summary>
		/// DbType = "Int NOT NULL", IsPrimaryKey = true
		/// </summary>
		public int ReviewID { get; set; }

		/// <summary>
		/// DbType = "Int NOT NULL"
		/// </summary>
		public int ReplyedMemberID { get; set; }

		/// <summary>
		/// DbType = "DateTime NOT NULL", DefaultValue = /*getdate()*/ DateTime.Now
		/// </summary>
		public DateTime CreationDate { get; set; } = /*getdate()*/ DateTime.Now;

		/// <summary>
		/// DbType = "NVarChar(2000)"
		/// </summary>
		public string ReplyText { get; set; }

		#endregion Columns

		#region Associations

		/// <summary>
		/// Association <br/>
		/// [FK][Many]tblReviewReply.ReplyedMemberID --- [PK][One]tblMember.MemberID <br/>
		/// </summary>
		public virtual tblMember tblMember { get; set; }

		/// <summary>
		/// Association <br/>
		/// [FK][One]tblReviewReply.ReviewID --- [PK][One]tblReview.ReviewID <br/>
		/// DeleteOnNull = true <br/>
		/// </summary>
		public virtual tblReview tblReview { get; set; }
		#endregion Associations

		partial void OnCreated();
	}
	#endregion dbo.tblReviewReply

	#region dbo.tblRole
	public partial class tblRole : EFCoreEntityBase<tblRole>
	{
		public tblRole()
		{
			OnCreated();
		}

		public tblRole(DbContext context) : base(context)
		{
			OnCreated();
		}

		#region Columns
		/// <summary>
		/// DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true, AutoSync = AutoSync.OnInsert
		/// </summary>
		public int RoleID { get; set; }

		/// <summary>
		/// DbType = "VarChar(50) NOT NULL"
		/// </summary>
		public string RoleName { get; set; }

		/// <summary>
		/// DbType = "Bit NOT NULL", DefaultValue = false
		/// </summary>
		public bool CanReadReviews { get; set; } = false;

		/// <summary>
		/// DbType = "Bit NOT NULL"
		/// </summary>
		public bool CanWriteReview { get; set; }

		/// <summary>
		/// DbType = "Bit NOT NULL"
		/// </summary>
		public bool CanCreateRestaurant { get; set; }

		/// <summary>
		/// DbType = "Bit NOT NULL"
		/// </summary>
		public bool CanEditRestaurant { get; set; }

		/// <summary>
		/// DbType = "Bit NOT NULL"
		/// </summary>
		public bool CanReplayToRestaurantReview { get; set; }

		/// <summary>
		/// DbType = "Bit NOT NULL"
		/// </summary>
		public bool CanEditMember { get; set; }

		/// <summary>
		/// DbType = "Bit NOT NULL"
		/// </summary>
		public bool CanEditRestaurantReview { get; set; }

		/// <summary>
		/// DbType = "Bit NOT NULL"
		/// </summary>
		public bool CanEditReplayToRestaurantReview { get; set; }

		/// <summary>
		/// DbType = "Bit NOT NULL", DefaultValue = false
		/// </summary>
		public bool CanEditRole { get; set; } = false;

		/// <summary>
		/// DbType = "Bit NOT NULL", DefaultValue = false
		/// </summary>
		public bool CanListOwnRestaurants { get; set; } = false;

		/// <summary>
		/// DbType = "Bit NOT NULL", DefaultValue = false
		/// </summary>
		public bool CanListAllRestaurants { get; set; } = false;

		#endregion Columns

		#region Associations

		/// <summary>
		/// Association <br/>
		/// [PK][One]tblRole.RoleID --- [FK][Many]tblMemberRole.RoleID <br/>
		/// DeleteRule = NoAction <br/>
		/// </summary>
		public virtual List<tblMemberRole> tblMemberRoleList { get; set; } = new List<tblMemberRole>();
		#endregion Associations

		partial void OnCreated();
	}
	#endregion dbo.tblRole


	#endregion

	#region Functions Return types

	#endregion

}
