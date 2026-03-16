using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace TutorsDomain.Model;

public partial class Урок : Entity
{
    public int StudentId { get; set; }
    public int TeacherId { get; set; }

    [Required(ErrorMessage = "Тема обов'язкова")]
    public string Тема { get; set; } = null!;

    [Required(ErrorMessage = "Дата обов'язкова")]
    public DateOnly Дата { get; set; }

    public string? СтатусУроку { get; set; }

    [Range(1, 120, ErrorMessage = "Тривалість має бути від 1 до 120 хвилин")]
    public int? ТривалістьУроку { get; set; }

    public virtual Учень Student { get; set; } = null!;
    public virtual Вчитель Teacher { get; set; } = null!;
    public virtual ICollection<Завдання> Завданняs { get; set; } = new List<Завдання>();
    public virtual ICollection<Матеріали> Матеріалиs { get; set; } = new List<Матеріали>();
}