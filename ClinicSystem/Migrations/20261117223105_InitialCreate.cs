using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClinicSystem.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "online_booking_log",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    client_hash = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "TODATETIMEOFFSET(SYSUTCDATETIME(), '+00:00')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_online_booking_log", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "staff",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    email = table.Column<string>(type: "nvarchar(254)", maxLength: 254, nullable: false),
                    password_hash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    name = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    shift = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "TODATETIMEOFFSET(SYSUTCDATETIME(), '+00:00')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_staff", x => x.id);
                    table.CheckConstraint("CK_Staff_Name", "LEN(name + N'#') - 1 BETWEEN 2 AND 40");
                    table.CheckConstraint("CK_Staff_Shift", "shift IS NULL OR LEN(shift + N'#') - 1 <= 40");
                });

            migrationBuilder.CreateTable(
                name: "visit_types",
                columns: table => new
                {
                    code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    name_ar = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    duration_min = table.Column<int>(type: "int", nullable: false),
                    price = table.Column<int>(type: "int", nullable: false),
                    patient_can_book = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    sort = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_visit_types", x => x.code);
                    table.CheckConstraint("CK_VisitTypes_DurationMin", "duration_min BETWEEN 5 AND 30");
                    table.CheckConstraint("CK_VisitTypes_Price", "price >= 0");
                });

            migrationBuilder.CreateTable(
                name: "bookings",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    slot_date = table.Column<DateOnly>(type: "date", nullable: false),
                    slot_time = table.Column<TimeOnly>(type: "time(0)", nullable: false),
                    patient_name = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    phone = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    visit_type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    booked_type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    price = table.Column<int>(type: "int", nullable: false),
                    duration_min = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "confirmed"),
                    source = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "online"),
                    created_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    created_by_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "TODATETIMEOFFSET(SYSUTCDATETIME(), '+00:00')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bookings", x => x.id);
                    table.CheckConstraint("CK_Bookings_PatientName", "LEN(patient_name + N'#') - 1 BETWEEN 2 AND 60");
                    table.CheckConstraint("CK_Bookings_Phone", "LEN(phone + N'#') = 12 AND phone LIKE N'01[0125]%' AND phone NOT LIKE N'%[^0-9]%'");
                    table.CheckConstraint("CK_Bookings_Price", "price >= 0");
                    table.CheckConstraint("CK_Bookings_SlotTime", "DATEPART(MINUTE, slot_time) IN (0, 30) AND DATEPART(SECOND, slot_time) = 0");
                    table.CheckConstraint("CK_Bookings_Source", "source IN ('online', 'staff')");
                    table.CheckConstraint("CK_Bookings_Status", "status IN ('confirmed', 'cancelled')");
                    table.ForeignKey(
                        name: "FK_bookings_staff_created_by",
                        column: x => x.created_by,
                        principalTable: "staff",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_bookings_visit_types_booked_type",
                        column: x => x.booked_type,
                        principalTable: "visit_types",
                        principalColumn: "code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_bookings_visit_types_visit_type",
                        column: x => x.visit_type,
                        principalTable: "visit_types",
                        principalColumn: "code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "booking_changes",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    booking_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    action = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    old_type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    new_type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    old_price = table.Column<int>(type: "int", nullable: true),
                    new_price = table.Column<int>(type: "int", nullable: true),
                    reason = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    changed_by = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    changed_by_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    changed_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "TODATETIMEOFFSET(SYSUTCDATETIME(), '+00:00')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_booking_changes", x => x.id);
                    table.CheckConstraint("CK_BookingChanges_Action", "action IN ('create', 'change_type', 'cancel')");
                    table.CheckConstraint("CK_BookingChanges_Reason", "LEN(reason + N'#') - 1 BETWEEN 3 AND 200");
                    table.ForeignKey(
                        name: "FK_booking_changes_bookings_booking_id",
                        column: x => x.booking_id,
                        principalTable: "bookings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_booking_changes_staff_changed_by",
                        column: x => x.changed_by,
                        principalTable: "staff",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "booking_changes_booking_idx",
                table: "booking_changes",
                column: "booking_id");

            migrationBuilder.CreateIndex(
                name: "booking_changes_time_idx",
                table: "booking_changes",
                column: "changed_at",
               descending: new[] { true });

            migrationBuilder.CreateIndex(
                name: "IX_booking_changes_changed_by",
                table: "booking_changes",
                column: "changed_by");

            migrationBuilder.CreateIndex(
                name: "bookings_phone_idx",
                table: "bookings",
                columns: new[] { "phone", "slot_date" },
                descending: new[] { false, true });

            migrationBuilder.CreateIndex(
                name: "IX_bookings_booked_type",
                table: "bookings",
                column: "booked_type");

            migrationBuilder.CreateIndex(
                name: "IX_bookings_created_by",
                table: "bookings",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "IX_bookings_visit_type",
                table: "bookings",
                column: "visit_type");

            migrationBuilder.CreateIndex(
                name: "one_booking_per_phone_day",
                table: "bookings",
                columns: new[] { "phone", "slot_date" },
                unique: true,
                filter: "[status] = N'confirmed'");

            migrationBuilder.CreateIndex(
                name: "one_booking_per_slot",
                table: "bookings",
                columns: new[] { "slot_date", "slot_time" },
                unique: true,
                filter: "[status] = N'confirmed'");

            migrationBuilder.CreateIndex(
                name: "online_booking_log_client_time",
                table: "online_booking_log",
                columns: new[] { "client_hash", "created_at" },
                descending: new[] { false, true });

            migrationBuilder.CreateIndex(
                name: "IX_staff_email",
                table: "staff",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "booking_changes");

            migrationBuilder.DropTable(
                name: "online_booking_log");

            migrationBuilder.DropTable(
                name: "bookings");

            migrationBuilder.DropTable(
                name: "staff");

            migrationBuilder.DropTable(
                name: "visit_types");
        }
    }
}
