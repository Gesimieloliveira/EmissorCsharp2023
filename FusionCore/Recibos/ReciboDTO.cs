using System;
using FusionCore.Repositorio.Legacy.Entidades.Adm;

namespace FusionCore.Recibos
{
    public class ReciboDTO
    {
        public EmpresaDTO Empresa { get; set; }
        public decimal Valor { get; set; }
        public ReciboPessoaDTO Pessoa { get; set; } 
        public CidadeDTO CidadeDTO { get; set; }
        public DateTime FeitoEm { get; set; }
        public bool Pagando { get; set; }
        public string ValorPorExtenso { get; set; }

        public string FeitoEmExtenso => ObterFeitoEmExtenso();

        private string ObterFeitoEmExtenso()
        {
            return FeitoEm.ToString("D");
        }

        public string RecebemosDeNome => ObterRecebemosDe();

        public string AssinaturaNome => ObterAssinaturaNome();

        public string AssinaturaDocumentoUnico => ObterAssinaturaDocumentoUnico();
        public string Referente { get; set; }

        private string ObterAssinaturaDocumentoUnico()
        {
            return Pagando == false ? Empresa.DocumentoUnicoFormatado : Pessoa.DocumentoUnico;
        }

        private string ObterAssinaturaNome()
        {
            return Pagando == false ? Empresa.RazaoSocial : Pessoa.Nome;
        }

        private string ObterRecebemosDe()
        {
            return Pagando == false ? Pessoa.Nome : Empresa.RazaoSocial;
        }
    }
}