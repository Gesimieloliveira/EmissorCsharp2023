using System;
using FusionCore.Repositorio.Base;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace FusionCore.FusionAdm.MdfeEletronico
{
    public class MDFeProdutoPerigoso : EntidadeBase<int>
    {
        private MDFeProdutoPerigoso()
        {
            //nhibernate
        }

        public MDFeProdutoPerigoso(
            string numeroOnu, 
            string nomeEmbarque, 
            string classeRisco, 
            string grupoEmbalagem, 
            string quantidadeTotalPorProduto, 
            string quantidadeTipoVolume) : this()
        {
            NumeroOnu = numeroOnu ?? string.Empty;
            NomeEmbarque = nomeEmbarque ?? string.Empty;
            ClasseRisco = classeRisco ?? string.Empty;
            GrupoEmbalagem = grupoEmbalagem ?? string.Empty;
            QuantidadeTotalPorProduto = quantidadeTotalPorProduto ?? string.Empty;
            QuantidadeTipoVolume = quantidadeTipoVolume ?? string.Empty;
        }

        public int Id { get; private set; }
        protected override int ChaveUnica => Id;
        public MDFeDescarregamento Descarregamento { get; private set; }
        public string NumeroOnu { get; private set; }
        public string NomeEmbarque { get; private set; }
        public string ClasseRisco { get; private set; }
        public string GrupoEmbalagem { get; private set; }
        public string QuantidadeTotalPorProduto { get; private set; }
        public string QuantidadeTipoVolume { get; private set; }

        public void Anexar(MDFeDescarregamento objeto)
        {
            if (Descarregamento == objeto)
            {
                return;
            }

            if (Descarregamento != null && Descarregamento != objeto)
            {
                throw  new InvalidOperationException("Produto perigoso já anexado a outro Documento");
            }

            Descarregamento = objeto;
        }
    }
}