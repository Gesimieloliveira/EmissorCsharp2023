using FusionCore.Repositorio.Base;

namespace FusionCore.Repositorio.Dtos.Consultas
{
    public class EmissorComboBoxDTO : EntidadeBase<byte>
    {
        public byte Id { get; set; }
        
        public EmpresaComboBoxDTO EmpresaComboBox { get; set; }

        protected override byte ChaveUnica => Id;
        public bool EmUso { get; set; }

        public string NomeEmpresa => $"[{ObterTipoEmissor()}] - {EmpresaComboBox.Nome}";

        private string ObterTipoEmissor()
        {
            if (IsNfce) return "NFC-e";

            return IsSat && IsMfe == false ? "SAT" : "MF-e";
        }

        public bool IsSat { get; set; }
        public bool IsMfe { get; set; }
        public bool IsNfce { get; set; }
    }
}