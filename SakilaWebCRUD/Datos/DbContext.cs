﻿using Microsoft.EntityFrameworkCore;
using SakilaWebCRUD.Models;

namespace SakilaWebCRUD.Datos
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSets de las tablas principales
        public DbSet<Actor> Actors { get; set; }
        public DbSet<ActorInfo> ActorInfos { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerList> CustomerLists { get; set; }
        public DbSet<Film> Films { get; set; }
        public DbSet<FilmActor> FilmActors { get; set; }
        public DbSet<FilmCategory> FilmCategories { get; set; }
        public DbSet<FilmList> FilmLists { get; set; }
        public DbSet<FilmText> FilmTexts { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<NicerButSlowerFilmList> NicerButSlowerFilmLists { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Rental> Rentals { get; set; }
        public DbSet<SalesByFilmCategory> SalesByFilmCategories { get; set; }
        public DbSet<SalesByStore> SalesByStores { get; set; }
        public DbSet<Staff> Staff { get; set; }
        public DbSet<StaffList> StaffLists { get; set; }
        public DbSet<Store> Stores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder
                .UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");
        
            modelBuilder
                .UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");

            modelBuilder.Entity<Actor>(entity =>
            {
                entity.HasKey(e => e.ActorId).HasName("PRIMARY");

                entity.ToTable("actor");

                entity.HasIndex(e => e.LastName, "idx_actor_last_name");

                entity.Property(e => e.ActorId).HasColumnName("actor_id");
                entity.Property(e => e.FirstName)
                    .HasMaxLength(45)
                    .HasColumnName("first_name");
                entity.Property(e => e.LastName)
                    .HasMaxLength(45)
                    .HasColumnName("last_name");
                entity.Property(e => e.LastUpdate)
                    .ValueGeneratedOnAddOrUpdate()
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .HasColumnType("timestamp")
                    .HasColumnName("last_update");
            });

            modelBuilder.Entity<ActorInfo>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToView("actor_info");

                entity.Property(e => e.ActorId).HasColumnName("actor_id");
                entity.Property(e => e.FilmInfo)
                    .HasColumnType("text")
                    .HasColumnName("film_info");
                entity.Property(e => e.FirstName)
                    .HasMaxLength(45)
                    .HasColumnName("first_name");
                entity.Property(e => e.LastName)
                    .HasMaxLength(45)
                    .HasColumnName("last_name");
            });

            modelBuilder.Entity<Address>(entity =>
            {
                entity.HasKey(e => e.AddressId).HasName("PRIMARY");

                entity.ToTable("address");

                entity.HasIndex(e => e.CityId, "idx_fk_city_id");

                entity.Property(e => e.AddressId).HasColumnName("address_id");
                entity.Property(e => e.Address1)
                    .HasMaxLength(50)
                    .HasColumnName("address");
                entity.Property(e => e.Address2)
                    .HasMaxLength(50)
                    .HasColumnName("address2");
                entity.Property(e => e.CityId).HasColumnName("city_id");
                entity.Property(e => e.District)
                    .HasMaxLength(20)
                    .HasColumnName("district");
                entity.Property(e => e.LastUpdate)
                    .ValueGeneratedOnAddOrUpdate()
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .HasColumnType("timestamp")
                    .HasColumnName("last_update");
                entity.Property(e => e.Phone)
                    .HasMaxLength(20)
                    .HasColumnName("phone");
                entity.Property(e => e.PostalCode)
                    .HasMaxLength(10)
                    .HasColumnName("postal_code");

                entity.HasOne(d => d.City).WithMany(p => p.Addresses)
                    .HasForeignKey(d => d.CityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_address_city");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.CategoryId).HasName("PRIMARY");

                entity.ToTable("category");

                entity.Property(e => e.CategoryId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("category_id");
                entity.Property(e => e.LastUpdate)
                    .ValueGeneratedOnAddOrUpdate()
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .HasColumnType("timestamp")
                    .HasColumnName("last_update");
                entity.Property(e => e.Name)
                    .HasMaxLength(25)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<City>(entity =>
            {
                entity.HasKey(e => e.CityId).HasName("PRIMARY");

                entity.ToTable("city");

                entity.HasIndex(e => e.CountryId, "idx_fk_country_id");

                entity.Property(e => e.CityId).HasColumnName("city_id");
                entity.Property(e => e.City1)
                    .HasMaxLength(50)
                    .HasColumnName("city");
                entity.Property(e => e.CountryId).HasColumnName("country_id");
                entity.Property(e => e.LastUpdate)
                    .ValueGeneratedOnAddOrUpdate()
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .HasColumnType("timestamp")
                    .HasColumnName("last_update");

                entity.HasOne(d => d.Country).WithMany(p => p.Cities)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_city_country");
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.HasKey(e => e.CountryId).HasName("PRIMARY");

                entity.ToTable("country");

                entity.Property(e => e.CountryId).HasColumnName("country_id");
                entity.Property(e => e.Country1)
                    .HasMaxLength(50)
                    .HasColumnName("country");
                entity.Property(e => e.LastUpdate)
                    .ValueGeneratedOnAddOrUpdate()
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .HasColumnType("timestamp")
                    .HasColumnName("last_update");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.CustomerId).HasName("PRIMARY");

                entity.ToTable("customer");

                entity.HasIndex(e => e.AddressId, "idx_fk_address_id");

                entity.HasIndex(e => e.StoreId, "idx_fk_store_id");

                entity.HasIndex(e => e.LastName, "idx_last_name");

                entity.Property(e => e.CustomerId).HasColumnName("customer_id");
                entity.Property(e => e.Active)
                    .IsRequired()
                    .HasDefaultValueSql("'1'")
                    .HasColumnName("active");
                entity.Property(e => e.AddressId).HasColumnName("address_id");
                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("create_date");
                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .HasColumnName("email");
                entity.Property(e => e.FirstName)
                    .HasMaxLength(45)
                    .HasColumnName("first_name");
                entity.Property(e => e.LastName)
                    .HasMaxLength(45)
                    .HasColumnName("last_name");
                entity.Property(e => e.LastUpdate)
                    .ValueGeneratedOnAddOrUpdate()
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .HasColumnType("timestamp")
                    .HasColumnName("last_update");
                entity.Property(e => e.StoreId).HasColumnName("store_id");

                entity.HasOne(d => d.Address).WithMany(p => p.Customers)
                    .HasForeignKey(d => d.AddressId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_customer_address");

                entity.HasOne(d => d.Store).WithMany(p => p.Customers)
                    .HasForeignKey(d => d.StoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_customer_store");
            });

            modelBuilder.Entity<CustomerList>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToView("customer_list");

                entity.Property(e => e.Address)
                    .HasMaxLength(50)
                    .HasColumnName("address");
                entity.Property(e => e.City)
                    .HasMaxLength(50)
                    .HasColumnName("city");
                entity.Property(e => e.Country)
                    .HasMaxLength(50)
                    .HasColumnName("country");
                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.Name)
                    .HasMaxLength(91)
                    .HasColumnName("name");
                entity.Property(e => e.Notes)
                    .HasMaxLength(6)
                    .HasDefaultValueSql("''")
                    .HasColumnName("notes");
                entity.Property(e => e.Phone)
                    .HasMaxLength(20)
                    .HasColumnName("phone");
                entity.Property(e => e.Sid).HasColumnName("SID");
                entity.Property(e => e.ZipCode)
                    .HasMaxLength(10)
                    .HasColumnName("zip code");
            });

            modelBuilder.Entity<Film>(entity =>
            {
                entity.HasKey(e => e.FilmId).HasName("PRIMARY");

                entity.ToTable("film");

                entity.HasIndex(e => e.LanguageId, "idx_fk_language_id");

                entity.HasIndex(e => e.OriginalLanguageId, "idx_fk_original_language_id");

                entity.HasIndex(e => e.Title, "idx_title");

                entity.Property(e => e.FilmId).HasColumnName("film_id");
                entity.Property(e => e.Description)
                    .HasColumnType("text")
                    .HasColumnName("description");
                entity.Property(e => e.LanguageId).HasColumnName("language_id");
                entity.Property(e => e.LastUpdate)
                    .ValueGeneratedOnAddOrUpdate()
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .HasColumnType("timestamp")
                    .HasColumnName("last_update");
                entity.Property(e => e.Length).HasColumnName("length");
                entity.Property(e => e.OriginalLanguageId).HasColumnName("original_language_id");
                entity.Property(e => e.Rating)
                    .HasDefaultValueSql("'G'")
                    .HasColumnType("enum('G','PG','PG-13','R','NC-17')")
                    .HasColumnName("rating");
                entity.Property(e => e.ReleaseYear)
                    .HasColumnType("year")
                    .HasColumnName("release_year");
                entity.Property(e => e.RentalDuration)
                    .HasDefaultValueSql("'3'")
                    .HasColumnName("rental_duration");
                entity.Property(e => e.RentalRate)
                    .HasPrecision(4, 2)
                    .HasDefaultValueSql("'4.99'")
                    .HasColumnName("rental_rate");
                entity.Property(e => e.ReplacementCost)
                    .HasPrecision(5, 2)
                    .HasDefaultValueSql("'19.99'")
                    .HasColumnName("replacement_cost");
                entity.Property(e => e.Title)
                    .HasMaxLength(128)
                    .HasColumnName("title");

                entity.HasOne(d => d.Language).WithMany(p => p.FilmLanguages)
                    .HasForeignKey(d => d.LanguageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_film_language");

                entity.HasOne(d => d.OriginalLanguage).WithMany(p => p.FilmOriginalLanguages)
                    .HasForeignKey(d => d.OriginalLanguageId)
                    .HasConstraintName("fk_film_language_original");
            });

            modelBuilder.Entity<FilmActor>(entity =>
            {
                entity.HasKey(e => new { e.ActorId, e.FilmId })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("film_actor");

                entity.HasIndex(e => e.FilmId, "idx_fk_film_id");

                entity.Property(e => e.ActorId).HasColumnName("actor_id");
                entity.Property(e => e.FilmId).HasColumnName("film_id");
                entity.Property(e => e.LastUpdate)
                    .ValueGeneratedOnAddOrUpdate()
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .HasColumnType("timestamp")
                    .HasColumnName("last_update");

                entity.HasOne(d => d.Actor).WithMany(p => p.FilmActors)
                    .HasForeignKey(d => d.ActorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_film_actor_actor");

                entity.HasOne(d => d.Film).WithMany(p => p.FilmActors)
                    .HasForeignKey(d => d.FilmId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_film_actor_film");
            });

            modelBuilder.Entity<FilmCategory>(entity =>
            {
                entity.HasKey(e => new { e.FilmId, e.CategoryId })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("film_category");

                entity.HasIndex(e => e.CategoryId, "fk_film_category_category");

                entity.Property(e => e.FilmId).HasColumnName("film_id");
                entity.Property(e => e.CategoryId).HasColumnName("category_id");
                entity.Property(e => e.LastUpdate)
                    .ValueGeneratedOnAddOrUpdate()
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .HasColumnType("timestamp")
                    .HasColumnName("last_update");

                entity.HasOne(d => d.Category).WithMany(p => p.FilmCategories)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_film_category_category");

                entity.HasOne(d => d.Film).WithMany(p => p.FilmCategories)
                    .HasForeignKey(d => d.FilmId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_film_category_film");
            });

            modelBuilder.Entity<FilmList>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToView("film_list");

                entity.Property(e => e.Actors)
                    .HasColumnType("text")
                    .HasColumnName("actors");
                entity.Property(e => e.Category)
                    .HasMaxLength(25)
                    .HasColumnName("category");
                entity.Property(e => e.Description)
                    .HasColumnType("text")
                    .HasColumnName("description");
                entity.Property(e => e.Fid).HasColumnName("FID");
                entity.Property(e => e.Length).HasColumnName("length");
                entity.Property(e => e.Price)
                    .HasPrecision(4, 2)
                    .HasDefaultValueSql("'4.99'")
                    .HasColumnName("price");
                entity.Property(e => e.Rating)
                    .HasDefaultValueSql("'G'")
                    .HasColumnType("enum('G','PG','PG-13','R','NC-17')")
                    .HasColumnName("rating");
                entity.Property(e => e.Title)
                    .HasMaxLength(128)
                    .HasColumnName("title");
            });

            modelBuilder.Entity<FilmText>(entity =>
            {
                entity.HasKey(e => e.FilmId).HasName("PRIMARY");

                entity.ToTable("film_text");

                entity.HasIndex(e => new { e.Title, e.Description }, "idx_title_description").HasAnnotation("MySql:FullTextIndex", true);

                entity.Property(e => e.FilmId)
                    .ValueGeneratedNever()
                    .HasColumnName("film_id");
                entity.Property(e => e.Description)
                    .HasColumnType("text")
                    .HasColumnName("description");
                entity.Property(e => e.Title).HasColumnName("title");
            });

            modelBuilder.Entity<Inventory>(entity =>
            {
                entity.HasKey(e => e.InventoryId).HasName("PRIMARY");

                entity.ToTable("inventory");

                entity.HasIndex(e => e.FilmId, "idx_fk_film_id");

                entity.HasIndex(e => new { e.StoreId, e.FilmId }, "idx_store_id_film_id");

                entity.Property(e => e.InventoryId)
                    .HasColumnType("mediumint unsigned")
                    .HasColumnName("inventory_id");
                entity.Property(e => e.FilmId).HasColumnName("film_id");
                entity.Property(e => e.LastUpdate)
                    .ValueGeneratedOnAddOrUpdate()
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .HasColumnType("timestamp")
                    .HasColumnName("last_update");
                entity.Property(e => e.StoreId).HasColumnName("store_id");

                entity.HasOne(d => d.Film).WithMany(p => p.Inventories)
                    .HasForeignKey(d => d.FilmId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_inventory_film");

                entity.HasOne(d => d.Store).WithMany(p => p.Inventories)
                    .HasForeignKey(d => d.StoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_inventory_store");
            });

            modelBuilder.Entity<Language>(entity =>
            {
                entity.HasKey(e => e.LanguageId).HasName("PRIMARY");

                entity.ToTable("language");

                entity.Property(e => e.LanguageId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("language_id");
                entity.Property(e => e.LastUpdate)
                    .ValueGeneratedOnAddOrUpdate()
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .HasColumnType("timestamp")
                    .HasColumnName("last_update");
                entity.Property(e => e.Name)
                    .HasMaxLength(20)
                    .IsFixedLength()
                    .HasColumnName("name");
            });

            modelBuilder.Entity<NicerButSlowerFilmList>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToView("nicer_but_slower_film_list");

                entity.Property(e => e.Actors)
                    .HasColumnType("text")
                    .HasColumnName("actors");
                entity.Property(e => e.Category)
                    .HasMaxLength(25)
                    .HasColumnName("category");
                entity.Property(e => e.Description)
                    .HasColumnType("text")
                    .HasColumnName("description");
                entity.Property(e => e.Fid).HasColumnName("FID");
                entity.Property(e => e.Length).HasColumnName("length");
                entity.Property(e => e.Price)
                    .HasPrecision(4, 2)
                    .HasDefaultValueSql("'4.99'")
                    .HasColumnName("price");
                entity.Property(e => e.Rating)
                    .HasDefaultValueSql("'G'")
                    .HasColumnType("enum('G','PG','PG-13','R','NC-17')")
                    .HasColumnName("rating");
                entity.Property(e => e.Title)
                    .HasMaxLength(128)
                    .HasColumnName("title");
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(e => e.PaymentId).HasName("PRIMARY");

                entity.ToTable("payment");

                entity.HasIndex(e => e.RentalId, "fk_payment_rental");

                entity.HasIndex(e => e.CustomerId, "idx_fk_customer_id");

                entity.HasIndex(e => e.StaffId, "idx_fk_staff_id");

                entity.Property(e => e.PaymentId).HasColumnName("payment_id");
                entity.Property(e => e.Amount)
                    .HasPrecision(5, 2)
                    .HasColumnName("amount");
                entity.Property(e => e.CustomerId).HasColumnName("customer_id");
                entity.Property(e => e.LastUpdate)
                    .ValueGeneratedOnAddOrUpdate()
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .HasColumnType("timestamp")
                    .HasColumnName("last_update");
                entity.Property(e => e.PaymentDate)
                    .HasColumnType("datetime")
                    .HasColumnName("payment_date");
                entity.Property(e => e.RentalId).HasColumnName("rental_id");
                entity.Property(e => e.StaffId).HasColumnName("staff_id");

                entity.HasOne(d => d.Customer).WithMany(p => p.Payments)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_payment_customer");

                entity.HasOne(d => d.Rental).WithMany(p => p.Payments)
                    .HasForeignKey(d => d.RentalId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("fk_payment_rental");

                entity.HasOne(d => d.Staff).WithMany(p => p.Payments)
                    .HasForeignKey(d => d.StaffId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_payment_staff");
            });

            modelBuilder.Entity<Rental>(entity =>
            {
                entity.HasKey(e => e.RentalId).HasName("PRIMARY");

                entity.ToTable("rental");

                entity.HasIndex(e => e.CustomerId, "idx_fk_customer_id");

                entity.HasIndex(e => e.InventoryId, "idx_fk_inventory_id");

                entity.HasIndex(e => e.StaffId, "idx_fk_staff_id");

                entity.HasIndex(e => new { e.RentalDate, e.InventoryId, e.CustomerId }, "rental_date").IsUnique();

                entity.Property(e => e.RentalId).HasColumnName("rental_id");
                entity.Property(e => e.CustomerId).HasColumnName("customer_id");
                entity.Property(e => e.InventoryId)
                    .HasColumnType("mediumint unsigned")
                    .HasColumnName("inventory_id");
                entity.Property(e => e.LastUpdate)
                    .ValueGeneratedOnAddOrUpdate()
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .HasColumnType("timestamp")
                    .HasColumnName("last_update");
                entity.Property(e => e.RentalDate)
                    .HasColumnType("datetime")
                    .HasColumnName("rental_date");
                entity.Property(e => e.ReturnDate)
                    .HasColumnType("datetime")
                    .HasColumnName("return_date");
                entity.Property(e => e.StaffId).HasColumnName("staff_id");

                entity.HasOne(d => d.Customer).WithMany(p => p.Rentals)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_rental_customer");

                entity.HasOne(d => d.Inventory).WithMany(p => p.Rentals)
                    .HasForeignKey(d => d.InventoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_rental_inventory");

                entity.HasOne(d => d.Staff).WithMany(p => p.Rentals)
                    .HasForeignKey(d => d.StaffId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_rental_staff");
            });

            modelBuilder.Entity<SalesByFilmCategory>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToView("sales_by_film_category");

                entity.Property(e => e.Category)
                    .HasMaxLength(25)
                    .HasColumnName("category");
                entity.Property(e => e.TotalSales)
                    .HasPrecision(27, 2)
                    .HasColumnName("total_sales");
            });

            modelBuilder.Entity<SalesByStore>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToView("sales_by_store");

                entity.Property(e => e.Manager)
                    .HasMaxLength(91)
                    .HasColumnName("manager");
                entity.Property(e => e.Store)
                    .HasMaxLength(101)
                    .HasColumnName("store");
                entity.Property(e => e.TotalSales)
                    .HasPrecision(27, 2)
                    .HasColumnName("total_sales");
            });

            modelBuilder.Entity<Staff>(entity =>
            {
                entity.HasKey(e => e.StaffId).HasName("PRIMARY");

                entity.ToTable("staff");

                entity.HasIndex(e => e.AddressId, "idx_fk_address_id");

                entity.HasIndex(e => e.StoreId, "idx_fk_store_id");

                entity.Property(e => e.StaffId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("staff_id");
                entity.Property(e => e.Active)
                    .IsRequired()
                    .HasDefaultValueSql("'1'")
                    .HasColumnName("active");
                entity.Property(e => e.AddressId).HasColumnName("address_id");
                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .HasColumnName("email");
                entity.Property(e => e.FirstName)
                    .HasMaxLength(45)
                    .HasColumnName("first_name");
                entity.Property(e => e.LastName)
                    .HasMaxLength(45)
                    .HasColumnName("last_name");
                entity.Property(e => e.LastUpdate)
                    .ValueGeneratedOnAddOrUpdate()
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .HasColumnType("timestamp")
                    .HasColumnName("last_update");
                entity.Property(e => e.Password)
                    .HasMaxLength(40)
                    .HasColumnName("password")
                    .UseCollation("utf8mb4_bin");
                entity.Property(e => e.Picture)
                    .HasColumnType("blob")
                    .HasColumnName("picture");
                entity.Property(e => e.StoreId).HasColumnName("store_id");
                entity.Property(e => e.Username)
                    .HasMaxLength(16)
                    .HasColumnName("username");

                entity.HasOne(d => d.Address).WithMany(p => p.Staff)
                    .HasForeignKey(d => d.AddressId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_staff_address");

                entity.HasOne(d => d.Store).WithMany(p => p.Staff)
                    .HasForeignKey(d => d.StoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_staff_store");
            });

            modelBuilder.Entity<StaffList>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToView("staff_list");

                entity.Property(e => e.Address)
                    .HasMaxLength(50)
                    .HasColumnName("address");
                entity.Property(e => e.City)
                    .HasMaxLength(50)
                    .HasColumnName("city");
                entity.Property(e => e.Country)
                    .HasMaxLength(50)
                    .HasColumnName("country");
                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.Name)
                    .HasMaxLength(91)
                    .HasColumnName("name");
                entity.Property(e => e.Phone)
                    .HasMaxLength(20)
                    .HasColumnName("phone");
                entity.Property(e => e.Sid).HasColumnName("SID");
                entity.Property(e => e.ZipCode)
                    .HasMaxLength(10)
                    .HasColumnName("zip code");
            });

            modelBuilder.Entity<Store>(entity =>
            {
                entity.HasKey(e => e.StoreId).HasName("PRIMARY");

                entity.ToTable("store");

                entity.HasIndex(e => e.AddressId, "idx_fk_address_id");

                entity.HasIndex(e => e.ManagerStaffId, "idx_unique_manager").IsUnique();

                entity.Property(e => e.StoreId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("store_id");
                entity.Property(e => e.AddressId).HasColumnName("address_id");
                entity.Property(e => e.LastUpdate)
                    .ValueGeneratedOnAddOrUpdate()
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .HasColumnType("timestamp")
                    .HasColumnName("last_update");
                entity.Property(e => e.ManagerStaffId).HasColumnName("manager_staff_id");

                entity.HasOne(d => d.Address).WithMany(p => p.Stores)
                    .HasForeignKey(d => d.AddressId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_store_address");

                entity.HasOne(d => d.ManagerStaff).WithOne(p => p.StoreNavigation)
                    .HasForeignKey<Store>(d => d.ManagerStaffId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_store_staff");
            });


        }
      
    }
}
