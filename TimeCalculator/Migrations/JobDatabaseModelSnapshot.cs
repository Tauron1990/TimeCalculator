﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;
using TimeCalculator.Data;

namespace TimeCalculator.Migrations
{
    [DbContext(typeof(JobDatabase))]
    partial class JobDatabaseModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125");

            modelBuilder.Entity("TimeCalculator.Data.JobEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("Amount");

                    b.Property<bool>("BigProblem");

                    b.Property<TimeSpan>("EffectiveTime");

                    b.Property<int?>("IterationTime");

                    b.Property<long>("Iterations");

                    b.Property<int>("Length");

                    b.Property<TimeSpan>("NormaizedTime");

                    b.Property<bool>("Problem");

                    b.Property<int?>("SetupTime");

                    b.Property<double>("Speed");

                    b.Property<DateTime>("StartTime");

                    b.Property<int>("Width");

                    b.HasKey("Id");

                    b.ToTable("JobEntities");
                });

            modelBuilder.Entity("TimeCalculator.Data.SetupEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("SetupType");

                    b.Property<DateTime>("StartTime");

                    b.Property<int>("Value");

                    b.HasKey("Id");

                    b.ToTable("SetupEntities");
                });
#pragma warning restore 612, 618
        }
    }
}
