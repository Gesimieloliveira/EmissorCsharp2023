using System;
using FusionCore.Excecoes.Sessao;
using FusionCore.FusionPdv.Sessao;
using FusionCore.Repositorio.Legacy.Ativos.Pdv;
using FusionCore.Repositorio.Legacy.Base.Execao;
using FusionCore.Repositorio.Legacy.Base.Helper;
using FusionCore.Repositorio.Legacy.Entidades.Pdv;
using FusionPdv.Ecf;
using NHibernate;

namespace FusionPdv.Servicos.Ecf
{
    public class ObterEcfAtiva
    {
        private EcfRepositorio _ecfRepositorio;
        private ISession _sessao;

        public EcfDt Buscar()
        {
            try
            {
                _sessao = GerenciaSessao.ObterSessao("SessaoPdv").AbrirSessao();
                _ecfRepositorio = new EcfRepositorio(_sessao);

            }
            catch (SessaoHelperException ex)
            {
                throw new ConexaoInvalidaException("Não existe conexão valida para o servidor. Porfavor configurar.", ex);
            }
            catch (InvalidOperationException ex)
            {
                throw new RepositorioExeption("Erro ao criar um ecf repositorio.", ex);
            }
            catch (TypeInitializationException ex)
            {
                throw new ConexaoInvalidaException("Não existe conexão valida para o servidor ou pdv. Porfavor configurar.", ex);
            }

            try
            {
                var ecf = _ecfRepositorio.BuscarEfAtivo(SessaoEcf.EcfFiscal.Serie());
                return ecf;
            }
            catch (Exception ex)
            {
                throw new RepositorioExeption("Erro ao criar um ecf repositorio.", ex);
            }
            finally
            {
                _sessao.Close();   
            }
        }
    }
}