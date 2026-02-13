using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FastServer.Infrastructure.Data.Migrations.PostgreSqlLogs
{
    /// <inheritdoc />
    public partial class ChangeLogFsIdToGuid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // PostgreSQL no puede convertir bigint a uuid directamente,
            // primero ponemos NULL y luego cambiamos el tipo con USING
            migrationBuilder.Sql(
                """
                UPDATE "FastServer_LogServices_Header_Historico" SET fastserver_log_fs_id = NULL;
                ALTER TABLE "FastServer_LogServices_Header_Historico"
                    ALTER COLUMN fastserver_log_fs_id TYPE uuid USING fastserver_log_fs_id::text::uuid;
                """);

            migrationBuilder.Sql(
                """
                UPDATE "FastServer_LogServices_Header" SET fastserver_log_fs_id = NULL;
                ALTER TABLE "FastServer_LogServices_Header"
                    ALTER COLUMN fastserver_log_fs_id TYPE uuid USING fastserver_log_fs_id::text::uuid;
                """);

            migrationBuilder.UpdateData(
                table: "FastServer_LogServices_Header",
                keyColumn: "fastserver_log_id",
                keyValue: 1L,
                column: "fastserver_log_fs_id",
                value: null);

            migrationBuilder.UpdateData(
                table: "FastServer_LogServices_Header",
                keyColumn: "fastserver_log_id",
                keyValue: 2L,
                column: "fastserver_log_fs_id",
                value: null);

            migrationBuilder.UpdateData(
                table: "FastServer_LogServices_Header_Historico",
                keyColumn: "fastserver_log_id",
                keyValue: 1L,
                column: "fastserver_log_fs_id",
                value: null);

            migrationBuilder.UpdateData(
                table: "FastServer_LogServices_Header_Historico",
                keyColumn: "fastserver_log_id",
                keyValue: 2L,
                column: "fastserver_log_fs_id",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "fastserver_log_fs_id",
                table: "FastServer_LogServices_Header_Historico",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "fastserver_log_fs_id",
                table: "FastServer_LogServices_Header",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "FastServer_LogServices_Header",
                keyColumn: "fastserver_log_id",
                keyValue: 1L,
                column: "fastserver_log_fs_id",
                value: null);

            migrationBuilder.UpdateData(
                table: "FastServer_LogServices_Header",
                keyColumn: "fastserver_log_id",
                keyValue: 2L,
                column: "fastserver_log_fs_id",
                value: null);

            migrationBuilder.UpdateData(
                table: "FastServer_LogServices_Header_Historico",
                keyColumn: "fastserver_log_id",
                keyValue: 1L,
                column: "fastserver_log_fs_id",
                value: null);

            migrationBuilder.UpdateData(
                table: "FastServer_LogServices_Header_Historico",
                keyColumn: "fastserver_log_id",
                keyValue: 2L,
                column: "fastserver_log_fs_id",
                value: null);
        }
    }
}
