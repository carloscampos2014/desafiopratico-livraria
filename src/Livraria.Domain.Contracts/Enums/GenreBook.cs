using System.ComponentModel;

namespace Livraria.Domain.Contracts.Enums;
public enum GenreBook
{
    [Description("Aventura")]
    Adventure = 0,

    [Description("Romance")]
    Romance = 1,

    [Description("Auto-Ajuda")]
    SelfHelp = 2,

    [Description("Ficção Científica")]
    Sciencefiction = 3,
}
