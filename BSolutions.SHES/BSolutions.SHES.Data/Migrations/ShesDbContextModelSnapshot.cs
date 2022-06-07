﻿// <auto-generated />
using System;
using BSolutions.SHES.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BSolutions.SHES.Data.Migrations
{
    [DbContext(typeof(ShesDbContext))]
    partial class ShesDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.5");

            modelBuilder.Entity("BSolutions.SHES.Models.Entities.Project", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("ClientEmail")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClientFirstName")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClientPhone")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClientSurname")
                        .HasColumnType("TEXT");

                    b.Property<string>("ConstructionCity")
                        .HasColumnType("TEXT");

                    b.Property<string>("ConstructionPostalCode")
                        .HasColumnType("TEXT");

                    b.Property<string>("ConstructionStreet")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("LastModificationTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Number")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("State")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Projects", (string)null);
                });

            modelBuilder.Entity("BSolutions.SHES.Models.Entities.ProjectItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Comment")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("LastModificationTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("ParentId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ParentId");

                    b.ToTable("ProjectItems", (string)null);
                });

            modelBuilder.Entity("BSolutions.SHES.Models.Entities.Building", b =>
                {
                    b.HasBaseType("BSolutions.SHES.Models.Entities.ProjectItem");

                    b.Property<Guid?>("ProjectId")
                        .HasColumnType("TEXT");

                    b.HasIndex("ProjectId");

                    b.ToTable("Buildings", (string)null);
                });

            modelBuilder.Entity("BSolutions.SHES.Models.Entities.BuildingPart", b =>
                {
                    b.HasBaseType("BSolutions.SHES.Models.Entities.ProjectItem");

                    b.ToTable("BuildingParts", (string)null);
                });

            modelBuilder.Entity("BSolutions.SHES.Models.Entities.Cabinet", b =>
                {
                    b.HasBaseType("BSolutions.SHES.Models.Entities.ProjectItem");

                    b.ToTable("Cabinets", (string)null);
                });

            modelBuilder.Entity("BSolutions.SHES.Models.Entities.Corridor", b =>
                {
                    b.HasBaseType("BSolutions.SHES.Models.Entities.ProjectItem");

                    b.ToTable("Corridors", (string)null);
                });

            modelBuilder.Entity("BSolutions.SHES.Models.Entities.Device", b =>
                {
                    b.HasBaseType("BSolutions.SHES.Models.Entities.ProjectItem");

                    b.Property<int>("BusType")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.ToTable("Devices", (string)null);
                });

            modelBuilder.Entity("BSolutions.SHES.Models.Entities.Floor", b =>
                {
                    b.HasBaseType("BSolutions.SHES.Models.Entities.ProjectItem");

                    b.ToTable("Floors", (string)null);
                });

            modelBuilder.Entity("BSolutions.SHES.Models.Entities.Room", b =>
                {
                    b.HasBaseType("BSolutions.SHES.Models.Entities.ProjectItem");

                    b.ToTable("Rooms", (string)null);
                });

            modelBuilder.Entity("BSolutions.SHES.Models.Entities.Stair", b =>
                {
                    b.HasBaseType("BSolutions.SHES.Models.Entities.ProjectItem");

                    b.ToTable("Stairs", (string)null);
                });

            modelBuilder.Entity("BSolutions.SHES.Models.Entities.ProjectItem", b =>
                {
                    b.HasOne("BSolutions.SHES.Models.Entities.ProjectItem", "Parent")
                        .WithMany("Children")
                        .HasForeignKey("ParentId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Parent");
                });

            modelBuilder.Entity("BSolutions.SHES.Models.Entities.Building", b =>
                {
                    b.HasOne("BSolutions.SHES.Models.Entities.ProjectItem", null)
                        .WithOne()
                        .HasForeignKey("BSolutions.SHES.Models.Entities.Building", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BSolutions.SHES.Models.Entities.Project", "Project")
                        .WithMany("Buildings")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Project");
                });

            modelBuilder.Entity("BSolutions.SHES.Models.Entities.BuildingPart", b =>
                {
                    b.HasOne("BSolutions.SHES.Models.Entities.ProjectItem", null)
                        .WithOne()
                        .HasForeignKey("BSolutions.SHES.Models.Entities.BuildingPart", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BSolutions.SHES.Models.Entities.Cabinet", b =>
                {
                    b.HasOne("BSolutions.SHES.Models.Entities.ProjectItem", null)
                        .WithOne()
                        .HasForeignKey("BSolutions.SHES.Models.Entities.Cabinet", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BSolutions.SHES.Models.Entities.Corridor", b =>
                {
                    b.HasOne("BSolutions.SHES.Models.Entities.ProjectItem", null)
                        .WithOne()
                        .HasForeignKey("BSolutions.SHES.Models.Entities.Corridor", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BSolutions.SHES.Models.Entities.Device", b =>
                {
                    b.HasOne("BSolutions.SHES.Models.Entities.ProjectItem", null)
                        .WithOne()
                        .HasForeignKey("BSolutions.SHES.Models.Entities.Device", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BSolutions.SHES.Models.Entities.Floor", b =>
                {
                    b.HasOne("BSolutions.SHES.Models.Entities.ProjectItem", null)
                        .WithOne()
                        .HasForeignKey("BSolutions.SHES.Models.Entities.Floor", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BSolutions.SHES.Models.Entities.Room", b =>
                {
                    b.HasOne("BSolutions.SHES.Models.Entities.ProjectItem", null)
                        .WithOne()
                        .HasForeignKey("BSolutions.SHES.Models.Entities.Room", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BSolutions.SHES.Models.Entities.Stair", b =>
                {
                    b.HasOne("BSolutions.SHES.Models.Entities.ProjectItem", null)
                        .WithOne()
                        .HasForeignKey("BSolutions.SHES.Models.Entities.Stair", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BSolutions.SHES.Models.Entities.Project", b =>
                {
                    b.Navigation("Buildings");
                });

            modelBuilder.Entity("BSolutions.SHES.Models.Entities.ProjectItem", b =>
                {
                    b.Navigation("Children");
                });
#pragma warning restore 612, 618
        }
    }
}
