using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FIT_PONG.Migrations
{
    public partial class BrojKorisnikaLog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BrojKorisnikaLog",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BrojKorisnika = table.Column<int>(nullable: false),
                    MaxBrojKorisnika = table.Column<int>(nullable: false),
                    Datum = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrojKorisnikaLog", x => x.ID);
                });

            migrationBuilder.Sql(@"

SET IDENTITY_INSERT [dbo].[BrojKorisnikaLog] ON 
-- 7 mjesec
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (1, 0, 10, CAST(N'2020-07-01T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (2, 0, 4, CAST(N'2020-07-02T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (3, 0, 6, CAST(N'2020-07-03T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (4, 0, 9, CAST(N'2020-07-04T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (5, 0, 6, CAST(N'2020-07-05T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (6, 0, 7, CAST(N'2020-07-06T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (7, 0, 10, CAST(N'2020-07-07T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (8, 0, 9, CAST(N'2020-07-08T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (9, 0, 1, CAST(N'2020-07-09T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (10, 0, 8, CAST(N'2020-07-10T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (11, 0, 2, CAST(N'2020-07-11T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (12, 0, 5, CAST(N'2020-07-12T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (13, 0, 4, CAST(N'2020-07-13T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (14, 0, 9, CAST(N'2020-07-14T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (15, 0, 10, CAST(N'2020-07-15T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (16, 0, 1, CAST(N'2020-07-16T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (17, 0, 9, CAST(N'2020-07-17T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (18, 0, 4, CAST(N'2020-07-18T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (19, 0, 6, CAST(N'2020-07-19T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (20, 0, 9, CAST(N'2020-07-20T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (21, 0, 10, CAST(N'2020-07-21T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (22, 0, 9, CAST(N'2020-07-22T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (23, 0, 10, CAST(N'2020-07-23T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (24, 0, 9, CAST(N'2020-07-24T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (25, 0, 10, CAST(N'2020-07-25T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (26, 0, 7, CAST(N'2020-07-26T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (27, 0, 5, CAST(N'2020-07-27T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (28, 0, 4, CAST(N'2020-07-28T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (29, 0, 9, CAST(N'2020-07-29T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (30, 0, 11, CAST(N'2020-07-30T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (31, 0, 6, CAST(N'2020-07-31T00:00:00.0000000' AS DateTime2))


-- 8 mjesec
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (32, 0, 6, CAST(N'2020-08-01T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (33, 0, 4, CAST(N'2020-08-02T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (34, 0, 3, CAST(N'2020-08-03T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (35, 0, 2, CAST(N'2020-08-04T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (36, 0, 1, CAST(N'2020-08-05T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (37, 0, 1, CAST(N'2020-08-06T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (38, 0, 1, CAST(N'2020-08-07T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (39, 0, 4, CAST(N'2020-08-08T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (40, 0, 5, CAST(N'2020-08-09T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (41, 0, 9, CAST(N'2020-08-11T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (42, 0, 10, CAST(N'2020-08-12T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (43, 0, 12, CAST(N'2020-08-13T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (44, 0, 11, CAST(N'2020-08-14T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (45, 0, 10, CAST(N'2020-08-15T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (46, 0, 10, CAST(N'2020-08-16T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (47, 0, 9, CAST(N'2020-08-17T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (48, 0, 5, CAST(N'2020-08-18T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (49, 0, 7, CAST(N'2020-08-19T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (50, 0, 10, CAST(N'2020-08-20T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (51, 0, 11, CAST(N'2020-08-21T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (52, 0, 9, CAST(N'2020-08-22T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (53, 0, 10, CAST(N'2020-08-23T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (54, 0, 9, CAST(N'2020-08-24T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (55, 0, 10, CAST(N'2020-08-25T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (56, 0, 8, CAST(N'2020-08-26T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (57, 0, 9, CAST(N'2020-08-27T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (58, 0, 10, CAST(N'2020-08-28T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (59, 0, 9, CAST(N'2020-08-29T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (60, 0, 11, CAST(N'2020-08-30T00:00:00.0000000' AS DateTime2))


-- 9 mjesec
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (61, 0, 11, CAST(N'2020-09-01T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (62, 0, 12, CAST(N'2020-09-02T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (63, 0, 10, CAST(N'2020-09-03T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (64, 0, 9, CAST(N'2020-09-04T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (65, 0, 6, CAST(N'2020-09-05T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (66, 0, 7, CAST(N'2020-09-06T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (67, 0, 9, CAST(N'2020-09-07T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (68, 0, 10, CAST(N'2020-09-08T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (69, 0, 12, CAST(N'2020-09-09T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (70, 0, 9, CAST(N'2020-09-10T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (71, 0, 4, CAST(N'2020-09-11T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (72, 0, 5, CAST(N'2020-09-12T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (73, 0, 4, CAST(N'2020-09-13T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (74, 0, 9, CAST(N'2020-09-14T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (75, 0, 11, CAST(N'2020-09-15T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (76, 0, 12, CAST(N'2020-09-16T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (77, 0, 9, CAST(N'2020-09-17T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (78, 0, 10, CAST(N'2020-09-18T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (79, 0, 4, CAST(N'2020-09-19T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (80, 0, 5, CAST(N'2020-09-20T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (81, 0, 11, CAST(N'2020-09-21T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (82, 0, 9, CAST(N'2020-09-22T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (83, 0, 10, CAST(N'2020-09-23T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (84, 0, 5, CAST(N'2020-09-24T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (85, 0, 4, CAST(N'2020-09-25T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (86, 0, 3, CAST(N'2020-09-26T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (87, 0, 6, CAST(N'2020-09-27T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (88, 0, 7, CAST(N'2020-09-28T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (89, 0, 9, CAST(N'2020-09-29T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (90, 0, 11, CAST(N'2020-09-30T00:00:00.0000000' AS DateTime2))


-- 10 mjesec
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (91, 0, 10, CAST(N'2020-10-01T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (92, 0, 4, CAST(N'2020-10-02T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (93, 0, 6, CAST(N'2020-10-03T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (94, 0, 9, CAST(N'2020-10-04T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (95, 0, 6, CAST(N'2020-10-05T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (96, 0, 7, CAST(N'2020-10-06T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (97, 0, 10, CAST(N'2020-10-07T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (98, 0, 9, CAST(N'2020-10-08T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (99, 0, 1, CAST(N'2020-10-09T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (100, 0, 8, CAST(N'2020-10-10T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (101, 0, 2, CAST(N'2020-10-11T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (102, 0, 5, CAST(N'2020-10-12T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[BrojKorisnikaLog] ([ID], [BrojKorisnika], [MaxBrojKorisnika], [Datum]) VALUES (103, 0, 4, CAST(N'2020-10-13T00:00:00.0000000' AS DateTime2))

SET IDENTITY_INSERT [dbo].[BrojKorisnikaLog] OFF

");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BrojKorisnikaLog");
        }
    }
}
