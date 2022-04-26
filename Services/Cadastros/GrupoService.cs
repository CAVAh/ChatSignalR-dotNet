using NHibernate;
using ChatSignalR.Models;
using ChatSignalR.NHibernate;
using ISession = NHibernate.ISession;

namespace ChatSignalR.Services
{
    public class GrupoService
    {
        public IEnumerable<Grupo> ConsultarLista()
        {
            IList<Grupo> Resultado = null;
            using (ISession Session = NHibernateHelper.GetSessionFactory().OpenSession())
            {
                NHibernateDAL<Grupo> DAL = new NHibernateDAL<Grupo>(Session);
                Resultado = DAL.Select(new Grupo());
            }
            return Resultado;
        }

        public IEnumerable<Grupo> ConsultarListaFiltro(Filtro filtro)
        {
            IList<Grupo> Resultado = null;
            using (ISession Session = NHibernateHelper.GetSessionFactory().OpenSession())
            {
                var consultaSql = "from Mensagem where " + filtro.Where;
                NHibernateDAL<Grupo> DAL = new NHibernateDAL<Grupo>(Session);
                Resultado = DAL.SelectListaSql<Grupo>(consultaSql);
            }
            return Resultado;
        }

        public Grupo ConsultarObjeto(int id)
        {
            Grupo Resultado = null;
            using (ISession Session = NHibernateHelper.GetSessionFactory().OpenSession())
            {
                NHibernateDAL<Grupo> DAL = new NHibernateDAL<Grupo>(Session);
                Resultado = DAL.SelectId<Grupo>(id);
            }
            return Resultado;
        }

        public void Inserir(Grupo objeto)
        {
            using (ISession Session = NHibernateHelper.GetSessionFactory().OpenSession())
            {
                NHibernateDAL<Grupo> DAL = new NHibernateDAL<Grupo>(Session);
                DAL.SaveOrUpdate(objeto);
                Session.Flush();
            }
        }

        public void Alterar(Grupo objeto)
        {
            using (ISession Session = NHibernateHelper.GetSessionFactory().OpenSession())
            {
                NHibernateDAL<Grupo> DAL = new NHibernateDAL<Grupo>(Session);
                DAL.SaveOrUpdate(objeto);
                Session.Flush();
            }
        }

        public void Excluir(Grupo objeto)
        {
            using (ISession Session = NHibernateHelper.GetSessionFactory().OpenSession())
            {
                NHibernateDAL<Grupo> DAL = new NHibernateDAL<Grupo>(Session);
                DAL.Delete(objeto);
                Session.Flush();
            }
        }
    }

}

