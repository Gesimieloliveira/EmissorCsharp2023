using System.Collections.Generic;
using Fusion.FastReport.DataSources;
using FusionCore.Repositorio.Legacy.Ativos.Adm;
using FusionCore.Sessao;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.Dashboard
{
    public class TotalizadoresContexto : ViewModel
    {
        private readonly ISessaoManager _sessaoManager;
        private int _quantidadeClientes;
        private int _quantidadeProdutos;
        private int _quantidadeNfesAutorizadas;
        private int _quantidadeNfesPendentes;
        private decimal _totalPagamentosHoje;
        private decimal _totalPagamentosVencidos;
        private IList<DsDocumentosPagar> _documentosAbertos;

        public TotalizadoresContexto(ISessaoManager sessaoManager)
        {
            _sessaoManager = sessaoManager;
        }

        public int QuantidadeClientes
        {
            get => _quantidadeClientes;
            set
            {
                _quantidadeClientes = value;
                PropriedadeAlterada();
            }
        }

        public int QuantidadeProdutos
        {
            get => _quantidadeProdutos;
            set
            {
                _quantidadeProdutos = value;
                PropriedadeAlterada();
            }
        }

        public int QuantidadeNfesAutorizadas
        {
            get => _quantidadeNfesAutorizadas;
            set
            {
                _quantidadeNfesAutorizadas = value;
                PropriedadeAlterada();
            }
        }

        public int QuantidadeNfesPendentes
        {
            get => _quantidadeNfesPendentes;
            set
            {
                _quantidadeNfesPendentes = value;
                PropriedadeAlterada();
            }
        }

        public decimal TotalPagamentosHoje
        {
            get => _totalPagamentosHoje;
            set
            {
                _totalPagamentosHoje = value;
                PropriedadeAlterada();
            }
        }

        public decimal TotalPagamentosVencidos
        {
            get => _totalPagamentosVencidos;
            set
            {
                _totalPagamentosVencidos = value;
                PropriedadeAlterada();
            }
        }

        public IList<DsDocumentosPagar> DocumentosAbertos
        {
            get => _documentosAbertos;
            set
            {
                _documentosAbertos = value;
                PropriedadeAlterada();

                var totalHoje = 0M;
                var totalVencidos = 0M;

                foreach (var doc in DocumentosAbertos)
                {
                    if (doc.IsVencido)
                    {
                        totalVencidos += doc.ValorRestante;
                        continue;
                    }

                    totalHoje += doc.ValorRestante;
                }

                TotalPagamentosHoje = totalHoje;
                TotalPagamentosVencidos = totalVencidos;
            }
        }

        public void Refresh()
        {
            using (var sessao = _sessaoManager.CriaSessao())
            {
                var repositorio = new DashboardRepositorio(sessao);

                QuantidadeProdutos = repositorio.QuantidadeProdutos();
                QuantidadeClientes = repositorio.QuanitdadeClientes();
                QuantidadeNfesAutorizadas = repositorio.QuantidadeNfesAutorizadas();
                QuantidadeNfesPendentes = repositorio.QuantidadeNfesPendentes();
            }
        }
    }
}