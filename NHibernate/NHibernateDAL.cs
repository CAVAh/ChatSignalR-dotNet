using NHibernate;
using NHibernate.Criterion;
using ISession = NHibernate.ISession;

namespace ChatSignalR.NHibernate
{
    public class NHibernateDAL<DTO> : IDAL<DTO>
    {
        protected ISession Session;

        public NHibernateDAL(ISession Session)
        {
            this.Session = Session;
        }

        public virtual T Save<T>(T objeto)
        {
            try
            {
                Session.Save(objeto);
                return objeto;
            }
            catch (Exception)
            {
                throw ;
            }
        }

        public virtual T SaveOrUpdate<T>(T objeto)
        {
            try
            {
                Session.SaveOrUpdate(objeto);
                return objeto;
            }
            catch (Exception )
            {
                throw ;
            }
        }

        public virtual T Update<T>(T objeto)
        {
            try
            {
                Session.Update(objeto);
                return objeto;
            }
            catch (Exception )
            {
                throw ;
            }
        }

        public virtual int DeleteSql(String consultaSql)
        {
            try
            {
                int Resultado = -1;
                Resultado = Session.CreateQuery(consultaSql).ExecuteUpdate();
                return Resultado;
            }
            catch (Exception )
            {
                throw ;
            }
        }

        public virtual int Delete<T>(T objeto)
        {
            try
            {
                int Resultado = -1;
                Session.Delete(objeto);
                Resultado = 0;
                return Resultado;
            }
            catch (Exception )
            {
                throw ;
            }
        }

        public virtual IList<T> Select<T>(T objeto)
        {
            try
            {
                IList<T> Resultado = new List<T>();
                Example Example = Example.Create(objeto).EnableLike(MatchMode.Anywhere).IgnoreCase().ExcludeNulls().ExcludeZeroes();
                Resultado = Session.CreateCriteria(typeof(T)).Add(Example).List<T>();
                //Resultado = Session.CreateCriteria(typeof(T)).Add(Example).SetMaxResults(Constantes.MAXIMO_REGISTROS_RETORNADOS).List<T>();
                return Resultado;
            }
            catch (Exception )
            {
                throw ;
            }
        }

        public virtual T SelectObjeto<T>(T objeto)
        {
            try
            {
                IList<T> Lista = new List<T>();
                Example Example = Example.Create(objeto).EnableLike(MatchMode.Anywhere).IgnoreCase().ExcludeNulls().ExcludeZeroes();
                Lista = Session.CreateCriteria(typeof(T)).Add(Example).List<T>();

                T Resultado = default(T);

                if (Lista.Count > 0)
                {
                    Resultado = Lista[0];
                }

                return Resultado;
            }
            catch (Exception )
            {
                throw ;
            }
        }

        public virtual T SelectId<T>(int id)
        {
            try
            {
                T Resultado = Session.Get<T>(id);
                return Resultado;
            }
            catch (Exception )
            {
                throw ;
            }

        }

        public virtual IList<T> SelectPagina<T>(int primeiroResultado, int quantidadeResultados, T objeto)
        {
            try
            {
                IList<T> Resultado = new List<T>();
                Example Example = Example.Create(objeto).EnableLike(MatchMode.Anywhere).IgnoreCase().ExcludeNulls().ExcludeZeroes();
                Resultado = Session.CreateCriteria(typeof(T)).Add(Example).SetFirstResult(primeiroResultado)
                    .SetMaxResults(quantidadeResultados).List<T>();
                return Resultado;
            }
            catch (Exception )
            {
                throw ;
            }
        }

        public virtual T SelectObjetoSql<T>(String consultaSql)
        {
            try
            {
                IQuery Consulta = Session.CreateQuery(consultaSql);
                IList<T> Lista = Consulta.List<T>();

                T Resultado = default(T);

                if (Lista.Count > 0)
                {
                    Resultado = Lista[0];
                }

                return Resultado;
            }
            catch (Exception )
            {
                throw ;
            }

        }

        public virtual IList<T> SelectListaSql<T>(String consultaSql)
        {
            try
            {
                IQuery Consulta = Session.CreateQuery(consultaSql);
                IList<T> Resultado = Consulta.List<T>();
                //IList<T> Resultado = Consulta.SetMaxResults(Constantes.MAXIMO_REGISTROS_RETORNADOS).List<T>();
                return Resultado;
            }
            catch (Exception )
            {
                throw ;
            }

        }

        public virtual IList<T> SelectListaSqlQ<T>(String consultaSql)
        {
            try
            {
                ISQLQuery Consulta = Session.CreateSQLQuery(consultaSql).AddEntity(typeof(T));
                IList<T> Resultado = Consulta.List<T>();
                //IList<T> Resultado = Consulta.SetMaxResults(Constantes.MAXIMO_REGISTROS_RETORNADOS).List<T>();
                return Resultado;
            }
            catch (Exception )
            {
                throw ;
            }

        }

        public virtual IList<T> SelectListaSql<T>(String consultaSql, int registros)
        {
            try
            {
                if (registros == 0)
                {
                    //registros = Constantes.MAXIMO_REGISTROS_RETORNADOS;
                }
                IQuery Consulta = Session.CreateQuery(consultaSql);
                IList<T> Resultado = Consulta.List<T>();
                //IList<T> Resultado = Consulta.SetMaxResults(registros).List<T>();
                return Resultado;
            }
            catch (Exception )
            {
                throw ;
            }

        }

    }
}