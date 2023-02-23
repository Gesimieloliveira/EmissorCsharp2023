using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Util;

namespace FusionCore.FusionAdm.Financeiro
{
    public sealed class CentroCusto
    {
        public CentroCusto()
        {
        }

        private IList<CentroCusto> _itens;

        public short Id { get; set; }
        public string Descricao { get; set; }
        public CentroCusto CentroCustoPai { get; set; }
        public string Nivel { get; set; }
        public string Ordenacao { get; set; }

        public void GerarNivel(CentroCusto centroCusto)
        {
            if (centroCusto == null)
            {
                Ordenacao = 1.ToString("D3");
                Nivel = 1.ToString();
                return;
            }

            var pai = CentroCustoPai;

            var adicionouFilho = false;

            var paiCopia = pai;

            pai?.Itens?.ForEach(p =>
            {
                adicionouFilho = true;

                var ordemLiteral = p.Nivel.Split('.').LastOrDefault();

                if (ordemLiteral == null) return;

                var ordem = int.Parse(ordemLiteral);
                ordem++;
                var novoNivel = paiCopia.Nivel + "." + ordem;
                Ordenacao = paiCopia.Ordenacao + "." + ordem.ToString("D3");
                Nivel = novoNivel;
            });

            if (adicionouFilho) return;

            if (pai != null)
            {
                pai = CentroCustoPai;
                var nivel = pai.Nivel;

                var novoNivel = nivel + "." + 1;
                Ordenacao = pai.Ordenacao + "." + 1.ToString("D3");
                Nivel = novoNivel;
                return;
            }

            var ordemRaiz = int.Parse(centroCusto.Ordenacao);
            ordemRaiz++;
            Ordenacao = ordemRaiz.ToString("D3");
            Nivel = ordemRaiz.ToString();
        }

        public IList<CentroCusto> Itens
        {
            get
            {
                return _itens;
            }
            set { _itens = value; }
        }

        public void Exibir()
        {
            Console.WriteLine(Descricao);
            _itens.ForEach(cc =>
            {
                cc.Exibir();
            });
        }

        private bool Equals(CentroCusto other)
        {
            return Equals(_itens, other._itens) && string.Equals(Descricao, other.Descricao) && Equals(CentroCustoPai, other.CentroCustoPai);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is CentroCusto && Equals((CentroCusto) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (_itens != null ? _itens.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Descricao != null ? Descricao.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (CentroCustoPai != null ? CentroCustoPai.GetHashCode() : 0);
                return hashCode;
            }
        }

        public override string ToString()
        {
            return $"{Nivel} - {Descricao}";
        }
    }
}