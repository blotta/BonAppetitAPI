using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BonAppetitAPI.Migrations
{
    public partial class MenuItemMenuSectionManyToMany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MenuItems_MenuSections_MenuSectionId",
                table: "MenuItems");

            migrationBuilder.DropIndex(
                name: "IX_MenuItems_MenuSectionId",
                table: "MenuItems");

            migrationBuilder.DropColumn(
                name: "MenuSectionId",
                table: "MenuItems");

            migrationBuilder.AddColumn<int>(
                name: "RestaurantId",
                table: "MenuItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "MenuItemMenuSection",
                columns: table => new
                {
                    ItemsId = table.Column<int>(type: "int", nullable: false),
                    MenuSectionsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuItemMenuSection", x => new { x.ItemsId, x.MenuSectionsId });
                    table.ForeignKey(
                        name: "FK_MenuItemMenuSection_MenuItems_ItemsId",
                        column: x => x.ItemsId,
                        principalTable: "MenuItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MenuItemMenuSection_MenuSections_MenuSectionsId",
                        column: x => x.MenuSectionsId,
                        principalTable: "MenuSections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MenuItems_RestaurantId",
                table: "MenuItems",
                column: "RestaurantId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuItemMenuSection_MenuSectionsId",
                table: "MenuItemMenuSection",
                column: "MenuSectionsId");

            migrationBuilder.AddForeignKey(
                name: "FK_MenuItems_Restaurants_RestaurantId",
                table: "MenuItems",
                column: "RestaurantId",
                principalTable: "Restaurants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MenuItems_Restaurants_RestaurantId",
                table: "MenuItems");

            migrationBuilder.DropTable(
                name: "MenuItemMenuSection");

            migrationBuilder.DropIndex(
                name: "IX_MenuItems_RestaurantId",
                table: "MenuItems");

            migrationBuilder.DropColumn(
                name: "RestaurantId",
                table: "MenuItems");

            migrationBuilder.AddColumn<int>(
                name: "MenuSectionId",
                table: "MenuItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MenuItems_MenuSectionId",
                table: "MenuItems",
                column: "MenuSectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_MenuItems_MenuSections_MenuSectionId",
                table: "MenuItems",
                column: "MenuSectionId",
                principalTable: "MenuSections",
                principalColumn: "Id");
        }
    }
}
