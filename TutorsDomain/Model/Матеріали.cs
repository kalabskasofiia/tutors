using System;
using System.Collections.Generic;

namespace TutorsDomain.Model;

public partial class Матеріали : Entity
{
    public int LessonId { get; set; }

    public string? ФайлЗТеорією { get; set; }

    public virtual Урок Lesson { get; set; } = null!;
}
