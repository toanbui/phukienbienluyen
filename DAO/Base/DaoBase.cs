using Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO.Base
{
    public class DaoBase : CoreDataContext
    {
        public static string ConnectionString => System.Configuration.ConfigurationManager.ConnectionStrings["KeyDbContext"].ConnectionString ?? "";
        public static CoreDataContext DaoContext()
        {
            return new CoreDataContext(ConnectionString);
        }
    }
}
