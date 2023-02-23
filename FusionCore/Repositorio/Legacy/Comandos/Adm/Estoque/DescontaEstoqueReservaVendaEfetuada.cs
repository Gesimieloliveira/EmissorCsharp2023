using System;
using System.IO;
using FusionCore.Repositorio.Legacy.Contratos.Base.Comando;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using NHibernate;

namespace FusionCore.Repositorio.Legacy.Comandos.Adm.Estoque
{
    public class DescontaEstoqueReservaVendaEfetuada : IComandoDTO<ProdutoEstoqueDTO>
    {
        private readonly ProdutoDTO _produto;
        private readonly decimal _quantidadeEstoqueReservado;

        public DescontaEstoqueReservaVendaEfetuada(ProdutoDTO produto, decimal quantidadeEstoqueReservado)
        {
            _produto = produto;
            _quantidadeEstoqueReservado = quantidadeEstoqueReservado;
        }

        public ProdutoEstoqueDTO ExecutaComando(ISession sessao)
        {
            var estoque = sessao.Get<ProdutoEstoqueDTO>(_produto.Id);

            if (estoque == null)
            {
                throw new InvalidDataException("Não foi localizado estoque para o produto informado!");
            }

            estoque.EstoqueReservado -= _quantidadeEstoqueReservado;
            estoque.AlteradoEm = DateTime.Now;

            sessao.Update(estoque);

            return estoque;
        }
    }
}