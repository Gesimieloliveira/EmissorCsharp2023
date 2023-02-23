using System;
using FusionCore.Seguranca.Licenciamento.Dominio;
using Newtonsoft.Json;

namespace FusionCore.Seguranca.Licenciamento.JsonConverters
{
    public class TipoLicencaConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var modulo = (TipoLicenca) value;
            writer.WriteValue(modulo.GetStringValue());
        }

        public override object ReadJson(JsonReader reader,
            Type objectType,
            object existingValue,
            JsonSerializer serializer)
        {
            var stringValue = (string) reader.Value;
            return TipoLicencaFactory.CriaApartirString(stringValue);
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof (string);
        }
    }
}