using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BSolutions.SHES.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProjectItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    Comment = table.Column<string>(type: "TEXT", nullable: true),
                    ParentId = table.Column<Guid>(type: "TEXT", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectItems_ProjectItems_ParentId",
                        column: x => x.ParentId,
                        principalTable: "ProjectItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Number = table.Column<string>(type: "TEXT", nullable: false),
                    ClientFirstName = table.Column<string>(type: "TEXT", nullable: true),
                    ClientSurname = table.Column<string>(type: "TEXT", nullable: true),
                    ClientPhone = table.Column<string>(type: "TEXT", nullable: true),
                    ClientEmail = table.Column<string>(type: "TEXT", nullable: true),
                    ConstructionStreet = table.Column<string>(type: "TEXT", nullable: true),
                    ConstructionPostalCode = table.Column<string>(type: "TEXT", nullable: true),
                    ConstructionCity = table.Column<string>(type: "TEXT", nullable: true),
                    State = table.Column<int>(type: "INTEGER", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BuildingParts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuildingParts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BuildingParts_ProjectItems_Id",
                        column: x => x.Id,
                        principalTable: "ProjectItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cabinets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cabinets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cabinets_ProjectItems_Id",
                        column: x => x.Id,
                        principalTable: "ProjectItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Corridors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Corridors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Corridors_ProjectItems_Id",
                        column: x => x.Id,
                        principalTable: "ProjectItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Devices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Identifier = table.Column<string>(type: "TEXT", nullable: true),
                    Manufacturer = table.Column<int>(type: "INTEGER", nullable: false),
                    OrderNumber = table.Column<string>(type: "TEXT", nullable: true),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    BusType = table.Column<int>(type: "INTEGER", nullable: false),
                    KnxTopologyArea = table.Column<int>(type: "INTEGER", nullable: true),
                    KnxTopologyLine = table.Column<int>(type: "INTEGER", nullable: true),
                    KnxTopologyAddress = table.Column<int>(type: "INTEGER", nullable: true),
                    IsRailMounted = table.Column<bool>(type: "INTEGER", nullable: false),
                    DivisionUnits = table.Column<float>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Devices_ProjectItems_Id",
                        column: x => x.Id,
                        principalTable: "ProjectItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Floors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Floors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Floors_ProjectItems_Id",
                        column: x => x.Id,
                        principalTable: "ProjectItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rooms_ProjectItems_Id",
                        column: x => x.Id,
                        principalTable: "ProjectItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Stairs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stairs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stairs_ProjectItems_Id",
                        column: x => x.Id,
                        principalTable: "ProjectItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Buildings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ProjectId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Buildings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Buildings_ProjectItems_Id",
                        column: x => x.Id,
                        principalTable: "ProjectItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Buildings_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BinaryInputs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Channels = table.Column<int>(type: "INTEGER", nullable: false),
                    ChannelNames = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BinaryInputs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BinaryInputs_Devices_Id",
                        column: x => x.Id,
                        principalTable: "Devices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BlindActuators",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Channels = table.Column<int>(type: "INTEGER", nullable: false),
                    ChannelNames = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlindActuators", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlindActuators_Devices_Id",
                        column: x => x.Id,
                        principalTable: "Devices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BusPowerSupplys",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Power = table.Column<ushort>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusPowerSupplys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BusPowerSupplys_Devices_Id",
                        column: x => x.Id,
                        principalTable: "Devices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DimmingActuators",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Channels = table.Column<int>(type: "INTEGER", nullable: false),
                    ChannelNames = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DimmingActuators", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DimmingActuators_Devices_Id",
                        column: x => x.Id,
                        principalTable: "Devices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HeatingActuators",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Channels = table.Column<int>(type: "INTEGER", nullable: false),
                    ChannelNames = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HeatingActuators", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HeatingActuators_Devices_Id",
                        column: x => x.Id,
                        principalTable: "Devices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SwitchingActuators",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Channels = table.Column<int>(type: "INTEGER", nullable: false),
                    ChannelNames = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SwitchingActuators", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SwitchingActuators_Devices_Id",
                        column: x => x.Id,
                        principalTable: "Devices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Buildings_ProjectId",
                table: "Buildings",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectItems_ParentId",
                table: "ProjectItems",
                column: "ParentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BinaryInputs");

            migrationBuilder.DropTable(
                name: "BlindActuators");

            migrationBuilder.DropTable(
                name: "BuildingParts");

            migrationBuilder.DropTable(
                name: "Buildings");

            migrationBuilder.DropTable(
                name: "BusPowerSupplys");

            migrationBuilder.DropTable(
                name: "Cabinets");

            migrationBuilder.DropTable(
                name: "Corridors");

            migrationBuilder.DropTable(
                name: "DimmingActuators");

            migrationBuilder.DropTable(
                name: "Floors");

            migrationBuilder.DropTable(
                name: "HeatingActuators");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropTable(
                name: "Stairs");

            migrationBuilder.DropTable(
                name: "SwitchingActuators");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "Devices");

            migrationBuilder.DropTable(
                name: "ProjectItems");
        }
    }
}
