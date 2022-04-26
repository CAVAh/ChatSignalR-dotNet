using NHibernate;
using NHibernate.Cfg;

namespace ChatSignalR.NHibernate
{
    public class NHibernateHelper
    {
        private static ISessionFactory? SessionFactory;

        public static ISessionFactory GetSessionFactory()
        {
            try
            {
                if (SessionFactory == null)
                {
                    lock (typeof(NHibernateHelper))
                    {
                        Configuration config = new Configuration();
                        config.Configure();
                        config.AddAssembly("ChatSignalR");
                        SessionFactory = config.BuildSessionFactory();
                    }
                }
                return SessionFactory;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
