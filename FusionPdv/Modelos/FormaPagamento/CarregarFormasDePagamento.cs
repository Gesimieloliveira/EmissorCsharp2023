using System;
using System.Collections.Generic;
using System.Linq;
using FusionCore.FusionPdv.Sessao;
using FusionCore.Repositorio.Legacy.Ativos.Pdv;
using FusionCore.Repositorio.Legacy.Base.Execao;
using FusionCore.Repositorio.Legacy.Entidades.Pdv;
using NHibernate;
using NHibernate.Util;

namespace FusionPdv.Modelos.FormaPagamento
{
    public static class CarregarFormasDePagamento
    {
        private static readonly Dictionary<string, IFormaPagamento> FormasPagamento = new Dictionary<string, IFormaPagamento>();

        public const string Dinheiro = "Dinheiro";
        public const string CartaoTef = "Cartão TEF";
        public const string CartaoPos = "Cartão POS";
        public const string Crediario = "Crediário";

        private static ISession _sessao;

        public static void RecuperarFormaDePagamento()
        {
            _sessao = GerenciaSessao.ObterSessao("SessaoPdv").AbrirSessao();
            FormasPagamento.Clear();
            var formaDePagamentoRepositorio = InstanciaRepositorio();

            var formasDePagamentos = CarregarFormaDepagamentoDt(formaDePagamentoRepositorio);

            PreencheDictionary(formasDePagamentos);
        }

        public static void Validar()
        {
            RecuperarFormaDePagamento();
            if (FormasPagamento.Count == 0)
            {
                throw new ExceptionCarregarFormaPagamento("Erro, não existe forma de pagamento cadastrada no sistema.\nPor favor mapear as formas de pagamento no sistema.");
            }
        }

        private static void PreencheDictionary(IEnumerable<FormaPagamentoEcfDt> formasDePagamentos)
        {
            foreach (var formaPagamentoEcfDt in formasDePagamentos)
            {
                var dt = formaPagamentoEcfDt;

                var iFormaPagamento = (IFormaPagamento)new ListaDeFormaDePagamento().ObterFormaDePagamentos()
                    .Where(f => f.Id.Equals(dt.Id)).FirstOrNull();

                iFormaPagamento.CodigoEcf = formaPagamentoEcfDt.CodigoEcf;
                FormasPagamento.Add(iFormaPagamento.Nome, iFormaPagamento);
            }
        }

        private static IEnumerable<FormaPagamentoEcfDt> CarregarFormaDepagamentoDt(FormaDePagamentoRepositorio formaDePagamentoRepositorio)
        {
            try
            {
                return formaDePagamentoRepositorio.BuscaTodos();
            }
            catch (Exception ex)
            {
                throw new RepositorioExeption("Erro ao buscar formas de pagamento.", ex);
            }
            finally
            {
                _sessao.Close();
            }
        }

        private static FormaDePagamentoRepositorio InstanciaRepositorio()
        {
            return new FormaDePagamentoRepositorio(_sessao);   
        }

        public static IFormaPagamento PegarFormaDePagamento(string descricao)
        {
            return FormasPagamento[descricao];
        }
    }
}
