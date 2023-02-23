using System;
using System.IO;
using FusionCore.Repositorio.Legacy.Contratos.Base.Comando;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using NHibernate;

namespace FusionCore.Repositorio.Legacy.Comandos.Adm.Estoque
{
    public class DescontaEstoque : IComandoDTO<ProdutoEstoqueDTO>
    {
        private readonly ProdutoDTO _produto;
        private readonly decimal _quantidade;

        public DescontaEstoque(ProdutoDTO produto, decimal quantidade)
        {
            _produto = produto;
            _quantidade = quantidade;
        }

        public ProdutoEstoqueDTO ExecutaComando(ISession sessao)
        {
            var estoque = sessao.Get<ProdutoEstoqueDTO>(_produto.Id);

            if (estoque == null)
            {
                throw new InvalidDataException("Não foi localizado estoque para o produto informado!");
            }

            estoque.Estoque -= _quantidade;
            estoque.AlteradoEm = DateTime.Now;

            sessao.Update(estoque);

            return estoque;
        }
    }
}