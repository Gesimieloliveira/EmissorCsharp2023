using System.Collections.Generic;
using FusionCore.FusionAdm.MdfeEletronico.Flags;
using FusionCore.Repositorio.Base;
using static System.String;

namespace FusionCore.FusionAdm.MdfeEletronico
{
    public class MDFeSeguroCarga : EntidadeBase<int>
    {
        public int Id { get; set; }
        protected override int ChaveUnica => Id;
        public MDFeEletronico MDFeEletronico { get; set; }
        public MDFeResponsavelSeguro Responsavel { get; set; }
        public string CnpjResponsavel { get; set; } = Empty;
        public string CpfResponsavel { get; set; } = Empty;
        public string NomeSeguradora { get; set; } = Empty;
        public string CnpjSeguradora { get; set; } = Empty;
        public string NumeroApolice { get; set; } = Empty;
        public IList<MdfeSeguroAverbacao> Averbacoes { get; set; } = new List<MdfeSeguroAverbacao>();
    }
}