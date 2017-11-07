using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Orient.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using JsonManagers;
using WebManagers;
using QueryManagers;
using POCO;

using OrientRealization;
using IOrientObjects;


namespace NSQLManager
{

    class OrientDriverConnnect
    {

        static void Main(string[] args)
        {            
            RepoCheck rc = new RepoCheck();
            rc.GO();
        }

    }

    public class RepoCheck
    {

        JSONManager jm;
        OrientTokenBuilder tb;
        TypeConverter tc;
        Textbuilder ocb;
        WebManager wm ;
        WebResponseReader wr;

        Repo repo;
        Person p;
        Unit u;
        SubUnit s;

        MainAssignment m;
        List<string> lp,lu;

        public RepoCheck()
        {
            jm = new JSONManager();
            tb = new OrientTokenBuilder();
            tc = new TypeConverter();
            ocb = new OrientCommandBuilder();
            wm = new WebManager();
            wr = new WebResponseReader();          

            repo = new Repo(jm, tb, tc, ocb, wm, wr);

            s = new SubUnit();

            p =
new Person() { Name = "0", GUID = "0", Changed = new DateTime(2017, 01, 01, 00, 00, 00), Created = new DateTime(2017, 01, 01, 00, 00, 00) };

            u =
new Unit() { Name = "0", GUID = "0", Changed = new DateTime(2017, 01, 01, 00, 00, 00), Created = new DateTime(2017, 01, 01, 00, 00, 00) };

            m =
new MainAssignment() { Name = "0", GUID = "0", Changed = new DateTime(2017, 01, 01, 00, 00, 00), Created = new DateTime(2017, 01, 01, 00, 00, 00) };

            lp = new List<string>();
            lu = new List<string>();
            
        }


        public void GO()
        {
            ExplicitCommandsCheck();
            AddCheck();
            DeleteCheck();
        }
        public void AddCheck()
        {
            int lim = 500;

            for (int i = 0; i <= lim; i++)
            {
                lp.Add(jm.DeserializeFromParentNode<Person>(repo.Add(p), new RESULT().Text).Select(s => s.id.Replace(@"#","")).FirstOrDefault());
                lu.Add(jm.DeserializeFromParentNode<Unit>(repo.Add(u), new RESULT().Text).Select(s=>s.id.Replace(@"#", "")).FirstOrDefault());
              
            }
            for (int i = 0; i <= lim/2; i++)
            {              
                repo.Add(m, new TextToken() { Text = lu[i] }, new TextToken() { Text = lp[i + 1] });                
            }
            for (int i = 0; i <= lim / 2; i++)
            {
                repo.Add(s, new TextToken() { Text = lu[i] }, new TextToken() { Text = lu[i + 1] });
            }
           

        }
        public void DeleteCheck()
        {
            string str;
            str = repo.Delete(typeof(Person));
            str = repo.Delete(typeof(Unit));
            str = repo.Delete(typeof(MainAssignment));
            str = repo.Delete(typeof(SubUnit));
        }
        public void ExplicitCommandsCheck()
        {

            OrientCommandBuilder cb = new OrientCommandBuilder();
            OrientTokenBuilderDistributed eb = new OrientTokenBuilderDistributed();
            ITypeConverter tc = new TypeConverter();

            List<IQueryManagers.ITypeToken> lt = new List<IQueryManagers.ITypeToken>();
            List<string> ls = new List<string>();


lt = eb.Create(new OrientClassToken() { Text = "VSCN" }, new OrientClassToken() { Text = "V" });
ls.Add(cb.Build(lt, new TextFormatGenerate(lt)));

lt = eb.Create(new OrientClassToken() { Text = "VSCN" }, new OrientPropertyToken() { Text = "Name" }, new OrientSTRINGToken(), true,true);
ls.Add(cb.Build(lt, new TextFormatGenerate(lt)));

lt = eb.Create(new OrientClassToken() { Text = "VSCN" }, new OrientPropertyToken() { Text = "Created" }, new OrientDATEToken(), true, true);
ls.Add(cb.Build(lt, new TextFormatGenerate(lt)));



lt = eb.Create(new OrientClassToken() { Text = "ESCN" }, new OrientClassToken() { Text = "E" });
ls.Add(cb.Build(lt, new TextFormatGenerate(lt)));

lt = eb.Create(new OrientClassToken() { Text = "ESCN" }, new OrientPropertyToken() { Text = "Name" }, new OrientSTRINGToken(), true, true);
ls.Add(cb.Build(lt, new TextFormatGenerate(lt)));

lt = eb.Create(new OrientClassToken() { Text = "ESCN" }, new OrientPropertyToken() { Text = "Created" }, new OrientDATEToken(), true, true);
ls.Add(cb.Build(lt, new TextFormatGenerate(lt)));



lt = eb.Create(new OrientClassToken() { Text = "VSCN" }, new OrientClassToken() { Text = "VSCN" });
ls.Add(cb.Build(lt, new TextFormatGenerate(lt)));

lt = eb.Create(new OrientClassToken() { Text = "Beer" }, new OrientClassToken() { Text = "VSCN" });
ls.Add(cb.Build(lt, new TextFormatGenerate(lt)));

lt = eb.Create(new OrientClassToken() { Text = "Produces" }, new OrientClassToken() { Text = "ESCN" });
ls.Add(cb.Build(lt, new TextFormatGenerate(lt)));



        }

    }
    
}

