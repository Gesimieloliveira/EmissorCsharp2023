using DFe.Classes.Entidades;
using FusionCore.FusionAdm.Fiscal.Extensoes.Flags;
using FusionCore.FusionAdm.Fiscal.Helpers;
using FusionCore.FusionAdm.Fiscal.NF;
using NFe.Classes.Informacoes.Emitente;

namespace FusionCore.FusionAdm.Fiscal.Extensoes.ZeusConversores
{
    public static class EmitenteParaZeus
    {
        public static emit ToZeus(this EmitenteNfe emitente)
        {
            var documento = emitente.DocumentoUnicoSemZeroAEsquerda;
            var telefone = emitente.Empresa.Fone1;

            var zeusEmitente = new emit
            {
                CNPJ = documento?.Length == 14 ? documento.TrimSefaz() : null,
                CPF = documento?.Length == 11 ? documento.TrimSefaz() : null,
                CRT = emitente.RegimeTributario.ToZeus(),
                IE = emitente.Empresa.InscricaoEstadual,
                xFant = emitente.Empresa.NomeFantasia.TrimSefaz(60),
                xNome = emitente.Empresa.RazaoSocial?.TrimSefaz(60),
                enderEmit = new enderEmit
                {
                    CEP = emitente.Empresa.Cep.TrimSefaz(),
                    UF = (Estado)emitente.Empresa.EstadoDTO.CodigoIbge, 
                    cMun = emitente.Empresa.CidadeDTO.CodigoIbge,
                    xMun = emitente.Empresa.CidadeDTO.Nome?.TrimSefaz(60),
                    cPais = 1058,
                    xPais = "BRASIL",
                    nro = emitente.Empresa.Numero?.TrimSefaz(60),
                    xBairro = emitente.Empresa.Bairro?.TrimSefaz(60),
                    xCpl = emitente.Empresa.Complemento?.TrimSefaz(60),
                    xLgr = emitente.Empresa.Logradouro?.TrimSefaz(60),
                    fone =
                        string.IsNullOrEmpty(telefone.TrimSefaz(14))
                            ? (long?) null
                            : long.Parse(telefone.TrimSefaz(14))
                }
            };

            return zeusEmitente;
        }
    }
}