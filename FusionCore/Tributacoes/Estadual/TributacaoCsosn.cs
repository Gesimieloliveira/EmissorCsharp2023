using System.Diagnostics.CodeAnalysis;
using FusionCore.Repositorio.Base;

// ReSharper disable UnusedMember.Global

namespace FusionCore.Tributacoes.Estadual
{
    [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local")]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class TributacaoCsosn : Entidade
    {
        public string Codigo { get; private set; }
        public string Descricao { get; private set; }
        public string DescricaoCompleta => $"{Codigo} - {Descricao}";
        protected override int ReferenciaUnica => Codigo?.GetHashCode() ?? 0;

        public bool PermiteCredito()
        {
            return Codigo == "101" || Codigo == "201" || Codigo == "900";
        }

        public bool PermiteIcms()
        {
            return Codigo == "900";
        }

        public bool PermiteIcmsSt()
        {
            return Codigo == "201" || Codigo == "202" || Codigo == "203" || Codigo == "900";
        }

        public bool PermiteFcpSt()
        {
            var permiteFcpSt = Codigo == "201" || Codigo == "202" || Codigo == "203" || Codigo == "900";

            return permiteFcpSt;
        }

        public bool NaoTemFcpSt()
        {
            var naoTemFcpSt = !PermiteFcpSt();

            return naoTemFcpSt;
        }

        public override string ToString()
        {
            return DescricaoCompleta;
        }
    }
}