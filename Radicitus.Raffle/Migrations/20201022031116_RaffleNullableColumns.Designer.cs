﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Radicitus.Data.Contexts.Raffles;

namespace Radicitus.Raffle.Migrations
{
    [DbContext(typeof(RadicitusDbContext))]
    [Migration("20201022031116_RaffleNullableColumns")]
    partial class RaffleNullableColumns
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("rad")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Radicitus.Data.Contexts.Raffles.Entities.RadRaffle", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<decimal?>("AmountWon")
                        .HasColumnName("amount_won")
                        .HasColumnType("numeric");

                    b.Property<DateTime>("DateCreatedUtc")
                        .HasColumnName("date_created_utc")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("EndDateUtc")
                        .HasColumnName("end_date_utc")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("RaffleName")
                        .HasColumnName("raffle_name")
                        .HasColumnType("character varying(128)")
                        .HasMaxLength(128);

                    b.Property<decimal>("SquareWorthAmount")
                        .HasColumnName("square_worth_amount")
                        .HasColumnType("numeric");

                    b.Property<DateTime>("StartDateUtc")
                        .HasColumnName("start_date_utc")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("WinnerName")
                        .HasColumnName("winner_name")
                        .HasColumnType("character varying(128)")
                        .HasMaxLength(128);

                    b.Property<int?>("WinningSquare")
                        .HasColumnName("winning_square")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("rad_raffle");
                });

            modelBuilder.Entity("Radicitus.Data.Contexts.Raffles.Entities.RaffleNumber", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Name")
                        .HasColumnName("name")
                        .HasColumnType("character varying(128)")
                        .HasMaxLength(128);

                    b.Property<int>("Number")
                        .HasColumnName("number")
                        .HasColumnType("integer");

                    b.Property<int>("RaffleId")
                        .HasColumnName("raffle_id")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("RaffleId");

                    b.ToTable("raffle_number");
                });

            modelBuilder.Entity("Radicitus.Data.Contexts.Raffles.Entities.RaffleNumber", b =>
                {
                    b.HasOne("Radicitus.Data.Contexts.Raffles.Entities.RadRaffle", null)
                        .WithMany("RaffleNumbers")
                        .HasForeignKey("Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Radicitus.Data.Contexts.Raffles.Entities.RadRaffle", "Raffle")
                        .WithMany()
                        .HasForeignKey("RaffleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
