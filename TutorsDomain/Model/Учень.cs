using System;
using System.Collections.Generic;

namespace TutorsDomain.Model;

public partial class Учень : Entity
{
    public int TeacherId { get; set; }

    public string Email { get; set; } = null!;

    public string Піб { get; set; } = null!;

    public string? Клас { get; set; }

    public virtual Вчитель Teacher { get; set; } = null!;

    public virtual ЖурналУспішності? ЖурналУспішності { get; set; }

    public virtual ICollection<Урок> Урокs { get; set; } = new List<Урок>();
}
