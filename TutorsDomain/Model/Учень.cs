using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace TutorsDomain.Model;

public partial class Учень : Entity
{
    public int TeacherId { get; set; }

    [Required(ErrorMessage = "Email обов'язковий")]
    [EmailAddress(ErrorMessage = "Невірний формат email")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "ПІБ обов'язкове")]
    [RegularExpression(@"^[a-zA-Zа-яА-ЯіІїЇєЄґҐ\s'-]+$",
        ErrorMessage = "ПІБ може містити лише літери")]
    public string Піб { get; set; } = null!;

    [Required(ErrorMessage = "Клас обов'язковий")]
    [RegularExpression(@"^([1-9]|1[0-2])$",
        ErrorMessage = "Клас має бути від 1 до 12")]
    public string? Клас { get; set; }

    public virtual Вчитель Teacher { get; set; } = null!;
    public virtual ЖурналУспішності? ЖурналУспішності { get; set; }
    public virtual ICollection<Урок> Урокs { get; set; } = new List<Урок>();
}