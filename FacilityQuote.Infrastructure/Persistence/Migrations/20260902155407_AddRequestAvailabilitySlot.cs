using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FacilityQuote.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddRequestAvailabilitySlot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AvailabilitySlotId",
                table: "Requests",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "TimeSlot",
                table: "Requests",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Requests_AvailabilitySlotId_TimeSlot",
                table: "Requests",
                columns: new[] { "AvailabilitySlotId", "TimeSlot" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Availabilities_AvailabilitySlotId",
                table: "Requests",
                column: "AvailabilitySlotId",
                principalTable: "Availabilities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Availabilities_AvailabilitySlotId",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_AvailabilitySlotId_TimeSlot",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "AvailabilitySlotId",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "TimeSlot",
                table: "Requests");
        }
    }
}
