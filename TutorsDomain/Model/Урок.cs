using System;
using System.Collections.Generic;

namespace TutorsDomain.Model;

public partial class Урок : Entity
{

    public int StudentId { get; set; }

    public int TeacherId { get; set; }

    public string Тема { get; set; } = null!;

    public DateOnly Дата { get; set; }

    public string? СтатусУроку { get; set; }

    public int? ТривалістьУроку { get; set; }

    public virtual Учень Student { get; set; } = null!;

    public virtual Вчитель Teacher { get; set; } = null!;

    public virtual ICollection<Завдання> Завданняs { get; set; } = new List<Завдання>();

    public virtual ICollection<Матеріали> Матеріалиs { get; set; } = new List<Матеріали>();
}
