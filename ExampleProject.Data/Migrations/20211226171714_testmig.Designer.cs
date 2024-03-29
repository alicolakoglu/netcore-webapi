﻿// <auto-generated />
using System;
using ExampleProject.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ExampleProject.Data.Migrations
{
    [DbContext(typeof(ProjectDBContext))]
    [Migration("20211226171714_testmig")]
    partial class testmig
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("ExampleProject.Core.Domain.Common.AppMenu", b =>
                {
                    b.Property<int>("ID")
                        .HasColumnType("int");

                    b.Property<string>("Href")
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("Icon")
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<int?>("ParentId")
                        .HasColumnType("int");

                    b.Property<int>("SortIndex")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(150)");

                    b.HasKey("ID");

                    b.ToTable("Common_AppMenu");
                });

            modelBuilder.Entity("ExampleProject.Core.Domain.Todo.TodoItem", b =>
                {
                    b.Property<Guid>("Key")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("newsequentialid()");

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<Guid>("CreatorUserKey")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsCompleted")
                        .HasColumnType("bit");

                    b.HasKey("Key");

                    b.HasIndex("CreatorUserKey");

                    b.ToTable("Todo_Item");
                });

            modelBuilder.Entity("ExampleProject.Core.Domain.Users.User", b =>
                {
                    b.Property<Guid>("Key")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("newsequentialid()");

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<string>("DisplayName")
                        .HasColumnType("varchar(150)");

                    b.Property<string>("Email")
                        .HasColumnType("varchar(150)");

                    b.Property<bool>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValueSql("1");

                    b.Property<bool>("IsAdministrator")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValueSql("0");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValueSql("0");

                    b.Property<string>("Password")
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Username")
                        .HasColumnType("varchar(20)");

                    b.HasKey("Key");

                    b.ToTable("Users_User");
                });

            modelBuilder.Entity("ExampleProject.Core.Domain.Todo.TodoItem", b =>
                {
                    b.HasOne("ExampleProject.Core.Domain.Users.User", "CreatorUser")
                        .WithMany()
                        .HasForeignKey("CreatorUserKey")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("CreatorUser");
                });
#pragma warning restore 612, 618
        }
    }
}
