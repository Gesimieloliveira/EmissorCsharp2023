using System;
using FusionCore.Cupom.Nfce;
using FusionLibrary.VisaoModel;
using SituacaoFiscal = FusionCore.Vendas.Autorizadores.Nfce.SituacaoFiscal;

namespace Fusion.Visao.ControlarNfces
{
    public class FiltroCupomFiscalDto : ViewModel, IFiltroCupomFiscalDto
    {
        private DateTime? _emitidasIgualOuApos;
        private int? _numeroIgual;
        private int? _codigoIdIgualA;
        private string _nomeEmpresaContenha;
        private string _nomeClienteContenha;
        private TipoEmissaoCupomFiscal? _tipoEmissao;
        private SituacaoFiscal? _situacaoFiscal;

        public DateTime? EmitidasIgualOuApos
        {
            get => _emitidasIgualOuApos;
            set
            {
                _emitidasIgualOuApos = value;
                PropriedadeAlterada();
            }
        }

        public int? NumeroIgual
        {
            get => _numeroIgual;
            set
            {
                _numeroIgual = value;
                PropriedadeAlterada();
            }
        }

        public int? CodigoIdIgualA
        {
            get => _codigoIdIgualA;
            set
            {
                _codigoIdIgualA = value;
                PropriedadeAlterada();
            }
        }

        public string NomeEmpresaContenha
        {
            get => _nomeEmpresaContenha;
            set
            {
                _nomeEmpresaContenha = value;
                PropriedadeAlterada();
            }
        }

        public string NomeClienteContenha
        {
            get => _nomeClienteContenha;
            set
            {
                _nomeClienteContenha = value;
                PropriedadeAlterada();
            }
        }

        public TipoEmissaoCupomFiscal? TipoEmissao
        {
            get => _tipoEmissao;
            set
            {
                _tipoEmissao = value;
                PropriedadeAlterada();
            }
        }

        public SituacaoFiscal? SituacaoFiscal
        {
            get => _situacaoFiscal;
            set
            {
                _situacaoFiscal = value;
                PropriedadeAlterada();
            }
        }
    }
}