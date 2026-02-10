using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FastServer.Infrastructure.Data.Migrations.SqlServer
{
    /// <inheritdoc />
    public partial class OptimizeIndexesAndAuth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_users_user_active",
                table: "users");

            migrationBuilder.DropIndex(
                name: "IX_microservices_clusters_microservices_cluster_active",
                table: "microservices_clusters");

            migrationBuilder.DropIndex(
                name: "IX_microservices_clusters_microservices_cluster_deleted",
                table: "microservices_clusters");

            migrationBuilder.DropIndex(
                name: "IX_microservice_registers_microservice_active",
                table: "microservice_registers");

            migrationBuilder.DropIndex(
                name: "IX_microservice_registers_microservice_deleted",
                table: "microservice_registers");

            migrationBuilder.DropIndex(
                name: "IX_microservice_methods_microservice_method_delete",
                table: "microservice_methods");

            migrationBuilder.DropIndex(
                name: "IX_core_connector_credentials_core_connector_credential_user",
                table: "core_connector_credentials");

            migrationBuilder.AddColumn<bool>(
                name: "email_confirmed",
                table: "users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "last_login",
                table: "users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "password_changed_at",
                table: "users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "password_hash",
                table: "users",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "refresh_token",
                table: "users",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "refresh_token_expiry_time",
                table: "users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "email_confirmed", "last_login", "password_changed_at", "password_hash", "refresh_token", "refresh_token_expiry_time" },
                values: new object[] { true, null, null, "$2a$11$6K8V4pZ4rX5YqLH8wX5Z8OZx3Q2ZsN5Y8mWvP5X8wX5Z8OZx3Q2Zs", null, null });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"),
                columns: new[] { "email_confirmed", "last_login", "password_changed_at", "password_hash", "refresh_token", "refresh_token_expiry_time" },
                values: new object[] { true, null, null, "$2a$11$7L9W5qA5sY6ZrMI9xY6A9PAy4R3AtO6Z9nXwQ6Y9xY6A9PAy4R3At", null, null });

            migrationBuilder.CreateIndex(
                name: "IX_MicroserviceRegister_ClusterId_Name",
                table: "microservice_registers",
                columns: new[] { "microservice_cluster_id", "microservice_name" });

            migrationBuilder.CreateIndex(
                name: "UX_CoreConnectorCredential_User",
                table: "core_connector_credentials",
                column: "core_connector_credential_user",
                unique: true,
                filter: "[core_connector_credential_user] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLog_UserId_CreateAt_Desc",
                table: "activity_logs",
                columns: new[] { "user_id", "create_at" },
                descending: new[] { false, true });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MicroserviceRegister_ClusterId_Name",
                table: "microservice_registers");

            migrationBuilder.DropIndex(
                name: "UX_CoreConnectorCredential_User",
                table: "core_connector_credentials");

            migrationBuilder.DropIndex(
                name: "IX_ActivityLog_UserId_CreateAt_Desc",
                table: "activity_logs");

            migrationBuilder.DropColumn(
                name: "email_confirmed",
                table: "users");

            migrationBuilder.DropColumn(
                name: "last_login",
                table: "users");

            migrationBuilder.DropColumn(
                name: "password_changed_at",
                table: "users");

            migrationBuilder.DropColumn(
                name: "password_hash",
                table: "users");

            migrationBuilder.DropColumn(
                name: "refresh_token",
                table: "users");

            migrationBuilder.DropColumn(
                name: "refresh_token_expiry_time",
                table: "users");

            migrationBuilder.CreateIndex(
                name: "IX_users_user_active",
                table: "users",
                column: "user_active");

            migrationBuilder.CreateIndex(
                name: "IX_microservices_clusters_microservices_cluster_active",
                table: "microservices_clusters",
                column: "microservices_cluster_active");

            migrationBuilder.CreateIndex(
                name: "IX_microservices_clusters_microservices_cluster_deleted",
                table: "microservices_clusters",
                column: "microservices_cluster_deleted");

            migrationBuilder.CreateIndex(
                name: "IX_microservice_registers_microservice_active",
                table: "microservice_registers",
                column: "microservice_active");

            migrationBuilder.CreateIndex(
                name: "IX_microservice_registers_microservice_deleted",
                table: "microservice_registers",
                column: "microservice_deleted");

            migrationBuilder.CreateIndex(
                name: "IX_microservice_methods_microservice_method_delete",
                table: "microservice_methods",
                column: "microservice_method_delete");

            migrationBuilder.CreateIndex(
                name: "IX_core_connector_credentials_core_connector_credential_user",
                table: "core_connector_credentials",
                column: "core_connector_credential_user");
        }
    }
}
