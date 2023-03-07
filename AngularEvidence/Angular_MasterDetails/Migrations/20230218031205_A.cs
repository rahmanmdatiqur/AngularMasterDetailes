using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Angular_MasterDetails.Migrations
{
    public partial class A : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Diseses",
                columns: table => new
                {
                    DiseseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DiseseName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Diseses", x => x.DiseseId);
                });

            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    PatientId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BirthDate = table.Column<DateTime>(type: "date", nullable: false),
                    PhoneNo = table.Column<int>(type: "int", nullable: false),
                    Picture = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaritalStatus = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.PatientId);
                });

            migrationBuilder.CreateTable(
                name: "TestEntries",
                columns: table => new
                {
                    TestEntryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    DiseseId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestEntries", x => x.TestEntryId);
                    table.ForeignKey(
                        name: "FK_TestEntries_Diseses_DiseseId",
                        column: x => x.DiseseId,
                        principalTable: "Diseses",
                        principalColumn: "DiseseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TestEntries_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "PatientId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Diseses",
                columns: new[] { "DiseseId", "DiseseName" },
                values: new object[,]
                {
                    { 1, "Fever" },
                    { 2, "Covid-19" },
                    { 3, "Cold" },
                    { 4, "Maleria" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_TestEntries_DiseseId",
                table: "TestEntries",
                column: "DiseseId");

            migrationBuilder.CreateIndex(
                name: "IX_TestEntries_PatientId",
                table: "TestEntries",
                column: "PatientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TestEntries");

            migrationBuilder.DropTable(
                name: "Diseses");

            migrationBuilder.DropTable(
                name: "Patients");
        }
    }
}
