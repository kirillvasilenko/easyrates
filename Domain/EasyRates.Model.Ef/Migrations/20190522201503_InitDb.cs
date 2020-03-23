using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace EasyRates.Model.Ef.Migrations
{
    public partial class InitDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CurrencyRate",
                columns: table => new
                {
                    From = table.Column<string>(nullable: false),
                    To = table.Column<string>(nullable: false),
                    Value = table.Column<decimal>(nullable: false),
                    TimeOfReceipt = table.Column<DateTime>(nullable: false),
                    ExpirationTime = table.Column<DateTime>(nullable: false),
                    OriginalPublishedTime = table.Column<DateTime>(nullable: false),
                    ProviderName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyRate", x => new { x.From, x.To });
                });

            migrationBuilder.CreateTable(
                name: "CurrencyRateHistoryItem",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    From = table.Column<string>(nullable: true),
                    To = table.Column<string>(nullable: true),
                    Value = table.Column<decimal>(nullable: false),
                    ExpirationTime = table.Column<DateTime>(nullable: false),
                    OriginalPublishedTime = table.Column<DateTime>(nullable: false),
                    TimeOfReceipt = table.Column<DateTime>(nullable: false),
                    ProviderName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyRateHistoryItem", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CurrencyRate");

            migrationBuilder.DropTable(
                name: "CurrencyRateHistoryItem");
        }
    }
}
