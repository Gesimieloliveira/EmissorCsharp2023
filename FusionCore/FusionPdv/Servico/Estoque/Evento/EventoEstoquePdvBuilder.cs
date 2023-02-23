using System;
using FusionCore.Repositorio.Legacy.Entidades.Pdv;
using FusionCore.Repositorio.Legacy.Flags;

namespace FusionCore.FusionPdv.Servico.Estoque.Evento
{
    public class EventoEstoquePdvBuilder
    {
        public EstoqueModel EstoqueModel { set; private get; }
        public TipoEventoEstoque TipoEvento { set; private get; }

        public EstoqueEventoPdvDt Build()
        {
            return new EstoqueEventoPdvDt
            {
                CadastradoEm = DateTime.Now,
                Movimento = EstoqueModel.Quantidade,
                OrigemEvento = EstoqueModel.OrigemEvento,
                ProdutoDt = EstoqueModel.Produto,
                TipoEvento = TipoEvento,
                UsuarioDt = EstoqueModel.Usuario
            };
        }
    }
}