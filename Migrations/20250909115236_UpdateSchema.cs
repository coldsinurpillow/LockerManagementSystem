using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LockerManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LockerPlaces_Lockers_LockerID",
                table: "LockerPlaces");

            migrationBuilder.RenameColumn(
                name: "LockerID",
                table: "LockerPlaces",
                newName: "LockerId");

            migrationBuilder.RenameIndex(
                name: "IX_LockerPlaces_LockerID_PlaceIndex",
                table: "LockerPlaces",
                newName: "IX_LockerPlaces_LockerId_PlaceIndex");

            migrationBuilder.AddForeignKey(
                name: "FK_LockerPlaces_Lockers_LockerId",
                table: "LockerPlaces",
                column: "LockerId",
                principalTable: "Lockers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LockerPlaces_Lockers_LockerId",
                table: "LockerPlaces");

            migrationBuilder.RenameColumn(
                name: "LockerId",
                table: "LockerPlaces",
                newName: "LockerID");

            migrationBuilder.RenameIndex(
                name: "IX_LockerPlaces_LockerId_PlaceIndex",
                table: "LockerPlaces",
                newName: "IX_LockerPlaces_LockerID_PlaceIndex");

            migrationBuilder.AddForeignKey(
                name: "FK_LockerPlaces_Lockers_LockerID",
                table: "LockerPlaces",
                column: "LockerID",
                principalTable: "Lockers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
