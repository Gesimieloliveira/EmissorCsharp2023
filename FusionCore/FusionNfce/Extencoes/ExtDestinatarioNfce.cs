using System;
using DFe.Classes.Flags;
using FusionCore.FusionAdm.Fiscal.Helpers;
using FusionCore.FusionNfce.Fiscal;
using FusionCore.Helpers.Hidratacao;
using NFe.Classes.Informacoes.Destinatario;

namespace FusionCore.FusionNfce.Extencoes
{
    public static class ExtDestinatarioNfce
    {
        public static dest ToZeus(this NfceDestinatario destinatario, VersaoServico versao)
        {
            if (destinatario == null) return null;

            var destinatarioZeus = new dest(versao);

            switch (destinatario.DocumentoUnico.Length)
            {
                case 14:
                    destinatarioZeus.CNPJ = destinatario.DocumentoUnico;
                    break;
                case 11:
                    destinatarioZeus.CPF = destinatario.DocumentoUnico;
                    break;
            }

            destinatarioZeus.xNome = destinatario.Nome.IsNullOrEmpty() ? null : destinatario.Nome.Trim();

            destinatarioZeus.indIEDest = indIEDest.NaoContribuinte;

            destinatarioZeus.email = destinatario.Email.TrimSefazOrNull(60);

            if (!destinatario.IsAddEndereco) return destinatarioZeus;

            DadosObrigatorio(destinatario);

            var enderDest = new enderDest();

            enderDest.xLgr = destinatario.Logradouro.TrimSefazOrNull(60);
            enderDest.nro = destinatario.Numero.TrimSefazOrNull(60);
            enderDest.xCpl = destinatario.Complemento.TrimSefazOrNull(60);
            enderDest.xBairro = destinatario.Bairro.TrimSefazOrNull(60);

            if (destinatario.Cidade != null)
            {
                enderDest.cMun = long.Parse(destinatario.Cidade?.CodigoIbge.ToString());
                enderDest.xMun = destinatario.Cidade.Nome.TrimSefazOrNull(60);
                enderDest.UF = destinatario.Cidade.SiglaUf;
                enderDest.CEP = destinatario.Cep.TrimSefazOrNull(8);
            }

            destinatarioZeus.enderDest = enderDest;

            return destinatarioZeus;
        }

        private static void DadosObrigatorio(NfceDestinatario destinatario)
        {
            if (destinatario.DocumentoUnico.IsNullOrEmpty())
                throw new ArgumentException("CPF/CNPJ Obrigatório");

            if (destinatario.Logradouro.IsNullOrEmpty())
                throw new ArgumentException("Logradouro obrigatório");

            if (destinatario.Numero.IsNullOrEmpty())
                throw new ArgumentException("Número obrigatório");

            if (destinatario.Bairro.IsNullOrEmpty())
                throw new ArgumentException("Bairro obrigatório");

            if (destinatario.Cidade == null)
                throw new ArgumentException("Cidade obrigatório");
        }
    }
}