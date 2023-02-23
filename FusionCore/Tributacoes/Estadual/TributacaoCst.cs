using FusionCore.Repositorio.Base;
using FusionCore.Tributacoes.Flags;

namespace FusionCore.Tributacoes.Estadual
{
    public class TributacaoCst : EntidadeBase<string>
    {
        public string Id { get; set; }
        protected override string ChaveUnica => Id;
        public string Descricao { get; set; }
        public RegimeTributario RegimeTributario { get; set; }
        public string DescricaoCompleta => $"{Id} - {Descricao}";

        public override string ToString()
        {
            return DescricaoCompleta;
        }

        public bool PermiteIcms()
        {
            return Id == "00" || Id == "10" || Id == "20" || Id == "51" || 
                   Id == "70" || Id == "90" || Id == "900";
        }

        public bool PermiteReducaoIcms()
        {
            return Id == "20" || Id == "51" || Id == "70" || Id == "90" || Id == "900";
        }

        public bool PermiteSubstituicao()
        {
            return Id == "10" || Id == "30" || Id == "70" || Id == "90" || 
                   Id == "30" || Id == "201" || Id == "202" || Id == "203" || 
                   Id == "900";
        }

        public bool PermiteFcp()
        {
            return Id == "00" || Id == "10" || Id == "20" || Id == "70" || Id == "90";
        }

        public bool PermiteFcpSt()
        {
            return Id == "10" || Id == "30" || Id == "51" || Id == "70" || Id == "90" || 
                   Id == "202" || Id == "203" || Id == "900";
        }

        public bool PermiteCredito()
        {
            return Id == "101" || Id == "201" || Id == "900";
        }
    }
}