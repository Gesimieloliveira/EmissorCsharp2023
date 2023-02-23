using System;
using FusionCore.Repositorio.Legacy.Contratos.Entidades;
using FusionCore.Repositorio.Legacy.Flags;

namespace FusionCore.Repositorio.Legacy.Entidades.Pdv
{
    public class EstoqueEventoPdvDt : IEntidade
    {
        public Int32 Id { get; set; }
        public ProdutoDt ProdutoDt { get; set; }
        public TipoEventoEstoque TipoEvento { get; set; }
        public OrigemEventoEstoque OrigemEvento { get; set; }
        public Decimal Movimento { get; set; }
        public UsuarioPdvDt UsuarioDt { get; set; }
        public DateTime CadastradoEm { get; set; }
        public Int32? IdentificadorRemoto { get; set; }
    }
}