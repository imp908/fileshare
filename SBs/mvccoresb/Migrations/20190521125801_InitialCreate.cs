using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace mvccoresb.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Blogs",
                columns: table => new
                {
                    BlogId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Url = table.Column<string>(nullable: true),
                    Rating = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blogs", x => x.BlogId);
                });

            migrationBuilder.CreateTable(
                name: "GeoPlan",
                columns: table => new
                {
                    PlanId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeoPlan", x => x.PlanId);
                });

            migrationBuilder.CreateTable(
                name: "Instructors",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Surname = table.Column<string>(nullable: true),
                    QualityGrade = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Instructors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    PersonID = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Surname = table.Column<string>(nullable: true),
                    Discriminator = table.Column<string>(nullable: false),
                    EnrollmentDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.PersonID);
                });

            migrationBuilder.CreateTable(
                name: "TagEF",
                columns: table => new
                {
                    TagId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TagEF", x => x.TagId);
                });

            migrationBuilder.CreateTable(
                name: "BlogImage",
                columns: table => new
                {
                    BlogImageId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Image = table.Column<byte[]>(nullable: true),
                    Caption = table.Column<string>(nullable: true),
                    BlogId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogImage", x => x.BlogImageId);
                    table.ForeignKey(
                        name: "FK_BlogImage_Blogs_BlogId",
                        column: x => x.BlogId,
                        principalTable: "Blogs",
                        principalColumn: "BlogId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    PostId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    BlogId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.PostId);
                    table.ForeignKey(
                        name: "FK_Posts_Blogs_BlogId",
                        column: x => x.BlogId,
                        principalTable: "Blogs",
                        principalColumn: "BlogId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GeoLayout",
                columns: table => new
                {
                    LayoutId = table.Column<Guid>(nullable: false),
                    PlanId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeoLayout", x => x.LayoutId);
                    table.ForeignKey(
                        name: "FK_GeoLayout_GeoPlan_PlanId",
                        column: x => x.PlanId,
                        principalTable: "GeoPlan",
                        principalColumn: "PlanId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PostTagEF",
                columns: table => new
                {
                    PostId = table.Column<int>(nullable: false),
                    TagId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostTagEF", x => new { x.PostId, x.TagId });
                    table.ForeignKey(
                        name: "FK_PostTagEF_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "PostId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostTagEF_TagEF_TagId",
                        column: x => x.TagId,
                        principalTable: "TagEF",
                        principalColumn: "TagId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GeoFacility",
                columns: table => new
                {
                    FacilityId = table.Column<Guid>(nullable: false),
                    LayoutId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeoFacility", x => x.FacilityId);
                    table.ForeignKey(
                        name: "FK_GeoFacility_GeoLayout_LayoutId",
                        column: x => x.LayoutId,
                        principalTable: "GeoLayout",
                        principalColumn: "LayoutId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GeoCategory",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ParentId = table.Column<Guid>(nullable: false),
                    FacilityId = table.Column<Guid>(nullable: false),
                    GeoCategoryId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeoCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GeoCategory_GeoFacility_FacilityId",
                        column: x => x.FacilityId,
                        principalTable: "GeoFacility",
                        principalColumn: "FacilityId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GeoCategory_GeoCategory_GeoCategoryId",
                        column: x => x.GeoCategoryId,
                        principalTable: "GeoCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ServiceTypes",
                columns: table => new
                {
                    ServiceID = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    GeoCategoryId = table.Column<Guid>(nullable: true),
                    GeoFacilityFacilityId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceTypes", x => x.ServiceID);
                    table.ForeignKey(
                        name: "FK_ServiceTypes_GeoCategory_GeoCategoryId",
                        column: x => x.GeoCategoryId,
                        principalTable: "GeoCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ServiceTypes_GeoFacility_GeoFacilityFacilityId",
                        column: x => x.GeoFacilityFacilityId,
                        principalTable: "GeoFacility",
                        principalColumn: "FacilityId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlogImage_BlogId",
                table: "BlogImage",
                column: "BlogId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GeoCategory_FacilityId",
                table: "GeoCategory",
                column: "FacilityId");

            migrationBuilder.CreateIndex(
                name: "IX_GeoCategory_GeoCategoryId",
                table: "GeoCategory",
                column: "GeoCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_GeoFacility_LayoutId",
                table: "GeoFacility",
                column: "LayoutId");

            migrationBuilder.CreateIndex(
                name: "IX_GeoLayout_PlanId",
                table: "GeoLayout",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_BlogId",
                table: "Posts",
                column: "BlogId");

            migrationBuilder.CreateIndex(
                name: "IX_PostTagEF_TagId",
                table: "PostTagEF",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceTypes_GeoCategoryId",
                table: "ServiceTypes",
                column: "GeoCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceTypes_GeoFacilityFacilityId",
                table: "ServiceTypes",
                column: "GeoFacilityFacilityId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceTypes_ServiceID",
                table: "ServiceTypes",
                column: "ServiceID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlogImage");

            migrationBuilder.DropTable(
                name: "Instructors");

            migrationBuilder.DropTable(
                name: "Persons");

            migrationBuilder.DropTable(
                name: "PostTagEF");

            migrationBuilder.DropTable(
                name: "ServiceTypes");

            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "TagEF");

            migrationBuilder.DropTable(
                name: "GeoCategory");

            migrationBuilder.DropTable(
                name: "Blogs");

            migrationBuilder.DropTable(
                name: "GeoFacility");

            migrationBuilder.DropTable(
                name: "GeoLayout");

            migrationBuilder.DropTable(
                name: "GeoPlan");
        }
    }
}
