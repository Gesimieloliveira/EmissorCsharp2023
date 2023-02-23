using System;
using System.Text.RegularExpressions;
using FusionCore.FusionPdv.Sessao;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Repositorio.Legacy.Ativos.Pdv;
using FusionCore.Repositorio.Legacy.Entidades.Pdv;
using FusionLibrary.VisaoModel;
using FusionPdv.Validacao;

namespace FusionPdv.Visao.Cliente
{
    public class AdicionarClienteModel : ModelBase
    {
        private ClienteDt _clienteDt;
        private string _observacao;
        private string _cpfOuCnpj;
        private string _nome;
        private string _endereco;


        public string Nome
        {
            get { return _nome; }
            set
            {
                _nome = value;
                PropriedadeAlterada();
            }
        }

        public string Endereco
        {
            get { return _endereco; }
            set
            {
                _endereco = value;
                PropriedadeAlterada();
            }
        }

        public string CpfOuCnpj
        {
            get { return _cpfOuCnpj; }
            set
            {
                _cpfOuCnpj = value;
                PropriedadeAlterada();
            }
        }

        public ClienteDt Cliente
        {
            get { return _clienteDt; }
            set
            {
                _clienteDt = value; 
                PropriedadeAlterada();
            }
        }

        public string Observacao
        {
            get { return _observacao; }
            set
            {
                _observacao = value; 
                PropriedadeAlterada();
            }
        }

        public ClienteDt BuscarClientePorCpfOuCpj()
        {
            try
            {
                if (string.IsNullOrEmpty(_cpfOuCnpj)) return null;

                _cpfOuCnpj = new Regex(@"[^\d]").Replace(_cpfOuCnpj, "");

                if (long.Parse(_cpfOuCnpj) == 0) throw new ExceptionCpfOuCnpj("Digite um Cpf ou Cnpj!");

                var cpfOuCnpj = _cpfOuCnpj;

                new CpfOuCnpj(cpfOuCnpj).Executar();

                using (var sessao = GerenciaSessao.ObterSessao("SessaoPdv").AbrirSessao())
                {
                    return new ClienteRepositorio(sessao).BuscarPorCpfOuCnpj(cpfOuCnpj);    
                }
                
            }
            catch (ExceptionCpfOuCnpj ex)
            {
                Nome = "";
                Endereco = "";
                Observacao = "";
                Cliente = null;
                throw new ExceptionCpfOuCnpj(ex.Message, ex);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void RecebeCliente(ClienteDt retorno)
        {
            if (retorno == null) return;

            Cliente = retorno;

            CpfOuCnpj = string.IsNullOrEmpty(Cliente.Cpf?.Trim()) ? Cliente.Cnpj : Cliente.Cpf;
            Nome = Cliente.Nome;
            Endereco = Cliente.Endereco;
        }

        public ClienteDt ClienteRetorno()
        {
            var cliente = new ClienteDt();


            if (Cliente != null)
            {
                cliente = Cliente;
            }
            else
            {
                Endereco = Endereco.TrimOrEmpty();
                if (Nome == null || CpfOuCnpj == null) return null;
                if (Nome.Trim().Equals("") || CpfOuCnpj.Trim().Equals(""))
                    return null;

                try
                {
                    new CpfOuCnpj(CpfOuCnpj.Trim()).Executar();
                }
                catch (Exception)
                {
                    return null;
                }
                


                cliente.Nome = Nome.Trim();

                var cpfoucnpj = new Regex(@"[^\d]").Replace(CpfOuCnpj.Trim(), "");

                if (long.Parse(cpfoucnpj) == 0) throw new ExceptionCpfOuCnpj("Digite um Cpf ou Cnpj!");

                if (cpfoucnpj.Length == 11)
                {
                    cliente.Cpf = cpfoucnpj;
                }
                else
                {
                    cliente.Cnpj = cpfoucnpj;
                }

                
                cliente.Endereco = Endereco.Trim();
            }

            return cliente;
        }

        public string Mascara(string cpfOuCnpj)
        {
            if (string.IsNullOrEmpty(cpfOuCnpj))
            {
                cpfOuCnpj = "";
            }

            var cpfcnpj = cpfOuCnpj.Trim();
            cpfcnpj = new Regex(@"[^\d]").Replace(cpfcnpj, "");

            return Convert.ToUInt64(cpfcnpj).ToString(cpfcnpj.Length == 11 ? @"000\.000\.000\-00" : @"00\.000\.000\/0000\-00");
        }
    }
}
