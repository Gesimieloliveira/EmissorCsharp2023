using System;

namespace FusionCore.Sintegra.Dto
{
    public class ItemRegistro61Nfce
    {
        public DateTime EmissaoNoDia { get; set; }
        public short Serie { get; set; }
        public int NumeroFiscal { get; set; }
        public decimal ValorTotal { get; set; }
        public string Cst { get; set; }
        public int Id { get; set; }

        public void AtualizarCst()
        {
            if (Cst == "500" || Cst == "60")
            {
                Cst = "Substituicao";
                return;
            }

            Cst = "Tributado";
        }
    }
}