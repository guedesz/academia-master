using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace academia.Migrations
{
    public partial class academia3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "nomePlano",
                table: "Assinatura",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "nomePlano",
                table: "Assinatura");
        }
    }
}
