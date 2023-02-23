using System.Collections.Generic;
using FusionCore.FusionAdm.Fiscal.Contratos;
using FusionCore.FusionAdm.Fiscal.Extensoes.Flags;
using FusionCore.FusionAdm.Fiscal.Helpers;
using FusionCore.FusionAdm.Fiscal.NF;
using NFe.Classes.Informacoes.Transporte;
using NHibernate.Util;

namespace FusionCore.FusionAdm.Fiscal.Extensoes.ZeusConversores
{
    public static class TransportadoraParaZeus
    {
        public static transp ToZeus(this TransportadoraNfe transportadora, Nfeletronica nfe)
        {
            var transp = new transp
            {
                modFrete = nfe.ModalidadeFrete.ToZeus(),
                transporta = GetTransportadora(transportadora),
                veicTransp = GetVeiculo(transportadora?.Veiculo),
                vol = GetVolumes(nfe.Volumes)
            };

            return transp;
        }

        private static List<vol> GetVolumes(IList<IVolume> volumes)
        {
            if (volumes == null || volumes.Count == 0) return null;

            var vols = new List<vol>();

            volumes.ForEach(v =>
            {
                vols.Add(new vol
                {
                    pesoL = v.PesoLiquido,
                    pesoB = v.PesoBruto,
                    marca = string.IsNullOrEmpty(v.Marca) ? null : v.Marca.TrimSefaz(),
                    nVol = string.IsNullOrEmpty(v.Numeracao) ? null : v.Numeracao.TrimSefaz(),
                    qVol = v.Quantidade,
                    esp = string.IsNullOrEmpty(v.Especie) ? null : v.Especie.TrimSefaz()
                });
            });

            return vols;
        }

        private static veicTransp GetVeiculo(VeiculoTransporte veiculo)
        {
            if (veiculo == null) return null;
            if (!veiculo.IsTemVeiculo()) return null;

            return new veicTransp
            {
                UF = veiculo.SiglaUF?.TrimSefaz(),
                placa = veiculo.Placa?.TrimSefaz()
            };
        }

        private static transporta GetTransportadora(TransportadoraNfe transportadora)
        {
            if (transportadora == null) return null;

            if (!transportadora.IsTemTransportadora()) return null;

            var documentoUnico = transportadora.DocumentoUnico;

            var ie = string.IsNullOrWhiteSpace(transportadora.InscricaoEstadual)
                ? null
                : transportadora.InscricaoEstadual?.TrimSefaz();

            return new transporta
            {
                IE = ie,
                UF = transportadora.SiglaUf?.TrimSefaz(),
                xEnder = transportadora.EnderecoCompleto?.TrimSefaz(),
                xMun = transportadora.NomeMunicipio?.TrimSefaz(),
                xNome = transportadora.Nome?.TrimSefaz(),
                CPF = documentoUnico?.Length == 11 ? documentoUnico.TrimSefaz() : null,
                CNPJ = documentoUnico?.Length == 14 ? documentoUnico.TrimSefaz() : null
            };
        }
    }
}