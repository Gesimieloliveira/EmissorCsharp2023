using System.Text.RegularExpressions;
using FusionCore.Excecoes.RegraNegocio;
using static System.String;

namespace FusionCore.FusionAdm.MdfeEletronico.Validador
{
    public static class MDFeRodoviarioValidador
    {
        public static void Checar(MDFeRodoviario rodoviario)
        {
            if (!IsNullOrWhiteSpace(rodoviario.Rntrc) && !Regex.IsMatch(rodoviario.Rntrc, "[0-9]{8}"))
            {
                throw new RegraNegocioException("MDF-E Rodoviário: Se informado RNTRC deve conter 8 digitos e apenas números");
            }
        }
    }
}