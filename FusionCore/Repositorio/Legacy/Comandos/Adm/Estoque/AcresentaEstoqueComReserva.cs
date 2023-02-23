using System;
using System.IO;
using FusionCore.Repositorio.Legacy.Contratos.Base.Comando;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using NHibernate;

namespace FusionCore.Repositorio.Legacy.Comandos.Adm.Estoque
{
    public class AcresentaEstoqueComReserva : IComandoDTO<ProdutoEstoqueDTO>
    {
        private readonly ProdutoDTO _produto;
        private readonly decimal _quantidade;
        private readonly decimal _quantidadeReservada;

        public AcresentaEstoqueComReserva(ProdutoDTO produto, decimal quantidade, decimal quantidadeReservada)
        {
            _produto = produto;
            _quantidade = quantidade;
            _quantidadeReservada = quantidadeReservada;
        }

        public ProdutoEstoqueDTO ExecutaComando(ISession sessao)
        {
            var estoque = sessao.Get<ProdutoEstoqueDTO>(_produto.Id);

            if (estoque == null)
            {
                throw new InvalidDataException("Não foi localizado estoque para o produto informado!");
            }

            estoque.Estoque += _quantidade;
            estoque.EstoqueReservado -= _quantidadeReservada;
            estoque.AlteradoEm = DateTime.Now;

            sessao.Update(estoque);

            return estoque;
        }
    }
}