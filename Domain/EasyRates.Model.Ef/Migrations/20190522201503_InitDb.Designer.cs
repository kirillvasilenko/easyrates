﻿// <auto-generated />
using System;
using EasyRates.Model.Ef;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace EasyRates.Model.Ef.Migrations
{
    [DbContext(typeof(RatesContext))]
    [Migration("20190522201503_InitDb")]
    partial class InitDb
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("EasyRates.Model.CurrencyRate", b =>
                {
                    b.Property<string>("From");

                    b.Property<string>("To");

                    b.Property<DateTime>("ExpirationTime");

                    b.Property<DateTime>("OriginalPublishedTime");

                    b.Property<string>("ProviderName");

                    b.Property<DateTime>("TimeOfReceipt");

                    b.Property<decimal>("Value");

                    b.HasKey("From", "To");

                    b.ToTable("CurrencyRate");
                });

            modelBuilder.Entity("EasyRates.Model.CurrencyRateHistoryItem", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("ExpirationTime");

                    b.Property<string>("From");

                    b.Property<DateTime>("OriginalPublishedTime");

                    b.Property<string>("ProviderName");

                    b.Property<DateTime>("TimeOfReceipt");

                    b.Property<string>("To");

                    b.Property<decimal>("Value");

                    b.HasKey("Id");

                    b.ToTable("CurrencyRateHistoryItem");
                });
#pragma warning restore 612, 618
        }
    }
}
