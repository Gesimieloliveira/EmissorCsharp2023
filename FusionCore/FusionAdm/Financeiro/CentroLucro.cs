using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Util;

namespace FusionCore.FusionAdm.Financeiro
{
    public sealed class CentroLucro
    {
        private IList<CentroLucro> _itens;
        public short Id { get; set; }
        public string Descricao { get; set; }
        public CentroLucro CentroLucroPai { get; set; }
        public string Nivel { get; set; }
        public string Ordenacao { get; set; }
        public bool Editar => Itens?.Count == 0;

        public IList<CentroLucro> Itens
        {
            get => _itens;
            set => _itens = value;
        }

        public void GerarNivel(CentroLucro centroCusto)
        {
            if (centroCusto == null)
            {
                Ordenacao = 1.ToString("D3");
                Nivel = 1.ToString();
                return;
            }

            var pai = CentroLucroPai;
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
                pai = CentroLucroPai;
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

        public void Exibir()
        {
            Console.WriteLine(Descricao);
            _itens.ForEach(cc => { cc.Exibir(); });
        }

        public override string ToString()
        {
            return $"{Nivel} - {Descricao}";
        }
    }
}
