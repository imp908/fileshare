using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

using Oracle.ManagedDataAccess.Client;
using System.Data.SqlClient;

using TransactionSite_.Models;
using System.Data.Entity;

namespace TransactionSite_.DAL
{
    public class Init : DropCreateDatabaseIfModelChanges<SQL_CONTEXT>
    {

        protected override void Seed(SQL_CONTEXT context)
        {
            context.SaveChanges();
        }

    }
}