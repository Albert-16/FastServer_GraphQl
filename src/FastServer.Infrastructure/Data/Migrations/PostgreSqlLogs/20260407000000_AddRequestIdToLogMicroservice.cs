using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FastServer.Infrastructure.Data.Migrations.PostgreSqlLogs
{
    /// <inheritdoc />
    public partial class AddRequestIdToLogMicroservice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "fastserver_log_requestid",
                table: "FastServer_LogMicroservice",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "fastserver_log_requestid",
                table: "FastServer_LogMicroservice_Historico",
                type: "bigint",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "fastserver_log_requestid",
                table: "FastServer_LogMicroservice");

            migrationBuilder.DropColumn(
                name: "fastserver_log_requestid",
                table: "FastServer_LogMicroservice_Historico");
        }
    }
}
