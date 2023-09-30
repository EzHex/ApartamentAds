﻿// <auto-generated />
using System;
using BackendAPI;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BackendAPI.Migrations
{
    [DbContext(typeof(ApartamentAdsDbContext))]
    [Migration("20230927155618_init")]
    partial class init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.22")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("BackendAPI.Models.Apartament", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Area")
                        .HasColumnType("float");

                    b.Property<int>("Floor")
                        .HasColumnType("int");

                    b.Property<byte[]>("Images")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<int>("Number")
                        .HasColumnType("int");

                    b.Property<double>("Rating")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("Apartaments");
                });

            modelBuilder.Entity("BackendAPI.Models.Object", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("RoomId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RoomId");

                    b.ToTable("Objects");
                });

            modelBuilder.Entity("BackendAPI.Models.Room", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("ApartamentId")
                        .HasColumnType("int");

                    b.Property<double>("Grade")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("ApartamentId");

                    b.ToTable("Rooms");
                });

            modelBuilder.Entity("BackendAPI.Models.Object", b =>
                {
                    b.HasOne("BackendAPI.Models.Room", "Room")
                        .WithMany("Objects")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Room");
                });

            modelBuilder.Entity("BackendAPI.Models.Room", b =>
                {
                    b.HasOne("BackendAPI.Models.Apartament", "Apartament")
                        .WithMany("Rooms")
                        .HasForeignKey("ApartamentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Apartament");
                });

            modelBuilder.Entity("BackendAPI.Models.Apartament", b =>
                {
                    b.Navigation("Rooms");
                });

            modelBuilder.Entity("BackendAPI.Models.Room", b =>
                {
                    b.Navigation("Objects");
                });
#pragma warning restore 612, 618
        }
    }
}
