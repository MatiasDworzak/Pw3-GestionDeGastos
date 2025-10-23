using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace GestionDeGastos;

public partial class GestionDeGastosBdContext : DbContext
{
    public GestionDeGastosBdContext()
    {
    }

    public GestionDeGastosBdContext(DbContextOptions<GestionDeGastosBdContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CategoriaUsuario> CategoriaUsuarios { get; set; }

    public virtual DbSet<Categorium> Categoria { get; set; }

    public virtual DbSet<Gasto> Gastos { get; set; }

    public virtual DbSet<Item> Items { get; set; }

    public virtual DbSet<MetodoDePago> MetodoDePagos { get; set; }

    public virtual DbSet<Presupuesto> Presupuestos { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-DD6PAVS\\SQLEXPRESS;Database=GestionDeGastosBD;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CategoriaUsuario>(entity =>
        {
            entity.HasKey(e => e.IdCategoria).HasName("PK__Categori__CD54BC5A4BA3EAFA");

            entity.ToTable("CategoriaUsuario");

            entity.Property(e => e.IdCategoria)
                .ValueGeneratedOnAdd()
                .HasColumnName("id_categoria");
            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");

            entity.HasOne(d => d.IdCategoriaNavigation).WithOne(p => p.CategoriaUsuario)
                .HasForeignKey<CategoriaUsuario>(d => d.IdCategoria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CategoriaUsuario_Categoria");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.CategoriaUsuarios)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CategoriaUsuario_Usuario");
        });

        modelBuilder.Entity<Categorium>(entity =>
        {
            entity.HasKey(e => e.IdCategoria).HasName("PK__Categori__CD54BC5A927E52E1");

            entity.Property(e => e.IdCategoria).HasColumnName("id_categoria");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("descripcion");
        });

        modelBuilder.Entity<Gasto>(entity =>
        {
            entity.HasKey(e => e.IdGasto).HasName("PK__Gasto__ECB8FB80E823BA4F");

            entity.ToTable("Gasto");

            entity.Property(e => e.IdGasto).HasColumnName("id_gasto");
            entity.Property(e => e.Fecha).HasColumnName("fecha");
            entity.Property(e => e.IdCategoria).HasColumnName("id_categoria");
            entity.Property(e => e.IdMetodoPago).HasColumnName("id_metodo_pago");
            entity.Property(e => e.IdTicket)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("id_ticket");
            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.MontoTotal)
                .HasColumnType("decimal(15, 2)")
                .HasColumnName("monto_total");
            entity.Property(e => e.Nombre)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("nombre");

            entity.HasOne(d => d.IdCategoriaNavigation).WithMany(p => p.Gastos)
                .HasForeignKey(d => d.IdCategoria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Gasto_Categoria");

            entity.HasOne(d => d.IdMetodoPagoNavigation).WithMany(p => p.Gastos)
                .HasForeignKey(d => d.IdMetodoPago)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Gasto_MetodoDePago");

            entity.HasOne(d => d.IdTicketNavigation).WithMany(p => p.Gastos)
                .HasForeignKey(d => d.IdTicket)
                .HasConstraintName("FK_Gasto_Ticket");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Gastos)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Gasto_Usuario");
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.IdItem).HasName("PK__Item__87C9438BC755011A");

            entity.ToTable("Item");

            entity.Property(e => e.IdItem).HasColumnName("id_item");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.IdTicket).HasColumnName("id_ticket");
            entity.Property(e => e.PrecioTotal)
                .HasColumnType("decimal(15, 2)")
                .HasColumnName("precio_total");
            entity.Property(e => e.PrecioUnitario)
                .HasColumnType("decimal(15, 2)")
                .HasColumnName("precio_unitario");

            entity.HasOne(d => d.IdTicketNavigation).WithMany(p => p.Items)
                .HasForeignKey(d => d.IdTicket)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Item_Ticket");
        });

        modelBuilder.Entity<MetodoDePago>(entity =>
        {
            entity.HasKey(e => e.IdMetodoPago).HasName("PK__MetodoDe__85BE0EBC92F5F3A2");

            entity.ToTable("MetodoDePago");

            entity.Property(e => e.IdMetodoPago).HasColumnName("id_metodo_pago");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("descripcion");
        });

        modelBuilder.Entity<Presupuesto>(entity =>
        {
            entity.HasKey(e => e.IdPresupuesto).HasName("PK__Presupue__3E94B4E5A4DB359A");

            entity.ToTable("Presupuesto");

            entity.Property(e => e.IdPresupuesto).HasColumnName("id_presupuesto");
            entity.Property(e => e.Anio).HasColumnName("anio");
            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.Mes).HasColumnName("mes");
            entity.Property(e => e.MontoActualGastado)
                .HasColumnType("decimal(15, 2)")
                .HasColumnName("monto_actual_gastado");
            entity.Property(e => e.MontoLimite)
                .HasColumnType("decimal(15, 2)")
                .HasColumnName("monto_limite");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Presupuestos)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Presupuesto_Usuario");
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.IdTicket).HasName("PK__Ticket__48C6F523E84E7851");

            entity.ToTable("Ticket");

            entity.Property(e => e.IdTicket).HasColumnName("id_ticket");
            entity.Property(e => e.RutaImagenBlob)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("ruta_imagen_blob");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__Usuario__4E3E04AD431D445A");

            entity.ToTable("Usuario");

            entity.HasIndex(e => e.Email, "UQ__Usuario__AB6E6164084152C4").IsUnique();

            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.Contrasenia)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("contrasenia");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
