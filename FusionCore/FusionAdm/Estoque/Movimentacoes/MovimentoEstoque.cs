using System;
using System.Collections.Generic;
using System.Linq;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Repositorio.Legacy.Flags;

// ReSharper disable UnusedMember.Local
// ReSharper disable ConvertToAutoProperty
// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace FusionCore.FusionAdm.Estoque.Movimentacoes
{
    public class MovimentoEstoque
    {
        private readonly IList<MovimentoItem> _itens = new List<MovimentoItem>();
        private readonly DateTime _cadastradoEm = DateTime.Now;
        private readonly UsuarioDTO _cadastradoPor;
        private readonly TipoEventoEstoque _tipoEvento;
        private string _descricao;
        public int Id { get; private set; }

        public string Descricao
        {
            get { return _descricao; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Descrição do movimento é inválida");

                _descricao = value;
            }
        }

        public TipoEventoEstoque TipoEvento => _tipoEvento;
        public DateTime DataMovimento { get; set; } = DateTime.Now;
        public decimal PrecoVendaTotal { get; private set; }
        public decimal PrecoCompraTotal { get; private set; }
        public UsuarioDTO CadastradoPor => _cadastradoPor;
        public DateTime CadastradoEm => _cadastradoEm;
        public IReadOnlyList<MovimentoItem> Itens => _itens.ToList();

        private MovimentoEstoque()
        {
            //nhibernate
        }

        public MovimentoEstoque(string descricao, TipoEventoEstoque tipoEvento, UsuarioDTO cadastradoPor)
        {
            if (!Enum.IsDefined(typeof (TipoEventoEstoque), tipoEvento))
                throw new ArgumentException("Tipo Evento movimentação é inválida");

            Descricao = descricao;
            _tipoEvento = tipoEvento;
            _cadastradoPor = cadastradoPor;
        }

        public void AdicionarItem(MovimentoItem item)
        {
            item.Movimento = this;

            _itens.Add(item);

            PrecoCompraTotal += item.PrecoCompraTotal;
            PrecoVendaTotal += item.PrecoVendaTotal;
        }

        public void RemoverItem(MovimentoItem item)
        {
            _itens.Remove(item);
            PrecoCompraTotal -= item.PrecoCompraTotal;
            PrecoVendaTotal -= item.PrecoVendaTotal;
        }
    }
}