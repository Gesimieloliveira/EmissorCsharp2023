using System.Collections.Generic;
using NHibernate;

namespace FusionWPF.Base.GridPicker.OpcoesBuscas
{
    public interface IOpcaoBusca
    {
        string Watermark { get; }
        IList<T> Listar<T>(string input, ISession sessao);
    }
}