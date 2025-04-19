using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UNISchedule.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UNIDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClassroomEntities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Number = table.Column<int>(type: "int", nullable: false),
                    Building = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassroomEntities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClassTimeEntities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Timeframe = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassTimeEntities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InstituteEntities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstituteEntities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SubjectEntities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectEntities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TypeSubjectEntities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeSubjectEntities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GroupEntities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    InstituteEntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupEntities_InstituteEntities_InstituteEntityId",
                        column: x => x.InstituteEntityId,
                        principalTable: "InstituteEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SubjectAssignmentEntities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ScheduleRecordEntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GroupEntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubjectEntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TypeSubjectEntityID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectAssignmentEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubjectAssignmentEntities_GroupEntities_GroupEntityId",
                        column: x => x.GroupEntityId,
                        principalTable: "GroupEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SubjectAssignmentEntities_SubjectEntities_SubjectEntityId",
                        column: x => x.SubjectEntityId,
                        principalTable: "SubjectEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SubjectAssignmentEntities_TypeSubjectEntities_TypeSubjectEntityID",
                        column: x => x.TypeSubjectEntityID,
                        principalTable: "TypeSubjectEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ScheduleRecordEntities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    AdditionalData = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ClassTimeEntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClassroomEntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubjectAssignmentEntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleRecordEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScheduleRecordEntities_ClassTimeEntities_ClassTimeEntityId",
                        column: x => x.ClassTimeEntityId,
                        principalTable: "ClassTimeEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ScheduleRecordEntities_ClassroomEntities_ClassroomEntityId",
                        column: x => x.ClassroomEntityId,
                        principalTable: "ClassroomEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ScheduleRecordEntities_SubjectAssignmentEntities_SubjectAssignmentEntityId",
                        column: x => x.SubjectAssignmentEntityId,
                        principalTable: "SubjectAssignmentEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GroupEntities_InstituteEntityId",
                table: "GroupEntities",
                column: "InstituteEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleRecordEntities_ClassroomEntityId",
                table: "ScheduleRecordEntities",
                column: "ClassroomEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleRecordEntities_ClassTimeEntityId",
                table: "ScheduleRecordEntities",
                column: "ClassTimeEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleRecordEntities_SubjectAssignmentEntityId",
                table: "ScheduleRecordEntities",
                column: "SubjectAssignmentEntityId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubjectAssignmentEntities_GroupEntityId",
                table: "SubjectAssignmentEntities",
                column: "GroupEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectAssignmentEntities_SubjectEntityId",
                table: "SubjectAssignmentEntities",
                column: "SubjectEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectAssignmentEntities_TypeSubjectEntityID",
                table: "SubjectAssignmentEntities",
                column: "TypeSubjectEntityID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScheduleRecordEntities");

            migrationBuilder.DropTable(
                name: "ClassTimeEntities");

            migrationBuilder.DropTable(
                name: "ClassroomEntities");

            migrationBuilder.DropTable(
                name: "SubjectAssignmentEntities");

            migrationBuilder.DropTable(
                name: "GroupEntities");

            migrationBuilder.DropTable(
                name: "SubjectEntities");

            migrationBuilder.DropTable(
                name: "TypeSubjectEntities");

            migrationBuilder.DropTable(
                name: "InstituteEntities");
        }
    }
}
