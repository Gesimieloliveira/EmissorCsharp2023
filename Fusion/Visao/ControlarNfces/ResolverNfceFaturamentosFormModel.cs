using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using FusionCore.Cupom.Nfce;
using FusionCore.Cupom.Nfce.Lotes;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.Utils;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.ControlarNfces
{
    public class ResolverNfceFaturamentosFormModel : ViewModel
    {
        private readonly IEnumerable<CupomFiscalSelecionado> _nfcesSelecionadas;
        private int _quantidadeNfce;
        private DateTime? _novaDataEmissao;

        public ResolverNfceFaturamentosFormModel(IEnumerable<CupomFiscalSelecionado> nfcesSelecionadas)
        {
            _nfcesSelecionadas = nfcesSelecionadas;
            QuantidadeNfce = _nfcesSelecionadas.Count();
        }

        public int QuantidadeNfce
        {
            get => _quantidadeNfce;
            set
            {
                _quantidadeNfce = value;
                PropriedadeAlterada();
            }
        }

        public DateTime? NovaDataEmissao
        {
            get => _novaDataEmissao;
            set
            {
                _novaDataEmissao = value;
                PropriedadeAlterada();
            }
        }

        public async void EnviarTodasNFCe()
        {
            RespostaEnvioLote respostaEnvio;

            
            respostaEnvio = new EnviarLoteNFCe(NovaDataEmissao, _nfcesSelecionadas)
                .Enviar();

            var linhasMensagens = new StringBuilder();

            foreach (var respostaEnvioLinhaMensagemSefaz in respostaEnvio.LinhaMensagemSefazes)
            {
                linhasMensagens.Append(respostaEnvioLinhaMensagemSefaz);
                linhasMensagens.Append("\n");
            }

            await Application.Current.Dispatcher.InvokeAsync(() => DialogBox.MostraAviso(linhasMensagens.ToString()));
        }
    }
}