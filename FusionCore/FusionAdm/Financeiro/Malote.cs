using System;
using System.Collections.Generic;
using FusionCore.FusionAdm.Financeiro.Flags;
using FusionCore.Repositorio.Base;
using FusionCore.Repositorio.Legacy.Entidades.Adm;

namespace FusionCore.FusionAdm.Financeiro
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class Malote : EntidadeBase<int>
    {
        private Malote()
        {
            CadastradoEm = DateTime.Now;
            DocumentosReceber = new List<DocumentoReceber>();
            DocumentosPagar = new List<DocumentoPagar>();
        }

        public int Id { get; set; }
        public DateTime CadastradoEm { get; set; }
        public UsuarioDTO Usuario { get; set; }
        public OrigemDocumento Origem { get; set; }
        public decimal Entrada { get; set; }
        public string OrigemTexto { get; set; }
        public string OrigemUuid { get; set; }
        public IList<DocumentoReceber> DocumentosReceber { get; }
        public IList<DocumentoPagar> DocumentosPagar { get; }

        public static Malote Cria(
            OrigemDocumento origemDocumento,
            string origemUuid,
            UsuarioDTO usuario,
            decimal valorEntrada)
        {
            return new Malote
            {
                Usuario = usuario,
                Origem = origemDocumento,
                OrigemTexto = origemDocumento.ToString(),
                OrigemUuid = origemUuid,
                Entrada = valorEntrada
            };
        }

        protected override int ChaveUnica => Id;
    }
}