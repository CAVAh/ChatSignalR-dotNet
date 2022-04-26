using NHibernate;
using ChatSignalR.Models;
using ChatSignalR.NHibernate;
using ISession = NHibernate.ISession;

namespace ChatSignalR.Services
{
    public class UserService
    {
        public IEnumerable<User> ConsultarLista()
        {
            IList<User> Resultado = null;
            using (ISession Session = NHibernateHelper.GetSessionFactory().OpenSession())
            {
                NHibernateDAL<User> DAL = new NHibernateDAL<User>(Session);
                Resultado = DAL.Select(new User());
            }
            return Resultado;
        }

        public IEnumerable<User> ConsultarListaFiltro(Filtro filtro)
        {
            IList<User> Resultado = null;
            using (ISession Session = NHibernateHelper.GetSessionFactory().OpenSession())
            {
                var consultaSql = "from User where " + filtro.Where;
                NHibernateDAL<User> DAL = new NHibernateDAL<User>(Session);
                Resultado = DAL.SelectListaSql<User>(consultaSql);
            }
            return Resultado;
        }

        public User ConsultarObjeto(int id)
        {
            User Resultado = null;
            using (ISession Session = NHibernateHelper.GetSessionFactory().OpenSession())
            {
                NHibernateDAL<User> DAL = new NHibernateDAL<User>(Session);
                Resultado = DAL.SelectId<User>(id);
            }
            return Resultado;
        }

        public void Inserir(User objeto)
        {
            using (ISession Session = NHibernateHelper.GetSessionFactory().OpenSession())
            {
                NHibernateDAL<User> DAL = new NHibernateDAL<User>(Session);
                DAL.SaveOrUpdate(objeto);
                Session.Flush();
            }
        }

        public void Alterar(User objeto)
        {
            using (ISession Session = NHibernateHelper.GetSessionFactory().OpenSession())
            {
                NHibernateDAL<User> DAL = new NHibernateDAL<User>(Session);
                DAL.SaveOrUpdate(objeto);
                Session.Flush();
            }
        }

        public void Excluir(User objeto)
        {
            using (ISession Session = NHibernateHelper.GetSessionFactory().OpenSession())
            {
                NHibernateDAL<User> DAL = new NHibernateDAL<User>(Session);
                DAL.Delete(objeto);
                Session.Flush();
            }
        }
    }

}
