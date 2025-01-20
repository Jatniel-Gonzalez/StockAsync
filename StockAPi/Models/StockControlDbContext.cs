using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace StockAPi.Models
{
    // Contexto de la base de datos para la gestión del inventario
    public partial class StockControlDbContext : DbContext
    {
        // Constructor sin parámetros
        public StockControlDbContext()
        {
        }

        // Constructor con opciones de configuración para el contexto de la base de datos
        public StockControlDbContext(DbContextOptions<StockControlDbContext> options)
            : base(options)
        {
        }

        // Definición de las tablas de la base de datos correspondientes a las entidades
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductHistory> ProductHistories { get; set; }
        public virtual DbSet<Sale> Sales { get; set; }

        // Método para configurar opciones adicionales de la base de datos
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

        // Método para configurar el modelo de la base de datos (entidades y relaciones)
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuración de la tabla 'Category'
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.CategoryId).HasName("PK__Category__19093A2B162E9B66");  // Definición de clave primaria

                entity.ToTable("Category");  // Nombre de la tabla

                // Definición de propiedades para la tabla 'Category'
                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("(getdate())")  // Valor por defecto para CreatedAt
                    .HasColumnType("datetime");
                entity.Property(e => e.Description).HasColumnType("text");
                entity.Property(e => e.Name)
                    .HasMaxLength(50)  // Longitud máxima para el nombre
                    .IsUnicode(false);  // No usar Unicode
                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            });

            // Configuración de la tabla 'Product'
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.ProductId).HasName("PK__Products__B40CC6ED8C22E405");  // Clave primaria

                // Definición de propiedades para la tabla 'Product'
                entity.Property(e => e.ProductId).HasColumnName("ProductID");
                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("(getdate())")  // Valor por defecto para CreatedAt
                    .HasColumnType("datetime");
                entity.Property(e => e.Description).HasColumnType("text");
                entity.Property(e => e.Name)
                    .HasMaxLength(100)  // Longitud máxima para el nombre del producto
                    .IsUnicode(false);  // No usar Unicode
                entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");  // Precio con formato decimal
                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                // Relación entre 'Product' y 'Category' (muchos productos pueden pertenecer a una categoría)
                entity.HasOne(d => d.Category).WithMany(p => p.Products)
                    .HasForeignKey(d => d.CategoryId)  // Clave foránea que conecta con 'Category'
                    .HasConstraintName("FK__Products__Catego__4D94879B");
            });

            // Configuración de la tabla 'ProductHistory'
            modelBuilder.Entity<ProductHistory>(entity =>
            {
                entity.HasKey(e => e.HistoryId).HasName("PK__ProductH__4D7B4ADD133E04F4");  // Clave primaria

                entity.ToTable("ProductHistory");

                // Definición de propiedades para la tabla 'ProductHistory'
                entity.Property(e => e.HistoryId).HasColumnName("HistoryID");
                entity.Property(e => e.OperationDate)
                    .HasDefaultValueSql("(getdate())")  // Valor por defecto para la fecha de la operación
                    .HasColumnType("datetime");
                entity.Property(e => e.ProductId).HasColumnName("ProductID");
                entity.Property(e => e.Remarks).HasColumnType("text");
                entity.Property(e => e.Type)
                    .HasMaxLength(10)  // Tipo de operación (entrada/salida)
                    .IsUnicode(false);

                // Relación entre 'ProductHistory' y 'Product' (un producto puede tener múltiples entradas en el historial)
                entity.HasOne(d => d.Product).WithMany(p => p.ProductHistories)
                    .HasForeignKey(d => d.ProductId)  // Clave foránea que conecta con 'Product'
                    .HasConstraintName("FK__ProductHi__Produ__52593CB8");
            });

            // Configuración de la tabla 'Sale'
            modelBuilder.Entity<Sale>(entity =>
            {
                entity.HasKey(e => e.SaleId).HasName("PK__Sales__1EE3C41F6B51EFAC");  // Clave primaria

                // Definición de propiedades para la tabla 'Sale'
                entity.Property(e => e.SaleId).HasColumnName("SaleID");
                entity.Property(e => e.CustomerName)
                    .HasMaxLength(100)  // Longitud máxima para el nombre del cliente
                    .IsUnicode(false);
                entity.Property(e => e.ProductId).HasColumnName("ProductID");
                entity.Property(e => e.Remarks).HasColumnType("text");
                entity.Property(e => e.SaleDate)
                    .HasDefaultValueSql("(getdate())")  // Valor por defecto para la fecha de la venta
                    .HasColumnType("datetime");
                entity.Property(e => e.TotalPrice).HasColumnType("decimal(10, 2)");  // Precio total de la venta

                // Relación entre 'Sale' y 'Product' (un producto puede estar asociado a múltiples ventas)
                entity.HasOne(d => d.Product).WithMany(p => p.Sales)
                    .HasForeignKey(d => d.ProductId)  // Clave foránea que conecta con 'Product'
                    .HasConstraintName("FK__Sales__ProductID__5629CD9C");
            });

            OnModelCreatingPartial(modelBuilder);  // Método parcial para personalizaciones adicionales
        }

        // Método parcial que permite agregar configuraciones adicionales al modelo
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
