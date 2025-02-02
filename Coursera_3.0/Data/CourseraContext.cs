﻿using System;
using System.Collections.Generic;
using Coursera_3._0.Dto;
using Coursera_3._0.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Coursera_3._0.Data;

public partial class CourseraContext : DbContext
{
    public CourseraContext()
    {
    }

    public CourseraContext(DbContextOptions<CourseraContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Instructor> Instructors { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<StudentsCoursesXref> StudentsCoursesXrefs { get; set; }

    public virtual DbSet<StudentReportDto> StudentReportDto { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=NECVERSAPRO\\SQLEXPRESS;Initial Catalog=coursera;Integrated Security=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Course>(entity =>
        {
            entity.ToTable("courses");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Credit).HasColumnName("credit");
            entity.Property(e => e.InstructorId).HasColumnName("instructor_id");
            entity.Property(e => e.Name)
                .HasMaxLength(150)
                .HasColumnName("name");
            entity.Property(e => e.TimeCreated)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("time_created");
            entity.Property(e => e.TotalTime).HasColumnName("total_time");

            entity.HasOne(d => d.Instructor).WithMany(p => p.Courses)
                .HasForeignKey(d => d.InstructorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_courses_instructors");
        });

        modelBuilder.Entity<Instructor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_teachers");

            entity.ToTable("instructors");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .HasColumnName("last_name");
            entity.Property(e => e.TimeCreated)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("time_created");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.Pin);

            entity.ToTable("students");

            entity.Property(e => e.Pin)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("pin");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .HasColumnName("last_name");
            entity.Property(e => e.TimeCreated)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("time_created");
        });

        modelBuilder.Entity<StudentsCoursesXref>(entity =>
        {
            entity.HasKey(e => new { e.StudentPin, e.CourseId });

            entity.ToTable("students_courses_xref");

            entity.Property(e => e.StudentPin)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("student_pin");
            entity.Property(e => e.CourseId).HasColumnName("course_id");
            entity.Property(e => e.CompletionDate).HasColumnName("completion_date");

            entity.HasOne(d => d.Course).WithMany(p => p.StudentsCoursesXrefs)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_students_courses_xref_courses");

            entity.HasOne(d => d.StudentPinNavigation).WithMany(p => p.StudentsCoursesXrefs)
                .HasForeignKey(d => d.StudentPin)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_students_courses_xref_students");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    internal object FromSqlRaw(string v, SqlParameter studentPinParam, SqlParameter creditParam, SqlParameter startingDateParam, SqlParameter endingDateParam)
    {
        throw new NotImplementedException();
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
