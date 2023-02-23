using System.Collections.Generic;
using FusionCore.Core.Flags;
using FusionCore.Core.Tributario;
using FusionCore.Excecoes.RegraNegocio;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.NfceSincronizador.Contratos;
using FusionCore.NfceSincronizador.Flags;
using FusionCore.Repositorio.Base;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Tributacoes.Estadual;
using FusionCore.Tributacoes.Flags;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace FusionCore.Tributacoes.Regras
{
    public class RegraTributacaoSaida : Entidade, ISincronizavelAdm
    {
        public RegraTributacaoSaida()
        {
            Ativo = true;
        }

        protected override int ReferenciaUnica => Id;
        public string Referencia => Id.ToString();
        public EntidadeSincronizavel EntidadeSincronizavel { get; } = EntidadeSincronizavel.RegraTributacaoSaida;
        public short Id { get; private set; }
        public bool Ativo { get; private set; }
        public string Descricao { get; private set; }
        public TributacaoIcms Cst { get; private set; }
        public TributacaoCsosn Csosn { get; private set; }
        public PerfilCfopDTO CfopIntermunicipal { get; private set; }
        public PerfilCfopDTO CfopInterestadual { get; private set; }
        public PerfilCfopDTO CfopExterior { get; private set; }
        public CfopDTO CfopNfce { get; private set; }

        public override string ToString()
        {
            return $"{Descricao}, CST: {Cst.Codigo}, CSOSN: {Csosn.Codigo}";
        }

        public void Update(
            bool isAtivo,
            string descricao,
            TributacaoIcms cst,
            TributacaoCsosn csosn,
            PerfilCfopDTO intermunicipal,
            PerfilCfopDTO interestadual,
            PerfilCfopDTO exterior,
            CfopDTO cfopNfce
        )
        {
            Ativo = isAtivo;
            Descricao = descricao;
            Cst = cst;
            Csosn = csosn;
            CfopIntermunicipal = intermunicipal;
            CfopInterestadual = interestadual;
            CfopExterior = exterior;
            CfopNfce = cfopNfce;
        }

        public void Valida()
        {
            var erros = new List<string>();

            if (string.IsNullOrWhiteSpace(Descricao))
            {
                erros.Add("Descrição não pode ser vazia");
            }

            if (Cst == null)
            {
                erros.Add("CST IMCS não pode ser vazio");
            }

            if (Csosn == null)
            {
                erros.Add("CSOSN não pode ser vazio");
            }

            if (CfopIntermunicipal?.Operacao != OpercaoCfop.Saida ||
                CfopIntermunicipal?.Origem != OrigemOperacao.InterMunicipal)
            {
                erros.Add("CFOP Intermnunicipal é inválido");
            }

            if (CfopIntermunicipal?.Operacao != OpercaoCfop.Saida ||
                CfopInterestadual?.Origem != OrigemOperacao.InterEstadual)
            {
                erros.Add("CFOP Interestadual é inválido");
            }

            if (CfopIntermunicipal?.Operacao != OpercaoCfop.Saida || CfopExterior?.Origem != OrigemOperacao.Importacao)
            {
                erros.Add("CFOP Exterior é inválido");
            }

            if (CfopNfce == null || !CfopNfce.ElegivelNfce)
            {
                erros.Add("CFOP NFC-E é inválido");
            }

            if (erros.Count > 0)
            {
                throw new RegraNegocioException("Regra de tributação para saidas é inválido", erros);
            }
        }

        public PerfilCfopDTO CfopParaNfeFrom(DestinoOperacao destino)
        {
            if (destino == DestinoOperacao.Exterior)
            {
                return CfopExterior;
            }

            if (destino == DestinoOperacao.Interestadual)
            {
                return CfopInterestadual;
            }

            return CfopIntermunicipal;
        }

        public string CodigoCstParaNfe(RegimeTributario regime)
        {
            return regime == RegimeTributario.SimplesNacional
                ? Csosn.Codigo
                : Cst.Codigo;
        }
    }
}