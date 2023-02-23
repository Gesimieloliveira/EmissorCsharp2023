using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using FusionCore.Preferencias.Repositorios;
using FusionCore.Sessao;

// ReSharper disable ConvertToUsingDeclaration

namespace FusionCore.Preferencias
{
    public class PreferenciaSistemaService
    {
        private readonly ISessaoManager _sessaoManager;

        public PreferenciaSistemaService(ISessaoManager sessaoManager)
        {
            _sessaoManager = sessaoManager;
        }

        private static readonly string IdMaquina = PreferenciaSistema.GetIdMaquina();
        private static readonly object MemoryCacheLock = new object();
        private static readonly Collection<PreferenciaSistema> MemoryCache = new Collection<PreferenciaSistema>();

        public void Salvar(string chave, string valor, bool regraGlobal = false)
        {
            using (var sessao = _sessaoManager.CriaSessao())
            {
                var idMaquina = regraGlobal ? Preferencias.Global : IdMaquina;

                var repositorio = new RepositorioPreferenciaSistema(sessao);
                var preferencia = repositorio.Buscar(idMaquina, chave);

                if (preferencia is null)
                {
                    preferencia = new PreferenciaSistema(chave, valor, regraGlobal);
                    repositorio.Inserir(preferencia);
                    UpdateCache(preferencia);
                    return;
                }

                preferencia.Valor = valor;
                repositorio.Alterar(preferencia);
                UpdateCache(preferencia);
            }
        }

        private static void UpdateCache(PreferenciaSistema preferencia)
        {
            lock (MemoryCacheLock)
            {
                var inCache = MemoryCache.FirstOrDefault(e => e.Chave == preferencia.Chave);
                if (inCache != null) MemoryCache.Remove(inCache);

                MemoryCache.Add(preferencia);
            }
        }

        public T Obter<T>(string chave, T valorPadrao = default(T))
        {
            if (TryGetFromCache(chave, out var inCache))
                return ConverterResultado(inCache, valorPadrao);

            using (var sessao = _sessaoManager.CriaSessao())
            {
                var repositorio = new RepositorioPreferenciaSistema(sessao);
                var preferencia = repositorio.Buscar(IdMaquina, chave);

                if (preferencia == null)
                {
                    preferencia = repositorio.Buscar(Preferencias.Global, chave);
                }

                if (preferencia?.Valor == null)
                    return valorPadrao;

                UpdateCache(preferencia);

                return ConverterResultado(preferencia, valorPadrao);
            }
        }

        private static T ConverterResultado<T>(PreferenciaSistema preferencia, T valorPadrao = default(T))
        {
            var converter = TypeDescriptor.GetConverter(typeof(T));
            var convertedValue = converter.ConvertFromString(preferencia.Valor);

            return convertedValue is T result
                ? result
                : valorPadrao;
        }

        private static bool TryGetFromCache(string chave, out PreferenciaSistema inCache)
        {
            lock (MemoryCacheLock)
            {
                inCache = MemoryCache.FirstOrDefault(e => e.Chave == chave && e.IdMaquina == IdMaquina);

                if (inCache is null)
                {
                    inCache = MemoryCache.FirstOrDefault(e => e.Chave == chave && e.IdMaquina == Preferencias.Global);
                }
            }

            return inCache != null;
        }
    }
}