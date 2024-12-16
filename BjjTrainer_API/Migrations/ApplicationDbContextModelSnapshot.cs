﻿// <auto-generated />
using System;
using System.Collections.Generic;
using BjjTrainer_API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BjjTrainer_API.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ApplicationUserLessonJoin", b =>
                {
                    b.Property<string>("ApplicationUserId")
                        .HasColumnType("text");

                    b.Property<int>("LessonId")
                        .HasColumnType("integer");

                    b.HasKey("ApplicationUserId", "LessonId");

                    b.HasIndex("LessonId");

                    b.ToTable("ApplicationUserLessonJoin");
                });

            modelBuilder.Entity("BjjTrainer_API.Models.Calendars.CalendarEvent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ApplicationUserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("date");

                    b.Property<DateTime?>("StartDate")
                        .HasColumnType("date");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationUserId");

                    b.ToTable("CalendarEvents");
                });

            modelBuilder.Entity("BjjTrainer_API.Models.Goals.TrainingGoal", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ApplicationUserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("GoalDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Notes")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationUserId");

                    b.ToTable("TrainingGoals");
                });

            modelBuilder.Entity("BjjTrainer_API.Models.Joins.SubLessonMove", b =>
                {
                    b.Property<int>("SubLessonId")
                        .HasColumnType("integer");

                    b.Property<int>("MoveId")
                        .HasColumnType("integer");

                    b.HasKey("SubLessonId", "MoveId");

                    b.HasIndex("MoveId");

                    b.ToTable("SubLessonMoves");
                });

            modelBuilder.Entity("BjjTrainer_API.Models.Joins.TrainingLogMove", b =>
                {
                    b.Property<int>("TrainingLogId")
                        .HasColumnType("integer");

                    b.Property<int>("MoveId")
                        .HasColumnType("integer");

                    b.HasKey("TrainingLogId", "MoveId");

                    b.HasIndex("MoveId");

                    b.ToTable("TrainingLogMoves");
                });

            modelBuilder.Entity("BjjTrainer_API.Models.Joins.UserTrainingGoalMove", b =>
                {
                    b.Property<int>("TrainingGoalId")
                        .HasColumnType("integer");

                    b.Property<int>("MoveId")
                        .HasColumnType("integer");

                    b.HasKey("TrainingGoalId", "MoveId");

                    b.HasIndex("MoveId");

                    b.ToTable("UserTrainingGoalMoves");
                });

            modelBuilder.Entity("BjjTrainer_API.Models.Lessons.Lesson", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Belt")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Difficulty")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Lessons", (string)null);
                });

            modelBuilder.Entity("BjjTrainer_API.Models.Lessons.LessonSection", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Difficulty")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("LessonId")
                        .HasColumnType("integer");

                    b.Property<int>("Order")
                        .HasColumnType("integer");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("LessonId");

                    b.ToTable("LessonSections");
                });

            modelBuilder.Entity("BjjTrainer_API.Models.Lessons.SubLesson", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("DocumentUrl")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<TimeSpan>("Duration")
                        .HasColumnType("interval");

                    b.Property<int>("LessonSectionId")
                        .HasColumnType("integer");

                    b.Property<string>("Notes")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("SkillLevel")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string[]>("Tags")
                        .IsRequired()
                        .HasColumnType("text[]");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("VideoUrl")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("LessonSectionId");

                    b.ToTable("SubLessons", (string)null);
                });

            modelBuilder.Entity("BjjTrainer_API.Models.Moves.Move", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ApplicationUserId")
                        .HasColumnType("text");

                    b.Property<string>("Category")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("Content")
                        .HasMaxLength(5000)
                        .HasColumnType("character varying(5000)");

                    b.Property<string>("CounterStrategies")
                        .HasMaxLength(1000)
                        .HasColumnType("character varying(1000)");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<string>("History")
                        .HasMaxLength(2000)
                        .HasColumnType("character varying(2000)");

                    b.Property<bool?>("LegalInCompetitions")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Scenarios")
                        .HasMaxLength(1000)
                        .HasColumnType("character varying(1000)");

                    b.Property<string>("SkillLevel")
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<string>("StartingPosition")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<List<string>>("Tags")
                        .HasColumnType("text[]");

                    b.Property<int>("TrainingLogCount")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationUserId");

                    b.ToTable("Moves");
                });

            modelBuilder.Entity("BjjTrainer_API.Models.Trainings.TrainingLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ApplicationUserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("Date")
                        .HasColumnType("date");

                    b.Property<string>("Notes")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("RoundsRolled")
                        .HasColumnType("integer");

                    b.Property<string>("SelfAssessment")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Submissions")
                        .HasColumnType("integer");

                    b.Property<int>("Taps")
                        .HasColumnType("integer");

                    b.Property<double>("TrainingTime")
                        .HasColumnType("double precision");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationUserId");

                    b.ToTable("TrainingLogs");
                });

            modelBuilder.Entity("BjjTrainer_API.Models.Users.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<string>("Belt")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("BeltStripes")
                        .HasColumnType("integer");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<DateOnly?>("LastLoginDate")
                        .HasColumnType("date");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("text");

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("text");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("PreferredTrainingStyle")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ProfilePictureUrl")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<int>("TotalRoundsRolled")
                        .HasColumnType("integer");

                    b.Property<int>("TotalSubmissions")
                        .HasColumnType("integer");

                    b.Property<int>("TotalTaps")
                        .HasColumnType("integer");

                    b.Property<double>("TotalTrainingTime")
                        .HasColumnType("double precision");

                    b.Property<int>("TrainingHoursThisWeek")
                        .HasColumnType("integer");

                    b.Property<DateOnly?>("TrainingStartDate")
                        .HasColumnType("date");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("UserName")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("ApplicationUsers");
                });

            modelBuilder.Entity("BjjTrainer_API.Models.Users.RefreshToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("Expires")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("Revoked")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("RefreshTokens");
                });

            modelBuilder.Entity("ApplicationUserLessonJoin", b =>
                {
                    b.HasOne("BjjTrainer_API.Models.Users.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("ApplicationUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BjjTrainer_API.Models.Lessons.Lesson", null)
                        .WithMany()
                        .HasForeignKey("LessonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BjjTrainer_API.Models.Calendars.CalendarEvent", b =>
                {
                    b.HasOne("BjjTrainer_API.Models.Users.ApplicationUser", "ApplicationUser")
                        .WithMany("CalendarEvents")
                        .HasForeignKey("ApplicationUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ApplicationUser");
                });

            modelBuilder.Entity("BjjTrainer_API.Models.Goals.TrainingGoal", b =>
                {
                    b.HasOne("BjjTrainer_API.Models.Users.ApplicationUser", "ApplicationUser")
                        .WithMany("TrainingGoals")
                        .HasForeignKey("ApplicationUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ApplicationUser");
                });

            modelBuilder.Entity("BjjTrainer_API.Models.Joins.SubLessonMove", b =>
                {
                    b.HasOne("BjjTrainer_API.Models.Moves.Move", "Move")
                        .WithMany("SubLessonMoves")
                        .HasForeignKey("MoveId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BjjTrainer_API.Models.Lessons.SubLesson", "SubLesson")
                        .WithMany("SubLessonMoves")
                        .HasForeignKey("SubLessonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Move");

                    b.Navigation("SubLesson");
                });

            modelBuilder.Entity("BjjTrainer_API.Models.Joins.TrainingLogMove", b =>
                {
                    b.HasOne("BjjTrainer_API.Models.Moves.Move", "Move")
                        .WithMany("TrainingLogMoves")
                        .HasForeignKey("MoveId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BjjTrainer_API.Models.Trainings.TrainingLog", "TrainingLog")
                        .WithMany("TrainingLogMoves")
                        .HasForeignKey("TrainingLogId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Move");

                    b.Navigation("TrainingLog");
                });

            modelBuilder.Entity("BjjTrainer_API.Models.Joins.UserTrainingGoalMove", b =>
                {
                    b.HasOne("BjjTrainer_API.Models.Moves.Move", "Move")
                        .WithMany("UserTrainingGoalMoves")
                        .HasForeignKey("MoveId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BjjTrainer_API.Models.Goals.TrainingGoal", "TrainingGoal")
                        .WithMany("UserTrainingGoalMoves")
                        .HasForeignKey("TrainingGoalId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Move");

                    b.Navigation("TrainingGoal");
                });

            modelBuilder.Entity("BjjTrainer_API.Models.Lessons.LessonSection", b =>
                {
                    b.HasOne("BjjTrainer_API.Models.Lessons.Lesson", "Lesson")
                        .WithMany("LessonSections")
                        .HasForeignKey("LessonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Lesson");
                });

            modelBuilder.Entity("BjjTrainer_API.Models.Lessons.SubLesson", b =>
                {
                    b.HasOne("BjjTrainer_API.Models.Lessons.LessonSection", "LessonSection")
                        .WithMany("SubLessons")
                        .HasForeignKey("LessonSectionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("LessonSection");
                });

            modelBuilder.Entity("BjjTrainer_API.Models.Moves.Move", b =>
                {
                    b.HasOne("BjjTrainer_API.Models.Users.ApplicationUser", null)
                        .WithMany("Moves")
                        .HasForeignKey("ApplicationUserId");
                });

            modelBuilder.Entity("BjjTrainer_API.Models.Trainings.TrainingLog", b =>
                {
                    b.HasOne("BjjTrainer_API.Models.Users.ApplicationUser", "ApplicationUser")
                        .WithMany("TrainingLogs")
                        .HasForeignKey("ApplicationUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ApplicationUser");
                });

            modelBuilder.Entity("BjjTrainer_API.Models.Users.RefreshToken", b =>
                {
                    b.HasOne("BjjTrainer_API.Models.Users.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("BjjTrainer_API.Models.Goals.TrainingGoal", b =>
                {
                    b.Navigation("UserTrainingGoalMoves");
                });

            modelBuilder.Entity("BjjTrainer_API.Models.Lessons.Lesson", b =>
                {
                    b.Navigation("LessonSections");
                });

            modelBuilder.Entity("BjjTrainer_API.Models.Lessons.LessonSection", b =>
                {
                    b.Navigation("SubLessons");
                });

            modelBuilder.Entity("BjjTrainer_API.Models.Lessons.SubLesson", b =>
                {
                    b.Navigation("SubLessonMoves");
                });

            modelBuilder.Entity("BjjTrainer_API.Models.Moves.Move", b =>
                {
                    b.Navigation("SubLessonMoves");

                    b.Navigation("TrainingLogMoves");

                    b.Navigation("UserTrainingGoalMoves");
                });

            modelBuilder.Entity("BjjTrainer_API.Models.Trainings.TrainingLog", b =>
                {
                    b.Navigation("TrainingLogMoves");
                });

            modelBuilder.Entity("BjjTrainer_API.Models.Users.ApplicationUser", b =>
                {
                    b.Navigation("CalendarEvents");

                    b.Navigation("Moves");

                    b.Navigation("TrainingGoals");

                    b.Navigation("TrainingLogs");
                });
#pragma warning restore 612, 618
        }
    }
}
