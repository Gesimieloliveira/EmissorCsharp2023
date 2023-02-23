using System;
using FusionCore.CadastroEmpresa;
using FusionCore.CadastroUsuario;
using FusionCore.FusionAdm.Financeiro.Flags;
using FusionCore.FusionAdm.Financeiro.Repositorios;
using FusionCore.FusionAdm.Pessoas;
using FusionCore.FusionWPF.Financeiro.Contratos.Financeiro;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using NHibernate;

namespace FusionCore.FusionAdm.Financeiro.Servicos
{
    public class ServicoCadastroDeDocumento
    {
        private readonly ISession _session;

        public ServicoCadastroDeDocumento(ISession session)
        {
            _session = session;
        }

        public DocumentoReceber ObterCadastroPeloId(int id)
        {
            var repositorio = new RepositorioDocumentoReceber(_session);

            var documento = repositorio.BuscarPeloId(id);

            if (documento == null)
            {
                throw new InvalidOperationException(
                    $"Não consegui encontrar Documento a Receber para este Id {id}");
            }

            return documento;
        }

        public Malote CriarMalote(OrigemDocumento origem, UsuarioDTO usuario)
        {
            var malote = Malote.Cria(origem, string.Empty, usuario, 0.00M);

            return malote;
        }

        public DocumentoReceber CriarNovo(
            IUsuario usuario, 
            IEmpresa empresa, 
            Cliente cliente,
            decimal valorDocumento,
            DateTime dataEmissao,
            DateTime dataVencimento,
            ITipoDocumento tipoDocumento,
            byte parcela = 1,
            string descriacao = null
        )
        {
            var repositorioEmpresa = new RepositorioEmpresa(_session);
            var repositorioUsuarios = new RepositorioUsuario(_session);

            var usuarioAcao = repositorioUsuarios.GetPeloId(usuario.Id);

            var doc = new DocumentoReceber
            {
                UsuarioCriacao = usuarioAcao,
                Empresa = repositorioEmpresa.GetPeloId(empresa.Id),
                ValorOriginal = valorDocumento,
                Cliente = cliente,
                Descricao = descriacao ?? string.Empty,
                EmitidoEm = dataEmissao,
                Vencimento = dataVencimento,
                TipoDocumento = tipoDocumento,
                Parcela = parcela
            };

            doc.AjustarValorPara(valorDocumento, usuarioAcao);

            return doc;
        }

        public void PersistirMalote(Malote malote)
        {
            var repositorio = new RepositorioMalote(_session);
            repositorio.Persiste(malote);
        }

        public void SalvarDocumento(DocumentoReceber documento)
        {
            var repositorio = new RepositorioDocumentoReceber(_session);
            repositorio.SalvarAlteracoes(documento);
        }
    }
}