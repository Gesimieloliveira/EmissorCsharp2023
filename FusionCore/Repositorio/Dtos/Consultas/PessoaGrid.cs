using System.Collections.Generic;
using FusionCore.Core.Formatadores;
using FusionCore.FusionAdm.Pessoas;
using static System.String;

// ReSharper disable ClassNeverInstantiated.Global

namespace FusionCore.Repositorio.Dtos.Consultas
{
    public class PessoaGrid
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public PessoaTipo Tipo { get; set; }
        public string Cpf { get; set; }
        public string Cnpj { get; set; }
        public string DocumentoExtrangeiro { get; set; }
        public string Rg { get; set; }
        public string InscricaoEstadual { get; set; }
        public bool IsCliente { get; set; }
        public bool IsFornecedor { get; set; }
        public bool IsTransportadora { get; set; }
        public bool IsVendedor { get; set; }
        public bool IsFisica => Tipo == PessoaTipo.Fisica;
        public bool IsJuridica => Tipo == PessoaTipo.Juridica;
        public bool IsExtrangeiro => Tipo == PessoaTipo.Extrangeiro;
        public string DescricaoTipo => IsJuridica ? "Júridica" : "Física";

        public string ExtensaoString
        {
            get
            {
                var extensao = new List<string>();

                if (IsCliente)
                {
                    extensao.Add("Cliente");
                }

                if (IsFornecedor)
                {
                    extensao.Add("Fornecedor");
                }

                if (IsTransportadora)
                {
                    extensao.Add("Transportadora");
                }

                return Join(", ", extensao);
            }
        }

        public string DocumentoUnicoString
        {
            get
            {
                if (IsFisica)
                {
                    return Cpf?.ToFormatoCpf();
                }

                if (IsJuridica)
                {
                    return Cnpj?.ToFormadoCnpj();
                }

                if (IsExtrangeiro)
                {
                    return DocumentoExtrangeiro;
                }

                return Empty;
            }
        }
    }
}