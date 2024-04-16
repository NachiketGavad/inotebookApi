using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace inotebookApi.Migrations
{
    public partial class removedUserNavigationProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notes_Users_User_id",
                table: "Notes");

            migrationBuilder.DropIndex(
                name: "IX_Notes_User_id",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "User_id",
                table: "Notes");

            migrationBuilder.AlterColumn<DateTime>(
                name: "date",
                table: "Users",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "date",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "User_id",
                table: "Notes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Notes_User_id",
                table: "Notes",
                column: "User_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_Users_User_id",
                table: "Notes",
                column: "User_id",
                principalTable: "Users",
                principalColumn: "_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
