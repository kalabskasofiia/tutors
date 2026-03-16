using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace TutorsDomain.Model;

public partial class Вчитель : Entity
{
    [Required(ErrorMessage = "Email обов'язковий")]
    [EmailAddress(ErrorMessage = "Невірний формат email")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "ПІБ обов'язкове")]
    public string Піб { get; set; } = null!;

    [Required(ErrorMessage = "Профіль обов'язковий")]
    public string Профіль { get; set; } = null!;

    public virtual ICollection<Урок> Урокs { get; set; } = new List<Урок>();
    public virtual ICollection<Учень> Ученьs { get; set; } = new List<Учень>();
}