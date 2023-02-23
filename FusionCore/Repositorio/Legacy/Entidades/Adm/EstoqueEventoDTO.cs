using System;
using FusionCore.NfceSincronizador.Contratos;
using FusionCore.NfceSincronizador.Flags;
using FusionCore.Repositorio.Legacy.Contratos.Entidades;
using FusionCore.Repositorio.Legacy.Flags;

namespace FusionCore.Repositorio.Legacy.Entidades.Adm
{
    public class EstoqueEventoDTO : IEntidade, ISincronizavelAdm
    {
        public int Id { get; set; }
        public ProdutoDTO ProdutoDTO { get; set; }
        public TipoEventoEstoque TipoEvento { get; set; }
        public string TipoEventoTexto { get; set; }
        public OrigemEventoEstoque OrigemEvento { get; set; }
        public string OrigemEventoTexto { get; set; }
        public string OrigemEventoDetalhe { get; set; }
        public decimal EstoqueAtual { get; set; }
        public decimal Movimento { get; set; }
        public decimal EstoqueFuturo { get; set; }
        public UsuarioDTO UsuarioDTO { get; set; }
        public DateTime CadastradoEm { get; set; }
        public string Referencia => ProdutoDTO.Referencia;
        public EntidadeSincronizavel EntidadeSincronizavel => EntidadeSincronizavel.Produto;
    }
}