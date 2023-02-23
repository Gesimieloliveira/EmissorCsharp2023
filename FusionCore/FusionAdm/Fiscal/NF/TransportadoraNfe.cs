using System;
using FusionCore.Helpers.Hidratacao;
using FusionLibrary.Validacao;
using FusionLibrary.Validacao.Regras;
using JetBrains.Annotations;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable NonReadonlyMemberInGetHashCode
// ReSharper disable UnusedAutoPropertyAccessor.Local
// ReSharper disable UnusedMember.Global

namespace FusionCore.FusionAdm.Fiscal.NF
{
    public class TransportadoraNfe
    {
        private string _documentoUnico;
        private int? _pessoaId;
        private int _nfeId;
        public Nfeletronica Nfe { get; private set; }
        public string Nome { get; set; }
        public string InscricaoEstadual { get; set; }
        public string EnderecoCompleto { get; set; }
        public string NomeMunicipio { get; set; }
        public string SiglaUf { get; set; }
        public VeiculoTransporte Veiculo { get; set; }

        public bool IsTemTransportadora()
        {
            return DocumentoUnico.IsNotNullOrEmpty() && Nome.IsNotNullOrEmpty()
                   && EnderecoCompleto.IsNotNullOrEmpty() && NomeMunicipio.IsNotNullOrEmpty() && SiglaUf.IsNotNullOrEmpty();
        }

        public string DocumentoUnico
        {
            get { return _documentoUnico; }
            set
            {
                if (new DocumentoUnicoRegra().NaoValido(value) && value.IsNotNullOrEmpty())
                    throw new ArgumentException("Você não pode utilizar CPF/CNPJ errado na transportadora");

                _documentoUnico = value;
            }
        }

        private TransportadoraNfe()
        {
            InscricaoEstadual = string.Empty;
            EnderecoCompleto = string.Empty;
            NomeMunicipio = string.Empty;
            SiglaUf = string.Empty;
        }

        public TransportadoraNfe(
            [NotNull] Nfeletronica nfe,
            [NotNull] string documentoUnico,
            [NotNull] string nome) : this()
        {
            DocumentoUnico = documentoUnico;
            Nome = nome;
            Nfe = nfe;
        }

        private bool Equals(TransportadoraNfe other)
        {
            return _nfeId == other._nfeId;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((TransportadoraNfe) obj);
        }

        public override int GetHashCode()
        {
            return _nfeId;
        }

        public static bool operator ==(TransportadoraNfe left, TransportadoraNfe right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(TransportadoraNfe left, TransportadoraNfe right)
        {
            return !Equals(left, right);
        }

        public int GetPessoaId()
        {
            return _pessoaId ?? 0;
        }

        public void ReferenciaUmaPessoaId(int pessoaId)
        {
            _pessoaId = pessoaId == 0 ? (int?) null : pessoaId;
        }
    }
}