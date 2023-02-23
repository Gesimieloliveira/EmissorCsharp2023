using FusionCore.FusionAdm.Nfce;
using FusionCore.FusionNfce.Fiscal;
using FusionCore.Repositorio.Legacy.Entidades.Adm;

namespace FusionCore.FusionNfce.Extencoes
{
    public static class ExtDestinatarioAdm
    {
        public static NfceDestinatarioAdm ToAdm(this NfceDestinatario destinatario, NfceAdm nfceAdm)
        {
            if (destinatario == null) return null;

            var destinatarioAdm = new NfceDestinatarioAdm
            {
                Nfce = nfceAdm,
                Email = destinatario.Email,
                Nome = destinatario.Nome,
                DocumentoUnico = destinatario.DocumentoUnico,
                NfceId = 0,
                Cliente = destinatario.Cliente.ToAdm(),
                Logradouro = destinatario.Logradouro,
                Numero = destinatario.Numero,
                Bairro = destinatario.Bairro,
                Cep = destinatario.Cep,
                Complemento = destinatario.Complemento,
                InscricaoEstadual = destinatario.InscricaoEstadual
            };

            if (destinatario.Cidade != null)
            {
                destinatarioAdm.Cidade = new CidadeDTO
                {
                    Id = destinatario.Cidade.Id
                };
            }

            return destinatarioAdm;
        }
    }
}