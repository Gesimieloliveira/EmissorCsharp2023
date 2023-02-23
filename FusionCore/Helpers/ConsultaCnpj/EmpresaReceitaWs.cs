using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using FusionCore.FusionAdm.Componentes;
using FusionCore.FusionAdm.Servico.Endereco;
using FusionCore.Helpers.Basico;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionLibrary.Validacao.Regras;

namespace FusionCore.Helpers.ConsultaCnpj
{
    public class EmpresaReceitaWs
    {
        private readonly LocalidadesServico _localidades = LocalidadesServico.GetInstancia();

        private EmpresaReceitaWs()
        {
            Telefone = new List<string>();
        }

        public string Cnpj { get; set; }
        public DateTime? InicioAtividade { get; set; }
        public string RazaoSocial { get; set; }
        public string NomeFantasia { get; set; }
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Cep { get; set; }
        public string Bairro { get; set; }
        public IList<string> Telefone { get; set; }
        public Email Email { get; set; }
        public CidadeDTO Municipio { get; set; }
        public EstadoDTO Uf { get; set; }

        public static EmpresaReceitaWs Cria(DataReceitaWs receitaWs)
        {
            var empresa = new EmpresaReceitaWs();

            empresa.InicioAtividade = DateTimeHelper.Parse(receitaWs.Abertura);
            empresa.Cnpj = Regex.Replace(receitaWs.Cnpj, @"[^0-9]", "");
            empresa.RazaoSocial = Regex.Replace(receitaWs.Nome, @"[\s]{2,}", " ");
            empresa.NomeFantasia = Regex.Replace(receitaWs.Fantasia, @"[\s]{2,}", " ");
            empresa.Logradouro = Regex.Replace(receitaWs.Logradouro, @"[\s]{2,}", " ");
            empresa.Numero = Regex.Replace(receitaWs.Numero, @"[\s]{2,}", " ");
            empresa.Complemento = Regex.Replace(receitaWs.Complemento, @"[\s]{2,}", " ");
            empresa.Cep = Regex.Replace(receitaWs.Cep, @"[^0-9]", "");
            empresa.Bairro = Regex.Replace(receitaWs.Bairro, @"[\s]{2,}", " ");

            SetTelefones(empresa, receitaWs);

            var email = Regex.Replace(receitaWs.Email, @"[\s]{2,}", " ");

            if (new EmailRegra().AplicaRegra(email))
            {
                empresa.Email = new Email(email.ToLower());
            }

            var municpio = Regex.Replace(receitaWs.Municipio, @"[\s]{2,}", " ");
            var uf = Regex.Replace(receitaWs.Uf, @"[\s]{2,}", " ");

            empresa.Municipio = empresa._localidades.GetCidade(c => c.CompareNome(municpio) && c.CompareSiglaUf(uf));
            empresa.Uf = empresa._localidades.GetEstado(u => u.CompareSigla(uf));

            return empresa;
        }

        private static void SetTelefones(EmpresaReceitaWs empresa, DataReceitaWs receitaWs)
        {
            if (receitaWs.Telefone == null)
            {
                return;
            }

            var partes = receitaWs.Telefone.Split('/');

            foreach (var parte in partes)
            {
                var telefone = Regex.Replace(parte, @"[^0-9]", "");

                if (telefone.Length == 10 || telefone.Length == 11)
                {
                    empresa.Telefone.Add(telefone);
                }
            }
        }
    }
}