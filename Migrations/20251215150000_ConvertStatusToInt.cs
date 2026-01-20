using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRTracker.Migrations
{
    [Migration("20251215150000_ConvertStatusToInt")]
    public partial class ConvertStatusToInt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // For SQLite: create temp table, copy with mapped values, drop old, rename temp
            migrationBuilder.Sql(@"
                PRAGMA foreign_keys=off;
                BEGIN TRANSACTION;
                CREATE TABLE IF NOT EXISTS ""LeaveRequests_temp"" (
                    ""LeaveRequestId"" INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                    ""EmployeeId"" INTEGER NOT NULL,
                    ""StartDate"" TEXT NOT NULL,
                    ""EndDate"" TEXT NOT NULL,
                    ""LeaveType"" TEXT NOT NULL,
                    ""Status"" INTEGER NOT NULL
                );

                INSERT INTO ""LeaveRequests_temp"" (""LeaveRequestId"",""EmployeeId"",""StartDate"",""EndDate"",""LeaveType"",""Status"")
                SELECT ""LeaveRequestId"",""EmployeeId"",""StartDate"",""EndDate"",""LeaveType"",
                    CASE ""Status""
                        WHEN 'Pending' THEN 0
                        WHEN 'Approved' THEN 1
                        WHEN 'Rejected' THEN 2
                        ELSE 0
                    END
                FROM ""LeaveRequests"";

                DROP TABLE ""LeaveRequests"";
                ALTER TABLE ""LeaveRequests_temp"" RENAME TO ""LeaveRequests"";

                -- recreate foreign key
                CREATE INDEX IF NOT EXISTS ""IX_LeaveRequests_EmployeeId"" ON ""LeaveRequests"" (""EmployeeId"");
                COMMIT;
                PRAGMA foreign_keys=on;
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                PRAGMA foreign_keys=off;
                BEGIN TRANSACTION;
                CREATE TABLE IF NOT EXISTS ""LeaveRequests_temp"" (
                    ""LeaveRequestId"" INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                    ""EmployeeId"" INTEGER NOT NULL,
                    ""StartDate"" TEXT NOT NULL,
                    ""EndDate"" TEXT NOT NULL,
                    ""LeaveType"" TEXT NOT NULL,
                    ""Status"" TEXT NOT NULL
                );

                INSERT INTO ""LeaveRequests_temp"" (""LeaveRequestId"",""EmployeeId"",""StartDate"",""EndDate"",""LeaveType"",""Status"")
                SELECT ""LeaveRequestId"",""EmployeeId"",""StartDate"",""EndDate"",""LeaveType"",
                    CASE ""Status""
                        WHEN 0 THEN 'Pending'
                        WHEN 1 THEN 'Approved'
                        WHEN 2 THEN 'Rejected'
                        ELSE 'Pending'
                    END
                FROM ""LeaveRequests"";

                DROP TABLE ""LeaveRequests"";
                ALTER TABLE ""LeaveRequests_temp"" RENAME TO ""LeaveRequests"";

                CREATE INDEX IF NOT EXISTS ""IX_LeaveRequests_EmployeeId"" ON ""LeaveRequests"" (""EmployeeId"");
                COMMIT;
                PRAGMA foreign_keys=on;
            ");
        }
    }
}
