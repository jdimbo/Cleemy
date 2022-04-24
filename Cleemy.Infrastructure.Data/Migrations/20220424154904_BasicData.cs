using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cleemy.Infrastructure.Data.Migrations
{
    public partial class BasicData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
          table: "Currency",
          columns: new[] { "Id", "Symbol", "ISOCode", "Name" },
          values: new object[,]
          {
                    { 1, "$", "USD", "Dollar américain" },
                    { 2, "€", "EUR", "Euro" },
                    { 3, "₽", "RUB", "Rouble russe" }
          });

            migrationBuilder.InsertData(
              table: "User",
              columns: new[] { "Id", "LastName", "FirstName", "CurrencyId" },
              values: new object[,]
              {
                    { 1, "Stark", "Anthony", 1 },
                    { 2, "Romanova", "Natasha", 3 }
              });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
