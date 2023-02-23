using Fusion.Sessao;
using FusionCore.Configuracoes;
using FusionCore.Preferencias;
using FusionCore.Preferencias.Repositorios;
using FusionCore.Sessao;
using FusionLibrary.VisaoModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fusion.Visao.Configuracao.Model
{
    public class ConfiguracaoVendedorModel : AutoSaveModel
    {
        private readonly PreferenciaSistemaService _preferencias = SessaoSistema.Instancia.Preferencias;

        public bool UsarVendedor
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        protected override void OnInicializa()
        {
            UsarVendedor = _preferencias.Obter(Preferencias.Vendedor.ObrigarUsoVendedor, false);
        }

        protected override void OnSalvaAlteracoes()
        {
            _preferencias.Salvar(
                Preferencias.Vendedor.ObrigarUsoVendedor, 
                UsarVendedor.ToString(), 
                regraGlobal: true);
        }
    }
}
