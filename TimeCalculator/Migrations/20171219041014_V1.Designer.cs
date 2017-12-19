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
    [Migration("20171219041014_V1")]
    partial class V1
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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

                    b.Property<long>("Iterations");

                    b.Property<TimeSpan>("NormaizedTime");

                    b.Property<bool>("Problem");

                    b.Property<DateTime>("StartTime");

                    b.HasKey("Id");

                    b.ToTable("JopEntities");
                });
#pragma warning restore 612, 618
        }
    }
}
