using System;
using FusionCore.NfceSincronizador.Contratos;
using FusionCore.NfceSincronizador.Flags;
using FusionCore.Repositorio.Base;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace FusionCore.Repositorio.Legacy.Entidades.Adm
{
    public class ProdutoEstoqueDTO : Entidade, ISincronizavelAdm
    {
        public ProdutoEstoqueDTO()
        {
            AlteradoEm = DateTime.Now;
        }

        protected override int ReferenciaUnica => ProdutoId;
        public int ProdutoId { get; set; }
        public ProdutoDTO ProdutoDTO { get; set; }
        public decimal Estoque { get; set; }
        public decimal EstoqueMinimo { get; set; }
        public decimal EstoqueMaximo { get; set; }
        public DateTime? AlteradoEm { get; set; }
        public decimal EstoqueReservado { get; set; }
        public string Referencia => ProdutoId.ToString();
        public EntidadeSincronizavel EntidadeSincronizavel => EntidadeSincronizavel.Produto;
    }
}