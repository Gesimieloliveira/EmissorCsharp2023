using System;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Repositorio.Legacy.Flags;

namespace FusionCore.FusionAdm.Servico.Estoque.Evento
{
    public class EventoEstoqueBuilder
    {
        public DateTime? CadastradoEm { set; get; }
        public EstoqueModel EstoqueModel { set; get; }
        public ProdutoEstoqueDTO EstoqueAtualizado { set; get; }
        public TipoEventoEstoque TipoEvento { set; get; }

        public EstoqueEventoDTO Build()
        {
            var evento = new EstoqueEventoDTO
            {
                CadastradoEm = CadastradoEm ?? DateTime.Now,
                EstoqueAtual = CalculaEstoqueAtual(),
                Movimento = EstoqueModel.Quantidade,
                EstoqueFuturo = EstoqueAtualizado.Estoque,
                TipoEvento = TipoEvento,
                TipoEventoTexto = TipoEvento.ToString(),
                OrigemEvento = EstoqueModel.OrigemEvento,
                OrigemEventoTexto = EstoqueModel.OrigemEvento.ToString(),
                OrigemEventoDetalhe = EstoqueModel.OrigemEventoToString(),
                ProdutoDTO = EstoqueModel.Produto,
                UsuarioDTO = EstoqueModel.Usuario
            };

            return evento;
        }

        private decimal CalculaEstoqueAtual()
        {
            try
            {
                switch (TipoEvento)
                {
                    case TipoEventoEstoque.Entrada:
                        return EstoqueAtualizado.Estoque - EstoqueModel.Quantidade;
                    case TipoEventoEstoque.Saida:
                        return EstoqueAtualizado.Estoque + EstoqueModel.Quantidade;
                }

                throw new InvalidOperationException("Erro ao registrar evento de estoque");
            }
            catch (Exception)
            {
                throw new InvalidOperationException("Erro ao registrar evento de estoque");
            }
        }
    }
}