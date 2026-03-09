using System;
using System.Collections.Generic;

namespace TutorsDomain.Model;

public partial class Оцінка : Entity
{

    public int AssigmentId { get; set; }

    public int GradebookId { get; set; }

    public decimal? Бал { get; set; }

    public decimal? МаксимальнийБал { get; set; }

    public virtual Завдання Assigment { get; set; } = null!;

    public virtual ЖурналУспішності Gradebook { get; set; } = null!;
}
