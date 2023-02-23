using DFe.Classes.Flags;
using FusionCore.FusionAdm.Fiscal.Contratos.Componentes;
using FusionCore.FusionAdm.Fiscal.Extensoes.Flags;
using FusionCore.FusionAdm.Fiscal.Helpers;
using FusionCore.FusionAdm.Fiscal.NF;
using NFe.Classes.Informacoes.Destinatario;

namespace FusionCore.FusionAdm.Fiscal.Extensoes.ZeusConversores
{
    public static class DestinatarioParaZeus
    {
        public static dest ToZeus(this DestinatarioNfe destinatario)
        {
            var documento = destinatario.DocumentoUnico;
            var email = string.IsNullOrWhiteSpace(destinatario.Email) ? null : destinatario.Email;

            var zdest = new dest(VersaoServico.Versao310)
            {
                xNome = destinatario.Nome?.TrimSefaz(60),
                IE = destinatario.ObterInscricaoEstadualZeus(),
                email = email?.TrimSefaz(60),
                indIEDest = destinatario.IndicadorIE.ToZeus(),
                enderDest = GetEndereco(destinatario.Endereco)
            };

            if (destinatario.ResideExterior())
            {
                zdest.idEstrangeiro = documento;
                return zdest;
            }

            zdest.CNPJ = documento?.Length == 14 ? documento : null;
            zdest.CPF = documento?.Length == 11 ? documento : null;

            return zdest;
        }

        private static enderDest GetEndereco(IEnderecoFiscal endereco)
        {
            var dest = new enderDest
            {
                xPais = endereco.Localizacao.NomePais?.TrimSefaz(60),
                cPais = endereco.Localizacao.CodigoPais,
                cMun = endereco.Localizacao.CodigoMunicipio,
                xMun = endereco.Localizacao.NomeMunicipio?.TrimSefaz(60),
                UF = endereco.Localizacao.SiglaUF?.TrimSefaz(),
                xLgr = endereco.Logradouro?.TrimSefaz(60),
                xCpl = endereco.Complemento?.TrimSefaz(60),
                nro = endereco.Numero?.TrimSefaz(60),
                xBairro = endereco.Bairro?.TrimSefaz(60),
                CEP = endereco.Cep.PadLeft(8, '0')
            };

            if (!string.IsNullOrEmpty(endereco.Telefone))
                dest.fone = long.Parse(endereco.Telefone);

            return dest;
        }
    }
}