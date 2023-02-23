using FusionCore.Repositorio.Base;
using FusionCore.Tributacoes.Flags;

namespace FusionCore.SelecionarNfce
{
    public class EmitenteDto : EntidadeBase<int>
    {
        private EmitenteDto() { }
 
        public EmitenteDto(string razaoSocialEmitente, int idEmitente, RegimeTributario regimeTributario) : this()
        {
            Id = idEmitente;
            RegimeTributario = regimeTributario;
            Nome = razaoSocialEmitente;
        }

        public int Id { get; }
        public RegimeTributario RegimeTributario { get; }
        public string Nome { get; }
        protected override int ChaveUnica => Id;
    }
}