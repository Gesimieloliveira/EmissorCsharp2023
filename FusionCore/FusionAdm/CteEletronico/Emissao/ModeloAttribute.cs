using System;

namespace FusionCore.FusionAdm.CteEletronico.Emissao
{
    [AttributeUsage(AttributeTargets.All)]
    public class ModeloAttribute : Attribute
    {
        public string Modelo { get; }

        public ModeloAttribute(string modelo)
        {
            Modelo = modelo;
        }
    }
}