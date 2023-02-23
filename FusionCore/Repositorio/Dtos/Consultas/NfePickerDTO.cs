using System;
using System.Diagnostics.CodeAnalysis;
using FusionCore.FusionAdm.Fiscal.NF.Cancelar;
using FusionCore.Helpers.DocumentoXml;

namespace FusionCore.Repositorio.Dtos.Consultas
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class NfePickerDTO
    {
        private bool _temCache;
        private decimal _cacheTotalNf;
        public int Id { get; set; }
        public string Chave { get; set; }
        public string NomeDestinatario { get; set; }
        public string NomeEmitente { get; set; }
        public DateTime? EmitidaEm { get; set; }
        public StatusCancelamento Status { get; set; }
        public string XmlAutorizacao { get; set; }
        public int MunicipioIbgeDestino { get; set; }

        public decimal TotalNf
        {
            get
            {
                if (_temCache)
                {
                    return _cacheTotalNf;
                }

                var xmlHelper = new XmlHelper(XmlAutorizacao);
                var value = xmlHelper.GetValueFromElement("vNF", "ICMSTot");

                _cacheTotalNf = value.GetDecimalValue();
                _temCache = true;

                return _cacheTotalNf;
            }
        }
    }
}