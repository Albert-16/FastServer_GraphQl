using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FastServer.Infrastructure.Data.Migrations.PostgreSql
{
    /// <inheritdoc />
    public partial class OptimizeIndexesAndAuth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FastServer_LogServices_Header_Historico_fastserver_log_state",
                table: "FastServer_LogServices_Header_Historico");

            migrationBuilder.DropIndex(
                name: "IX_FastServer_LogServices_Header_fastserver_log_state",
                table: "FastServer_LogServices_Header");

            migrationBuilder.DropIndex(
                name: "IX_FastServer_LogServices_Content_Historico_fastserver_log_id",
                table: "FastServer_LogServices_Content_Historico");

            migrationBuilder.DropIndex(
                name: "IX_FastServer_LogServices_Content_fastserver_log_id",
                table: "FastServer_LogServices_Content");

            migrationBuilder.DropIndex(
                name: "IX_FastServer_LogMicroservice_Historico_fastserver_log_id",
                table: "FastServer_LogMicroservice_Historico");

            migrationBuilder.DropIndex(
                name: "IX_FastServer_LogMicroservice_fastserver_log_id",
                table: "FastServer_LogMicroservice");

            migrationBuilder.CreateIndex(
                name: "IX_LogServicesHeader_LogDateIn_MicroserviceName",
                table: "FastServer_LogServices_Header",
                columns: new[] { "fastserver_log_date_in", "fastserver_microservice_name" });

            migrationBuilder.CreateIndex(
                name: "IX_LogServicesHeader_UserId_LogDateIn",
                table: "FastServer_LogServices_Header",
                columns: new[] { "fastserver_user_id", "fastserver_log_date_in" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LogServicesHeader_LogDateIn_MicroserviceName",
                table: "FastServer_LogServices_Header");

            migrationBuilder.DropIndex(
                name: "IX_LogServicesHeader_UserId_LogDateIn",
                table: "FastServer_LogServices_Header");

            migrationBuilder.CreateIndex(
                name: "IX_FastServer_LogServices_Header_Historico_fastserver_log_state",
                table: "FastServer_LogServices_Header_Historico",
                column: "fastserver_log_state");

            migrationBuilder.CreateIndex(
                name: "IX_FastServer_LogServices_Header_fastserver_log_state",
                table: "FastServer_LogServices_Header",
                column: "fastserver_log_state");

            migrationBuilder.CreateIndex(
                name: "IX_FastServer_LogServices_Content_Historico_fastserver_log_id",
                table: "FastServer_LogServices_Content_Historico",
                column: "fastserver_log_id");

            migrationBuilder.CreateIndex(
                name: "IX_FastServer_LogServices_Content_fastserver_log_id",
                table: "FastServer_LogServices_Content",
                column: "fastserver_log_id");

            migrationBuilder.CreateIndex(
                name: "IX_FastServer_LogMicroservice_Historico_fastserver_log_id",
                table: "FastServer_LogMicroservice_Historico",
                column: "fastserver_log_id");

            migrationBuilder.CreateIndex(
                name: "IX_FastServer_LogMicroservice_fastserver_log_id",
                table: "FastServer_LogMicroservice",
                column: "fastserver_log_id");
        }
    }
}
