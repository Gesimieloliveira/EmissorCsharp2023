using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using FontAwesome.WPF;
using FusionLibrary.VisaoModel;
using NHibernate.Util;

namespace FusionNfce.Visao.Avisos
{
    public class AvisosFormModel : ModelBase
    {
        public AvisosFormModel()
        {
            Itens = new ObservableCollection<Aviso>();
        }

        public ObservableCollection<Aviso> Itens { get; set; }
        public Aviso ItemSelecionado { get; set; }

        public void AddAviso(FontAwesomeIcon fontAwesomeIcon, string mensagem, ICommand action = null)
        {
            var aviso = new Aviso
            {
                Action = action,
                Mensagem = mensagem,
                Icone = fontAwesomeIcon
            };

            Itens.Insert(0, aviso);
        }

        public void AddAvisos(IEnumerable<Aviso> avisos)
        {
            avisos.ForEach(Itens.Add);
        }
    }
}