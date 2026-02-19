using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FastServer.Infrastructure.Data.Migrations.PostgreSqlLogs
{
    /// <inheritdoc />
    public partial class RemoveRequestIdAndChangeRequestDurationToString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "fastserver_request_id",
                table: "FastServer_LogServices_Header_Historico");

            migrationBuilder.DropColumn(
                name: "fastserver_request_id",
                table: "FastServer_LogServices_Header");

            migrationBuilder.DropColumn(
                name: "fastserver_request_id",
                table: "FastServer_LogMicroservice_Historico");

            migrationBuilder.DropColumn(
                name: "fastserver_request_id",
                table: "FastServer_LogMicroservice");

            migrationBuilder.AlterColumn<string>(
                name: "fastserver_request_duration",
                table: "FastServer_LogServices_Header_Historico",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "fastserver_request_duration",
                table: "FastServer_LogServices_Header",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "FastServer_LogServices_Header",
                keyColumn: "fastserver_log_id",
                keyValue: 1L,
                column: "fastserver_request_duration",
                value: "150.00 ms");

            migrationBuilder.UpdateData(
                table: "FastServer_LogServices_Header",
                keyColumn: "fastserver_log_id",
                keyValue: 2L,
                column: "fastserver_request_duration",
                value: "320.00 ms");

            migrationBuilder.UpdateData(
                table: "FastServer_LogServices_Header_Historico",
                keyColumn: "fastserver_log_id",
                keyValue: 1L,
                column: "fastserver_request_duration",
                value: "145.00 ms");

            migrationBuilder.UpdateData(
                table: "FastServer_LogServices_Header_Historico",
                keyColumn: "fastserver_log_id",
                keyValue: 2L,
                column: "fastserver_request_duration",
                value: "280.00 ms");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "fastserver_request_duration",
                table: "FastServer_LogServices_Header_Historico",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "fastserver_request_id",
                table: "FastServer_LogServices_Header_Historico",
                type: "uuid",
                nullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "fastserver_request_duration",
                table: "FastServer_LogServices_Header",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "fastserver_request_id",
                table: "FastServer_LogServices_Header",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "fastserver_request_id",
                table: "FastServer_LogMicroservice_Historico",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "fastserver_request_id",
                table: "FastServer_LogMicroservice",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.UpdateData(
                table: "FastServer_LogMicroservice",
                keyColumn: "fastserver_log_microservice_id",
                keyValue: new Guid("01965a00-0000-7000-8000-000000000001"),
                column: "fastserver_request_id",
                value: 1001L);

            migrationBuilder.UpdateData(
                table: "FastServer_LogMicroservice",
                keyColumn: "fastserver_log_microservice_id",
                keyValue: new Guid("01965a00-0000-7000-8000-000000000002"),
                column: "fastserver_request_id",
                value: 1002L);

            migrationBuilder.UpdateData(
                table: "FastServer_LogMicroservice_Historico",
                keyColumn: "fastserver_log_microservice_id",
                keyValue: new Guid("01965a00-0000-7000-8000-000000000003"),
                column: "fastserver_request_id",
                value: 2001L);

            migrationBuilder.UpdateData(
                table: "FastServer_LogMicroservice_Historico",
                keyColumn: "fastserver_log_microservice_id",
                keyValue: new Guid("01965a00-0000-7000-8000-000000000004"),
                column: "fastserver_request_id",
                value: 2002L);

            migrationBuilder.UpdateData(
                table: "FastServer_LogServices_Header",
                keyColumn: "fastserver_log_id",
                keyValue: 1L,
                columns: new[] { "fastserver_request_duration", "fastserver_request_id" },
                values: new object[] { 150L, null });

            migrationBuilder.UpdateData(
                table: "FastServer_LogServices_Header",
                keyColumn: "fastserver_log_id",
                keyValue: 2L,
                columns: new[] { "fastserver_request_duration", "fastserver_request_id" },
                values: new object[] { 320L, null });

            migrationBuilder.UpdateData(
                table: "FastServer_LogServices_Header_Historico",
                keyColumn: "fastserver_log_id",
                keyValue: 1L,
                columns: new[] { "fastserver_request_duration", "fastserver_request_id" },
                values: new object[] { 145L, null });

            migrationBuilder.UpdateData(
                table: "FastServer_LogServices_Header_Historico",
                keyColumn: "fastserver_log_id",
                keyValue: 2L,
                columns: new[] { "fastserver_request_duration", "fastserver_request_id" },
                values: new object[] { 280L, null });
        }
    }
}
