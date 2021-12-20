using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CallCenter.DBModel
{
    public partial class FactuMexContext : DbContext
    {
        public FactuMexContext()
        {
        }

        public FactuMexContext(DbContextOptions<FactuMexContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Abono> Abono { get; set; }
        public virtual DbSet<Anexos> Anexos { get; set; }
        public virtual DbSet<Bitacora> Bitacora { get; set; }
        public virtual DbSet<ColaImpresion> ColaImpresion { get; set; }
        public virtual DbSet<Comprobante> Comprobante { get; set; }
        public virtual DbSet<DesgloseFactura> DesgloseFactura { get; set; }
        public virtual DbSet<DirxEnte> DirxEnte { get; set; }
        public virtual DbSet<Ente> Ente { get; set; }
        public virtual DbSet<Factura> Factura { get; set; }
        public virtual DbSet<FacxDesVal> FacxDesVal { get; set; }
        public virtual DbSet<FacxVal> FacxVal { get; set; }
        public virtual DbSet<Incompleto> Incompleto { get; set; }
        public virtual DbSet<Item> Item { get; set; }
        public virtual DbSet<LstPermiso> LstPermiso { get; set; }
        public virtual DbSet<Moneda> Moneda { get; set; }
        public virtual DbSet<Municipio> Municipio { get; set; }
        public virtual DbSet<Ncf100> Ncf100 { get; set; }
        public virtual DbSet<Ncf3> Ncf3 { get; set; }
        public virtual DbSet<NcfXxxxxx> NcfXxxxxx { get; set; }
        public virtual DbSet<PermisosxUsuario> PermisosxUsuario { get; set; }
        public virtual DbSet<PrecioxItem> PrecioxItem { get; set; }
        public virtual DbSet<Provincia> Provincia { get; set; }
        public virtual DbSet<RefxFac> RefxFac { get; set; }
        public virtual DbSet<RenoUsuario> RenoUsuario { get; set; }
        public virtual DbSet<Rncempresas> Rncempresas { get; set; }
        public virtual DbSet<Sector> Sector { get; set; }
        public virtual DbSet<Sesion> Sesion { get; set; }
        public virtual DbSet<SlaxItem> SlaxItem { get; set; }
        public virtual DbSet<TasaUs> TasaUs { get; set; }
        public virtual DbSet<Valija> Valija { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=domex-test;Initial Catalog=FactuMex;Persist Security Info=True;User ID=sa;Password=Sila.1009");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<Abono>(entity =>
            {
                entity.HasKey(e => e.Idabono);

                entity.Property(e => e.Idabono).HasColumnName("IDAbono");

                entity.Property(e => e.Idfactura).HasColumnName("IDFactura");
            });

            modelBuilder.Entity<Anexos>(entity =>
            {
                entity.HasKey(e => e.Idanexo);

                entity.Property(e => e.Idanexo).HasColumnName("IDAnexo");

                entity.Property(e => e.Nombre).HasMaxLength(250);
            });

            modelBuilder.Entity<Bitacora>(entity =>
            {
                entity.HasKey(e => e.Idbitacora)
                    .HasName("PK_Bitacoras");

                entity.Property(e => e.Idbitacora).HasColumnName("IDBitacora");

                entity.Property(e => e.Accion).HasMaxLength(50);

                entity.Property(e => e.Entidad).HasMaxLength(50);

                entity.Property(e => e.Fecha).HasColumnType("datetime");

                entity.Property(e => e.Pk).HasColumnName("PK");
            });

            modelBuilder.Entity<ColaImpresion>(entity =>
            {
                entity.HasKey(e => e.Idprint);

                entity.Property(e => e.Idprint).HasColumnName("IDPrint");

                entity.Property(e => e.Fhcola)
                    .HasColumnName("FHCola")
                    .HasColumnType("datetime");

                entity.Property(e => e.Fhimpresion)
                    .HasColumnName("FHImpresion")
                    .HasColumnType("datetime");

                entity.Property(e => e.Idfac).HasColumnName("IDFac");

                entity.Property(e => e.Ubicacion).HasMaxLength(50);
            });

            modelBuilder.Entity<Comprobante>(entity =>
            {
                entity.HasKey(e => e.Idcomp);

                entity.Property(e => e.Idcomp).HasColumnName("IDComp");

                entity.Property(e => e.Empresa).HasMaxLength(200);

                entity.Property(e => e.EstatDesde).HasMaxLength(50);

                entity.Property(e => e.EstatHasta).HasMaxLength(50);

                entity.Property(e => e.Rnc)
                    .HasColumnName("RNC")
                    .HasMaxLength(20);

                entity.Property(e => e.TipoComp).HasMaxLength(20);

                entity.Property(e => e.Venc).HasColumnType("date");
            });

            modelBuilder.Entity<DesgloseFactura>(entity =>
            {
                entity.HasKey(e => e.Iddesglose);

                entity.Property(e => e.Iddesglose).HasColumnName("IDDesglose");

                entity.Property(e => e.Concepto).HasMaxLength(255);

                entity.Property(e => e.Idfactura).HasColumnName("IDFactura");

                entity.Property(e => e.Idprovincia).HasColumnName("IDProvincia");

                entity.Property(e => e.Moneda).HasMaxLength(10);

                entity.Property(e => e.Producto).HasMaxLength(255);
            });

            modelBuilder.Entity<DirxEnte>(entity =>
            {
                entity.HasKey(e => e.Iddireccion);

                entity.Property(e => e.Iddireccion).HasColumnName("IDDireccion");

                entity.Property(e => e.Apto).HasMaxLength(200);

                entity.Property(e => e.Calle).HasMaxLength(200);

                entity.Property(e => e.Correo).HasMaxLength(200);

                entity.Property(e => e.Edificio).HasMaxLength(200);

                entity.Property(e => e.Idente).HasColumnName("IDEnte");

                entity.Property(e => e.Idmunicipio).HasColumnName("IDMunicipio");

                entity.Property(e => e.Idpais).HasColumnName("IDPais");

                entity.Property(e => e.Idprovincia).HasColumnName("IDProvincia");

                entity.Property(e => e.Idsector).HasColumnName("IDSector");

                entity.Property(e => e.Municipio).HasMaxLength(200);

                entity.Property(e => e.Nro).HasMaxLength(200);

                entity.Property(e => e.Pais).HasMaxLength(200);

                entity.Property(e => e.Provincia).HasMaxLength(200);

                entity.Property(e => e.Residencial).HasMaxLength(200);

                entity.Property(e => e.Sector).HasMaxLength(200);

                entity.Property(e => e.TipoDir).HasMaxLength(10);
            });

            modelBuilder.Entity<Ente>(entity =>
            {
                entity.HasKey(e => e.Idente);

                entity.Property(e => e.Idente).HasColumnName("IDEnte");

                entity.Property(e => e.Correo).HasMaxLength(200);

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasMaxLength(50);

                entity.Property(e => e.Nombre).HasMaxLength(200);

                entity.Property(e => e.Tel1).HasMaxLength(20);

                entity.Property(e => e.Tel2).HasMaxLength(20);

                entity.Property(e => e.Tel3).HasMaxLength(20);

                entity.Property(e => e.TipoEnte).HasMaxLength(10);

                entity.Property(e => e.TipoId)
                    .HasColumnName("TipoID")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Factura>(entity =>
            {
                entity.HasKey(e => e.Idfactura);

                entity.Property(e => e.Idfactura).HasColumnName("IDFactura");

                entity.Property(e => e.Fecha).HasColumnType("datetime");

                entity.Property(e => e.FormaPago).HasMaxLength(20);

                entity.Property(e => e.Idcliente).HasColumnName("IDCliente");

                entity.Property(e => e.Idempresa).HasColumnName("IDEmpresa");

                entity.Property(e => e.MetodoPago).HasMaxLength(20);

                entity.Property(e => e.Ncf)
                    .HasColumnName("NCF")
                    .HasMaxLength(50);

                entity.Property(e => e.Tipo).HasMaxLength(255);
            });

            modelBuilder.Entity<FacxDesVal>(entity =>
            {
                entity.HasKey(e => e.IdfacxVal);

                entity.Property(e => e.IdfacxVal).HasColumnName("IDFacxVal");

                entity.Property(e => e.Fec).HasColumnType("datetime");

                entity.Property(e => e.Idfac).HasColumnName("IDFac");

                entity.Property(e => e.Idval).HasColumnName("IDVal");
            });

            modelBuilder.Entity<FacxVal>(entity =>
            {
                entity.HasKey(e => e.IdfacxVal);

                entity.Property(e => e.IdfacxVal).HasColumnName("IDFacxVal");

                entity.Property(e => e.Fec).HasColumnType("datetime");

                entity.Property(e => e.Idfac).HasColumnName("IDFac");

                entity.Property(e => e.Idval).HasColumnName("IDVal");
            });

            modelBuilder.Entity<Incompleto>(entity =>
            {
                entity.HasKey(e => e.Idinc);

                entity.Property(e => e.Idinc).HasColumnName("IDInc");

                entity.Property(e => e.FecDsp).HasColumnType("datetime");

                entity.Property(e => e.FecRec).HasColumnType("datetime");

                entity.Property(e => e.Idfac).HasColumnName("IDFac");

                entity.Property(e => e.Idval).HasColumnName("IDVal");

                entity.Property(e => e.Relacionado).HasMaxLength(250);
            });

            modelBuilder.Entity<Item>(entity =>
            {
                entity.HasKey(e => e.Iditem);

                entity.Property(e => e.Iditem).HasColumnName("IDItem");

                entity.Property(e => e.Descripcion).HasMaxLength(250);

                entity.Property(e => e.Nombre).HasMaxLength(100);
            });

            modelBuilder.Entity<LstPermiso>(entity =>
            {
                entity.HasKey(e => e.Idpermiso);

                entity.Property(e => e.Idpermiso).HasColumnName("IDPermiso");

                entity.Property(e => e.Grupo).HasMaxLength(100);

                entity.Property(e => e.Permiso).HasMaxLength(100);
            });

            modelBuilder.Entity<Moneda>(entity =>
            {
                entity.HasKey(e => e.Idmoneda);

                entity.Property(e => e.Idmoneda).HasColumnName("IDMoneda");

                entity.Property(e => e.Moneda1)
                    .HasColumnName("Moneda")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Municipio>(entity =>
            {
                entity.HasKey(e => e.Idmunicipio);

                entity.Property(e => e.Idmunicipio).HasColumnName("IDMunicipio");

                entity.Property(e => e.Cp)
                    .HasColumnName("CP")
                    .HasMaxLength(255);

                entity.Property(e => e.Idprovincia).HasColumnName("IDProvincia");

                entity.Property(e => e.Municipio1)
                    .HasColumnName("Municipio")
                    .HasMaxLength(255);

                entity.Property(e => e.Zona).HasMaxLength(255);
            });

            modelBuilder.Entity<Ncf100>(entity =>
            {
                entity.ToTable("NCF100");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Idfactura).HasColumnName("IDFactura");

                entity.Property(e => e.Secuencial).HasMaxLength(50);
            });

            modelBuilder.Entity<Ncf3>(entity =>
            {
                entity.ToTable("NCF3");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Idfactura).HasColumnName("IDFactura");

                entity.Property(e => e.Secuencial).HasMaxLength(50);
            });

            modelBuilder.Entity<NcfXxxxxx>(entity =>
            {
                entity.ToTable("NCF-XXXXXX");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Idfactura).HasColumnName("IDFactura");

                entity.Property(e => e.Secuencial).HasMaxLength(50);
            });

            modelBuilder.Entity<PermisosxUsuario>(entity =>
            {
                entity.HasKey(e => e.IdpermisoxUsuario)
                    .HasName("PK_PermisosxUsuarios");

                entity.Property(e => e.IdpermisoxUsuario).HasColumnName("IDPermisoxUsuario");

                entity.Property(e => e.Idusuario).HasColumnName("IDUsuario");

                entity.Property(e => e.Permiso).HasMaxLength(50);
            });

            modelBuilder.Entity<PrecioxItem>(entity =>
            {
                entity.HasKey(e => e.Idprecio);

                entity.Property(e => e.Idprecio).HasColumnName("IDPrecio");

                entity.Property(e => e.Iditem).HasColumnName("IDItem");

                entity.Property(e => e.Idprovincia).HasColumnName("IDProvincia");

                entity.Property(e => e.Moneda).HasMaxLength(10);
            });

            modelBuilder.Entity<Provincia>(entity =>
            {
                entity.HasKey(e => e.Idprovincia);

                entity.Property(e => e.Idprovincia).HasColumnName("IDProvincia");

                entity.Property(e => e.Cp).HasColumnName("CP");

                entity.Property(e => e.Provincia1)
                    .HasColumnName("Provincia")
                    .HasMaxLength(255);

                entity.Property(e => e.Zona).HasMaxLength(255);
            });

            modelBuilder.Entity<RefxFac>(entity =>
            {
                entity.HasKey(e => e.Idref);

                entity.Property(e => e.Idref).HasColumnName("IDRef");

                entity.Property(e => e.Idfac).HasColumnName("IDFac");

                entity.Property(e => e.Ref).HasMaxLength(250);

                entity.Property(e => e.TipoRef).HasMaxLength(50);
            });

            modelBuilder.Entity<RenoUsuario>(entity =>
            {
                entity.HasKey(e => e.Idusuario);

                entity.Property(e => e.Idusuario).HasColumnName("IDUsuario");

                entity.Property(e => e.Cedula).HasMaxLength(15);

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.Foto).HasMaxLength(100);

                entity.Property(e => e.Nombre).HasMaxLength(250);

                entity.Property(e => e.Pwd)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Telefono).HasMaxLength(10);
            });

            modelBuilder.Entity<Rncempresas>(entity =>
            {
                entity.HasKey(e => e.Rnc);

                entity.ToTable("RNCEmpresas");

                entity.Property(e => e.Rnc)
                    .HasColumnName("RNC")
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.ActividadEconomica)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Constitucion)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Dir1)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Dir2)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Dir3)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Estatus)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre1)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre2)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Regimen)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Tel)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Sector>(entity =>
            {
                entity.HasKey(e => e.Idlocalidad);

                entity.Property(e => e.Idlocalidad).HasColumnName("IDLocalidad");

                entity.Property(e => e.Cp).HasColumnName("CP");

                entity.Property(e => e.Idmunicipio).HasColumnName("IDMunicipio");

                entity.Property(e => e.Localidad).HasMaxLength(255);
            });

            modelBuilder.Entity<Sesion>(entity =>
            {
                entity.HasKey(e => e.Idsesion);

                entity.Property(e => e.Idsesion).HasColumnName("IDSesion");

                entity.Property(e => e.Bfp)
                    .HasColumnName("BFP")
                    .HasMaxLength(1000);

                entity.Property(e => e.CodSesion).HasMaxLength(50);

                entity.Property(e => e.Expira).HasColumnType("datetime");

                entity.Property(e => e.Idusuario).HasColumnName("IDUsuario");
            });

            modelBuilder.Entity<SlaxItem>(entity =>
            {
                entity.HasKey(e => e.Idsla);

                entity.ToTable("SLAxItem");

                entity.Property(e => e.Idsla).HasColumnName("IDSLA");

                entity.Property(e => e.Correo).HasMaxLength(250);

                entity.Property(e => e.Iditem).HasColumnName("IDItem");

                entity.Property(e => e.Idprovincia).HasColumnName("IDProvincia");

                entity.Property(e => e.TipoSla)
                    .HasColumnName("TipoSLA")
                    .HasMaxLength(10);

                entity.Property(e => e.Und).HasMaxLength(10);
            });

            modelBuilder.Entity<TasaUs>(entity =>
            {
                entity.HasKey(e => e.Idtasa);

                entity.ToTable("TasaUS");

                entity.Property(e => e.Idtasa).HasColumnName("IDTasa");

                entity.Property(e => e.Fecha).HasColumnType("datetime");
            });

            modelBuilder.Entity<Valija>(entity =>
            {
                entity.HasKey(e => e.Idvalija);

                entity.Property(e => e.Idvalija).HasColumnName("IDValija");

                entity.Property(e => e.Cb)
                    .HasColumnName("CB")
                    .HasMaxLength(250);

                entity.Property(e => e.Cs)
                    .HasColumnName("CS")
                    .HasMaxLength(250);

                entity.Property(e => e.Etiqueta).HasMaxLength(50);

                entity.Property(e => e.Fecha).HasColumnType("datetime");

                entity.Property(e => e.Usuario).HasMaxLength(250);
            });
        }
    }
}
