using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DemoMVC.Migrations
{
    /// <inheritdoc />
    public partial class SeedKhoaData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Khoas",
                columns: new[] { "MaKhoa", "TenKhoa" },
                values: new object[,]
                {
                    { "01", "Dầu khí" },
                    { "02", "Mỏ" },
                    { "03", "Trắc địa - Bản đồ và Quản lý đất đai" },
                    { "04", "Cơ - Điện" },
                    { "05", "Công nghệ thông tin" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Khoas",
                keyColumn: "MaKhoa",
                keyValue: "01");

            migrationBuilder.DeleteData(
                table: "Khoas",
                keyColumn: "MaKhoa",
                keyValue: "02");

            migrationBuilder.DeleteData(
                table: "Khoas",
                keyColumn: "MaKhoa",
                keyValue: "03");

            migrationBuilder.DeleteData(
                table: "Khoas",
                keyColumn: "MaKhoa",
                keyValue: "04");

            migrationBuilder.DeleteData(
                table: "Khoas",
                keyColumn: "MaKhoa",
                keyValue: "05");
        }
    }
}
