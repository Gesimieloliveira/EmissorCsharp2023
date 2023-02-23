using FusionLibrary.Validacao;
using FusionLibrary.Validacao.Regras;
using static System.String;

namespace FusionCore.FusionAdm.Fiscal.NF.Componentes
{
    public class DocumentoUnico
    {
        private readonly string _documento;
        public bool IsCpf => _documento?.Length == 11;
        public bool IsCnpj => _documento?.Length == 14;
        public bool IsOutros => !IsCpf || !IsCnpj;
        public bool IsValido => new DocumentoUnicoRegra().Valido(ToString());

        public DocumentoUnico(string documento)
        {
            _documento = documento;
        }

        public override string ToString()
        {
            return _documento ?? Empty;
        }
    }
}