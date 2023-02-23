using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Fusion.Visao.Base.VisaoModel;
using FusionCore.FusionAdm.Sessao;
using FusionCore.FusionPdv.ModeloEcf;
using FusionCore.Repositorio.Legacy.Ativos;
using FusionCore.Repositorio.Legacy.Buscas.Adm.Ecf;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using NHibernate.Util;

namespace Fusion.Visao.Ecf
{
    public class EcfFormModel : FormModelValidationBase<PdvEcfDTO>
    {
        private bool _ativo;
        private IList<ModeloEcfTemplate> _modelosDeEcf;

        public EcfFormModel(PdvEcfDTO ecf)
        {
            Model = ecf;
        }

        public virtual bool Ativo
        {
            get { return _ativo; }
            set
            {
                if (value.Equals(_ativo)) return;
                _ativo = value;
                PropriedadeAlterada();
            }
        }

        [Required(ErrorMessage = @"Numero é obrigatório")]
        public virtual string NumeroEcf
        {
            get { return GetValue(() => NumeroEcf); }
            set
            {
                SetValue(value);
            }
        }

        [Required(ErrorMessage = @"Serie é obrigatório")]
        public virtual string Serie
        {
            get { return GetValue(() => Serie); }
            set
            {
                SetValue(value);
            }
        }

        [Required(ErrorMessage = @"Modelo é obrigatório")]
        public virtual ModeloEcfTemplate Modelo
        {
            get { return GetValue(() => Modelo); }
            set
            {
                SetValue(value);
            }
        }

        public IList<ModeloEcfTemplate> ModelosDeEcf
        {
            get { return _modelosDeEcf; }
            set
            {
                if (Equals(value, _modelosDeEcf)) return;
                _modelosDeEcf = value;
                PropriedadeAlterada();
            }
        }

        public override void PreencherViewModel()
        {
            if (_modelosDeEcf == null)
                PreencherListaDeModelos();

            var modeloEcf = ModelosDeEcf.Where(m => m.ObterModeloEcf == Model.Modelo).FirstOrNull();

            Modelo = (ModeloEcfTemplate) modeloEcf;
            Ativo = Model.Ativo;
            NumeroEcf = Model.NumeroEcf;
            Serie = Model.Serie;
        }

        private void PreencherListaDeModelos()
        {
            var listador = new ListaModeloEcf();
            _modelosDeEcf = listador.ObterModelosEcf();
        }

        public override void SalvarModel()
        {

            try
            {
                using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
                {
                    var repositorio = new RepositorioComun<PdvEcfDTO>(sessao);
                    var ecfEncontrado = repositorio.Busca(new EcfPeloNumeroSerie(Serie));

                    if (ecfEncontrado != null && ecfEncontrado.Id != Model.Id)
                        LancarExcecaoDeErro("Número de serie já cadastrado");

                    Model.Modelo = Modelo?.ObterModeloEcf;
                    Model.Ativo = Ativo;
                    Model.NumeroEcf = NumeroEcf;
                    Model.Serie = Serie;
                    Model.AlteradoEm = DateTime.Now;

                    Model = Model.Id > 0 ? repositorio.Mescla(Model) : repositorio.Persiste(Model);
                }
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(e.Message);
            }
        }
    }
}