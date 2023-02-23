using FusionCore.Helpers.Hidratacao;
using static System.String;

namespace FusionCore.FusionAdm.Fiscal.NF
{
    public class VeiculoTransporte
    {
        private string _placa;
        private string _siglaUF;

        private VeiculoTransporte()
        {
            //nhibernate
        }

        public VeiculoTransporte(string placa, string siglaUF) : this()
        {
            Placa = placa ?? Empty;
            SiglaUF = siglaUF ?? Empty;
        }

        public string Placa
        {
            get => _placa;
            private set => _placa = value ?? Empty;
        }

        public string SiglaUF
        {
            get => _siglaUF;
            private set => _siglaUF = value ?? Empty;
        }

        public bool IsTemVeiculo()
        {
            return Placa.IsNotNullOrEmpty() && SiglaUF.IsNotNullOrEmpty();
        }
    }
}