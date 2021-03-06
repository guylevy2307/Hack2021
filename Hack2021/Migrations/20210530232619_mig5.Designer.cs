// <auto-generated />
using System;
using Hack2021.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Hack2021.Migrations
{
    [DbContext(typeof(Hack2021Context))]
    [Migration("20210530232619_mig5")]
    partial class mig5
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.6")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Hack.Model.CreditCard", b =>
                {
                    b.Property<string>("Number")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CVV")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ExpirationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("FullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("email")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Number");

                    b.ToTable("CreditCard");
                });

            modelBuilder.Entity("Hack.Model.SplitItTransaction", b =>
                {
                    b.Property<string>("TransactionID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("NumPayments")
                        .HasColumnType("int");

                    b.Property<double>("TotalAmount")
                        .HasColumnType("float");

                    b.HasKey("TransactionID");

                    b.ToTable("SplitItTransaction");
                });

            modelBuilder.Entity("Hack.Model.SplitItTransaction+Payment", b =>
                {
                    b.Property<string>("TransactionID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<double>("Amount")
                        .HasColumnType("float");

                    b.Property<DateTime>("DueDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("SplitItTransactionTransactionID")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("TransactionID");

                    b.HasIndex("SplitItTransactionTransactionID");

                    b.ToTable("Payment");
                });

            modelBuilder.Entity("Hack.Model.Transaction", b =>
                {
                    b.Property<int>("TransactionID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("Amount")
                        .HasColumnType("float");

                    b.Property<string>("CardNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreditCardInfoNumber")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<DateTime>("TransactionDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("mId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TransactionID");

                    b.HasIndex("CreditCardInfoNumber");

                    b.ToTable("Transaction");
                });

            modelBuilder.Entity("Hack.Model.SplitItTransaction+Payment", b =>
                {
                    b.HasOne("Hack.Model.SplitItTransaction", null)
                        .WithMany("Payments")
                        .HasForeignKey("SplitItTransactionTransactionID");
                });

            modelBuilder.Entity("Hack.Model.Transaction", b =>
                {
                    b.HasOne("Hack.Model.CreditCard", "CreditCardInfo")
                        .WithMany()
                        .HasForeignKey("CreditCardInfoNumber");

                    b.Navigation("CreditCardInfo");
                });

            modelBuilder.Entity("Hack.Model.SplitItTransaction", b =>
                {
                    b.Navigation("Payments");
                });
#pragma warning restore 612, 618
        }
    }
}
