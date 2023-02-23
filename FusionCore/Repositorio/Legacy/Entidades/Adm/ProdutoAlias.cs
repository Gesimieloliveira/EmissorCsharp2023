using System;
using FusionCore.FusionAdm.Estoque.Produto;
using FusionCore.Helpers.Basico;
using FusionCore.NfceSincronizador.Contratos;
using FusionCore.NfceSincronizador.Flags;
using FusionCore.Repositorio.Base;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
// ReSharper disable NonReadonlyMemberInGetHashCode
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace FusionCore.Repositorio.Legacy.Entidades.Adm
{
    public class ProdutoAlias : Entidade, ISincronizavelAdm
    {
        private ProdutoAlias()
        {
            // nhibernate
        }

        public ProdutoAlias(string alias, bool isCodigoBarra = true, bool isGtin = false) : this()
        {
            if (string.IsNullOrWhiteSpace(alias))
            {
                throw new InvalidOperationException("Código não pode ser vazio");
            }

            if (isCodigoBarra && !Gs1GtinHelper.EhUmGtinValido(alias))
            {
                throw new InvalidOperationException("Código não é um Código de Barras válido!");
            }

            if (isGtin && !Gs1GtinHelper.EhUmGtinDoBrasilValido(alias))
            {
                throw new InvalidOperationException("Código não é um GTIN válido!");
            }

            Alias = alias;
            IsCodigoBarras = isCodigoBarra;
            IsGtin = isGtin;
        }

        public int Id { get; set; }
        public ProdutoDTO Produto { get; set; }
        public bool IsCodigoBarras { get; private set; }
        public bool IsGtin { get; private set; }
        public string Alias { get; private set; }
        public TipoAlias TipoAlias => ObterTipoAlias();

        private TipoAlias ObterTipoAlias()
        {
            if (IsGtin == false && IsCodigoBarras) return TipoAlias.CodigoBarra;
            if (IsGtin && IsCodigoBarras) return TipoAlias.Gtin;

            return TipoAlias.Codigo;
        }

        public string Referencia => Produto.Referencia;
        public EntidadeSincronizavel EntidadeSincronizavel => EntidadeSincronizavel.Produto;
        protected override int ReferenciaUnica => Id;

        public static ProdutoAlias Criar(ProdutoDTO produto, string alias)
        {
            if (string.IsNullOrEmpty(alias))
            {
                throw new InvalidOperationException("Preciso de um código para criar o Produto Alias");
            }

            var entidade = new ProdutoAlias(alias, Gs1GtinHelper.EhUmGtinValido(alias))
            {
                Produto = produto
            };

            return entidade;
        }

        public void Update(bool isCodigoBarras, bool isGtin)
        {
            if (isCodigoBarras && !Gs1GtinHelper.EhUmGtinValido(Alias))
            {
                throw new InvalidOperationException("Esse Código não é um GTIN válido");
            }

            if (isGtin && !Gs1GtinHelper.EhUmGtinDoBrasilValido(Alias))
            {
                throw new InvalidOperationException("Esse Código não é um GTIN válido");
            }

            IsCodigoBarras = isCodigoBarras;
            IsGtin = isGtin;
        }
    }
}