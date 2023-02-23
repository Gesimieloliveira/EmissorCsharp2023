using System;
using System.Collections.Generic;
using System.Linq;
using FusionCore.FusionPdv.Ecf;
using FusionCore.FusionPdv.Sessao;
using FusionCore.Repositorio.Legacy.Ativos.Pdv;
using FusionCore.Repositorio.Legacy.Entidades.Pdv;
using FusionLibrary.VisaoModel;
using FusionPdv.Modelos.FormaPagamento;
using FusionPdv.Servicos.Ecf;
using NHibernate.Util;

namespace FusionPdv.Visao.MapearFormasDePagamentos
{
    public class MapearFormaDePagamentoModel : ModelBase
    {

        private IList<FormaPagamento> _formaPagamentosEcf;
        private IList<FormaPagamentoEcfDt> _formaPagamentoEcfDt;

        public MapearFormaDePagamentoModel()
        {
            AtualizarListaDeFormaPagamentoEcf();
            _formaPagamentoEcfDt =
                BuscarProfmaFormaPagamentoEcfDts();
        }

        private static IList<FormaPagamentoEcfDt> BuscarProfmaFormaPagamentoEcfDts()
        {
            return new FormaDePagamentoRepositorio(GerenciaSessao.ObterSessao("SessaoPdv").AbrirSessao()).BuscaTodos();
        }

        public void AtualizarListaDeFormaPagamentoEcf()
        {
            FormaPagamentosEcf = new EcfPegarTiposPagamentos().TipoPagamento();
        }

        public IList<FormaPagamento> FormaPagamentosEcf
        {
            get { return _formaPagamentosEcf; }
            set
            {
                _formaPagamentosEcf = value;
                PropriedadeAlterada();
            }
        }

        public IList<FormaPagamentoEcfDt> FormaPagamentoEcfDt
        {
            get { return _formaPagamentoEcfDt; }
            set
            {
                _formaPagamentoEcfDt = value;
                PropriedadeAlterada();
            }
        }

        public void SalvarFormaPagamentoDt(FormaPagamentoEcfDt objeto)
        {
            try
            {
                ExisteFormaDePagamento(objeto);

                using (var sessao = GerenciaSessao.ObterSessao("SessaoPdv").AbrirSessao())
                {
                    new FormaDePagamentoRepositorio(sessao).Salvar(objeto);    
                }

                
                CarregarFormasDePagamento.RecuperarFormaDePagamento();
            }
            catch (ExceptionFormaPagamentoNaoExiste ex)
            {
                throw new InvalidOperationException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Falha ao tentar salvar a forma de pagamento.", ex);
            }
            
        }

        private void ExisteFormaDePagamento(FormaPagamentoEcfDt objeto)
        {
            var forma =
                (FormaPagamento) _formaPagamentosEcf.Where(f => f.Indice.Equals(objeto.CodigoEcf)).FirstOrNull();

            if (forma == null)
            {
                throw new ExceptionFormaPagamentoNaoExiste("Forma não existe.");
            }
        }
    }
}
