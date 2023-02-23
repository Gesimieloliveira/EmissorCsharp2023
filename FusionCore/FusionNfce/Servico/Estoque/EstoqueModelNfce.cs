using System;
using FusionCore.FusionNfce.Produto;
using FusionCore.FusionNfce.Sessao.Sistema;
using FusionCore.FusionNfce.Usuario;
using FusionCore.Repositorio.Legacy.Flags;

namespace FusionCore.FusionNfce.Servico.Estoque
{
    public class EstoqueModelNfce
    {
        private EstoqueModelNfce() { }

        public int Id { get; set; }
        public ProdutoNfce Produto { get; set; }
        public UsuarioNfce Usuario { get; set; }
        public TipoEventoEstoque TipoEvento { get; set; }
        public OrigemEventoEstoque OrigemEvento { get; set; }
        public decimal Movimento { get; set; }
        public DateTime? CadastradoEm { get; set; }
        public int IdRemoto { get; set; }
        public bool Sincronizado { get; set; }

        public static EstoqueModelNfce Cria(ProdutoNfce produto, OrigemEventoEstoque origemEventoEstoque, TipoEventoEstoque tipoEvento, decimal movimento)
        {
            return new EstoqueModelNfce
            {
                CadastradoEm = DateTime.Now,
                OrigemEvento = origemEventoEstoque,
                Usuario = SessaoSistemaNfce.Usuario,
                TipoEvento = tipoEvento,
                Movimento = movimento,
                Produto = produto,
                Sincronizado = false
            };
        }
    }
}