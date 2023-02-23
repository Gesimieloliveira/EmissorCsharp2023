using System;
using DFe.Utils;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionNfce.Empresa;
using FusionCore.Repositorio.Base;

namespace FusionCore.FusionNfce.CertificadosDigitais
{
    public class CertificadoDigitalNfce : EntidadeBase<int>
    {
        public int Id { get; set; }
        public EmpresaNfce Empresa { get; set; }
        public TipoCertificadoDigital Tipo { get; set; }
        public string SerialRepositorio { get; set; }
        public string CaminhoArquivo { get; set; }
        public string Senha { get; set; }

        protected override int ChaveUnica => Id;

        public TipoCertificado TipoToZeus()
        {
            switch (Tipo)
            {
                case TipoCertificadoDigital.A1Repositorio:
                    return TipoCertificado.A1Repositorio;
                case TipoCertificadoDigital.A1Arquivo:
                    return TipoCertificado.A1Arquivo;
                case TipoCertificadoDigital.A3:
                    return TipoCertificado.A3;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}