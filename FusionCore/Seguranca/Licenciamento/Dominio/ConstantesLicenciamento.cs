using System;

namespace FusionCore.Seguranca.Licenciamento.Dominio
{
    public static class ConstantesLicenciamento
    {
        public static int IntervaloChecadorSegundos = 120;
        public static int IntervaloTentativaSegundos = 30;
        public static int NumeroTenativas = 3;
        public static int ExpiracaoLicencaSegundos => IntervaloTentativaSegundos * NumeroTenativas + IntervaloTentativaSegundos + 10;
    }
}