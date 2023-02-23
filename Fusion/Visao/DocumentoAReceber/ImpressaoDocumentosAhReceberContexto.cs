using System;
using System.Collections.Generic;
using System.Linq;
using Fusion.Impressoes.Financeiro;
using Fusion.Sessao;
using FusionCore.Impressoras;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.DocumentoAReceber
{
    public class ImpressaoDocumentosAhReceberContexto : ViewModel
    {
        public ImpressaoDocumentosAhReceberContexto()
        {
            PreVisualizar = true;
        }

        public bool PreVisualizar
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public IEnumerable<string> Impressoras
        {
            get => GetValue<IEnumerable<string>>();
            set => SetValue(value);
        }

        public string ImpressoraSelecionada
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public IList<IOpcaoImpressao> OpcoesImpressao
        {
            get => GetValue<IList<IOpcaoImpressao>>();
            set => SetValue(value);
        }

        public IOpcaoImpressao OpcaoSelecionada
        {
            get => GetValue<IOpcaoImpressao>();
            set => SetValue(value);
        }

        public int? NumeroDoMalote
        {
            get => GetValue<int?>();
            set => SetValue(value);
        }

        public void CarregarDados()
        {
            Impressoras = Impressora.ObterImpressorasDoComputador();
            OpcoesImpressao = CriarOpcoesDeImpressao();
            OpcaoSelecionada = OpcoesImpressao.FirstOrDefault();
        }

        private IList<IOpcaoImpressao> CriarOpcoesDeImpressao()
        {
            return new List<IOpcaoImpressao>
            {
                new ImpressaoPromissoriaComCarne(SessaoSistema.Instancia.SessaoManager),
                new ImpressaoDePromissorias(SessaoSistema.Instancia.SessaoManager)
            };
        }

        public void FazerImpresao()
        {
            if (PreVisualizar == false && string.IsNullOrEmpty(ImpressoraSelecionada))
            {
                throw new InvalidOperationException("Preciso de uma impressora quando a opção de Pre Visualizar for Não.");
            }

            if (NumeroDoMalote == null || NumeroDoMalote.Value == 0)
            {
                throw new InvalidOperationException("Preciso que informe o Número do Malote para imprimir os documentos vinculados.");
            }

            if (OpcaoSelecionada == null)
            {
                throw new InvalidOperationException("Preciso que escolha um Modelo de Impressão.");
            }

            OpcaoSelecionada.PreVisualizar = PreVisualizar;
            OpcaoSelecionada.Impresora = ImpressoraSelecionada;

            OpcaoSelecionada.FazerImpressao(NumeroDoMalote.Value);
        }
    }
}