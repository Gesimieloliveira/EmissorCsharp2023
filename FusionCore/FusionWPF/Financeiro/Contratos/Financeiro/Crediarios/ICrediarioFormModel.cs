using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using FusionCore.FusionAdm.Financeiro;

namespace FusionCore.FusionWPF.Financeiro.Contratos.Financeiro.Crediarios
{
    public interface ICrediarioFormModel
    {
        bool Editar { get; set; }
        ICommand CommandButtonLimpar { get; }
        ObservableCollection<IDocumentoReceberModel> Items { get; set; }
        ObservableCollection<ITipoDocumento> TipoDocumentos { get; set; }
        ObservableCollection<CentroLucro> ItemsCentroLucro { get; set; }
        CentroLucro CentroLucro { get; set; }
        ITipoDocumento TipoDocumento { get; set; }
        string Historico { get; set; }
        byte Parcela { get; set; }
        decimal Valor { get; set; }
        event EventHandler<IParcelamento> GerouComSucesso;
        event EventHandler LimpaCampos;
        void GerarParcelas();
        void LimparJanela(object obj);
    }
}