using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace TutorsDomain.Model;

public partial class Матеріали : Entity
{
    public int LessonId { get; set; }

    [Required(ErrorMessage = "Назва обов'язкова")]
    public string Назва { get; set; } = null!;

    public string? ФайлЗТеорією { get; set; }
    public virtual Урок Lesson { get; set; } = null!;
}