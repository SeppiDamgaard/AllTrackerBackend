using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AllTracker.Migrations
{
    public partial class ConsumptionPosts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConsumptionPost",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Unit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CronString = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstIncrement = table.Column<double>(type: "float", nullable: false),
                    SecondIncrement = table.Column<double>(type: "float", nullable: false),
                    ThirdIncrement = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsumptionPost", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConsumptionPost_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ConsumptionRegistration",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PostId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsumptionRegistration", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConsumptionRegistration_ConsumptionPost_PostId",
                        column: x => x.PostId,
                        principalTable: "ConsumptionPost",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConsumptionPost_UserId",
                table: "ConsumptionPost",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsumptionRegistration_PostId",
                table: "ConsumptionRegistration",
                column: "PostId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConsumptionRegistration");

            migrationBuilder.DropTable(
                name: "ConsumptionPost");
        }
    }
}
