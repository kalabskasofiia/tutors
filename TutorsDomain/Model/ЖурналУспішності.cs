using System;
using System.Collections.Generic;

namespace TutorsDomain.Model;

public partial class ЖурналУспішності : Entity
{
    public int AssigmentId { get; set; }

    public int StudentId { get; set; }

    public virtual Завдання Assigment { get; set; } = null!;

    public virtual Учень Student { get; set; } = null!;

    public virtual ICollection<Оцінка> Оцінкаs { get; set; } = new List<Оцінка>();
}
