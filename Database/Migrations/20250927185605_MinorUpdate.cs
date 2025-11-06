using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class MinorUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupInstances_Groups_GroupId",
                table: "GroupInstances");

            migrationBuilder.AlterColumn<Guid>(
                name: "GroupId",
                table: "GroupInstances",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupInstances_Groups_GroupId",
                table: "GroupInstances",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "GroupId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupInstances_Groups_GroupId",
                table: "GroupInstances");

            migrationBuilder.AlterColumn<Guid>(
                name: "GroupId",
                table: "GroupInstances",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupInstances_Groups_GroupId",
                table: "GroupInstances",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "GroupId");
        }
    }
}
