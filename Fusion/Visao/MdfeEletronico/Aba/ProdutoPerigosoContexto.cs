using System;
using FusionCore.FusionAdm.MdfeEletronico;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.MdfeEletronico.Aba
{
    public  class ProdutoPerigosoContexto : ViewModel
    {
        public string NumeroOnu
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string NomeEmbarque
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string ClasseRisco
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string GrupoEmbalagem
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string QuantidadeTotalPorProduto
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string QuantidadeTipoVolume
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public event EventHandler<MDFeProdutoPerigoso> Sucesso;

        public void AdicionarNovoProdutoPerigoso()
        {
            var novo = new MDFeProdutoPerigoso(
                NumeroOnu,
                NomeEmbarque,
                ClasseRisco,
                GrupoEmbalagem,
                QuantidadeTotalPorProduto,
                QuantidadeTipoVolume
            );

            Sucesso?.Invoke(this, novo);

            LimparMapaValores();
        }
    }
}