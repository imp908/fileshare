using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Configuration;

using System.Linq;

using JsonManagers;
using WebManagers;
using QueryManagers;
using POCO;

using APItesting;
using IQueryManagers;
using OrientRealization;
using Repos;
using UOWs;

namespace NSQLManager
{

    class OrientDriverConnnect
    {

        static void Main(string[] args)
        {
            Trash.FormatRearrange.StringsCheck();

            RepoCheck rc = new RepoCheck();
            rc.GO();
        }

    }

    public class RepoCheck
    {

        JSONManager jm;
        OrientTokenBuilder tb;
        TypeConverter tc;
        CommandBuilder ocb;
        OrientWebManager wm ;
        WebResponseReader wr;

        Repo repo;
        Person p;
        Unit u;
        SubUnit s;

        MainAssignment m;
        List<string> lp,lu;

        UserSettings us;
        CommonSettings cs;
        string guid_;
        
        public RepoCheck()
        {            
            
            jm = new JSONManager();
            tb = new OrientTokenBuilder();
            tc = new TypeConverter();
            ocb = new OrientCommandBuilder(new TokenMiniFactory(), new FormatFactory());
            wm = new OrientWebManager();
            wr = new WebResponseReader();

            us = new UserSettings() { showBirthday = true };
            cs = new CommonSettings();
            
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

            guid_ = "ba124b8e-9857-11e7-8119-005056813668";

        }


        public void GO()
        {            

            TrackBirthdaysPtP();
            DeleteBirthdays();
            DbCreateDeleteCheck();
            AddCheck();
            APItester_sngltnCheck();

                        
            TrackBirthdaysOneToAll();          
            
            DeleteCheck();
            ExplicitCommandsCheck();
            BirthdayConditionAdd();
        }

        public void APItester_sngltnCheck()
        {
            APItester_sngltn at = new APItester_sngltn();
            at.Initialize();
            at.GO();
        }
        public void AddCheck()
        {
            int lim = 10;

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
            str = repo.Delete(typeof(Person), new TextToken() { Text = @"Name =0" });
            str = repo.Delete(typeof(Unit), new TextToken() { Text = @"Name =0" });
            str = repo.Delete(typeof(MainAssignment), new TextToken() { Text = @"Name =0" });
            str = repo.Delete(typeof(SubUnit), new TextToken() { Text = @"Name =0" });
        }
        public void ExplicitCommandsCheck()
        {

            OrientCommandBuilder cb = new OrientCommandBuilder(new TokenMiniFactory(), new FormatFactory());
            OrientTokenBuilderExplicit eb = new OrientTokenBuilderExplicit();
            ITypeConverter tc = new TypeConverter();

            List<IQueryManagers.ITypeToken> lt = new List<IQueryManagers.ITypeToken>();
            List<string> ls = new List<string>();

      
lt = eb.Create(new OrientClassToken() { Text = "VSCN" }, new OrientClassToken() { Text = "V" });           
ls.Add(new CommandBuilder(new TokenMiniFactory(), new FormatFactory(), lt, new TextFormatGenerate(lt)).Text.Text);

lt = eb.Create(new OrientClassToken() { Text = "VSCN" }, new OrientPropertyToken() { Text = "Name" }, new OrientSTRINGToken(), true,true);
ls.Add(new CommandBuilder(new TokenMiniFactory(), new FormatFactory(), lt, new TextFormatGenerate(lt)).Text.Text);

lt = eb.Create(new OrientClassToken() { Text = "VSCN" }, new OrientPropertyToken() { Text = "Created" }, new OrientDATEToken(), true, true);
ls.Add(new CommandBuilder(new TokenMiniFactory(), new FormatFactory(), lt, new TextFormatGenerate(lt)).Text.Text);



lt = eb.Create(new OrientClassToken() { Text = "ESCN" }, new OrientClassToken() { Text = "E" });
ls.Add(new CommandBuilder(new TokenMiniFactory(), new FormatFactory(), lt, new TextFormatGenerate(lt)).Text.Text);

lt = eb.Create(new OrientClassToken() { Text = "ESCN" }, new OrientPropertyToken() { Text = "Name" }, new OrientSTRINGToken(), true, true);
ls.Add(new CommandBuilder(new TokenMiniFactory(), new FormatFactory(), lt, new TextFormatGenerate(lt)).Text.Text);

lt = eb.Create(new OrientClassToken() { Text = "ESCN" }, new OrientPropertyToken() { Text = "Created" }, new OrientDATEToken(), true, true);
ls.Add(new CommandBuilder(new TokenMiniFactory(), new FormatFactory(), lt, new TextFormatGenerate(lt)).Text.Text);



lt = eb.Create(new OrientClassToken() { Text = "VSCN" }, new OrientClassToken() { Text = "VSCN" });
ls.Add(new CommandBuilder(new TokenMiniFactory(), new FormatFactory(), lt, new TextFormatGenerate(lt)).Text.Text);

lt = eb.Create(new OrientClassToken() { Text = "Beer" }, new OrientClassToken() { Text = "VSCN" });
ls.Add(new CommandBuilder(new TokenMiniFactory(), new FormatFactory(), lt, new TextFormatGenerate(lt)).Text.Text);

lt = eb.Create(new OrientClassToken() { Text = "Produces" }, new OrientClassToken() { Text = "ESCN" });
ls.Add(new CommandBuilder(new TokenMiniFactory(), new FormatFactory(), lt, new TextFormatGenerate(lt)).Text.Text);



        }
        public void BirthdayConditionAdd()
        {

            List<string> persIds = new List<string>();
            List<string> edgeIds = new List<string>();

            var PersList = jm.DeserializeFromParentNode<Person>(
        repo.Select(
            typeof(Person),
        new TextToken() { Text = "1=1 and outE(\"CommonSettings\").inv(\"UserSettings\").showBirthday[0] is null" })
        , new RESULT().Text);

            foreach(Person pers in PersList)
            {
                persIds.Add(pers.id.Replace(@"#", ""));
            }          

            for (int i = 0; i < persIds.Count(); i++)
            {
                string id = jm.DeserializeFromParentNode<UserSettings>(repo.Add(us), new RESULT().Text).Select(s => s.id.Replace(@"#", "")).FirstOrDefault();

                repo.Add(cs, new TextToken() { Text = persIds[i] }, new TextToken() { Text = id });
            }


            repo.Delete(typeof(UserSettings), new TextToken() { Text = @"1 =1" });
            repo.Delete(typeof(CommonSettings), new TextToken() { Text = @"1 =1" });

        }
        public void DbCreateDeleteCheck()
        {

            repo.Add(new TextToken() { Text = "test_db" }, new DELETE());
            repo.Add(new TextToken() { Text = "test_db" }, new POST());

        }
        public void TrackBirthdaysOneToAll()
        {
            repo.changeAuthCredentials(
                ConfigurationManager.AppSettings["ParentLogin"]
                , ConfigurationManager.AppSettings["ParentPassword"]
                );

            PersonUOW pUOW = new PersonUOW();
            TrackBirthdays tb = new TrackBirthdays();

            Person fromPerson = pUOW.GetObjByGUID(guid_).FirstOrDefault();
            List<Person> personsTo = pUOW.GetAll().ToList();
            List<string> ids = new List<string>() { };
            
            foreach (Person pt in personsTo)
            {
                ids.Add(repo.Add(tb, fromPerson, pt));
            }

            repo.Delete(typeof(TrackBirthdays), new TextToken() { Text= "1=1" } );

        }
        public void TrackBirthdaysPtP()
        {
            repo.changeAuthCredentials(
                ConfigurationManager.AppSettings["orient_login"]
                , ConfigurationManager.AppSettings["orient_pswd"]
                );

            PersonUOW pUOW = new PersonUOW();
            TrackBirthdays tb = new TrackBirthdays();
            List<Person> personsTo = pUOW.GetAll().Take(3).ToList();
            string personsfrom = null;

            List<string> ids = new List<string>() { };

            repo.Delete(typeof(TrackBirthdays), new TextToken() { Text = "1=1" });

            foreach (Person pf in personsTo)
            {
                foreach (Person pt in personsTo)
                {
                    if (pf.GUID != pt.GUID)
                    {
                        ids.Add(pUOW.AddTrackBirthday(tb, pf.GUID, pt.GUID));
                    }
                }               
            }

            personsfrom = pUOW.GetTrackedBirthday(personsTo.FirstOrDefault().GUID);

            foreach (Person pf in personsTo)
            {
                foreach (Person pt in personsTo)
                {
                    if (pf.GUID != pt.GUID)
                    {
                        ids.Add(pUOW.DeleteTrackedBirthday(tb, pf.GUID, pt.GUID));
                    }
                }
            }

           
        }
        public void DeleteBirthdays()
        {
            repo.changeAuthCredentials(
              ConfigurationManager.AppSettings["ParentLogin"]
              , ConfigurationManager.AppSettings["ParentPassword"]
              );

            PersonUOW pUOW = new PersonUOW();
            TrackBirthdays tb = new TrackBirthdays();

        }

    }
    
}

