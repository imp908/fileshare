using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Collections.Specialized;
using Orient.Client;
using System.Globalization;
using Intranet.WeatherHelper;
using Intranet.Models.QuizModel;
using System.Web.Script.Serialization;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Schema.Generation;

namespace Intranet.Models
{
    public class OrientDbContext : IDatabase//, IDisposable
    {
        private static string _hostname;
        private static int _port;
        private static string _rootUserName;
        private static string _rootUserPassword;

        private static OServer _server;
        public static ODatabase _database { get; set; }

        private static string _DBname;
        private static string _username;
        private static string _password;

        public OrientDbContext()
        {
            _hostname = ConfigurationManager.AppSettings.Get("OrientHostName");

            _port = int.Parse(ConfigurationManager.AppSettings.Get("OrientPort"));
            _rootUserName = ConfigurationManager.AppSettings.Get("RootUserName");
            _rootUserPassword = ConfigurationManager.AppSettings.Get("RootUserPassword");
            _DBname = ConfigurationManager.AppSettings.Get("DataBaseName");

            _username = _rootUserName;
            _password = _rootUserPassword;

            _server = new OServer(_hostname, _port, _rootUserName, _rootUserPassword);
            DataBaseInitialize();
        }

        public ODatabase GetDataBase<TODatabase>()
        {
      //      if (_database == null) DataBaseInitialize();
            DataBaseInitialize();
                return _database;
        }

        private void DataBaseInitialize()
        {
            if (!_server.DatabaseExist(_DBname, OStorageType.PLocal))
            {
                throw new ArgumentNullException("DB is not Exist");
            }

            _database = new ODatabase
             (
                 _hostname,
                 _port,
                 _DBname,
                 ODatabaseType.Graph,
                 _username,
                 _password
             );
        }

        public List<object> GetData(string userName)
        {
            var p = new WeatherHelper.WeatherHelper(_database);
            return p.GetData(userName);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}

