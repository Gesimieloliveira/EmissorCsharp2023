using System.Collections.Generic;
using Fusion.Sessao;
using FusionCore.FusionAdm.Financeiro;
using FusionCore.FusionAdm.Financeiro.Flags;
using FusionCore.FusionAdm.Financeiro.Repositorios;
using FusionCore.FusionAdm.Financeiro.Servicos;
using FusionCore.Sessao;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.DocumentoAReceber
{
    public class GridGerenciarAhReceberContexto : ViewModel
    {
        private readonly ISessaoManager _sessaoManager = SessaoSistema.Instancia.SessaoManager;

        public GridGerenciarAhReceberContexto()
        {
            Documentos = new List<ResumoDocumentoReceberDTO>();

            Filtro = new FiltroResumoDocumentoReceberDTO
            {
                SituacaoIgual = Situacao.Aberto
            };
        }

        public FiltroResumoDocumentoReceberDTO Filtro
        {
            get => GetValue<FiltroResumoDocumentoReceberDTO>();
            private set => SetValue(value);
        }

        public IList<ResumoDocumentoReceberDTO> Documentos
        {
            get => GetValue<IList<ResumoDocumentoReceberDTO>>();
            private set => SetValue(value);
        }

        public decimal TotalCancelado
        {
            get => GetValue<decimal>();
            private set => SetValue(value);
        }

        public decimal TotalDosDocumentos
        {
            get => GetValue<decimal>();
            private set => SetValue(value);
        }

        public decimal TotalRecebido
        {
            get => GetValue<decimal>();
            private set => SetValue(value);
        }

        public decimal TotalRestante
        {
            get => GetValue<decimal>();
            private set => SetValue(value);
        }

        public decimal TotalVencido
        {
            get => GetValue<decimal>();
            private set => SetValue(value);
        }

        public decimal TotalAbertoSelecionado
        {
            get => GetValue<decimal>();
            private set => SetValue(value);
        }

        public decimal TotalVencidoSelecionado
        {
            get => GetValue<decimal>();
            private set => SetValue(value);
        }

        public decimal TotalDevedorSelecionado
        {
            get => GetValue<decimal>();
            private set => SetValue(value);
        }

        public void CarregarDocumentos()
        {
            var servico = new ServicoListagemDocumentos(_sessaoManager);
            var documentos = servico.ListarDocumentos(Filtro);

            Documentos = new List<ResumoDocumentoReceberDTO>(documentos);
            TotalizarDocumentos();
        }

        private void TotalizarDocumentos()
        {
            TotalDosDocumentos = 0.00M;
            TotalCancelado = 0.00M;
            TotalRecebido = 0.00M;
            TotalRestante = 0.00M;
            TotalVencido = 0.00M;

            foreach (var d in Documentos)
            {
                TotalDosDocumentos += d.ValorDocumento;
                TotalCancelado += d.EstaCancelado ? d.ValorDocumento : 0;
                TotalRecebido += d.ValorRecebido;
                TotalRestante += d.ValorRestanteCorrigido;
                TotalVencido += d.ValorRestanteVencido;
            }
        }

        public void TotalizarSelecionados(IEnumerable<ResumoDocumentoReceberDTO> items)
        {
            TotalAbertoSelecionado = 0.00M;
            TotalVencidoSelecionado = 0.00M;
            TotalDevedorSelecionado = 0.00M;

            foreach (var i in items)
            {
                if (i.Situacao != Situacao.Aberto)
                {
                    continue;
                }

                TotalAbertoSelecionado += i.ValorRestante;
                TotalVencidoSelecionado += i.ValorRestanteVencido;
                TotalDevedorSelecionado += i.ValorRestanteCorrigido;
            }
        }
    }
}