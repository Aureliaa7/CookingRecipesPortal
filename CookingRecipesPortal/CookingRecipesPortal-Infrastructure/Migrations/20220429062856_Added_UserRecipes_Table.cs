using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CookingRecipesPortal_Infrastructure.Migrations
{
    public partial class Added_UserRecipes_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserRecipes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RecipeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ActionType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRecipes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRecipes_AppUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserRecipes_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserRecipes_RecipeId",
                table: "UserRecipes",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRecipes_UserId",
                table: "UserRecipes",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserRecipes");
        }
    }
}
