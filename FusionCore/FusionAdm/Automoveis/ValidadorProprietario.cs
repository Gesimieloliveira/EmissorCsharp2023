using FusionCore.Excecoes.RegraNegocio;

namespace FusionCore.FusionAdm.Automoveis
{
    public static class ValidadorProprietario
    {
        public static void Checa(ProprietarioVeiculo proprietario)
        {
            if (proprietario.IsTransportadora == false)
            {
                throw new RegraNegocioException("Proprietário do veículo precisa ser uma Transportadora");
            }

            if (!proprietario.PossuiCnpj() && !proprietario.PossuiCpf())
            {
                throw new RegraNegocioException("Proprietário do veículo precisa ter CPF ou CNPJ");
            }
        }
    }
}