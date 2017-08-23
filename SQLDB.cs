namespace DAL.DAL
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Collections.Generic;

    using Model.SQLmodel;


    /// <summary>
    /// Context with recreate if model changes
    /// </summary>
    public class SQLDB_CHANGE : DbContext
    {
        // Your context has been configured to use a 'SQLDB' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'DAL.DAL.SQLDB' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'SQLDB' 
        // connection string in the application configuration file.
        public SQLDB_CHANGE()
            : base("name=SQLDB")
        {
            Database.SetInitializer<SQLDB_CHANGE>(new DropCreateDatabaseIfModelChanges<SQLDB_CHANGE>());

        }

        public SQLDB_CHANGE(string ConnectionName)
            : base(ConnectionName)
        {
            //Database.SetInitializer<SQLDB_CHANGE>(new DropCreateDatabaseIfModelChanges<SQLDB_CHANGE>());
            Database.SetInitializer(new InitializerAtCreation());

            Database.Initialize(true);
        }

        public class InitializerAtCreation : DropCreateDatabaseIfModelChanges<SQLDB_CHANGE>
        {
            protected override void Seed(SQLDB_CHANGE context)
            {
                IList<KEY_CLIENTS_SQL> kk = new List<KEY_CLIENTS_SQL>()
                {
                    new KEY_CLIENTS_SQL(){ID=0,MERCHANT=9290000000, RESPONSIBILITY_MANAGER=@"SERNAME1", SECTOR_ID=1},
                    new KEY_CLIENTS_SQL(){ID=1,MERCHANT=9290000002, RESPONSIBILITY_MANAGER=@"SERNAME1", SECTOR_ID=1},
                    new KEY_CLIENTS_SQL(){ID=3,MERCHANT=9290000001, RESPONSIBILITY_MANAGER=@"SERNAME2",SECTOR_ID=2},
                    new KEY_CLIENTS_SQL(){ID=4,MERCHANT=9290000003, RESPONSIBILITY_MANAGER=@"SERNAME3",SECTOR_ID=3},
                    new KEY_CLIENTS_SQL(){ID=5,MERCHANT=9290000005, RESPONSIBILITY_MANAGER=@"SERNAME1",SECTOR_ID=1},
                    new KEY_CLIENTS_SQL(){ID=6,MERCHANT=9290000007, RESPONSIBILITY_MANAGER=@"SERNAME3",SECTOR_ID=3}
                };

                context.KEY_CLIENTS.AddRange(kk);
                context.SaveChanges();

                IList<REFMERCHANTS_SQL> rm = new List<REFMERCHANTS_SQL>()
                {
                    new REFMERCHANTS_SQL { MERCHANT = 9290000000 },
                    new REFMERCHANTS_SQL { MERCHANT = 9290000001 },
                    new REFMERCHANTS_SQL { MERCHANT = 9290000002 },
                    new REFMERCHANTS_SQL { MERCHANT = 9290000003 },
                    new REFMERCHANTS_SQL { MERCHANT = 9290000004 },
                };

                context.REFMERCHANTS_SQL.AddRange(rm);
                context.SaveChanges();

                List<MERCHANT_LIST_SQL> ml = new List<MERCHANT_LIST_SQL>() {
                    new MERCHANT_LIST_SQL(){MERCHANT=9290000000, USER_ID =1, UPDATE_DATE=new DateTime(2017,08,03,00,00,01)},
                    new MERCHANT_LIST_SQL(){MERCHANT=9290000001, USER_ID =1, UPDATE_DATE=new DateTime(2017,08,03,00,00,02)},
                    new MERCHANT_LIST_SQL(){MERCHANT=9290000003, USER_ID =1, UPDATE_DATE=new DateTime(2017,08,03,00,00,03)},
                    new MERCHANT_LIST_SQL(){MERCHANT=9290000005, USER_ID =1, UPDATE_DATE=new DateTime(2017,08,03,00,00,03)},
                    new MERCHANT_LIST_SQL(){MERCHANT=9290000007, USER_ID =3, UPDATE_DATE=new DateTime(2017,08,03,00,00,04)},
                    new MERCHANT_LIST_SQL(){MERCHANT=9290000008, USER_ID =3, UPDATE_DATE=new DateTime(2017,08,03,00,00,05)},
                    new MERCHANT_LIST_SQL(){MERCHANT=9290000009, USER_ID =3, UPDATE_DATE=new DateTime(2017,08,03,00,00,06)}
                };

                context.MERCHANT_LIST.AddRange(ml);
                context.SaveChanges();

                List<USERS_SQL> users = new List<USERS_SQL>(){
                    new USERS_SQL(){ID=1, Name=@"NAME1", Sername=@"SERNAME1", mail=@"NAME1@rsb.ru"},
                    new USERS_SQL(){ID=2, Name=@"NAME2", Sername=@"SERNAME2", mail=@"NAME2@rsb.ru"},
                    new USERS_SQL(){ID=3, Name=@"NAME3", Sername=@"SERNAME3", mail=@"NAME3@rsb.ru"},
                    new USERS_SQL(){ID=4, Name=@"NAME4", Sername=@"SERNAME4", mail=@"NAME4@rsb.ru"},
                    new USERS_SQL(){ID=5, Name=@"NAME5", Sername=@"SERNAME5", mail=@"NAME5@rsb.ru"}
                };

                context.USERS.AddRange(users);
                context.SaveChanges();

                List<T_ACQ_M_SQL> acq_m = 
                new List<T_ACQ_M_SQL>(){
                    new T_ACQ_M_SQL(){MERCHANT=9290000000, AMT=1, DATE=new DateTime(2017,01,05,00,00,13)},
                    new T_ACQ_M_SQL(){MERCHANT=9290000000, AMT=2, DATE=new DateTime(2017,02,06,00,00,14)},
                    new T_ACQ_M_SQL(){MERCHANT=9290000000, AMT=3, DATE=new DateTime(2017,03,07,00,00,15)},
                    new T_ACQ_M_SQL(){MERCHANT=9290000000, AMT=4, DATE=new DateTime(2017,04,08,00,00,15)},

                    new T_ACQ_M_SQL(){MERCHANT=9290000001, AMT=5, DATE=new DateTime(2017,01,05,00,00,17)},
                    new T_ACQ_M_SQL(){MERCHANT=9290000001, AMT=6, DATE=new DateTime(2017,02,06,00,00,18)},
                    new T_ACQ_M_SQL(){MERCHANT=9290000001, AMT=7, DATE=new DateTime(2017,03,07,00,00,19)},
                    new T_ACQ_M_SQL(){MERCHANT=9290000001, AMT=8, DATE=new DateTime(2017,04,08,00,00,20)},

                    new T_ACQ_M_SQL(){MERCHANT=9290000003, AMT=9, DATE=new DateTime(2017,01,05,00,00,21)},
                    new T_ACQ_M_SQL(){MERCHANT=9290000003, AMT=10, DATE=new DateTime(2017,02,06,00,00,22)},
                    new T_ACQ_M_SQL(){MERCHANT=9290000003, AMT=11, DATE=new DateTime(2017,03,07,00,00,23)},
                    new T_ACQ_M_SQL(){MERCHANT=9290000003, AMT=12, DATE=new DateTime(2017,04,08,00,00,24)},

                    new T_ACQ_M_SQL(){MERCHANT=9290000008, AMT=13, DATE=new DateTime(2017,01,05,00,00,01)},
                    new T_ACQ_M_SQL(){MERCHANT=9290000008, AMT=13, DATE=new DateTime(2017,02,06,00,00,01)},
                    new T_ACQ_M_SQL(){MERCHANT=9290000008, AMT=13, DATE=new DateTime(2017,03,07,00,00,01)},
                    new T_ACQ_M_SQL(){MERCHANT=9290000008, AMT=13, DATE=new DateTime(2017,04,08,00,00,01)}
                };

                context.T_ACQ_M.AddRange(acq_m);
                context.SaveChanges();               

                base.Seed(context);
            }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        // public virtual DbSet<MyEntity> MyEntities { get; set; }

        public virtual DbSet<Model.SQLmodel.REFMERCHANTS_SQL> REFMERCHANTS_SQL { get; set; }
        public virtual DbSet<Model.SQLmodel.T_ACQ_M_SQL> T_ACQ_M { get; set; }
        public virtual DbSet<Model.SQLmodel.T_ACQ_D_SQL> T_ACQ_D { get; set; }
        public virtual DbSet<Model.SQLmodel.KEY_CLIENTS_SQL> KEY_CLIENTS { get; set; }
        public virtual DbSet<Model.SQLmodel.MERCHANT_LIST_SQL> MERCHANT_LIST { get; set; }
        public virtual DbSet<Model.SQLmodel.USERS_SQL> USERS { get; set; }
    }

}