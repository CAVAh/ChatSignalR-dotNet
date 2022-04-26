using NHibernate;
using ChatSignalR.Models;
using ChatSignalR.NHibernate;
using ISession = NHibernate.ISession;

namespace ChatSignalR.Services
{
    public class MensagemService
    {
        public IEnumerable<Mensagens> ConsultarLista()
        {
            IList<Mensagens> Resultado = null;
            using (ISession Session = NHibernateHelper.GetSessionFactory().OpenSession())
            {
                NHibernateDAL<Mensagens> DAL = new NHibernateDAL<Mensagens>(Session);
                Resultado = DAL.Select(new Mensagens());
            }
            return Resultado;
        }

        public IEnumerable<Mensagens> ConsultarListaFiltro(Filtro filtro)
        {
            IList<Mensagens> Resultado = null;
            using (ISession Session = NHibernateHelper.GetSessionFactory().OpenSession())
            {
                var consultaSql = "from Mensagem where " + filtro.Where;
                NHibernateDAL<Mensagens> DAL = new NHibernateDAL<Mensagens>(Session);
                Resultado = DAL.SelectListaSql<Mensagens>(consultaSql);
            }
            return Resultado;
        }

        public Mensagens ConsultarObjeto(int id)
        {
            Mensagens Resultado = null;
            using (ISession Session = NHibernateHelper.GetSessionFactory().OpenSession())
            {
                NHibernateDAL<Mensagens> DAL = new NHibernateDAL<Mensagens>(Session);
                Resultado = DAL.SelectId<Mensagens>(id);
            }
            return Resultado;
        }

        public void Inserir(Mensagens objeto)
        {
            using (ISession Session = NHibernateHelper.GetSessionFactory().OpenSession())
            {
                NHibernateDAL<Mensagens> DAL = new NHibernateDAL<Mensagens>(Session);
                DAL.SaveOrUpdate(objeto);
                Session.Flush();
            }
        }

        public void Alterar(Mensagens objeto)
        {
            using (ISession Session = NHibernateHelper.GetSessionFactory().OpenSession())
            {
                NHibernateDAL<Mensagens> DAL = new NHibernateDAL<Mensagens>(Session);
                DAL.SaveOrUpdate(objeto);
                Session.Flush();
            }
        }

        public void Excluir(Mensagens objeto)
        {
            using (ISession Session = NHibernateHelper.GetSessionFactory().OpenSession())
            {
                NHibernateDAL<Mensagens> DAL = new NHibernateDAL<Mensagens>(Session);
                DAL.Delete(objeto);
                Session.Flush();
            }
        }
    }

}
