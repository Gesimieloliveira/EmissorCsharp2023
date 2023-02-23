using System;
using System.Collections.Generic;
using System.Linq;
using FusionCore.FusionAdm.Fiscal.Extensoes.ZeusConversores.Factories;

namespace FusionCore.FusionAdm.Fiscal.Extensoes.ZeusConversores.Providers
{
    public class FactoryIcmsZeusProvider
    {
        private readonly IList<IFactoryIcmsZeus> _factories = new List<IFactoryIcmsZeus>();

        private FactoryIcmsZeusProvider()
        {
            _factories.Add(new FactoryIcmsZeus00());
            _factories.Add(new FactoryIcmsZeus10());
            _factories.Add(new FactoryIcmsZeus20());
            _factories.Add(new FactoryIcmsZeus30());
            _factories.Add(new FactoryIcmsZeus40());
            _factories.Add(new FactoryIcmsZeus41());
            _factories.Add(new FactoryIcmsZeus50());
            _factories.Add(new FactoryIcmsZeus51());
            _factories.Add(new FactoryIcmsZeus60());
            _factories.Add(new FactoryIcmsZeus70());
            _factories.Add(new FactoryIcmsZeus90());

            _factories.Add(new FactoryIcmsZeus101());
            _factories.Add(new FactoryIcmsZeus102());
            _factories.Add(new FactoryIcmsZeus103());
            _factories.Add(new FactoryIcmsZeus201());
            _factories.Add(new FactoryIcmsZeus202());
            _factories.Add(new FactoryIcmsZeus203());
            _factories.Add(new FactoryIcmsZeus300());
            _factories.Add(new FactoryIcmsZeus400());
            _factories.Add(new FactoryIcmsZeus500());
            _factories.Add(new FactoryIcmsZeus900());
        }

        public static FactoryIcmsZeusProvider Instance { get; } = new FactoryIcmsZeusProvider();

        public IFactoryIcmsZeus Get(string cst)
        {
            var factory = _factories.FirstOrDefault(i => i.Cst == cst);

            if (factory == null)
            {
                throw new InvalidOperationException($"Não encontrei a Factory correspondente para CST {cst}");
            }

            return factory;
        }
    }
}