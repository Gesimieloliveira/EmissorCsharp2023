namespace Sped.Dominio
{
    public class Participante
    {
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public short Pais => 1058;
        public string Cnpj { get; set; }
        public string Cpf { get; set; }
        public string InscricaoEstadual { get; set; }
        public string CodigoMunicipio { get; set; }
        public string Suframa { get; set; }
        public string Endereco { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
    }
}