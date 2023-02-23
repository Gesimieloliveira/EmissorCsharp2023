using System.Collections.Generic;
using FusionCore.Excecoes.RegraNegocio;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.FusionAdm;

namespace FusionCore.FusionAdm.Compras
{
    public static class NotaFiscalCompraValidator
    {
        public static void ValidaOsDados(NotaFiscalCompra nf)
        {
            var erros = new List<string>();

            if (nf.NumeroDocumento <= 0)
            {
                erros.Add("Número da nota de compra deve ser maior que 0");
            }

            if (nf.Chave.IsValida() == false)
            {
                erros.Add("Chave da nota é inválida");
            }

            if (nf.Empresa == null)
            {
                erros.Add("Nota de compra precisa de uma Empresa");
            }

            if (nf.Fornecedor == null)
            {
                erros.Add("Nota de compra precisa de um Fornecedor");
            }

            if (erros.Count > 0)
            {
                throw new RegraNegocioException("Nota de compra está errada", erros);
            }
        }

        public static void ValidaExistencia(NotaFiscalCompra nf)
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioNotaFiscalCompra(sessao);

                if (repositorio.JaExiste(nf))
                {
                    throw new RegraNegocioException(
                        "Econtrei uma nota com essa Série, Número e Fornecedor lançada no sistema");
                }
            }
        }

        public static void ValidaExistenciaPelaChave(string chave)
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioNotaFiscalCompra(sessao);

                if (repositorio.JaExisteChaveIgual(chave))
                {
                    throw new RegraNegocioException("Encontrei uma nota com essa Chave lançada no sistema.");
                }
            }
        }
    }
}