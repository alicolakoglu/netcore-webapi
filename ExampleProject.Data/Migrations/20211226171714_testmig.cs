using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ExampleProject.Data.Migrations
{
    public partial class testmig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Common_AppMenu",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(150)", nullable: true),
                    Href = table.Column<string>(type: "nvarchar(150)", nullable: true),
                    Icon = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    SortIndex = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Common_AppMenu", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Users_User",
                columns: table => new
                {
                    Key = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newsequentialid()"),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()"),
                    Username = table.Column<string>(type: "varchar(20)", nullable: true),
                    DisplayName = table.Column<string>(type: "varchar(150)", nullable: true),
                    Email = table.Column<string>(type: "varchar(150)", nullable: true),
                    Password = table.Column<string>(type: "varchar(50)", nullable: true),
                    IsAdministrator = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users_User", x => x.Key);
                });

            migrationBuilder.CreateTable(
                name: "Todo_Item",
                columns: table => new
                {
                    Key = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newsequentialid()"),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatorUserKey = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Todo_Item", x => x.Key);
                    table.ForeignKey(
                        name: "FK_Todo_Item_Users_User_CreatorUserKey",
                        column: x => x.CreatorUserKey,
                        principalTable: "Users_User",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Todo_Item_CreatorUserKey",
                table: "Todo_Item",
                column: "CreatorUserKey");

            migrationBuilder.Sql(@"INSERT INTO Common_AppMenu (ID,Title,Href,Icon,ParentId,SortIndex,IsActive) VALUES
(1,'Home','/','home',NULL,1,1)
,(9,'Manage','/manage','settings',NULL,8,1)
,(90,'Users','/manage/users','loop',9,1,1)

INSERT INTO [Users_User] (DisplayName, Username, Email, IsAdministrator, Password) VALUES
('Test User', 'test', 'qoridor@gmail.com', 1, '098f6bcd4621d373cade4e832627b4f6')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Common_AppMenu");

            migrationBuilder.DropTable(
                name: "Todo_Item");

            migrationBuilder.DropTable(
                name: "Users_User");
        }
    }
}
