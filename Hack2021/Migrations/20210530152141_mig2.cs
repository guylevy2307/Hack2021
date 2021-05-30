using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Hack2021.Migrations
{
    public partial class mig2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SplitItTransaction",
                columns: table => new
                {
                    TransactionID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TotalAmount = table.Column<double>(type: "float", nullable: false),
                    NumPayments = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SplitItTransaction", x => x.TransactionID);
                });

            migrationBuilder.CreateTable(
                name: "Payment",
                columns: table => new
                {
                    TransactionID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    SplitItTransactionTransactionID = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payment", x => x.TransactionID);
                    table.ForeignKey(
                        name: "FK_Payment_SplitItTransaction_SplitItTransactionTransactionID",
                        column: x => x.SplitItTransactionTransactionID,
                        principalTable: "SplitItTransaction",
                        principalColumn: "TransactionID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Payment_SplitItTransactionTransactionID",
                table: "Payment",
                column: "SplitItTransactionTransactionID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Payment");

            migrationBuilder.DropTable(
                name: "SplitItTransaction");
        }
    }
}
