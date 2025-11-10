using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace GestionDeGastos.Entidades;

public partial class GestionDeGastosBdContext : DbContext
{
    public GestionDeGastosBdContext()
    {
    }

    public GestionDeGastosBdContext(DbContextOptions<GestionDeGastosBdContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Categorium> Categoria { get; set; }

    public virtual DbSet<Gasto> Gastos { get; set; }

    public virtual DbSet<Ingreso> Ingresos { get; set; }

    public virtual DbSet<Item> Items { get; set; }

    public virtual DbSet<MetodoDePago> MetodoDePagos { get; set; }

    public virtual DbSet<Presupuesto> Presupuestos { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categorium>(entity =>
        {
            entity.HasKey(e => e.IdCategoria).HasName("PK__Categori__CD54BC5A96CC5038");

            entity.Property(e => e.IdCategoria).HasColumnName("id_categoria");
            entity.Property(e => e.Color)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("color");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.Icono)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("icono");
        });

        modelBuilder.Entity<Gasto>(entity =>
        {
            entity.HasKey(e => e.IdGasto).HasName("PK__Gasto__ECB8FB80A87E781B");

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

        modelBuilder.Entity<Ingreso>(entity =>
        {
            entity.HasKey(e => e.IdIngreso).HasName("PK__Ingreso__8FF0F0DEC95F336A");

            entity.ToTable("Ingreso");

            entity.Property(e => e.IdIngreso).HasColumnName("id_ingreso");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.Fecha).HasColumnName("fecha");
            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.Monto)
                .HasColumnType("decimal(15, 2)")
                .HasColumnName("monto");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Ingresos)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ingreso_Usuario");
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.IdItem).HasName("PK__Item__87C9438BCAFA96E7");

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
            entity.HasKey(e => e.IdMetodoPago).HasName("PK__MetodoDe__85BE0EBC3C373F02");

            entity.ToTable("MetodoDePago");

            entity.Property(e => e.IdMetodoPago).HasColumnName("id_metodo_pago");
            entity.Property(e => e.Color)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("color");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.Icono)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("icono");
        });

        modelBuilder.Entity<Presupuesto>(entity =>
        {
            entity.HasKey(e => e.IdPresupuesto).HasName("PK__Presupue__3E94B4E5C55C5D2E");

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
            entity.HasKey(e => e.IdTicket).HasName("PK__Ticket__48C6F5239F1DF4A7");

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
            entity.HasKey(e => e.IdUsuario).HasName("PK__Usuario__4E3E04AD69D826CC");

            entity.ToTable("Usuario");

            entity.HasIndex(e => e.Email, "UQ__Usuario__AB6E6164AA9E6718").IsUnique();

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

            entity.HasMany(d => d.IdCategoria).WithMany(p => p.IdUsuarios)
                .UsingEntity<Dictionary<string, object>>(
                    "CategoriaUsuario",
                    r => r.HasOne<Categorium>().WithMany()
                        .HasForeignKey("IdCategoria")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_CategoriaUsuario_Categoria"),
                    l => l.HasOne<Usuario>().WithMany()
                        .HasForeignKey("IdUsuario")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_CategoriaUsuario_Usuario"),
                    j =>
                    {
                        j.HasKey("IdUsuario", "IdCategoria").HasName("PK__Categori__F2EB4F683B0FD06F");
                        j.ToTable("CategoriaUsuario");
                        j.IndexerProperty<int>("IdUsuario").HasColumnName("id_usuario");
                        j.IndexerProperty<int>("IdCategoria").HasColumnName("id_categoria");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
