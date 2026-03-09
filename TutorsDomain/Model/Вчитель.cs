using System;
using System.Collections.Generic;

namespace TutorsDomain.Model;

public partial class Вчитель : Entity
{
    public string Email { get; set; } = null!;

    public string Піб { get; set; } = null!;

    public string? Профіль { get; set; }

    public virtual ICollection<Урок> Урокs { get; set; } = new List<Урок>();

    public virtual ICollection<Учень> Ученьs { get; set; } = new List<Учень>();
}
