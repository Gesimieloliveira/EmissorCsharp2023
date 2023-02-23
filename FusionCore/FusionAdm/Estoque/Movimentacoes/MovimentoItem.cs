using System;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Repositorio.Legacy.Flags;
using FusionLibrary.Helper.Criptografia;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable NonReadonlyMemberInGetHashCode
// ReSharper disable UnusedAutoPropertyAccessor.Local
// ReSharper disable UnusedMember.Local
// ReSharper disable ConvertToAutoProperty

namespace FusionCore.FusionAdm.Estoque.Movimentacoes
{
    public class MovimentoItem
    {
        private readonly string _hash = Md5Helper.ComputaUnique();
        private decimal _quantidade;
        private decimal _precoCompra;
        private decimal _precoVenda;
        private decimal _margemLucro;
        private readonly UsuarioDTO _cadastradoPor;
        private readonly DateTime _cadastradoEm = DateTime.Now;
        private ProdutoDTO _produto;
        private MovimentoEstoque _movimento;
        private string _siglaUnidade;
        private decimal _precoVendaTotal;
        private decimal _precoCompraTotal;
        public int Id { get; private set; }

        public MovimentoEstoque Movimento
        {
            get { return _movimento; }
            set
            {
                _movimento = value;
                TipoEvento = value.TipoEvento;
            }
        }

        public ProdutoDTO Produto
        {
            get { return _produto; }
            set
            {
                if (value == null || value.Id == 0)
                    throw new ArgumentException("Produto adicionado no item é inválido");

                _produto = value;
                _siglaUnidade = value.ProdutoUnidadeDTO.Sigla;
            }
        }

        public TipoEventoEstoque TipoEvento { get; private set; }

        public decimal Quantidade
        {
            get { return _quantidade; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Quantidade do item não pode ser zerado");

                if (value > 99999999.9999M)
                    throw new ArgumentException("Quantidade maior que o máximo permitido");

                _quantidade = value;
            }
        }

        public decimal PrecoCompra
        {
            get { return _precoCompra; }
            set
            {
                if (value < 0 && PermitePrecoCompraZerado == false)
                    throw new ArgumentException("Preço Compra do item não pode ser zerado");

                if (value > 99999999.9999M)
                    throw new ArgumentException("Preço Compra maior que o máximo permitido");

                _precoCompra = value;
            }
        }

        public decimal MargemLucro
        {
            get { return _margemLucro; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Margem de Lucro do item não pode ser zerado");

                if (value > 999999.999999M)
                    throw new ArgumentException("Margem de Lucro maior que o máximo permitido");

                _margemLucro = value;
            }
        }

        public decimal PrecoVenda
        {
            get { return _precoVenda; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Preço Venda do item não pode ser zerado");

                if (value > 99999999.9999M)
                    throw new ArgumentException("Preço Venda maior que o máximo permitido");

                _precoVenda = value;
            }
        }

        public decimal PrecoVendaTotal
        {
            get { return _precoVendaTotal; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Preço Venda Total do item não pode ser zerado");

                if (value > 99999999.9999M)
                    throw new ArgumentException("Preço Venda Total maior que o máximo permitido");

                _precoVendaTotal = value;
            }
        }

        public decimal PrecoCompraTotal
        {
            get { return _precoCompraTotal; }
            set
            {
                if (value <= 0 && PermitePrecoCompraZerado == false)
                    throw new ArgumentException("Preço Compra Total do item não pode ser zerado");

                if (value > 99999999.9999M)
                    throw new ArgumentException("Preço Compra Total maior que o máximo permitido");

                _precoCompraTotal = value;
            }
        }

        public string SiglaUnidade => _siglaUnidade;
        public UsuarioDTO CadastradoPor => _cadastradoPor;
        public DateTime CadastradoEm => _cadastradoEm;
        public bool PermitePrecoCompraZerado { get; set; } = false;

        private MovimentoItem()
        {
            //nhiberante
        }

        public MovimentoItem(ProdutoDTO produto, UsuarioDTO cadastradoPor)
        {
            Produto = produto;
            _cadastradoPor = cadastradoPor;
        }

        private bool Equals(MovimentoItem other)
        {
            return GetHashCode() == other.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((MovimentoItem) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                if (Id != 0) return Id*398;
                return _hash.GetHashCode();
            }
        }

        public static bool operator ==(MovimentoItem left, MovimentoItem right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(MovimentoItem left, MovimentoItem right)
        {
            return !Equals(left, right);
        }
    }
}