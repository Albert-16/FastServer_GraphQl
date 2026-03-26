using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FastServer.Infrastructure.Data.Migrations.PostgreSqlMicroservices
{
    /// <inheritdoc />
    public partial class AddUserIdToMicroserviceRegister : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "fastserver_user_id",
                table: "FastServer_Microservice_Register",
                type: "uuid",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "FastServer_Microservice_Register",
                keyColumn: "fastserver_microservice_id",
                keyValue: new Guid("cc000000-0000-0000-0000-000000000001"),
                column: "fastserver_user_id",
                value: null);

            migrationBuilder.UpdateData(
                table: "FastServer_Microservice_Register",
                keyColumn: "fastserver_microservice_id",
                keyValue: new Guid("cc000000-0000-0000-0000-000000000002"),
                column: "fastserver_user_id",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_FastServer_Microservice_Register_UserId",
                table: "FastServer_Microservice_Register",
                column: "fastserver_user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_FastServer_Microservice_Register_FastServer_User_fastserver~",
                table: "FastServer_Microservice_Register",
                column: "fastserver_user_id",
                principalTable: "FastServer_User",
                principalColumn: "fastserver_user_id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FastServer_Microservice_Register_FastServer_User_fastserver~",
                table: "FastServer_Microservice_Register");

            migrationBuilder.DropIndex(
                name: "IX_FastServer_Microservice_Register_UserId",
                table: "FastServer_Microservice_Register");

            migrationBuilder.DropColumn(
                name: "fastserver_user_id",
                table: "FastServer_Microservice_Register");
        }
    }
}
