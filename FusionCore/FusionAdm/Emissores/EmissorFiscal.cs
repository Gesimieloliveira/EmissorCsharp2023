using System;
using DFe.Utils;
using FusionCore.Core.Net;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionNfce.Extencoes;
using FusionCore.NfceSincronizador.Contratos;
using FusionCore.NfceSincronizador.Flags;
using FusionCore.Repositorio.Base;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionLibrary.Helper.Criptografia;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace FusionCore.FusionAdm.Emissores
{
    public class EmissorFiscal : Entidade, ISincronizavelAdm
    {
        public byte Id { get; set; }
        public EmpresaDTO Empresa { get; set; }
        public string Descricao { get; set; }
        public TipoCertificadoDigital TipoCertificadoDigital { get; set; } = TipoCertificadoDigital.A1Arquivo;
        public string ArquivoCertificado { get; set; }
        public string SenhaCertificado { get; set; }
        public EmissorFiscalNFE EmissorFiscalNfe { get; set; }
        public EmissorFiscalNFCE EmissorFiscalNfce { get; set; }
        public DateTime? AlteradoEm { get; set; }
        public EmissorFiscalCTE EmissorFiscalCte { get; set; }
        public EmissorFiscalCTeOS EmissorFiscalCteOs { get; set; }
        public EmissorFiscalSAT EmissorFiscalSat { get; set; }
        public AutorizadoBaixarXml AutorizadoBaixarXml { get; set; }
        public bool FlagNfe { get; set; }
        public bool FlagNfce { get; set; }
        public bool FlagCte { get; set; }
        public bool FlagCteOs { get; set; }
        public bool FlagMdfe { get; set; }
        public bool FlagSat { get; set; }
        public string SerialNumberCertificado { get; set; }
        public string Referencia => Id.ToString();
        public EntidadeSincronizavel EntidadeSincronizavel { get; set; } = EntidadeSincronizavel.EmissorFiscal;
        public EmissorFiscalMDFE EmissorFiscalMdfe { get; set; }
        protected override int ReferenciaUnica => Id;
        public ProtocoloSeguranca ProtocoloSeguranca { get; set; } = ProtocoloSeguranca.Tls12;
        public string Uf => Empresa.EstadoDTO.Sigla;
        public string Cnpj => Empresa.Cnpj;
        public TerminalOffline.TerminalOffline TerminalOffline { get; set; }
        public bool IsFaturamento { get; set; }

        public ConfiguracaoCertificado GetCertificadoZeus()
        {
            var configCert = new ConfiguracaoCertificado
            {
                TipoCertificado = TipoCertificadoDigital.ToZeus(),
                ManterDadosEmCache = true,
                Senha = SimetricaCrip.Descomputar(SenhaCertificado),
                CacheId = Id.ToString(),
                Serial = SimetricaCrip.Descomputar(SerialNumberCertificado),
                Arquivo = ArquivoCertificado
            };

            return configCert;
        }

        public override string ToString()
        {
            return Descricao;
        }

        public bool PermiteAlterarTipo()
        {
            return Id == 0;
        }
    }
}