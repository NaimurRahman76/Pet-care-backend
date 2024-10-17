﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetCareBackend.Migrations
{
    /// <inheritdoc />
    public partial class userFullnameadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Username",
                table: "Users",
                newName: "FullName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "Users",
                newName: "Username");
        }
    }
}
