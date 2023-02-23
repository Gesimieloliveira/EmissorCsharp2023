using System;
using FusionCore.FusionAdm.Fiscal.Flags;

// ReSharper disable ClassNeverInstantiated.Global

namespace FusionCore.Servicos.Core.Exportacao
{
    public class DocumentoXml
    {
        public int Id { get; set; }
        public string Chave { get; set; }
        public string Xml { get; set; }
        public int CodigoCancelamento { get; set; }
        public DateTime Recebimento { get; set; }
        public TipoAmbiente Ambiente { get; set; }

        public string GetCnpjEmitente()
        {
            return Chave.Substring(6, 14);
        }

        public string GetModelo()
        {
            return Chave.Substring(20, 2);
        }

        public string GetMesAnoAutorizacao()
        {
            return Recebimento.ToString("yyyy-MM");
        }
    }
}