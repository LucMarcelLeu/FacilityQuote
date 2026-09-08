using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FacilityQuote.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddRequestLanguage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Language",
                table: "Requests",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Language",
                table: "Requests");
        }
    }
}
