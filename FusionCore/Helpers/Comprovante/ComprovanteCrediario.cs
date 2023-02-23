using System.Collections.Generic;

namespace FusionCore.Helpers.Comprovante
{
    public enum TipoDoc
    {
        Pdv,
        Nfce
    }

    public class ComprovanteCrediario
    {
        private readonly string _nomeCliente;
        private readonly string _documentoCpfOuCnpj;
        private readonly string _numeroOperacao;
        private readonly decimal _valorTotal;
        private readonly List<string> _parcelas;
        private readonly TipoDoc _doc;

        public ComprovanteCrediario(string nomeCliente, string documentoCpfOuCnpj, string numeroOperacao, 
            decimal valorTotal, List<string> parcelas, TipoDoc doc)
        {
            _nomeCliente = nomeCliente;
            _documentoCpfOuCnpj = documentoCpfOuCnpj;
            _numeroOperacao = numeroOperacao;
            _valorTotal = valorTotal;
            _parcelas = parcelas;
            _doc = doc;
        }

        public string[] Via1 { get; set; }
        public string[] Via2 { get; set; }

        public void MontaVias()
        {
            ViaCliente();

            ViaLojista();
        }

        private void ViaCliente()
        {
            var via1 = new List<string>
            {
                "          COMPROVANTE DE VENDA A PRAZO",
                "             * VIA DO CLIENTE * ",
                string.Empty,
                string.Empty,
                "Cliente: " + _nomeCliente,
                "CPF/CNPJ: " + _documentoCpfOuCnpj,
                "COO da operação: " + _numeroOperacao,
                string.Empty,
                string.Empty,
                string.Empty,
                "              DOCUMENTOS A RECEBER",
                string.Empty,
                string.Empty,
                string.Empty,
                "Vencimento - Documento - Valor"
            };

            _parcelas.ForEach(via1.Add);

            via1.Add(string.Empty);
            via1.Add(string.Empty);

            via1.Add("Valor Total");
            via1.Add(_valorTotal.ToString("N2"));

            Via1 = via1.ToArray();
        }

        private void ViaLojista()
        {
            var via2 = new List<string>
            {
                "           COMPROVANTE DE VENDA A PRAZO",
                "                * VIA DA LOJA * ",
                string.Empty,
                string.Empty,
                "Cliente: " + _nomeCliente,
                "CPF/CNPJ: " + _documentoCpfOuCnpj,
                "COO da operação: " + _numeroOperacao,
                string.Empty,
                string.Empty,
                string.Empty,
                "               DOCUMENTOS A RECEBER",
                string.Empty,
                string.Empty,
                string.Empty,
                "Vencimento - Documento - Valor"
            };

            _parcelas.ForEach(via2.Add);

            via2.Add(string.Empty);
            via2.Add(string.Empty);

            via2.Add("Valor Total");
            via2.Add(_valorTotal.ToString("N2"));

            via2.Add(string.Empty);
            via2.Add(string.Empty);
            via2.Add(string.Empty);
            via2.Add(string.Empty);

            via2.Add("   Reconheço e pagarei a dívida acima detalhada");
            via2.Add(string.Empty);
            via2.Add(string.Empty);
            via2.Add(string.Empty);

            via2.Add("       ----------------------------------");
            via2.Add("             ASSINATURA DO CLIENTE");

            Via2 = via2.ToArray();
        }
    }
}