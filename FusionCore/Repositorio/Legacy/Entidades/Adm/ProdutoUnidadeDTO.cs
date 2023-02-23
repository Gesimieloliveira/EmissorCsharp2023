using System;
using FusionCore.NfceSincronizador.Contratos;
using FusionCore.NfceSincronizador.Flags;
using FusionCore.Repositorio.Base;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace FusionCore.Repositorio.Legacy.Entidades.Adm
{
    public class ProdutoUnidadeDTO : Entidade, ISincronizavelAdm
    {
        public ProdutoUnidadeDTO()
        {
            AlteradoEm = DateTime.Now;
            PodeFracionar = 0;
            SolicitaTotalPdv = false;
            SolicitarPeso = false;
        }

        public int Id { get; set; }
        public string Sigla { get; set; }
        public string Nome { get; set; }
        public int PodeFracionar { get; set; }
        public bool SolicitaTotalPdv { get; set; }
        public bool SolicitarPeso { get; private set; }
        public DateTime? AlteradoEm { get; private set; }
        protected override int ReferenciaUnica => Id;
        public string FrancionadoTexto => PodeFracionar == 1 ? "Sim" : "Não";
        public string SolicitaTotalPdvTexto => SolicitaTotalPdv ? "Sim" : "Não";
        public string SolicitarPesoTexto => SolicitarPeso ? "Sim" : "Não";

        // ISincronizavelAdm
        public EntidadeSincronizavel EntidadeSincronizavel => EntidadeSincronizavel.ProdutoUnidade;
        public string Referencia => Id.ToString();

        public override string ToString()
        {
            return Sigla;
        }

        public void Alterar(string nome, string sigla, int podeFacionar, bool solicitarTotal, bool solicitarPeso)
        {
            if (sigla.Length < 2)
                throw new ArgumentException(@"Sigla não pode ser menor que 2 caracteres", nameof(sigla));

            if (nome.Length < 2)
                throw new ArgumentException(@"Nome não pode ser menor que 2 caracteres", nameof(nome));

            if (podeFacionar == 0 && (solicitarTotal || solicitarPeso))
                throw new ArgumentException(@"PodeFracionar precisa estar ativo para Solicitar Total ou Peso", nameof(podeFacionar));

            if (solicitarTotal && solicitarPeso)
                throw new ArgumentException(@"Solicitar Total ou Peso não podem ambos ser ativos", nameof(solicitarTotal));

            AlteradoEm = DateTime.Now;
            Nome = nome;
            Sigla = sigla;
            PodeFracionar = podeFacionar;
            SolicitaTotalPdv = solicitarTotal;
            SolicitarPeso = solicitarPeso;
        }
    }
}