using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FastServer.Infrastructure.Data.Migrations.PostgreSqlMicroservices
{
    /// <inheritdoc />
    public partial class MoveClusterFkFromRegisterToMethod : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_microservice_registers_microservices_clusters_microservice_~",
                table: "microservice_registers");

            migrationBuilder.DropIndex(
                name: "IX_microservice_registers_microservice_cluster_id",
                table: "microservice_registers");

            migrationBuilder.DropIndex(
                name: "IX_MicroserviceRegister_ClusterId_Name",
                table: "microservice_registers");

            migrationBuilder.DropColumn(
                name: "microservice_cluster_id",
                table: "microservice_registers");

            migrationBuilder.AddColumn<long>(
                name: "microservices_cluster_id",
                table: "microservice_methods",
                type: "bigint",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "microservice_methods",
                keyColumn: "microservice_method_id",
                keyValue: 1L,
                column: "microservices_cluster_id",
                value: 1L);

            migrationBuilder.UpdateData(
                table: "microservice_methods",
                keyColumn: "microservice_method_id",
                keyValue: 2L,
                column: "microservices_cluster_id",
                value: 1L);

            migrationBuilder.CreateIndex(
                name: "IX_microservice_methods_microservices_cluster_id",
                table: "microservice_methods",
                column: "microservices_cluster_id");

            migrationBuilder.AddForeignKey(
                name: "FK_microservice_methods_microservices_clusters_microservices_c~",
                table: "microservice_methods",
                column: "microservices_cluster_id",
                principalTable: "microservices_clusters",
                principalColumn: "microservices_cluster_id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_microservice_methods_microservices_clusters_microservices_c~",
                table: "microservice_methods");

            migrationBuilder.DropIndex(
                name: "IX_microservice_methods_microservices_cluster_id",
                table: "microservice_methods");

            migrationBuilder.DropColumn(
                name: "microservices_cluster_id",
                table: "microservice_methods");

            migrationBuilder.AddColumn<long>(
                name: "microservice_cluster_id",
                table: "microservice_registers",
                type: "bigint",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "microservice_registers",
                keyColumn: "microservice_id",
                keyValue: 1L,
                column: "microservice_cluster_id",
                value: 1L);

            migrationBuilder.UpdateData(
                table: "microservice_registers",
                keyColumn: "microservice_id",
                keyValue: 2L,
                column: "microservice_cluster_id",
                value: 1L);

            migrationBuilder.CreateIndex(
                name: "IX_microservice_registers_microservice_cluster_id",
                table: "microservice_registers",
                column: "microservice_cluster_id");

            migrationBuilder.CreateIndex(
                name: "IX_MicroserviceRegister_ClusterId_Name",
                table: "microservice_registers",
                columns: new[] { "microservice_cluster_id", "microservice_name" });

            migrationBuilder.AddForeignKey(
                name: "FK_microservice_registers_microservices_clusters_microservice_~",
                table: "microservice_registers",
                column: "microservice_cluster_id",
                principalTable: "microservices_clusters",
                principalColumn: "microservices_cluster_id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
