using System;
using System.Collections.ObjectModel;
using System.Linq;
using Fusion.Visao.MdfeEletronico.IncluirPagamento;
using FusionCore.FusionAdm.MdfeEletronico.Autorizador;
using FusionCore.FusionAdm.MdfeEletronico.EventoPagamento.Entidades;
using FusionCore.Helpers.Hidratacao;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.MdfeEletronico.Aba.IncluirPagamento
{
    public class IncluirPagamentoRetorno
    {
        public MdfeAutorizacaoInformacaoPagamento Novo { get; }
        public MdfeAutorizacaoInformacaoPagamento Remover { get; }

        public IncluirPagamentoRetorno(MdfeAutorizacaoInformacaoPagamento novo,
            MdfeAutorizacaoInformacaoPagamento remover)
        {
            Novo = novo;
            Remover = remover;
        }
    }

    public class IncluirPagamentoFormModel : ViewModel
    {
        private readonly MdfeAutorizacaoInformacaoPagamento _informacaoPagamento;

        public IncluirPagamentoFormModel(MdfeAutorizacaoInformacaoPagamento informacaoPagamento)
        {
            _informacaoPagamento = informacaoPagamento;
        }

        private bool _informarPenasCnpjIpef;
        private IndicadorPagamento _indicadorPagamento = IndicadorPagamento.PagamentoAVista;
        private bool _aPrazo;
        private TipoComponente _tipoComponente = TipoComponente.ValePedagio;
        private decimal _valorComponente;
        private bool _outrosComponentes;
        private string _descricaoOutrosComponentes;
        private decimal _valorTotalContrato;
        private string _nomeResponsavelPeloPagamento;
        private string _documentoUnicoResponsavelPagamento;
        private string _cnpjIpef;
        private ObservableCollection<ParcelaDTO> _parcelasPagamento = new ObservableCollection<ParcelaDTO>();
        private ObservableCollection<TipoComponenteDTO> _componentesFrete = new ObservableCollection<TipoComponenteDTO>();
        private TipoComponenteDTO _componenteSelecionado;
        private string _numeroBanco;
        private string _agenciaBancaria;
        public event EventHandler<IncluirPagamentoFormModel> IncluidoPagamento;

        public bool EUmaEdicao { get; set; }

        public bool InformarPenasCnpjIpef
        {
            get => _informarPenasCnpjIpef;
            set
            {
                _informarPenasCnpjIpef = value;
                PropriedadeAlterada();

                AgenciaBancaria = string.Empty;
                NumeroBanco = string.Empty;
                CnpjIpef = string.Empty;
            }
        }

        public IndicadorPagamento IndicadorPagamento
        {
            get => _indicadorPagamento;
            set
            {
                _indicadorPagamento = value;
                PropriedadeAlterada();
                APrazo = value == IndicadorPagamento.PagamentoAPrazo;

                LimparParcelas();
            }
        }

        private void LimparParcelas()
        {
            // todo limpar parcelas

            ParcelasPagamento.Clear();
        }

        public bool APrazo
        {
            get => _aPrazo;
            set
            {
                _aPrazo = value;
                PropriedadeAlterada();
            }
        }

        public TipoComponente TipoComponente
        {
            get => _tipoComponente;
            set
            {
                _tipoComponente = value;
                PropriedadeAlterada();
                OutrosComponentes = value == TipoComponente.Outros;
            }
        }

        public decimal ValorComponente
        {
            get => _valorComponente;
            set
            {
                _valorComponente = value;
                PropriedadeAlterada();
            }
        }

        public bool OutrosComponentes
        {
            get => _outrosComponentes;
            set
            {
                _outrosComponentes = value;
                PropriedadeAlterada();
            }
        }

        public string DescricaoOutrosComponentes
        {
            get => _descricaoOutrosComponentes;
            set
            {
                _descricaoOutrosComponentes = value;
                PropriedadeAlterada();
            }
        }

        public decimal ValorTotalContrato
        {
            get => _valorTotalContrato;
            set
            {
                _valorTotalContrato = value;
                PropriedadeAlterada();
            }
        }

        public string NomeResponsavelPeloPagamento
        {
            get => _nomeResponsavelPeloPagamento;
            set
            {
                _nomeResponsavelPeloPagamento = value;
                PropriedadeAlterada();
            }
        }

        public string DocumentoUnicoResponsavelPagamento
        {
            get => _documentoUnicoResponsavelPagamento;
            set
            {
                _documentoUnicoResponsavelPagamento = value;
                PropriedadeAlterada();
            }
        }

        public string CnpjIpef
        {
            get => _cnpjIpef;
            set
            {
                _cnpjIpef = value;
                PropriedadeAlterada();
            }
        }

        public TipoComponenteDTO ComponenteSelecionado
        {
            get => _componenteSelecionado;
            set
            {
                _componenteSelecionado = value;
                PropriedadeAlterada();
            }
        }

        public ObservableCollection<ParcelaDTO> ParcelasPagamento
        {
            get => _parcelasPagamento;
            set
            {
                _parcelasPagamento = value;
                PropriedadeAlterada();
            }
        }

        public string NumeroBanco
        {
            get => _numeroBanco;
            set
            {
                _numeroBanco = value;
                PropriedadeAlterada();
            }
        }

        public string AgenciaBancaria
        {
            get => _agenciaBancaria;
            set
            {
                _agenciaBancaria = value;
                PropriedadeAlterada();
            }
        }

        public ObservableCollection<TipoComponenteDTO> ComponentesFrete
        {
            get => _componentesFrete;
            set
            {
                _componentesFrete = value;
                PropriedadeAlterada();
            }
        }

        public void AdicionarComponente()
        {
            DescricaoOutrosComponentes = DescricaoOutrosComponentes.TrimOrEmpty();

            if (TipoComponente == TipoComponente.Outros && DescricaoOutrosComponentes.IsNullOrEmpty())
                throw new InvalidOperationException("Quando tipo componente for outros, deve-se adicionar descrição outros");


            ComponentesFrete.Add(new TipoComponenteDTO
            {
                TipoComponente = TipoComponente,
                ValorComponente = ValorComponente,
                DescricaoOutros = DescricaoOutrosComponentes
            });

            TipoComponente = TipoComponente.ValePedagio;
            ValorComponente = 0.0m;
            DescricaoOutrosComponentes = string.Empty;
        }

        public void DeletarComponente()
        {
            ComponentesFrete.Remove(ComponenteSelecionado);
        }

        protected virtual void OnIncluidoPagamento()
        {
            IncluidoPagamento?.Invoke(this, this);
        }

        public void IncluirPagamento()
        {
            NomeResponsavelPeloPagamento = NomeResponsavelPeloPagamento.TrimOrEmpty();
            DocumentoUnicoResponsavelPagamento = DocumentoUnicoResponsavelPagamento.TrimOrEmpty();
            CnpjIpef = CnpjIpef.TrimOrEmpty();
            NumeroBanco = NumeroBanco.TrimOrEmpty();
            AgenciaBancaria = AgenciaBancaria.TrimOrEmpty();

            if (DocumentoUnicoResponsavelPagamento.Length < 2)
                throw new InvalidOperationException("Documento do contratante/responsável inválido");

            if (InformarPenasCnpjIpef)
            {
                if (CnpjIpef.IsNullOrEmpty())
                    throw new InvalidOperationException("Adicionar Cnpj da Instituição de pagamento Eletrônico do Frete");
            }

            if (InformarPenasCnpjIpef == false)
            {
                if (NumeroBanco.IsNullOrEmpty())
                    throw new InvalidOperationException("Informar o número do banco");

                if (NumeroBanco.Length < 3)
                    throw new InvalidOperationException("Número do banco não pode ser menor que 3");

                if (AgenciaBancaria.IsNullOrEmpty())
                    throw new InvalidOperationException("Informar agencia bancária");
            }

            OnIncluidoPagamento();
            OnFechar();
        }

        public IncluirPagamentoRetorno IncluirPagamentoRetorno()
        {
            var informacaoPagamento = new MdfeAutorizacaoInformacaoPagamento();


            foreach (var tipoComponenteDTO in ComponentesFrete)
            {
                informacaoPagamento.ComponentePagamentoFrete.Add(new MdfeAutorizacaoComponentePagamentoFrete
                {
                    TipoComponente = tipoComponenteDTO.TipoComponente,
                    Valor = tipoComponenteDTO.ValorComponente,
                    Descricao = tipoComponenteDTO.DescricaoOutros,
                    InformacaoPagamento = informacaoPagamento
                });
            }

            informacaoPagamento.NomeContratante = NomeResponsavelPeloPagamento;
            informacaoPagamento.DocumentoUnicoContratante = DocumentoUnicoResponsavelPagamento;

            informacaoPagamento.IndicadorPagamento = IndicadorPagamento;
            informacaoPagamento.ValorTotalContrato = ValorTotalContrato;

            foreach (var parcelaDTO in ParcelasPagamento)
            {
                informacaoPagamento.Parcelas.Add(new MdfeAutorizacaoParcela
                {
                    DataDeVencimento = parcelaDTO.VencimentoEm,
                    Numero = parcelaDTO.Numero,
                    Valor = parcelaDTO.Valor,
                    InformacaoPagamento = informacaoPagamento
                });
            }

            informacaoPagamento.CnpjIpef = CnpjIpef;
            informacaoPagamento.AgenciaBancaria = AgenciaBancaria;
            informacaoPagamento.ContaBancaria = NumeroBanco;
            informacaoPagamento.InformarApenasCnpjIpef = InformarPenasCnpjIpef;

            return new IncluirPagamentoRetorno(informacaoPagamento, _informacaoPagamento);
        }

        public void Inicializar()
        {
            if (_informacaoPagamento != null)
                EUmaEdicao = true;

            if (EUmaEdicao)
            {
                NomeResponsavelPeloPagamento = _informacaoPagamento.NomeContratante;
                DocumentoUnicoResponsavelPagamento = _informacaoPagamento.DocumentoUnicoContratante;
                IndicadorPagamento = _informacaoPagamento.IndicadorPagamento;
                ValorTotalContrato = _informacaoPagamento.ValorTotalContrato;
                InformarPenasCnpjIpef = _informacaoPagamento.InformarApenasCnpjIpef;

                NumeroBanco = _informacaoPagamento.ContaBancaria;
                AgenciaBancaria = _informacaoPagamento.AgenciaBancaria;

                if (InformarPenasCnpjIpef)
                {
                    NumeroBanco = string.Empty;
                    AgenciaBancaria = string.Empty;
                    CnpjIpef = _informacaoPagamento.CnpjIpef;
                }

                var parcelas = _informacaoPagamento.Parcelas.Select(x => new ParcelaDTO
                {
                    Valor = x.Valor,
                    Numero = x.Numero,
                    VencimentoEm = x.DataDeVencimento,
                    MdfeParcelaAba = x
                }).ToList();

                ParcelasPagamento = new ObservableCollection<ParcelaDTO>(parcelas);

                var componentes = _informacaoPagamento
                    .ComponentePagamentoFrete
                    .Select(x => new TipoComponenteDTO
                    {
                        TipoComponente = x.TipoComponente,
                        ValorComponente = x.Valor,
                        DescricaoOutros = x.Descricao,
                        Id = x.Id,
                        MdfeComponenteAba = x
                    })
                    .ToList();

                ComponentesFrete = new ObservableCollection<TipoComponenteDTO>(componentes);
            }
        }

        public MdfeAutorizacaoInformacaoPagamento ObterInformacaoPagamentoEditavel()
        {
            return _informacaoPagamento;
        }
    }
}