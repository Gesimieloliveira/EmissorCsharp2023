using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fusion.Controles.Utilitarios.ComboBox.Dados
{
    internal interface IDadosCombobox
    {
        string NomeMembroExibicao { get; }
        Task<IEnumerable<object>> DadosAsync();
    }
}