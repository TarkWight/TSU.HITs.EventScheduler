using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventScheduler.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameEventEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventStudent");

            migrationBuilder.CreateTable(
                name: "EventEntityStudent",
                columns: table => new
                {
                    RegisteredEventsId = table.Column<Guid>(type: "uuid", nullable: false),
                    RegisteredStudentsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventEntityStudent", x => new { x.RegisteredEventsId, x.RegisteredStudentsId });
                    table.ForeignKey(
                        name: "FK_EventEntityStudent_Events_RegisteredEventsId",
                        column: x => x.RegisteredEventsId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventEntityStudent_Students_RegisteredStudentsId",
                        column: x => x.RegisteredStudentsId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventEntityStudent_RegisteredStudentsId",
                table: "EventEntityStudent",
                column: "RegisteredStudentsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventEntityStudent");

            migrationBuilder.CreateTable(
                name: "EventStudent",
                columns: table => new
                {
                    RegisteredEventsId = table.Column<Guid>(type: "uuid", nullable: false),
                    RegisteredStudentsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventStudent", x => new { x.RegisteredEventsId, x.RegisteredStudentsId });
                    table.ForeignKey(
                        name: "FK_EventStudent_Events_RegisteredEventsId",
                        column: x => x.RegisteredEventsId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventStudent_Students_RegisteredStudentsId",
                        column: x => x.RegisteredStudentsId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventStudent_RegisteredStudentsId",
                table: "EventStudent",
                column: "RegisteredStudentsId");
        }
    }
}
