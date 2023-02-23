using System;
using FusionCore.FusionNfce.Produto;

namespace FusionNfce.Visao.Principal.Model
{
    public class ItemEvent : EventArgs
    {
        public ItemEvent(ProdutoNfce produto)
        {
            Produto = produto;
        }

        public ProdutoNfce Produto { get; set; }
    }
}