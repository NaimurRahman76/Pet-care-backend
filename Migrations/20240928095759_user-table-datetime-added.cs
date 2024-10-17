using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetCareBackend.Migrations
{
    /// <inheritdoc />
    public partial class usertabledatetimeadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostImage_Posts_PostId",
                table: "PostImage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PostImage",
                table: "PostImage");

            migrationBuilder.RenameTable(
                name: "PostImage",
                newName: "PostImages");

            migrationBuilder.RenameIndex(
                name: "IX_PostImage_PostId",
                table: "PostImages",
                newName: "IX_PostImages_PostId");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOnUtc",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOnUtc",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOnUtc",
                table: "Posts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOnUtc",
                table: "Posts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOnUtc",
                table: "PostImages",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOnUtc",
                table: "PostImages",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_PostImages",
                table: "PostImages",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PostImages_Posts_PostId",
                table: "PostImages",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostImages_Posts_PostId",
                table: "PostImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PostImages",
                table: "PostImages");

            migrationBuilder.DropColumn(
                name: "CreatedOnUtc",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UpdatedOnUtc",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CreatedOnUtc",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "UpdatedOnUtc",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "CreatedOnUtc",
                table: "PostImages");

            migrationBuilder.DropColumn(
                name: "UpdatedOnUtc",
                table: "PostImages");

            migrationBuilder.RenameTable(
                name: "PostImages",
                newName: "PostImage");

            migrationBuilder.RenameIndex(
                name: "IX_PostImages_PostId",
                table: "PostImage",
                newName: "IX_PostImage_PostId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PostImage",
                table: "PostImage",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PostImage_Posts_PostId",
                table: "PostImage",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
