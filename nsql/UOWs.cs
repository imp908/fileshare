using System;
using System.Collections.Generic;
using System.Linq;

using IUOWs;
using IQueryManagers;
using IWebManagers;
using POCO;
using OrientRealization;
using QueryManagers;
using IRepos;
using JsonManagers;
using IJsonManagers;
using WebManagers;
using Repos;

namespace UOWs
{
    
    public class PersonUOW : IPersonUOW
    {
        IRepo _repo;
        OreintNewsTokenBuilder ob = new OreintNewsTokenBuilder();
        ITypeConverter _typeConverter;
        ICommandBuilder _CommandBuilder;
        IJsonManger _jsonManager;
        ITokenBuilder _tokenAggregator;
        IWebManager wm;
        IResponseReader wr;

        public PersonUOW()
        {
            _jsonManager = new JSONManager();
            _tokenAggregator = new OrientTokenBuilder();
            _typeConverter = new TypeConverter();
            _CommandBuilder = new OrientCommandBuilder();
            wm = new OrientWebManager();
            wr = new WebResponseReader();

            _repo = new Repo(_jsonManager, _tokenAggregator, _typeConverter, _CommandBuilder, wm, wr);
        }


        public IEnumerable<Person> GetObjByGUID(string GUID)
        {
            IEnumerable<Person> result = null;
            TextToken condition_ = new TextToken() { Text = "1=1 and GUID ='" + GUID + "'" };
            try
            {
                result = _repo.Select<Person>(typeof(Person), condition_);
            }
            catch (Exception e) { System.Diagnostics.Trace.WriteLine(e.Message); }

            return result;
        }
        public string GetByGUID(string GUID)
        {
            string result = string.Empty;
            IEnumerable<Person> persons = null;
            TextToken condition_ = new TextToken() { Text = "1=1 and GUID ='" + GUID + "'" };
            try
            {
                persons = _repo.Select<Person>(typeof(Person), condition_);
                result = _jsonManager.SerializeObject(persons);
            }
            catch (Exception e) { System.Diagnostics.Trace.WriteLine(e.Message); }

            return result;
        }
        public IEnumerable<Person> GetAll()
        {

            IEnumerable<Person> result = null;
            TextToken condition_ = new TextToken() { Text = "1=1" };
            try
            {
                result = _repo.Select<Person>(typeof(Person), condition_);

            }
            catch (Exception e) { System.Diagnostics.Trace.WriteLine(e.Message); }

            return result;
        }

        public string GetTrackedBirthday(string GUID)
        {
            string result = string.Empty;
            IEnumerable<Person> persons = null;
            TextToken condition_ = new TextToken() { Text = "1=1 and GUID ='" + GUID + "'" };
            List<ITypeToken> tokens = ob.outEinVExp(new OrientSelectToken(),
                _typeConverter.Get(typeof(Person)), _typeConverter.Get(typeof(TrackBirthdays)), condition_);

            _CommandBuilder.AddTokens(tokens);
            _CommandBuilder.AddFormat(new OrientOutEinVFormat() { });
            string command = _CommandBuilder.Build();

            persons = _repo.Select<Person>(command);
            result = _jsonManager.SerializeObject(persons);
            return result;
        }

        public string AddTrackBirthday(OrientEdge edge_, string guidFrom, string guidTo)
        {
            string result = null;
            Person from = GetObjByGUID(guidFrom).FirstOrDefault();
            Person to = GetObjByGUID(guidTo).FirstOrDefault();

            if (from != null && to != null)
            {
                result = _repo.Add(edge_, from, to);
            }
            return result;
        }
        public string DeleteTrackedBirthday(OrientEdge edge_, string guidFrom, string guidTo)
        {
            string result = null;
            Person from = GetObjByGUID(guidFrom).FirstOrDefault();
            Person to = GetObjByGUID(guidTo).FirstOrDefault();

            List<ITypeToken> condTokens_ = ob.outVinVcnd(typeof(Person), new TextToken() { Text = "GUID" },
                new TextToken() { Text = from.GUID }, new TextToken() { Text = to.GUID });

            _CommandBuilder.AddTokens(condTokens_);
            _CommandBuilder.AddFormat(new OrientOutVinVFormat() { });
            string command = _CommandBuilder.Build();
      

            if (from != null && to != null)
            {
                result = _repo.Delete(edge_.GetType(), new TextToken() { Text = command });
            }
            return result;
        }

    }

}
