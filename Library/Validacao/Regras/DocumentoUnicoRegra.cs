namespace FusionLibrary.Validacao.Regras
{
    public class DocumentoUnicoRegra : IRegra
    {
        private static DocumentoUnicoRegra _instancia;

        public bool AplicaRegra(object objeto)
        {
            return ((string) objeto)?.Length == 14
                ? new CnpjRegra().Valido(objeto)
                : new CpfRegra().Valido(objeto);
        }

        public static bool IsValido(string documento)
        {
            if (_instancia == null)
            {
                _instancia = new DocumentoUnicoRegra();
            }

            return _instancia.AplicaRegra(documento);
        }
    }
}