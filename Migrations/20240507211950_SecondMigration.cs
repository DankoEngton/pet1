using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication2.Migrations
{
    public partial class SecondMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrdersDrugs_Sales_SaleId",
                table: "OrdersDrugs");

            migrationBuilder.DropIndex(
                name: "IX_OrdersDrugs_SaleId",
                table: "OrdersDrugs");

            migrationBuilder.DropColumn(
                name: "SaleId",
                table: "OrdersDrugs");

            migrationBuilder.CreateTable(
                name: "SalesDrugs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DrugId = table.Column<int>(type: "int", nullable: false),
                    SaleId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesDrugs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalesDrugs_Drugs_DrugId",
                        column: x => x.DrugId,
                        principalTable: "Drugs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SalesDrugs_Sales_SaleId",
                        column: x => x.SaleId,
                        principalTable: "Sales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SalesDrugs_DrugId",
                table: "SalesDrugs",
                column: "DrugId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesDrugs_SaleId",
                table: "SalesDrugs",
                column: "SaleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SalesDrugs");

            migrationBuilder.AddColumn<int>(
                name: "SaleId",
                table: "OrdersDrugs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrdersDrugs_SaleId",
                table: "OrdersDrugs",
                column: "SaleId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrdersDrugs_Sales_SaleId",
                table: "OrdersDrugs",
                column: "SaleId",
                principalTable: "Sales",
                principalColumn: "Id");
        }
    }
}
