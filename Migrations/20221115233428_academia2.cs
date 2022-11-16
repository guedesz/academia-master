using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace academia.Migrations
{
    public partial class academia2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "idplano",
                table: "Assinatura",
                newName: "idPlano");

            migrationBuilder.RenameColumn(
                name: "idcliente",
                table: "Assinatura",
                newName: "idCliente");

            migrationBuilder.RenameColumn(
                name: "tipoPlano",
                table: "Assinatura",
                newName: "nomeCliente");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "idPlano",
                table: "Assinatura",
                newName: "idplano");

            migrationBuilder.RenameColumn(
                name: "idCliente",
                table: "Assinatura",
                newName: "idcliente");

            migrationBuilder.RenameColumn(
                name: "nomeCliente",
                table: "Assinatura",
                newName: "tipoPlano");
        }
    }
}
