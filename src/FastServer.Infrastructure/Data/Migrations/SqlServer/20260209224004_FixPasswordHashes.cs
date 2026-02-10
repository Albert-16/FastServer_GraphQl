using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FastServer.Infrastructure.Data.Migrations.SqlServer
{
    /// <inheritdoc />
    public partial class FixPasswordHashes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                column: "password_hash",
                value: "$2a$11$N9qo8uLOickgx2ZMRZoMyeIjZAgcfl7p92ldGxad68LJZdL17lhWy");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"),
                column: "password_hash",
                value: "$2a$11$VRjNO0ZRwK7x1Z.XfJcKAOKs7ggzwhPB3QVpLp2PF3cxyMq7R5rHu");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                column: "password_hash",
                value: "$2a$11$6K8V4pZ4rX5YqLH8wX5Z8OZx3Q2ZsN5Y8mWvP5X8wX5Z8OZx3Q2Zs");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"),
                column: "password_hash",
                value: "$2a$11$7L9W5qA5sY6ZrMI9xY6A9PAy4R3AtO6Z9nXwQ6Y9xY6A9PAy4R3At");
        }
    }
}
