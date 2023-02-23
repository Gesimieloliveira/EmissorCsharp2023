using DFe.Classes.Entidades;
using FusionCore.FusionAdm.Fiscal.Extensoes.Flags;
using FusionCore.FusionAdm.Fiscal.Helpers;
using FusionCore.FusionNfce.Fiscal;
using NFe.Classes.Informacoes.Emitente;

namespace FusionCore.FusionNfce.Extencoes
{
    public static class ExtEmitenteNfce
    {
        public static emit ToZeus(this NfceEmitente emitente)
        {
            var emit = new emit
            {
                CNPJ = emitente.Empresa.Cnpj.Trim(),
                CRT = emitente.Empresa.RegimeTributario.ToZeus(),
                IE = emitente.Empresa.InscricaoEstadual.TrimSefaz(14),
                xFant = emitente.Empresa.NomeFantasia.TrimSefaz(60),
                xNome = emitente.Empresa.RazaoSocial.TrimSefaz(60),
                enderEmit = new enderEmit
                {
                    CEP = emitente.Empresa.Cep.Trim(),
                    UF = (Estado) emitente.Empresa.Estado.CodigoIbge,
                    cMun = emitente.Empresa.Cidade.CodigoIbge,
                    xMun = emitente.Empresa.Cidade.Nome.TrimSefaz(60),
                    cPais = 1058,
                    xPais = "BRASIL", 
                    nro = emitente.Empresa.Numero.TrimSefaz(60),
                    xBairro = emitente.Empresa.Bairro.TrimSefaz(60),
                    xCpl = emitente.Empresa.Complemento.TrimSefazOrNull(60),
                    xLgr = emitente.Empresa.Logradouro.TrimSefaz(60)
                }
            };

            if (!string.IsNullOrEmpty(emitente.Empresa.Fone1))
                emit.enderEmit.fone = long.Parse(emitente.Empresa.Fone1);

            return emit;
        }
    }
}