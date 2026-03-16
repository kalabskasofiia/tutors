using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace TutorsDomain.Model;

public partial class Завдання : Entity
{
    public int LessonId { get; set; }
    public string? ОписЗавдання { get; set; }
    public DateOnly? Дедлайн { get; set; }
    public DateOnly? ДатаЗдачі { get; set; }
    public string? ТипЗавдання { get; set; }
    public string? ФайлШлях { get; set; }
    public string? ФайлУчня { get; set; } // ← файл зданої роботи

    public virtual Урок Lesson { get; set; } = null!;
    public virtual ICollection<Оцінка> Оцінкаs { get; set; } = new List<Оцінка>();
}