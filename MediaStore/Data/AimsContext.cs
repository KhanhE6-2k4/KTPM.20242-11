using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MediaStore.Data
{
    public partial class AimsContext : DbContext
    {
        public AimsContext()
        {
        }

        public AimsContext(DbContextOptions<AimsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Book> Books { get; set; }

        public virtual DbSet<CdAndLp> CdAndLps { get; set; }

        public virtual DbSet<DeliveryInfo> DeliveryInfos { get; set; }

        public virtual DbSet<Dvd> Dvds { get; set; }

        public virtual DbSet<Invoice> Invoices { get; set; }

        public virtual DbSet<Media> Media { get; set; }

        public virtual DbSet<OrderInfo> OrderInfos { get; set; }

        public virtual DbSet<OrderMedia> OrderMedia { get; set; }

        public virtual DbSet<PaymentTransaction> PaymentTransactions { get; set; }

        public virtual DbSet<RushOrderInfo> RushOrderInfos { get; set; }

        public virtual DbSet<User> Users { get; set; }

        //    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        //        => optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=AIMS;User ID=sa;Password=bmthhkttv;Trust Server Certificate=True");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>(entity =>
            {
                // entity.HasKey(e => e.MediaId).HasName("PK__Book__D0A840F46E110C35");

                entity.ToTable("Book");

                entity.Property(e => e.MediaId)
                    .ValueGeneratedNever()
                    .HasColumnName("media_id");
                entity.Property(e => e.Authors)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("authors");
                entity.Property(e => e.CoverType)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("coverType");
                entity.Property(e => e.Genre)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("genre");
                entity.Property(e => e.Language)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("language");
                entity.Property(e => e.Pages).HasColumnName("pages");
                entity.Property(e => e.PublicationDate).HasColumnName("publicationDate");
                entity.Property(e => e.Publisher)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("publisher");

                entity.HasOne(d => d.Media).WithOne(p => p.Book)
                    .HasForeignKey<Book>(d => d.MediaId)
                    .HasConstraintName("FK__Book__media_id__3B75D760");
            });

            modelBuilder.Entity<CdAndLp>(entity =>
            {
                // entity.HasKey(e => e.MediaId).HasName("PK__CD_and_L__D0A840F4981B42E8");

                entity.ToTable("CD_and_LP");

                entity.Property(e => e.MediaId)
                    .ValueGeneratedNever()
                    .HasColumnName("media_id");
                entity.Property(e => e.Artists)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("artists");
                entity.Property(e => e.Genre)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("genre");
                entity.Property(e => e.IsCd).HasColumnName("isCD");
                entity.Property(e => e.RecordLabel)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("recordLabel");
                entity.Property(e => e.ReleaseDate).HasColumnName("releaseDate");
                entity.Property(e => e.TrackList)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("trackList");

                entity.HasOne(d => d.Media).WithOne(p => p.CdAndLp)
                    .HasForeignKey<CdAndLp>(d => d.MediaId)
                    .HasConstraintName("FK__CD_and_LP__media__3E52440B");
            });

            modelBuilder.Entity<DeliveryInfo>(entity =>
            {
                entity.HasKey(e => e.DeliveryId).HasName("PK__Delivery__1C5CF4F51843C7D2");

                entity.ToTable("DeliveryInfo");

                entity.Property(e => e.DeliveryId).HasColumnName("delivery_id");
                entity.Property(e => e.Address)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("address");
                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("email");
                entity.Property(e => e.Message)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("message");
                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("name");
                entity.Property(e => e.Phone)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("phone");
                entity.Property(e => e.Province)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("province");
            });

            modelBuilder.Entity<Dvd>(entity =>
            {
                // entity.HasKey(e => e.MediaId).HasName("PK__DVD__D0A840F4B4B866A8");

                entity.ToTable("DVD");

                entity.Property(e => e.MediaId)
                    .ValueGeneratedNever()
                    .HasColumnName("media_id");
                entity.Property(e => e.Director)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("director");
                entity.Property(e => e.DvdType)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("dvdType");
                entity.Property(e => e.Genre)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("genre");
                entity.Property(e => e.Language)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("language");
                entity.Property(e => e.ReleasedDate).HasColumnName("releasedDate");
                entity.Property(e => e.Runtime).HasColumnName("runtime");
                entity.Property(e => e.Studio)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("studio");
                entity.Property(e => e.Subtitles)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("subtitles");

                entity.HasOne(d => d.Media).WithOne(p => p.Dvd)
                    .HasForeignKey<Dvd>(d => d.MediaId)
                    .HasConstraintName("FK__DVD__media_id__412EB0B6");
            });

            modelBuilder.Entity<Invoice>(entity =>
            {
                entity.HasKey(e => e.InvoiceId).HasName("PK__Invoice__F58DFD49678E89FA");

                entity.ToTable("Invoice");

                entity.HasIndex(e => e.OrderId, "invoice_order_id_index");

                entity.HasIndex(e => e.TransactionId, "invoice_transaction_id_index");

                entity.Property(e => e.InvoiceId).HasColumnName("invoice_id");
                entity.Property(e => e.OrderId).HasColumnName("order_id");
                entity.Property(e => e.TotalAmount).HasColumnName("totalAmount");
                entity.Property(e => e.TransactionId).HasColumnName("transaction_id");

                entity.HasOne(d => d.Order).WithMany(p => p.Invoices)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK__Invoice__order_i__52593CB8");

                entity.HasOne(d => d.Transaction).WithMany(p => p.Invoices)
                    .HasForeignKey(d => d.TransactionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Invoice__transac__5165187F");
            });

            modelBuilder.Entity<Media>(entity =>
            {
                entity.HasKey(e => e.MediaId); // .HasName("PK__Media__D0A840F4B2BCA4F4"); // Giữ lại tên PK nếu bạn muốn khớp với script

                entity.Property(e => e.MediaId)
                    .HasColumnName("media_id")
                    .ValueGeneratedOnAdd(); // << --- THÊM DÒNG NÀY VÀO ĐỂ SỬA LỖI

                // Các cấu hình khác cho thuộc tính của Media giữ nguyên
                entity.Property(e => e.Title)
                    .HasMaxLength(50) // Kiểm tra lại độ dài này với ViewModel và script.sql
                    .IsUnicode(false)
                    .HasColumnName("title");
                entity.Property(e => e.Price).HasColumnName("price");
                entity.Property(e => e.TotalQuantity).HasColumnName("totalQuantity");
                entity.Property(e => e.Weight).HasColumnName("weight");
                entity.Property(e => e.RushOrderSupported).HasColumnName("rushOrderSupported");
                entity.Property(e => e.ImageUrl)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("imageUrl");
                entity.Property(e => e.Barcode)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("barcode");
                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("description");
                entity.Property(e => e.ProductDimension)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("productDimension");
                entity.Property(e => e.ImportDate).HasColumnName("importDate");

                // Các mối quan hệ một-một với các bảng con (Book, Dvd, CdAndLp)
                // EF Core thường tự phát hiện các mối quan hệ này dựa trên navigation properties
                // và khóa ngoại đã được định nghĩa trong các thực thể con.
                // Nếu bạn muốn tường minh, bạn có thể thêm:
                // entity.HasOne(p => p.Book).WithOne(b => b.Media).HasForeignKey<Book>(b => b.MediaId);
                // entity.HasOne(p => p.Dvd).WithOne(d => d.Media).HasForeignKey<Dvd>(d => d.MediaId);
                // entity.HasOne(p => p.CdAndLp).WithOne(c => c.Media).HasForeignKey<CdAndLp>(c => c.MediaId);
            });

            modelBuilder.Entity<OrderInfo>(entity =>
            {
                entity.HasKey(e => e.OrderId).HasName("PK__OrderInf__4659622974660CAF");

                entity.ToTable("OrderInfo");

                entity.HasIndex(e => e.DeliveryId, "delivery_id_index");

                entity.Property(e => e.OrderId).HasColumnName("order_id");
                entity.Property(e => e.DeliveryId).HasColumnName("delivery_id");
                entity.Property(e => e.ShippingFees).HasColumnName("shippingFees");
                entity.Property(e => e.Status)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("status");
                entity.Property(e => e.Subtotal).HasColumnName("subtotal");

                entity.HasOne(d => d.Delivery).WithMany(p => p.OrderInfos)
                    .HasForeignKey(d => d.DeliveryId)
                    .HasConstraintName("FK__OrderInfo__deliv__45F365D3");
            });

            modelBuilder.Entity<OrderMedia>(entity =>
            {
                entity.HasKey(e => new { e.MediaId, e.OrderId }).HasName("PK__Order_Me__54CDD6D66232FC4D");

                entity.ToTable("Order_Media");

                entity.Property(e => e.MediaId).HasColumnName("media_id");
                entity.Property(e => e.OrderId).HasColumnName("order_id");
                entity.Property(e => e.OrderType).HasColumnName("orderType");
                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.HasOne(d => d.Media).WithMany(p => p.OrderMedia)
                    .HasForeignKey(d => d.MediaId)
                    .HasConstraintName("FK__Order_Med__media__48CFD27E");

                entity.HasOne(d => d.Order).WithMany(p => p.OrderMedia)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK__Order_Med__order__49C3F6B7");
            });

            modelBuilder.Entity<PaymentTransaction>(entity =>
            {
                entity.HasKey(e => e.TransactionId).HasName("PK__PaymentT__85C600AFA86E167F");

                entity.ToTable("PaymentTransaction");

                entity.Property(e => e.TransactionId).HasColumnName("transaction_id");
                entity.Property(e => e.BankTransactionId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("bankTransactionId");
                entity.Property(e => e.CardType)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("cardType");
                entity.Property(e => e.Content)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("content");
                entity.Property(e => e.PaymentAmount).HasColumnName("paymentAmount");
                entity.Property(e => e.PaymentTime)
                    .HasColumnType("datetime")
                    .HasColumnName("paymentTime");
            });

            modelBuilder.Entity<RushOrderInfo>(entity =>
            {
                entity.HasKey(e => e.RushId).HasName("PK__RushOrde__DA913AC3DD5150B0");

                entity.ToTable("RushOrderInfo");

                entity.HasIndex(e => e.OrderId, "rush_order_id_index");

                entity.Property(e => e.RushId).HasColumnName("rush_id");
                entity.Property(e => e.DeliveryTime)
                    .HasColumnType("datetime")
                    .HasColumnName("deliveryTime");
                entity.Property(e => e.Instruction)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("instruction");
                entity.Property(e => e.OrderId).HasColumnName("order_id");

                entity.HasOne(d => d.Order).WithMany(p => p.RushOrderInfos)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK__RushOrder__order__4CA06362");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserId).HasName("PK__User__B9BE370F512152E5");

                entity.ToTable("User");

                entity.Property(e => e.UserId).HasColumnName("user_id");
                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("email");
                entity.Property(e => e.IsAdmin).HasColumnName("isAdmin");
                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("password");
                entity.Property(e => e.Username)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("username");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
