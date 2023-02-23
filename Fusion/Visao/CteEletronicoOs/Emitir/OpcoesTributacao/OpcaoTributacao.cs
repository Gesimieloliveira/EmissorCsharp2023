using System;
using Fusion.Sessao;
using FusionCore.Comum;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Sessao;
using FusionCore.Tributacoes.Estadual;

namespace Fusion.Visao.CteEletronicoOs.Emitir.OpcoesTributacao
{
    public class OpcaoTributacao : Comparavel<string>
    {
        public OpcaoTributacao(
            string descricao,
            string cst,
            bool permiteIcms,
            bool permiteReducao,
            bool permiteCredito)
        {
            Descricao = descricao;
            Cst = cst;
            PermiteIcms = permiteIcms;
            PermiteReducao = permiteReducao;
            PermiteCredito = permiteCredito;
        }

        protected override string ChaveUnica => Cst;
        public string Descricao { get; }
        public string Cst { get; }
        public bool PermiteIcms { get; }
        public bool PermiteReducao { get; }
        public bool PermiteCredito { get; }

        public override string ToString()
        {
            return $"{Cst} - {Descricao}";
        }

        public TributacaoIcms ObterTributacao()
        {
            using (var sessao = SessaoSistema.Instancia.SessaoManager.CriaSessao())
            {
                var tributacao = sessao.QueryOver<TributacaoIcms>()
                    .Where(i => i.Codigo == Cst)
                    .SingleOrDefault();

                if (tributacao == null)
                    throw new InvalidOperationException($"Não foi possível obter a ST {Cst} no Banco de Dados!");

                return tributacao;
            }
        }
    }
}