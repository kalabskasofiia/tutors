using System;
using System.Collections.Generic;

namespace TutorsDomain.Model;

public partial class ЖурналУспішності : Entity
{
    public int StudentId { get; set; }

    public virtual Учень Student { get; set; } = null!;

    public virtual ICollection<Оцінка> Оцінкаs { get; set; } = new List<Оцінка>();
}
