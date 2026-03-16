using System;
using System.Collections.Generic;
using TutorsDomain.Model;
using Microsoft.EntityFrameworkCore;

namespace TutorsInfrastructure;

public partial class ІстпContext : DbContext
{
    public ІстпContext()
    {
    }

    public ІстпContext(DbContextOptions<ІстпContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Вчитель> Вчительs { get; set; }
    public virtual DbSet<ЖурналУспішності> ЖурналУспішностіs { get; set; }
    public virtual DbSet<Завдання> Завданняs { get; set; }
    public virtual DbSet<Матеріали> Матеріалиs { get; set; }
    public virtual DbSet<Оцінка> Оцінкаs { get; set; }
    public virtual DbSet<Урок> Урокs { get; set; }
    public virtual DbSet<Учень> Ученьs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Вчитель>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Вчитель_pkey");
            entity.ToTable("Вчитель");
            entity.HasIndex(e => e.Email, "Вчитель_email_key").IsUnique();
            entity.Property(e => e.Id)
                .UseSerialColumn()
                .HasColumnName("teacher_id");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Профіль).HasMaxLength(255);
            entity.Property(e => e.Піб)
                .HasMaxLength(255)
                .HasColumnName("ПІБ");
        });

        modelBuilder.Entity<ЖурналУспішності>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Журнал_успішності_pkey");
            entity.ToTable("Журнал_успішності");
            entity.HasIndex(e => e.StudentId, "uq_журнал_учень").IsUnique();
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("gradebook_id");
            entity.Property(e => e.StudentId).HasColumnName("student_id");

            entity.HasOne(d => d.Student).WithOne(p => p.ЖурналУспішності)
                .HasForeignKey<ЖурналУспішності>(d => d.StudentId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_журнал_учень");
        });

        modelBuilder.Entity<Завдання>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Завдання_pkey");
            entity.ToTable("Завдання");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("assigment_id");
            entity.Property(e => e.LessonId).HasColumnName("lesson_id");
            entity.Property(e => e.ДатаЗдачі).HasColumnName("Дата_здачі");
            entity.Property(e => e.ОписЗавдання).HasColumnName("Опис_завдання");
            entity.Property(e => e.ТипЗавдання)
                .HasMaxLength(100)
                .HasColumnName("Тип_завдання");
            entity.Property(e => e.ФайлШлях)
                .HasMaxLength(500)
                .HasColumnName("ФайлШлях");
            entity.Property(e => e.ФайлУчня)
                .HasMaxLength(500)
                .HasColumnName("ФайлУчня");

            entity.HasOne(d => d.Lesson).WithMany(p => p.Завданняs)
                .HasForeignKey(d => d.LessonId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_завдання_урок");
        });

        modelBuilder.Entity<Матеріали>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Матеріали_pkey");
            entity.ToTable("Матеріали");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("material_id");
            entity.Property(e => e.LessonId).HasColumnName("lesson_id");
            entity.Property(e => e.Назва)
                .HasMaxLength(255)
                .HasColumnName("Назва");
            entity.Property(e => e.ФайлЗТеорією)
                .HasMaxLength(500)
                .HasColumnName("Файл_з_теорією");

            entity.HasOne(d => d.Lesson).WithMany(p => p.Матеріалиs)
                .HasForeignKey(d => d.LessonId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_матеріали_урок");
        });

        modelBuilder.Entity<Оцінка>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Оцінка_pkey");
            entity.ToTable("Оцінка");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("mark_id");
            entity.Property(e => e.AssigmentId).HasColumnName("assigment_id");
            entity.Property(e => e.GradebookId).HasColumnName("gradebook_id");
            entity.Property(e => e.Бал).HasPrecision(5, 2);
            entity.Property(e => e.МаксимальнийБал)
                .HasPrecision(5, 2)
                .HasColumnName("Максимальний_бал");

            entity.HasOne(d => d.Assigment).WithMany(p => p.Оцінкаs)
                .HasForeignKey(d => d.AssigmentId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_оцінка_завдання");

            entity.HasOne(d => d.Gradebook).WithMany(p => p.Оцінкаs)
                .HasForeignKey(d => d.GradebookId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_оцінка_журнал");
        });

        modelBuilder.Entity<Урок>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Урок_pkey");
            entity.ToTable("Урок");
            entity.Property(e => e.Id)
                .UseSerialColumn()
                .HasColumnName("lesson_id");
            entity.Property(e => e.Дата).HasColumnName("Дата");
            entity.Property(e => e.StudentId).HasColumnName("student_id");
            entity.Property(e => e.TeacherId).HasColumnName("teacher_id");
            entity.Property(e => e.СтатусУроку)
                .HasMaxLength(100)
                .HasColumnName("Статус_уроку");
            entity.Property(e => e.Тема).HasMaxLength(255);
            entity.Property(e => e.ТривалістьУроку).HasColumnName("Тривалість_уроку");

            entity.HasOne(d => d.Student).WithMany(p => p.Урокs)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_урок_учень");

            entity.HasOne(d => d.Teacher).WithMany(p => p.Урокs)
                .HasForeignKey(d => d.TeacherId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_урок_вчитель");
        });

        modelBuilder.Entity<Учень>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Учень_pkey");
            entity.ToTable("Учень");
            entity.HasIndex(e => e.Email, "Учень_email_key").IsUnique();
            entity.Property(e => e.Id)
                .UseSerialColumn()
                .HasColumnName("student_id");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.TeacherId).HasColumnName("teacher_id");
            entity.Property(e => e.Клас).HasMaxLength(50);
            entity.Property(e => e.Піб)
                .HasMaxLength(255)
                .HasColumnName("ПІБ");

            entity.HasOne(d => d.Teacher).WithMany(p => p.Ученьs)
                .HasForeignKey(d => d.TeacherId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_учень_вчитель");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}