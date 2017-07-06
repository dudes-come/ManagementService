﻿// <auto-generated />
using JoyOI.ManagementService.Model.Enums;
using JoyOI.ManagementService.WebApi.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace JoyOI.ManagementService.WebApi.Migrations
{
    [DbContext(typeof(MigrationJoyOIManagementContext))]
    partial class MigrationJoyOIManagementContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                .HasAnnotation("ProductVersion", "2.0.0-preview2-25794");

            modelBuilder.Entity("JoyOI.ManagementService.Model.Entities.ActorEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Body");

                    b.Property<DateTime>("CreateTime");

                    b.Property<string>("Name");

                    b.Property<DateTime>("UpdateTime");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Actors");
                });

            modelBuilder.Entity("JoyOI.ManagementService.Model.Entities.BlobEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("BlobId");

                    b.Property<byte[]>("Body");

                    b.Property<string>("BodyHash");

                    b.Property<int>("ChunkIndex");

                    b.Property<DateTime>("CreateTime");

                    b.Property<string>("Remark");

                    b.Property<DateTime>("TimeStamp");

                    b.HasKey("Id");

                    b.HasIndex("BlobId");

                    b.HasIndex("BodyHash");

                    b.HasIndex("BlobId", "ChunkIndex")
                        .IsUnique();

                    b.ToTable("Blobs");
                });

            modelBuilder.Entity("JoyOI.ManagementService.Model.Entities.StateMachineEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Body");

                    b.Property<DateTime>("CreateTime");

                    b.Property<string>("Name");

                    b.Property<DateTime>("UpdateTime");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("StateMachine");
                });

            modelBuilder.Entity("JoyOI.ManagementService.Model.Entities.StateMachineInstanceEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CurrentContainer");

                    b.Property<string>("CurrentNode");

                    b.Property<DateTime>("EndTime");

                    b.Property<string>("FromManagementService");

                    b.Property<string>("Name");

                    b.Property<int>("ReRunTimes");

                    b.Property<DateTime>("StartTime");

                    b.Property<int>("Status");

                    b.Property<string>("_CurrentActor")
                        .HasColumnName("CurrentActor");

                    b.Property<string>("_FinishedActors")
                        .HasColumnName("FinishedActors");

                    b.HasKey("Id");

                    b.HasIndex("Name");

                    b.HasIndex("Name", "Status");

                    b.ToTable("StateMachineInstances");
                });
#pragma warning restore 612, 618
        }
    }
}
