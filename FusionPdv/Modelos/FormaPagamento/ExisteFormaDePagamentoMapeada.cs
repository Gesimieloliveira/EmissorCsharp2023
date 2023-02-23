using System.Linq;
using FusionCore.FusionPdv.Sessao;
using FusionCore.Repositorio.Legacy.Ativos.Pdv;
using FusionCore.Repositorio.Legacy.Entidades.Pdv;
using FusionPdv.Servicos.Ecf;
using FusionPdv.Visao.MapearFormasDePagamentos;
using NHibernate.Util;

namespace FusionPdv.Modelos.FormaPagamento
{
    public class ExisteFormaDePagamentoMapeada
    {
        public void Executar()
        {
            using (var sessao = GerenciaSessao.ObterSessao("SessaoPdv").AbrirSessao())
            {
                var listaInterno = new FormaDePagamentoRepositorio(sessao).BuscaTodos();
                var formaPagamento = (FormaPagamentoEcfDt) listaInterno.Where(fp => !fp.CodigoEcf.Equals("")).FirstOrNull();

                if (formaPagamento == null)
                {
                    throw new ExceptionFormaPagamentoNaoExiste("Porfavor mapear uma forma de pagamento.");
                }

                ExisteFormaDePagamento(formaPagamento);   
            }
        }

        private static void ExisteFormaDePagamento(FormaPagamentoEcfDt objeto)
        {
            var listaEcf = new EcfPegarTiposPagamentos().TipoPagamento();

            var forma = listaEcf.Where(f => f.Indice.Equals(objeto.CodigoEcf)).FirstOrNull();

            if (forma == null)
            {
                throw new ExceptionFormaPagamentoNaoExiste("Forma não existe.\n Porfavor cadastrar uma.");
            }
        }
    }
}
