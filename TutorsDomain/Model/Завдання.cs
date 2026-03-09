using System;
using System.Collections.Generic;

namespace TutorsDomain.Model;

public partial class Завдання : Entity
{
    public int LessonId { get; set; }

    public string? ОписЗавдання { get; set; }

    public DateOnly? Дедлайн { get; set; }

    public DateOnly? ДатаЗдачі { get; set; }

    public string? ТипЗавдання { get; set; }

    public virtual Урок Lesson { get; set; } = null!;

    public virtual ICollection<ЖурналУспішності> ЖурналУспішностіs { get; set; } = new List<ЖурналУспішності>();

    public virtual ICollection<Оцінка> Оцінкаs { get; set; } = new List<Оцінка>();
}
