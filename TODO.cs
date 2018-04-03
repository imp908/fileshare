
#region TODO

TODO [
	
	ACTUAL[

	c++
		overall
			->Employee class
			->OpenGltutorials
			->linked list reference graph

	c#
	
		ProofOfConcept
			-> Linq from custom object to custom string[
					<- done for equality for sample properties int,bool,string
				]
				-> custom expression from nodes concat
				-> eliminate brakets from expression binary string result
				-> complex types expression left chain to string dot concat
			-> Polinom Parse[
				parse *^/ to expressions and add priorities for exprs
			]
			-> Command line Console parameters parse app dll -> to SB
			-> Multithread socket message ping pong -> to SB
			-> CollectionsComparison -> to SB
			-> Pluggin/addon architecture -> to SB


		NSQLmanager[
		
			Info[
				
				VS_ref[
Install-Package xunit -ProjectName NSQLManagerIntegrationTests
Install-Package moq -ProjectName NSQLManagerIntegrationTests
Install-Package Newtonsoft.Json -ProjectName NSQLManagerIntegrationTests
Install-Package Microsoft.AspNet.WebApi -ProjectName NSQLManagerIntegrationTests

Install-Package NUnit -ProjectName NSQLManagerIntegrationTests
Install-Package NUnit.ConsoleRunner -ProjectName NSQLManagerIntegrationTests
Install-Package NUnit.Console -ProjectName NSQLManagerIntegrationTests
Install-Package NUnit3TestAdapter  -ProjectName NSQLManagerIntegrationTests
//OWIN install
Install-Package Microsoft.Owin.Security.OAuth 
Install-Package Microsoft.AspNet.Identity.EntityFramework 
Install-Package Microsoft.Owin.Host.SystemWeb 

//not used
Install-Package Microsoft.AspNet.Identity.Owin 
Install-Package Microsoft.AspNet.WebApi.Owin 
Install-Package Microsoft.Owin.Cors 
				]

				,GitMoove[
				
cd C:\workflow\projects\Dev\gitLab\nsql
git init
git remote add ns http://gitlab.nspk.ru/Neprintsevia/NSQLManager.git
git pull ns prod_moove

cd C:\workflow\projects\Dev\gitLab\napi
git init
git remote add ns http://gitlab.nspk.ru/Neprintsevia/NewsApi.git
git pull ns prod_move

cd C:\workflow\projects\Dev\gitLab\napi
git clone http://gitlab.nspk.ru/Neprintsevia/NewsApi.git
cd C:\workflow\projects\Dev\gitLab\napi\NewsApi
git checkout nsql_mng

				]
				,MergeManualTotalCommanderSynch[

exceptlist[
.vs
.git
*.config
Release
bin
dll
*.csproj*
*.asax*
libs
obj
Properties
]

//GET TARGET BRANCH
cd C:\workflow\projects\Dev\gitLab\napi_nsql
git clone http://gitlab.nspk.ru/Neprintsevia/NewsApi.git

cd C:\workflow\projects\Dev\gitLab\napi_nsql\NewsApi

git branch nsql_mng
git branch prod_move
git checkout nsql_mng
git remote add np http://gitlab.nspk.ru/Neprintsevia/NewsApi.git
git pull np nsql_mng

//GET SOURCE BRANCH
cd C:\workflow\projects\Dev\gitLab\napi_prod_move
git clone http://gitlab.nspk.ru/Neprintsevia/NewsApi.git

cd C:\workflow\projects\Dev\gitLab\napi_prod_move\NewsApi

git branch nsql_mng
git branch prod_move
git checkout prod_move
git remote add np http://gitlab.nspk.ru/Neprintsevia/NewsApi.git
git pull np prod_move


				]
				,Queries[

where_composed[
select from Note where 1=1
and GUID='30c7fc2b-696a-499f-bb59-2b53f4a4eb5e' 
and pinned.isTrue=True 
and published.isTrue=True 
and in('Authorship')[0].GUID='477a5bf9-1bec-4959-ae7f-35e08bba33b3' 
order by changed asc limit 4
]

QuizSelect[

select from Quiz where State ='Published' and StartDate between '2017-12-06 00:00:00' and '2018-07-30 00:00:00'
create vertex Quiz content {"StartDate":"2017-12-03 17:01:01","State":"Published","EndDate":"2018-12-04 01:01:01","QuizDescription":"test quiz with large finish date for publishing","Title":"Test quiz title2"}

]

				]
				
				,NewsOrientQueries[

//Traverse Commentaries to News
traverse outE('Comment'),inV('Commentary'),inE('Authorship','Comment') from 26:3
traverse out('Comment'),in('Comment') from 25:4

//traverse comments to comment
select from (traverse out('Comment') from 26:5)
where commentDepth >=0 and commentDepth <=3

//traverse up fromcomment to news
traverse in('Authorshp','Comment') from (select from 24:53)

//Get persons from news
Select expand(a1) from(Select inE('Authorship').outV('Person') as a1 from 26:1 )

//Person authorships
traverse out() from 31:6
//Author find
select expand(in('Authorship')) from 23:1

//Check Notes without authorship
select from Note where in('Authorship').@class!='Person'


//Select from News with depth
select from (traverse outE('Comment'),inV('Commentary'),inE('Authorship','Comment') from 26:2)
where commentDepth >=2 and commentDepth <=2 

//
traverse out('Comment','Authorship'),in('Comment') from 32:7

				]
									
				,Shema[
				
				
				//--------------------------------

				//Class  model <- done
				(Person) - [Authorship] -> (Commentary) <- [Comment] - (Person)
				(Person) - [Authorship] -> (News) <- [Comment] - (Person)
				(Person) - [Tagged] -> (Tag)
				(Person) - [Liked] -> (Note)
				
				//Relations model -> obsolette

				Post new tag by name, post tag id news id
				person to like

				(Person) - [Authorship] -> (Object{pinned:true})
				News //editable by hardcode string acc group to every tag
				(Person) - [Authorship] -> (Object{"pinned":"true";"published":"true"})
				Commentary
				(Person) - [Authorship] -> (Object) - [Comment{CommentLevel:0+1}] -> (Object)
				
				//updatable by person. cerated by Destiny list
				News
				{
					[not updatable from POST,changes while note POST update]
					public Person author_ { get; set; }

				}				
				//Author never edited from POSTs
				//updatable from UOW
				Commentary
				
				if "Name":null -> если null оставляем как в базе
				if "Name":"" -> если явное "" empty перезаписываем в базе
				
				if author_:null or "" -> не обноялем напрямую,
				только из пользователя при изменнеии объекта
				
				personReturn
				{ 
				  "GUID": "ba124b8e-9857-11e7-8119-005056813668",
				  "division": "Отдел поддержки прикладных систем",
				  "PGUID": "c1b0ff45-5cb1-11e7-8117-005056813668",
				  "phone": "1312",
				  "mail": "Neprintsevia@nspk.ru",
				  "Name": "Непринцев Илья Александрович",  
				  "shortFName": "Непринцев И. А.",
				  "description": "Главный специалист",
				  "itemTitleColor": "rgb(44,50,124)",
				  "templateName": "PersonTemplate",
				  "birthday": "09.03",
				  "colorClass": "c-dit",
				  "departmentName": "Департамент ИТ",
				  "login": "Neprintsevia"
				}
				
				NoteReturn{
					
					author personReturn_ {get;set;}
					
					note_ {get;set;}
					
					commentaryCount {get;set;}
					likesCount {get;set;}
					
				}
				
				//--------------------------------

				]
				
				,Migration_sceneries[

	scenery_theoretical[
	 
//migration scenery

//collisions check:
wrtie object no check
check if exists 
	-> leave as in target DB
	-> delete and create as in source DB

sort conditional items -> nodes first
	detect if list contains references:
	if contains -> detect referenced classes
	if referenced classes exist in list -> for referenced Nodes preserve ID to Object
	
for each Node item:
if no objects in Source exist -> mark as processed
else for each object -> 
if no chek -> write when finished 
if exists target leave 
-> delete Node 
-> createne wnode 
if exists source leave -> ignore
-| mark as processed

foreach Reference item
GUID objects
-> Get source Nodes
-> Get target Nodes by Source Nodes Guid
-> Create Target Relation between Target Nodes
NOT GUID objects (id only)

/* ID restoration (too murky):[ */
(preserve Source ID values to Target created object item,
check referenced Nodes to created 
-> if trying to migrate reference but not class it's structural? migrator error)
],

	GUIDed_Nodes(more realistic):[
Node always has GUID
Ref can have no GUID
take target ref nodes,
if exist -> allready created
if not exist -> create relations
]

				]
			
			]
			
			,TODO[
			
-> ETL[
	1C load
	Person sync (GUID,CN ad Fname+Lname+Mdlname )
]
-> no class specific vertex creation
-> custom LINQ expression trees {Class.prop.value}{condType}{targetValue}
-> integrate custom expression into class vertex
	
			-> split <Note> object from DB [
				-> for every Note reference fields (Likes,Tabs,Author) to NoteReturn add
				-> return aggr <NoteReturn> to front client
			]			
			-> synch Person from prod[
				-> Custom comparisors <- done
			]											
			-> clean commentary creation
			-> new db model [
(company)<-[UnitOf]-(Unit)<-[Working,WorkedIn,Fired,Assigned]-(Person)
,(Person)-[Comments,Likes,Reads,Searches]->(Person)]
			-> rewrite old Person functions for birthdays, working structure, management roles		  
			-> Neo4j and Mongo repos add
			-> Plugin arhiteckture			
-> RepoFactory add interaface parameters
-> extend with ability to load tokens from model class
-> add all tokens to tokenlist
-> Moove builders and command invokers to Generic
-> Tokens to static?
-> add authentication timer for orient connection
-> add interface segregation for different token types => {IdataType,ICreateType}
-> add logging
-> AD synch


			
			-> property comparer with custom attributes[
				-> common custom toggled atribute (IsUpdatable)
				-> passing attribute to method and detecting its toggle value (true,false)] <-done
			-> Prod Moove[
			
				received: 24.01.2018 estimate 1.5 week
				detailed: news,quiz?!
				
-> timeline[

01.02.2018

13:18
13:21 база [news_prod] перенесена на http://msk1-vm-indb01.nspk.ru:2480
13:33 local tested Ok
13:37 [news_prod] mooved to http://msk1-vm-inapp01:8185
13:45 mooved
13:50 tags,likes, tested
13:48 edge in out properties NOTSynchronised addded
13:48 test objects deleted
13:48


				-> 05.02.2018 Moove front and finish date
				-> 01.02.2018 Moove back date
				
				-> 01.02.2018 11:23 -> quiz check <- done ovisp02 8185
				-> 01.02.2018 10:20 -> news check <- done ovisp02 8185
				-> 25.01.2018 check db moove <- done
	]
	
plan real
[
orient db moove->
crete new orient db news_prod on target host(msk1-vm-indb01 [News_prod])
synch classes (moove classes from msk1-vm-ovisp02:2480 [news_test5])
synch objects (moove Person objects from  msk1-vm-ovisp02:2480 [news_test5])
API moove (No merge scenery)->
host cntroller proj with NOSQLmanager api lib included on (http://msk1-vm-inapp01:8184 [newstest3] )
testing ->
check API from html
(holivation,getmanager kpa,post news,update news, get news,tag news, untag news, like news dislike news, get news after every change)
NEWS Front moove ->
host my.nspk under my.npsk2
redirect it to http://msk1-vm-inapp01:8184

testing ->
test front + birthdays + settings + news
switch->
replace my.nspk with my.nspk2 to the same host
]
						] <- done
			-> tags,like[
				-> Getparameters{..,bool? liked=null,string tag=null}[
					-> Tag(Tag tg_) <-done
					-> TagNews(Note nt) <-done
					-> Tag object creation,delete Tagged reference creation, delete <-done
				
					Like reference created <-done
					Liked notes select add <-done
					Like(Note,Person=null) <-done
					Dislike(Note,Person=null) check person like <-done
				] <-done
			] <-done	
			-> RepoFactory add interaface parameters <- done
			-> to DLL <- done
			-> pinned,published <- done
			->new object inheritance for toggled properties ToggledProperty <- done
			-> change class inheritance for type converter, only 2 lvl allowed and basetype is used <- done			
-> move TextBuider Build to new class above TextBuiler? to contain collections of tokens with formats <- done Token+foramt->shcema->builder
-> Custom formatter generation, for builder formats, moove from strings to class <-done
-> token format generation from string rewrite <-done
-> extend token format generator for collection and array of delimeters
-> signature: tg(Ienumerable<Tokens> tk,string[] delimeters) <- done
-> Query manager <- done
-> TokenBuilder <- done
-> Token,Orient,URL factories <- done
-> Command shema base with factories with Build from collections of tokens and commandbuilders <- done
-> Rewrite old shemas for new base shema methods <- done
-> Check fire working request <- done

			]
			
		]

		Quiz[
		
			TODO[

			actual[
			
drop down question type model
question type init
dropdown select event DOM change

questions model add
draw question DOM
receive input

print input on submit

			]

->
[
	back sends and receives Json or JsonShema
	Fron draws receive JSON or sh, redraws by user input and returns new JSON/JSNshm
]
-> Generate Form from JSON or JSONshema
[
	Linq Angular to MVC
]
-> Get JSON or JSONshema from Form
[
	angular-schema-form.min.js,
	https://github.com/json-schema-form/angular-schema-form/blob/master/docs/index.md#basic-usage
]
	
			]
			Deployed[
				FrontHost
				http://my.nspk.ru/Quiz/List
				Deployed
				MSK1-VM-INAPP01.nspk.ru
				,my.nspk.ru
				GitLab
				http://gitlab.nspk.ru/Intranet_Development/Intranet/

				DbHost:
				msk1-vm-indb01.nspk.ru:2480
				Intranet

			]
			LogicExists[
				
				FU[
no relations in Orient;
No Interface realization;
very arguable interface and autofac purpose -> no variations exists;
very arguable abstract class usage and inheritance logic ->
sealed override async Task<IEnumerable<Quiz>> SelectAsync();
no type specific code -> 
InsertNewQuizData("QuizResult", new QuizResult,
InsertNewQuizData(typeof(QuizResult).Name, new QuizResult;

				]
				InOrientDB[
					
					!!!has no relation class
					relation via QuizID!!!
					select from Quiz
					select from QuizResult
					select from Question
				]
				
				POCOs[

Quiz{
	...
	List<Question> Question{
		question{
			...
			List<Answer> Answers{}
			...
		}
	}
	...
}

QuizResult{
...
List<PerformerAnswer>{
	...
	PerformerQuestion{}
	...
}
...
}

				]
				
				MVC_angularlogic[
				View(Constructor.cshtml) uses 
				~/Scripts/CustomScripts/QuizJsonEditorForm/CKEditor-Bootstrap/angular-schema-form-base64-file-upload.js
				
				Constructor.cshtml[
ng-controller="FormController"
ConstructorSchema.json,ConstructorForm.json
OnSubmit 
poststs JSON.stringify($scope.model) to Quiz/Constructor
				]-> [HttpPost]QuizController[
public ActionResult Constructor(string output){
to QuizHelper.Insert();

}
				]
				-> QuizHelper[
				
QuizHelper.Insert() ->
//inserts quiz
QuizHelper.InsertQuestions(){
//inserts questions
InsertNewQuizData()
{
//это просто шедевр б...ь
InsertNewQuizData("QuizResult", new QuizResult
//вместо
InsertNewQuizData(typeof(QuizResult).Name, new QuizResult
//я в восторге, честно
}
}

QuizHelper.Execute(Quiz quiq){
	
	QuizHelper.InsertNewPerformerData(){
		insert PerformerQuestion as answer to quiz
	}
}

QuizHelper.SelectAsync(){
	Select Quiz
		additional select Question
}
				
				]
				
				View 
				Execute.cshtml
					Form QuzExecute 
					->
				Controller(Quiz)
					[HttpPost]
					public ActionResult Execute(Quiz quiz)
					...
					 ((IQuiz)Quizhelper).InsertNewPerformerData(quiz);
					->
				IQuiz interface has not binded to realization
				Realization in 
				QuizHelper.cs
				  public void InsertNewPerformerData(Quiz quiz)
					  ->
					 InsertNewQuizData("QuizResult", new QuizResult
					{
						QuizId = GetSelectedAnswerText(quiz).Questions.FirstOrDefault().QuizId,
						FinishDate = DateTime.Now,
						PerformerName = UserName,
						PerformerAnswer = answers
					});
						->
						private string InsertNewQuizData(string className, object obj)
						{
							var json = JsonConvert.SerializeObject(
								obj,
								new IsoDateTimeConverter {DateTimeFormat = "yyyy-MM-dd"},
								new StringEnumConverter()
							);
							return ODatabase.Insert().Into(className + " Content " + json).Run().ORID.RID;
						}
						
				]
				
				JsonForms[
C:\workflow\projects\Dev\gitLab\quiz\Intranet
\Intranet\Scripts\CustomScripts\QuizJsonEditorForm
\Json
ConstructorSchema.json
ConstructorForm.json
					]
					
			]
			WantedDesciption[
				Quest type: NPS - баллы для департамента, "Затрудняюсь ответить" 
				Question types: [radio, checkbox, + texbox], textbox, dropbox,pictures
				export excel
			]
			
		]
		
		DocsVision[
		
			OverallDescription[
	1CBatch,Newscontroller,AccountControoler,...
	1C,Ad->ProdOrient


	NSQL[
	JSON[Newtonsoft],Webmanager[Http,body,method],
	repo, UOW (Person,News), Mng()]
	NAPI[Mng.GetNews(....)]

	Person[]->NewsOrient->DV[Hierarhy Mng,mng of unit ,mng of person,person to company manager]
	(double)

	:Node
	Company
	:Reference
	WorkIn,Person{Disabled}
	[assignments],OldMainAssignment

	Disables, !WorkIn OldMainAssignment maxDate

	пшенистова Рук отд Chart 
	mng of unit dep (1000)
	weight

	sol (50)

	Person,manger
	]

		]
		
		NSQLUOW
		
			-> move current UOW logic to Manager <- done
			-> split Persons and News api <- done

		ApiTester	
			-> JSON response objects count and expect			
			
			-> POST with NTLM <- done
			-> Config export/import <- done
			-> Exeption expected <- done
			-> Null expected <- done
			-> Ok not OK to config print <- done

		SQLRepo
			-> Repo (against SQL base and Northwind base)
			-> Repo (Chaining IRepo implementation for Date + list filter)

		SQLUOW
			-> Finilize for Nowthwind, Neo4j,mongo
			
		SQLWebAPI
			-> between repo and MVC site
			-> Add BLL Business logic layer above UOW
			
		SQLPresentationSite
			-> parsing excel file to objects method decompose to several	
			-> Export to excel
			-> Type converter for migration (
			-> SQL to CLR type conversion ; 
			-> SQLEntity to DWH entity converter; 
			-> Converter logic to string name or type name;)
			-> Add JS multiple model items update at one time
		
		
	WebManager[
	
		TODO:[
			-> NTLM authentication
			
			-> Handling http methods POST,GET,DEL,PUT <- done
			-> Adding headers <- done
			-> Add request body <- done
			-> Orient DB cookie authorization <- done
		]
			
	]
	
	JsonManager[
	
		-> Parsing to/from objects/strings to/from JSON string <- done
		-> parsing to string <- done
		-> make generic <- done

	]
	
	JS
			-> Angular2.io chart
			
			-> Angular2 ajax <- done			
			-> Ajax console log function dependency <- done
			
		nsql presentation[
		
			INFO[
			
				prototype sites[
					http://nu-today.ru/
				]
				
				]
			
			TODO[
			->functional prototype newsApi
				ember, angular2, react
			->tree chart d3
			]
			
			]
			
		MyNSPK[
			
			info[
			
				HostedUrlsandSites[
//staic
http://static.nspk.ru
\\msk1-vm-ovisp01\e$
http://msk1-vm-ovisp01/v9/
http://my.nspk.ru/chart/
[my.nspk.ru]
http://msk1-vm-inapp01:80

				]
				
				UsedURLs[
				
	var SearchPersonUrl = 'http://msk1-vm-inapp01.nspk.ru:81/api/Structure/SearchPerson/'	
	var SearchByUrl = 'http://msk1-vm-inapp01.nspk.ru:81/api/Structure/SearchByFNameLName/'	
	var NoobsUrl = 'http://msk1-vm-inapp01.nspk.ru:81/api/Structure/GetPersonsLastTwoWeeks/last'
	[
StructureController
Get(string p1, string p2)
run orient function 
	]
	
	var UserSettingsUrl = 'http://msk1-vm-inapp01:8081/api/UserSettings/'
	var FavoritesUrl = 'http://msk1-vm-inapp01:8081/api/PersonRelation/'
	var PersonBirthdays = 'http://msk1-vm-inapp01:8081/api/PersonBirthdays'
	var AccountUrl = 'http://msk1-vm-inapp01.nspk.ru:8081/api/Account/'
	
	var VacationsUrl = 'http://msk1-vm-inapp01:8081/api/Person/HoliVationAcc'
	
	var QuizListURL = 'http://msk1-vm-inapp01:8185/api/Quiz'	
	var NewsApiUrl = 'http://msk1-vm-inapp01:8185/api/news2/'
	var OwnNewsUrl = NewsApiUrl + 'GetParam'
	
	var NewsFeedUrl = 'http://nspk.online/api/news.php?list=0&num=25'
	
	var MenuUrl = 'static.data/static.menu.json'
	var GalUrl = 'static.data/gal.json'		

CrossTeamWorkingGroupController
public string GetTr(string p1)
http://msk1-vm-indb01:2480/function/Orgchart_prod/GetFullTraverseBysAMAccountName/" + p1

PhotoController
public HttpResponseMessage Get(string email)
var url = "https://webmail.nspk.ru/ews/exchange.asmx/s/GetUserPhoto?email=" + email + "&size=HR120x120"
new MediaTypeHeaderValue("application/octet-stream");

				]

			]
			
		]
		
		jsHacks
			-> New API drop box above textbox add. Textbox no ovewrite
			-> Select2 Js To site <- done
		
	]
	
	,job[
	
quiz [


[
ang io [ http, VS template ]
,.net core [ web api 2 template, ang io template ]
,linq to orient [ extend ]
]
domen pass [
http://help.nspk.ru/Task/view/62582
62582
]
prod mynspk Olga[
deploy testOnProd
]

]

	]
	
	,DONE[

	PersonAPI
	-> HttpManager <- done
	-> JSONmanager <- done
	-> PersonManager <- done
	-> WebApiConfig <- done
	
	HTTP,JSON,Orient Managers <- done
	
	NewsAPI
	->add controller with methods from manager <-done

	
	PresentationSite	
-> POCO to DataContract converter <- done
-> Add WCF between Repo and Site <- done
-> UOW bind repo to generic with as and type conversion <- done
-> UOW bind context to every repo  <- done
-> Specific repository for every entity <- done
-> IRepository<T> with CRUD operations lightweight  <- done
-> SB , Repo and UOF to different projects  <- done
-> Emplicit implicit repos
1-explicit repo (mostly type by merchant)
Repo<T>() { GetItem1<T>() where T: IOne{}; ... GetItemn<T>() where T: In{}}
2-implicit repo (mostly one type by repo instance)
Repo<T>() where T: IOne { GetByOne(); }
...
Repo<T>() where T: In { GetByN(); } <- done

Northwind
-> change login employee get from TempData to model  <- done
-> add complex model add  <- done
->  Tables migration - test with repo <- done

	]
	
	,Global[
      
      NSQLRepo 
        -> finish <- done 
      NSQLUOW 
        -> finish <- done 
      NSQLManager 
        -> finish <- done
		Birthday
		->
		News
		->
		            		
	SQLrepo
		-> finish <- done 
	SQLUOW
		-> finish <- done 
	SQLwebApi
		-> finish
	SQLpresentationsite
		-> finish
		
	Quiz 
		-> finish <- done 
		-> refactor

	AdinTce store
		-> finish
	News store 
		-> finish
	
    ]
    
    ,Descriptions[
    
      {Intraservice
      
        То, что называется "JS хаки" это, например файлы из папки /js/ на inter02
        alertmessage.js
        autoexecutorpick.js
        IB_auto_pick_date.js
        select2.js
      
        Все местоположения http://10.31.14.76/cleverence_ui/hs/IntraService/location/full
        Частичные местоположения (215) http://10.31.14.76/cleverence_ui/hs/IntraService/location/part/215
        это не GUID Person,Unit и MainAssignment из orientDB.
        новые положения UI
        http://msk1-vm-onesweb01/nspk_ui/hs/IntraService/location/full
        
        ?Масленникова - заказчик
        завялов - по Intraservice
        Селиверстов - по Intraservice2 1135
        Акшарумов  1335
        завялов 
          ? -> карточки в is учёт миущества 1C
          ? -> добавление тестовых форм
          авторизации от админов - вопрос
        
        OrientFunction - rewrite GetPerson, return GUID exclude Disabled  persons
        JS hack - to fill IS form with values from			
          
          1)	У Завъялова - > форма intraservice взаимодействующая с 1C<> Cleverence
          2)	На сайт JS обёртка, подключающая к полям вьюхи 
            o ФИО и GUID должности из OrientDB по вводимому ФИО
            o Адресс местоположения по вводимомму адресу с :
              Все местоположения http://10.31.14.76/cleverence_ui/hs/IntraService/location/full
                [
                  переносы в конце строки: \n"
                  незакрытые кавычки: \"НСПК, 4 этаж, рм. 4.10-061 Кладовая"
                  обе красоты в одной: \"Голден Гейт\"\n\nРезервный офис АО \"НСПК"
                  и фиальная, адресс тока: "Адрес": "."
                  
                ]
              Частичные местоположения (215) http://10.31.14.76/cleverence_ui/hs/IntraService/location/part/215

          {
            HelpDeskTestAPI:
            [
              http://msk1-vm-inter02.nspk.ru/api/Task/
              http://msk1-vm-inter02.nspk.ru/api/Task/53883
              http://msk1-vm-inter02.nspk.ru/api/Service
            ]
            
          }
          
          {checkbox for text input
            
          }
          
      }
      
      ,{Intraservice structure
        
        [
          {
          "FILE":"IntraService.Domain.dll",
          "Description":"Entity models. Entity + model in one huge block"
          }
        ]
      
      }
      
      ,{ElMA:26.10.17
        
        [
          {
          
          }
        ]
      
      }
      
      ,{News Site
      
     
          
        [API
        
  User-Agent: Fiddler
  Authorization: Basic cm9vdDptUiVtekpVR3ExRQ==
  Host: msk1-vm-ovisp01:8184
  Content-Length: 112
  Content-Type: application/json

  POST
  http://msk1-vm-ovisp01:8184/api/News

  POST,PUT
  http://msk1-vm-ovisp01:8184/api/News/174

  {"Changed": "2017-10-19 18:00:09", "Created": "2015-02-02 12:43:56", "GUID": "1", "Name": "0"}


        ]

        ,[user settings 
          
  (Person)-[CommonSettings]->(UserSettings{"showBirthday":"true"})
          
          ,{check serverside filter <- done
          
            add to all orient functions if to publish birthdate according to condition for birthday on/of
            return all birthdays for those who on
            
          }
          
          ,{Birthdays
          post -> receive GUID
          add edge (seeBirthdays) from authenticated person to guided
          return edge collection (Person auth)-[seeBirthday]->(Persons selected)
            
          ,add birthday objects Creation to ETL
              
            
          }			
              
        ]
                      
        ,[AdinTce
            
          {AdinTce URLS
  "Source":


  http://msk1-vm-onesweb01/nspk_zup/hs/Portal_Holiday/location/graph/full
  http://msk1-vm-onesweb01/nspk_zup/hs/Portal_Holiday/location/holiday/full
  http://msk1-vm-onesweb01/nspk_zup/hs/Portal_Holiday/location/vacation/full
  /part/592a863e-8642-11e7-8119-005056813668

  http://msk1-vm-ovisp01:8184/api/Person/Acc
  http://msk1-vm-ovisp01:8184/api/Person/HoliVation/kpa
  http://msk1-vm-ovisp01:8184/api/Person/HoliVationAcc

  http://msk1-vm-ovisp01:8085/api/Person/Acc
  http://msk1-vm-ovisp01:8085/api/Person/HoliVation/kpa
  http://msk1-vm-ovisp01:8085/api/Person/HoliVationAcc

  http://msk1-vm-inapp01:8081/api/Person/Acc
  http://msk1-vm-inapp01:8081/api/Person/HoliVation/kpa
  http://msk1-vm-inapp01:8081/api/Person/HoliVationAcc
        
          [test

          ]

          }
          
          ,{Preliminary
        
  intranet backend
  NTLM -> orient - > GUID ->  1C api -> result

  UI
  УИ (чёт активов)
  (password from 1c batch)
  http://msk1-vm-onesweb01/nspk_ui/ws/ExchangeInv.1cws?wsdl

  http://msk1-vm-onesweb01/nspk_ui/hs/IntraService/location/full
  Ok {GUID,Адресс} old cleverence
  http://msk1-vm-onesweb01/nspk_ui/hs/IntraService/location/part/215

  ZUP
  ЗУП (1c batch) (WS:12345qweQWE!@#)
  V1M6MTIzNDVxd2VRV0UhQCM=

  http://msk1-vm-onesweb01/nspk_zup/ws/ExportData.1cws?wsdl
  GET
  http://msk1-vm-onesweb01/test3/hs/Portal_Holiday/location/holiday/full
  "GUID": "cc109057-39aa-11e4-95f3-00c2c66d1ae5",
  "ВидОтпуска": "Основной",
  "Должность": "Начальник Управления бухгалтерского учета и отчетности - Главный бухгалтер",
  "Дни": 14.67

  http://msk1-vm-onesweb01/test3/hs/Portal_Holiday/location/holiday/part/18a14516-cbb4-11e4-b849-f80f41d3dd35
  {HTTPСервис._Portal_Holiday.Модуль(109)}: Ошибка при вызове метода контекста (ПолучитьСсылку)

  http://msk1-vm-onesweb01/test3/hs/Portal_Holiday/location/graph/full
  [
  {
  "GUID": "0629685d-7e6b-11e7-8119-005056813668",
  "ВидОтпуска": "Основной",
  "Должность": "Руководитель направления",
  "ДатаНачала": "20180409",
  "ДатаОкончания": "20180422",
  "Дни": 0
  },
  http://msk1-vm-onesweb01/test3/hs/Portal_Holiday/location/graph/part/215
  {HTTPСервис._Portal_Holiday.Модуль(253)}: Ошибка при вызове метода контекста (ПолучитьСсылку)

  http://msk1-vm-onesweb01/test3/hs/Portal_Holiday/location/vacation/full
  http://msk1-vm-onesweb01/test3/hs/Portal_Holiday/location/vacation/part/88906e68-e697-11e5-80d4-005056813668


  УХ

  http://msk1-vm-onesweb01/nspk_uh/ws/ExchangeClev.1cws?wsdl
  http://msk1-vm-onesweb01/nspk_uh/ws/DocsVision.1cws?wsdl

  Так же как я понимаю вам еще нужны доп данные для подключения..
  Этих будет достаточно? Могу переназвать если надо..
  Если все устроит могу в рабочую поместить..
  Корневой URL: Portal_Holiday
  Остатки отпусков по физ лицу - /location/holiday/{ФизЛицо}
  Остатки отпусков - /location/holiday/full
  График отпусков по физ лицу - /location/graph/{ФизЛицо}
  График отпусков - /location/graph/full
  Отпуска по физ лицу - /location/vacation/{ФизЛицо}
  Отпуска  - /location/vacation/full

        }
        
        
      ]
        
        ]
        
        ,[Person API
      
  Сейчас, похожи на вес две функции

  GetWeight

  На вход передается Position

  возвращает
  isManager 0 
  isZam 1000000
  others 2000000

  GetPositionBar

  На вход передается Position

  возвращает
  isManager Руководитель
  isZam Заместитель
  others null

  как я понимаю seed рассчитывается из JS и именно от него иерархия строится.
  Для perosn API нужно больше весов.

      ]
                            
        ,[MyNspk URLs from

  http://msk1-vm-inapp01.nspk.ru:81/api/Photo/?email={{cnt.mail}}
  var GetStructureByUnitGUID = 'http://msk1-vm-inapp01.nspk.ru:81/api/Structure/GetStructureByUnitGUID/'
  var GetStructureByPersonGUID = 'http://msk1-vm-inapp01.nspk.ru:81/api/Structure/GetStructureByPersonGUID/'
  var GetStructureByUnitGUIDAtCurrentLevel = 'http://msk1-vm-inapp01.nspk.ru:81/api/Structure/GetStructureByUnitGUIDAtCurrentLevel/'
  var SearchPersonUrl = 'http://msk1-vm-inapp01.nspk.ru:81/api/Structure/SearchPerson/'
  //var NewsFeedUrl = 'http://nspk.online/api/news.php?list=0&num=25'
  var AccountUrl = 'http://msk1-vm-inapp01:8081/api/Account/'
  var SearchByUrl = 'http://msk1-vm-inapp01.nspk.ru:81/api/Structure/SearchByFNameLName/'
  //var MenuUrl = 'static.data/static.menu.json'
  //var GalUrl = 'static.data/gal.json'
  var NoobsUrl = 'http://msk1-vm-inapp01.nspk.ru:81/api/Structure/GetPersonsLastTwoWeeks/last';
  var PersonalSettingsUrl = "http://msk1-vm-ovisp01:8084/api/UserSettings/";
  // var NewsFeedUrl = 'http://nspk.online/api/news.php?list=0&num=25'
  // var AccountUrl = 'http://msk1-vm-inapp01:8081/api/Account/'
  // var SearchByUrl = 'http://msk1-vm-inapp01.nspk.ru:81/api/Structure/SearchByFNameLName/'
  // var MenuUrl = 'static.data/static.menu.json'
  // var GalUrl = 'static.data/gal.json'
  // // var VacationsUrl = 'http://msk1-vm-ovisp01:8085/api/Person/HoliVationAcc'
  // var VacationsUrl = 'http://msk1-vm-ovisp01:8085/api/Person/HoliVation/saa'
  // var NoobsUrl = 'http://msk1-vm-inapp01.nspk.ru:81/api/Structure/GetPersonsLastTwoWeeks/last'
  // var UserSettingsUrl = 'http://msk1-vm-ovisp01:8084/api/UserSettings/'
  // var FavoritesUrl = 'http://msk1-vm-ovisp01:8084/api/PersonRelation/'
  // var PersonBirthdays = 'http://msk1-vm-ovisp01:8084/api/PersonBirthdays'
  var NewsFeedUrl = 'http://nspk.online/api/news.php?list=0&num=25'
  var AccountUrl = 'http://msk1-vm-inapp01.nspk.ru:8081/api/Account/'
  var SearchByUrl = 'http://msk1-vm-inapp01.nspk.ru:81/api/Structure/SearchByFNameLName/'
  var MenuUrl = 'static.data/static.menu.json'
  var GalUrl = 'static.data/gal.json'
  var VacationsUrl = 'http://msk1-vm-inapp01:8081/api/Person/HoliVationAcc'
  var NoobsUrl = 'http://msk1-vm-inapp01.nspk.ru:81/api/Structure/GetPersonsLastTwoWeeks/last'
  var UserSettingsUrl = 'http://msk1-vm-inapp01:8081/api/UserSettings/'
  var FavoritesUrl = 'http://msk1-vm-inapp01:8081/api/PersonRelation/'
  var PersonBirthdays = 'http://msk1-vm-inapp01:8081/api/PersonBirthdays'
  //var VacationsUrl = 'http://msk1-vm-ovisp01:8085/api/Person/HoliVationAcc'
    
        ]
        
        ,[JSON exmpls
  {"result":[{"A":"B"},{"A":"B"},{"A":"B"}]}
  {"result":[{"A":"B","A2":"B2","A3":"B3"},{"A":"B"},{"A":"B"}]}
  {"A":"B","A2":"B2","A3":"B3","result":[{"A":"B"},{"A":"B"},{"A":"B"}]}

        ]
        
        ,[NewsAPI

  //Traverse Commentaries to News
  traverse outE('Comment'),inV('Commentary'),inE('Authorship','Comment') from 26:3
  traverse out('Comment'),in('Comment') from 25:4

  //traverse comments to comment
  select from (traverse out('Comment') from 26:5)
  where commentDepth >=0 and commentDepth <=3

  //traverse up fromcomment to news
  traverse in('Authorshp','Comment') from (select from 24:53)

  //Get persons from news
  Select expand(a1) from(Select inE('Authorship').outV('Person')  as a1 from 26:1 )

  //Person authorships
  traverse out() from 31:6
  //Author find
  select expand(in('Authorship')) from 23:1

  //Check Notes without authorship
  select from Note where in('Authorship').@class!='Person'


  //Select from News with depth
  select from (traverse outE('Comment'),inV('Commentary'),inE('Authorship','Comment') from 26:2)
  where commentDepth >=2 and commentDepth <=2 

  //
  traverse out('Comment','Authorship'),in('Comment') from 32:7

        ]
        
        ,[Shema
        
        //--------------------------------

        //Class  model <- done

        (Person) - [Authorship] -> (Commentary) <- [Comment] - (Person)
        (Person) - [Authorship] -> (News) <- [Comment] - (Person)
         
        //Relations model -> obsolette

        Post new tag by name, post tag id news id
        person to like

        (Person) - [Authorship] -> (Object{pinned:true})
        News //editable by hardcode string acc group to every tag
        (Person) - [Authorship] -> (Object{"pinned":"true";"published":"true"})
        Commentary
        (Person) - [Authorship] -> (Object) - [Comment{CommentLevel:0+1}] -> (Object)
        
        //All note fields
        Note
        {

          public override string id { get; set; }       	
          public override string @version {get; set;} 
          
          public Person author_ { get; set; }

          public string PGUID { get; set; }=string.Empty;

          public string authAcc { get; set; }=string.Empty;
          public string authGUID { get; set; }=string.Empty;
          public string authName { get; set; }=string.Empty;

          public string pic {get;set;}=string.Empty;
          public string name {get;set;}=string.Empty;               

          [JsonProperty("content_")]
          public virtual string content { get; set; }=string.Empty;
          public string description { get; set; }=string.Empty;

          public DateTime? pinned { get; set; }=null;
          public DateTime? published { get; set; }=null;

          public int? commentDepth { get; set; }=0;
          public bool hasComments { get; set; }=false;

          public int likes { get; set; }=0;
          public bool liked { get; set; }=false;

        }

        //updatable
        News
        {
          
          public Person author_ { get; set; }

          public string pic {get;set;}=string.Empty;
          public string name {get;set;}=string.Empty;               

          [JsonProperty("content_")]
          public virtual string content { get; set; }=string.Empty;
          public string description { get; set; }=string.Empty;				

          public bool liked { get; set; }=false;
          
          public DateTime? pinned { get; set; }=null;
          public DateTime? published { get; set; }=null;

        }
        
        //Author never edited from Edtors
        //updatable from author
        Commentary
        {
              
          public string pic {get;set;}=string.Empty;
          public string name {get;set;}=string.Empty;               

          [JsonProperty("content_")]
          public virtual string content { get; set; }=string.Empty;
          public string description { get; set; }=string.Empty;
          
          public DateTime? published { get; set; }=null;

          public bool liked { get; set; }=false;
          
          public DateTime? pinned { get; set; }=null;
          public DateTime? published { get; set; }=null;

        }

        if "Name":null -> если null оставляем как в базе
        if "Name":"" -> если явное "" empty перезаписываем в базе
        
        if author_:null or "" -> если пустое или null оставляем как в базе
        
        personReturn
        { 
          "GUID": "ba124b8e-9857-11e7-8119-005056813668",
          "division": "Отдел поддержки прикладных систем",
          "PGUID": "c1b0ff45-5cb1-11e7-8117-005056813668",
          "phone": "1312",
          "mail": "Neprintsevia@nspk.ru",
          "Name": "Непринцев Илья Александрович",  
          "shortFName": "Непринцев И. А.",
          "description": "Главный специалист",
          "itemTitleColor": "rgb(44,50,124)",
          "templateName": "PersonTemplate",
          "birthday": "09.03",
          "colorClass": "c-dit",
          "departmentName": "Департамент ИТ",
          "login": "Neprintsevia"
        }
        
        NoteReturn{
          
          author personReturn_ {get;set;}
          
          note_ {get;set;}
          
          commentaryCount {get;set;}
          likesCount {get;set;}
          
        }
        
        //--------------------------------

        ]
        
        ,Test API[
          
          Bulk[
          

    http://localhost:63282/api/news2/2
    
    
          
    http://localhost:63282/api/news2/10
    http://localhost:63282/api/news2/45bdc4fa-5952-475f-a408-9a277b714bcb/2
    http://localhost:63282/api/Quiz


    http://msk1-vm-ovisp01:8184/api/Person/GetUnit/kpa
    http://msk1-vm-ovisp01:8184/api/Person/GetDepartment/kpa
    http://msk1-vm-ovisp01:8184/api/Person/GetManager/kpa
    http://msk1-vm-ovisp01:8184/api/Person/GetUnit/kpa
    http://msk1-vm-ovisp01:8184/api/Person/GetUnit/kpa
    http://msk1-vm-ovisp01:8184/api/Person/GetUnit/kpa
    http://msk1-vm-ovisp01:8184/api/Person/GetUnit/kpa
    http://msk1-vm-ovisp01:8184/api/Person/GetUnit/kpa
    http://msk1-vm-ovisp01:8184/api/Person/HoliVation/kpa

    http://msk1-vm-ovisp01:8184/api/Person/HoliVationAcc

    http://msk1-vm-ovisp01:8184/api/Person2/acc
    http://msk1-vm-ovisp01:8184/api/news2/2
    http://msk1-vm-ovisp01:8184/api/news2/b71bec8b-ebac-40ff-852b-ba2ba1bc6d81/2
    http://msk1-vm-ovisp01:8184/api/Quiz
            ]

          , Api[

    //Person
    //////////////////////////////////////////////
    GET
    http://msk1-vm-ovisp01:8184/api/Person/Acc
    NTLMAccountName

    http://msk1-vm-ovisp01:8184/api/Person/GetUnit/kpa
    Юридическое управление

    http://msk1-vm-ovisp01:8184/api/Person/GetDepartment/kpa
    Организационно-правовой департамент

    http://msk1-vm-ovisp01:8184/api/Person/GetManager/kpa
    tishakovoi


    //AdinTce
    //////////////////////////////////////////////
    GET
    http://msk1-vm-ovisp01:8184/api/Person/Holivation/kpa
    {"GUID":"348f1d6f-58ea-11e4-a7e1-00c2c66d13b0","Position":"Начальник управления","Holidays":[{"LeaveType":"Основной","Days":44.0},{"LeaveType":"За ненормированный рабочий день","Days":1.0}],"Vacations":[{"LeaveType":"Отпуск основной","DateStart":"07.08.2017","DateFinish":"20.08.2017","DaysSpent":14},{"LeaveType":"Отпуск основной","DateStart":"30.10.2017","DateFinish":"03.11.2017","DaysSpent":5}]}

    GET
    http://msk1-vm-ovisp01:8184/api/Person/HolivationAcc
    {"GUID":"ba124b8e-9857-11e7-8119-005056813668","Position":"Главный специалист","Holidays":[{"LeaveType":"Основной","Days":7.0}],"Vacations":[]}



    //Quiz
    //////////////////////////////////////////////
    GET
    http://msk1-vm-ovisp01:8184/api/Quiz
    [{"title":"Опросы","href":{"link":"http://my.nspk.ru/Quiz/Execute/","target":"_self"},"id":50,"parentid":1},{"title":"Title_500","href":{"link":"http://my.nspk.ru/Quiz/Execute/?#19:43","target":"_self"},"id":500,"parentid":50},{"title":"Title_501","href":{"link":"http://my.nspk.ru/Quiz/Execute/?#19:42","target":"_self"},"id":501,"parentid":50},{"title":"Title_502","href":{"link":"http://my.nspk.ru/Quiz/Execute/?#20:38","target":"_self"},"id":502,"parentid":50},{"title":"Title_503","href":{"link":"http://my.nspk.ru/Quiz/Execute/?#20:39","target":"_self"},"id":503,"parentid":50}]



    //News
    //////////////////////////////////////////////

    Post news
    [METHOD,URL,HEADER,BODY,RESPONSE]
    POST
    http://msk1-vm-ovisp01:8184/api/news2
    Content-Type: application/json
    {"authAcc":"Neprintsevia","pic":"","name":"TestNews3","content_":"news text","description":"","commentDepth":0,"hasComments":false,"likes":0,"liked":false}
    {"PGUID":"000","authAcc":"Neprintsevia","authGUID":"000","authName":"Neprintsevia","pic":"","name":"TestNews","content_":"","description":"","commentDepth":0,"hasComments":false,"likes":0,"liked":false,"@class":"News","GUID":"dac34a61-d01c-487c-8d1e-694f8383a21f","Created":"2017-12-19 09:54:59"}

    Post comment
    [METHOD,URL,HEADER,BODY,RESPONSE]
    POST
    http://msk1-vm-ovisp01:8184/api/news2/{NoteGUID}
    Content-Type: application/json
    {"authAcc":"Neprintsevia","pic":"","name":"TestComment1","content_":"comment text","description":"","commentDepth":0,"hasComments":false,"likes":0,"liked":false}
    {"pic":"","name":"TestComment1","content_":"comment text","description":"","commentDepth":1,"hasComments":false,"likes":0,"liked":false,"@class":"Commentary","GUID":"0a35caf2-1045-47e2-bb41-46c931b6b1da","Created":"2017-12-19 09:57:09"}

    Put News
    [METHOD,URL,HEADER,BODY,RESPONSE]
    PUT
    http://msk1-vm-ovisp01:8184/api/news2
    Content-Type: application/json
    {"GUID":"dac34a61-d01c-487c-8d1e-694f8383a21f","authAcc":"Neprintsevia","pic":"","name":"TestComment2","content_":"comment text updated","description":"","commentDepth":0,"hasComments":false,"likes":0,"liked":false}
    {"pic":"","name":"TestComment2","content_":"comment text updated","description":"","commentDepth":0,"hasComments":false,"likes":0,"liked":false,"@class":"News","GUID":"dac34a61-d01c-487c-8d1e-694f8383a21f","Created":"2017-12-19 10:03:11"}

    GET News\Comment with comments with depth
    /Guid/depthOffset
    http://msk1-vm-ovisp01:8184/api/news2/{NoteGUID}/2
    [{"Name":"Commentary0","pic":"","name":"","content_":"fucking bullshit comentary","description":"","commentDepth":3,"hasComments":false,"likes":0,"liked":false,"@class":"Commentary","GUID":"47eb0894-7fb1-4b24-aabd-77f264537678","Created":"2017-12-18 03:30:10"},{"Name":"Commentary6","pic":"","name":"","content_":"fucking bullshit comentary","description":"","commentDepth":4,"hasComments":false,"likes":0,"liked":false,"@class":"Commentary","GUID":"288fdabd-66c1-4042-be81-d7ff18d864b2","Created":"2017-12-18 03:30:10"},{"Name":"Commentary4","pic":"","name":"","content_":"fucking bullshit comentary","description":"","commentDepth":5,"hasComments":false,"likes":0,"liked":false,"@class":"Commentary","GUID":"16f8988e-beb5-419a-bfb3-f00da0e57af2","Created":"2017-12-18 03:30:10"},{"Name":"Commentary8","pic":"","name":"","content_":"fucking bullshit comentary","description":"","commentDepth":6,"hasComments":false,"likes":0,"liked":false,"@class":"Commentary","GUID":"01396598-0a21-476e-b566-98765f9e2011","Created":"2017-12-18 03:30:10"},{"Name":"Commentary5","pic":"","name":"","content_":"fucking bullshit comentary","description":"","commentDepth":6,"hasComments":false,"likes":0,"liked":false,"@class":"Commentary","GUID":"0a4d8a0b-b034-4249-a717-0bf32fa22034","Created":"2017-12-18 03:30:11"}]

    GET get News with depth
    http://msk1-vm-ovisp01:8184/api/news2/5
    [{"Name":"Real news","pic":"","name":"","content_":"","description":"","commentDepth":0,"hasComments":false,"likes":0,"liked":false,"@class":"News","GUID":"057ba719-bd7e-4827-84b9-33f013765e4a","Created":"2017-12-18 03:28:03"},{"Name":"News1","pic":"","name":"","content_":"fucking interesting news","description":"","commentDepth":0,"hasComments":false,"likes":0,"liked":false,"@class":"News","GUID":"a47a360a-33d7-46fd-8132-0dcdb4aa88fb","Created":"2017-12-18 03:30:09"},{"Name":"News1","pic":"","name":"","content_":"fucking interesting news","description":"","commentDepth":0,"hasComments":false,"likes":0,"liked":false,"@class":"News","GUID":"73e24beb-84bb-4c55-9735-830412a807ea","Created":"2017-12-18 03:30:09"},{"Name":"News0","pic":"","name":"","content_":"fucking interesting news","description":"","commentDepth":0,"hasComments":false,"likes":0,"liked":false,"@class":"News","GUID":"edec4cc2-d66c-4d51-8691-a762e763abf3","Created":"2017-12-18 03:30:09"},{"Name":"News0","pic":"","name":"","content_":"fucking interesting news","description":"","commentDepth":0,"hasComments":false,"likes":0,"liked":false,"@class":"News","GUID":"82f83601-d5cd-4108-b1b7-d27ac5a3933a","Created":"2017-12-18 03:30:09"}]

          ]

        ]
        
        ,[TODO

  >> todo
  add personsource UOW to manager
  manager generator 
  << done

  >>
  Test Repo, UOWs
  <-integrateion tests added+db boilerplate generate
  << done
 
  >>
  Ref repo
    >>property specific methods.Methods receive only condition in form of string.
    >>Property specific moove to UOW.Methods with Strings to condition.
    >>Remove instances where types needed.


  >>
  Like
  (Person) - [Like] -> (Object)
  Tags //editable by hardcode string acc group to every tag
  (Person)->[Tag{Text}]->(News)
  (Tag{"tagName":""}) - [Tags] -> (Object)
  Publish,Pinn>>
  {"Published"=datetime.now,"Pinned"=datetime.now}

  >>
  new return object from Note and Person
  >>
  return object Person card

        ]
        
        ,[DONE
  
  >>Move repo initialization from UOWs to external. UOW receives Manager  << done
  >>Move all build repo and requesting bock into methods.  << done
  
  TokenBuilder -> rewrite <-done
  Add patterns for common command signatures in separate class -> rewrite  <-done
  Command class add chaining -> rewrite  <-done
      
  Rspo -> string to URL -> rewrite or depricate  <-done
  UOW -> tokens and collections to string with typeconverter -> rewrite or depricate  <-done
        
  <<Methods >> <- done
  post
  published no

  <<new get/news/count/depth>>  <- done
  published null <=sysdate


  <<PUT >> <- done
  update account author
  change auth_ person to personAcc

  <<PUT rules <- done
  public T UpdateObjects<T>()
        ]
      
      }
        
      ,{NOSQLmanager
             
        
        {URlbuilder test
                
          http://msk1-vm-ovisp02:2480/command/news_test3/sql/create vertex Person content {"Changed": "2017-10-19 18:00:09", "Created": "2015-02-02 12:43:56", "GUID": "0", "Name": "0"}

          { "transaction" : true,
            "operations" : [
            {
              "type" : "script",
              "language" : "sql",
              "script" : [ "INSERT INTO Person content {\"Changed\": \"2017-10-19 18:00:09\", \"Created\": \"2015-02-02 12:43:56\", \"GUID\": \"0\", \"Name\": \"0\"}",
                     "INSERT INTO Person content {\"Changed\": \"2017-10-19 18:00:09\", \"Created\": \"2015-02-02 12:43:56\", \"GUID\": \"0\", \"Name\": \"0\"}"
                     ]
            }
            ]
          }


          { "transaction" : true,
            "operations" : [
            {
              "type" : "script",
              "language" : "sql",
              "script" : [
               "create vertex Person content {\"Changed\": \"2017-10-19 18:00:09\", \"Created\": \"2015-02-02 12:43:56\", \"GUID\": \"0\", \"Name\": \"0\"}"
               ,"create vertex Person content {\"Changed\": \"2017-10-19 18:00:09\", \"Created\": \"2015-02-02 12:43:56\", \"GUID\": \"0\", \"Name\": \"0\"}"
              ]
            }
            ]
          }


          "http://msk1-vm-ovisp02:2480/connect/news_test3"
          "http://msk1-vm-ovisp02:2480/command/news_test3/sql"
          "Select from Person"
          "where 1=1"
          http://msk1-vm-ovisp02:2480/command/news_test3/sql/Select from Person where 1=1


          Create Vertex Person content {"Created":"2017-10-27T14:45:11.3738136+03:00","GUID":"0","Changed":"2017-10-27T14:45:11.3733119+03:00","Name":"0"}
          Create Vertex Person content {"Created":"2017-10-27 14:55:02","GUID":"0","Changed":"2017-10-27 14:55:02","Name":"0"}

          http://msk1-vm-ovisp02:2480/command/news_test3/sql/Create Vertex Person content 
          {"Created":"2017-10-27 14:55:02","GUID":"0","Changed":"2017-10-27 14:55:02","Name":"0"}

          Create Vertex Person content {\"Created\":\"2017-01-01 00:00:00\",\"GUID\":\"0\",\"Changed\":\"2017-01-01 00:00:00\",\"Name\":\"0\"
          }
          
          Create Vertex Person content {"Created":"2017-10-27 15:03:08","GUID":"0","Changed":"2017-10-27 15:03:08","Name":"0"}
                  
          Delete Vertex Person where "Name" = "0"
          
          http://msk1-vm-ovisp02:2480/command/news_test3/sql/Delete Vertex Person where Name = 0
                                
        }
      
        ,{TODO

  -> RepoFactory add interaface parameters
  -> extend with ability to load tokens from model class
  -> add all tokens to tokenlist
  -> Moove builders and command invokers to Generic
  -> Tokens to static?
  -> add authentication timer for orient connection
  -> add interface segregation for different token types => {IdataType, ICreateType}
  -> add logging

        }
        
        ,{DONE
  -> manager class added. UOWs splitted. Check for manager to Controller.
  -> change class inheritance for type converter, only 2 lvl allowed and basetype is used <- done
  -> move TextBuider Build to new class above TextBuiler? to contain collections of tokens with formats <- done Token+foramt->shcema->builder
  -> Custom formatter generation, for builder formats, moove from strings to class <-done
  -> token format generation from string rewrite <-done
  -> extend token format generator for collection and array of delimeters
  signature: tg(Ienumerable<Tokens> tk,string[] delimeters) <- done
        }
          
        
      }

      ,{Servers
            
        лицухи tfs 2017

        васильев 
        максимов

        заказ сервера helpdesk

      }
            
      ,{Quiz:
      
        {Overall
      

  {"menu" : [
           {"title":"Сотруднику", "href":{"link":0, "target":"_self"} ,"id":1, "parentid" : 0}
          ,{"title":"Скидки",  "href":{"link":"http://list.nspk.ru/sale/", "target":"_blank"} ,"id":11, "parentid" : 1}
          ,{"title":"Фотоальбом",  "href":{"link":"https://photo.nspk.ru/", "target":"_blank"} ,"id":12, "parentid" : 1}
          ,{"title":"Tutor",  "href":{"link":"http://tutor.nspk.ru/", "target":"_blank"} ,"id":10, "parentid" : 1}
          ,{"title":"Мировое радио",  "href":{"link":"http://nspk.online/", "target":"_blank"} ,"id":17, "parentid" : 1}
          ,{"title":"MyBook",  "href":{"link":"http://nspk.mybook.ru/", "target":"_blank"} ,"id":16, "parentid" : 1}
          ,{"title":"Оргструктура",  "href":{"link":"chart/", "target":"_blank"} ,"id":13, "parentid" : 1}
          ,{"title":"FAQ",  "href":{"link":"http://list.nspk.ru/faq/", "target":"_blank"} ,"id":15, "parentid" : 1}
          ,{"title":"Список сотрудников",  "href":{"link":"files/list.xlsx", "target":"_blank"} ,"id":14, "parentid" : 1}
          ,{"title":"Листок достижений",  "href":{"link":0, "target":"_self"} ,"id":18, "parentid" : 1}
          ,{"title":"Май",  "href":{"link":"http://list.nspk.ru/may/", "target":"_blank"} ,"id":181, "parentid" : 18}
          ,{"title":"Июнь",  "href":{"link":"http://list.nspk.ru/june/", "target":"_blank"} ,"id":182, "parentid" : 18}
          ,{"title":"Июль",  "href":{"link":"http://list.nspk.ru/july/", "target":"_blank"} ,"id":183, "parentid" : 18}
          ,{"title":"Август",  "href":{"link":"http://list.nspk.ru/august/", "target":"_blank"} ,"id":184, "parentid" : 18}
          ,{"title":"Сентябрь",  "href":{"link":"http://list.nspk.ru/september/", "target":"_blank"} ,"id":185, "parentid" : 18}
          ,{"title":"Системы",  "href":{"link":0, "target":"_self"} ,"id":2, "parentid" : 0}
          ,{"title":"Help",  "href":{"link":"http://help.nspk.ru/", "target":"_blank"} ,"id":21, "parentid" : 2}
          ,{"title":"Jira",  "href":{"link":"http://jira.nspk.ru/", "target":"_blank"} ,"id":22, "parentid" : 2}
          ,{"title":"Confluence",  "href":{"link":"http://confluence.nspk.ru/pages/viewpage.action?pageId=10380689", "target":"_blank"} ,"id":23, "parentid" : 2}
          ,{"title":"RealTimeFeadback",  "href":{"link":"https://go-only.com/", "target":"_blank"} ,"id":24, "parentid" : 2}
          ,{"title":"МИР НСПК - Узнай первым",  "href":{"link":"https://t.me/joinchat/ANiZ5z8Ovkz0DcDtzG_nng/", "target":"_blank"} ,"id":25, "parentid" : 1}
          ,{"title":"В рабочем порядке",  "href":{"link":0, "target":"_self"} ,"id":19, "parentid" : 1}
          ,{"title":"02.10.17",  "href":{"link":"http://list.nspk.ru/pdf/1/02.10.17.pdf", "target":"_blank"} ,"id":191, "parentid" : 19}
          ,{"title":"09.10.17",  "href":{"link":"http://list.nspk.ru/pdf/1/09.10.17.pdf", "target":"_blank"} ,"id":192, "parentid" : 19}
          ,{"title":"16.10.17",  "href":{"link":"http://list.nspk.ru/pdf/1/16.10.17.pdf", "target":"_blank"} ,"id":193, "parentid" : 19}
          ,{"title":"23.10.17",  "href":{"link":"http://list.nspk.ru/pdf/1/23.10.17.pdf", "target":"_blank"} ,"id":194, "parentid" : 19}
          ,{"title":"30.10.17",  "href":{"link":"http://list.nspk.ru/pdf/1/30.10.17.pdf", "target":"_blank"} ,"id":195, "parentid" : 19}
          ,{"title":"13.11.17",  "href":{"link":"http://list.nspk.ru/wa/13.11.17/", "target":"_blank"} ,"id":196, "parentid" : 19}
          ,{"title":"27.11.17",  "href":{"link":"http://list.nspk.ru/wa/27.11.17/", "target":"_blank"} ,"id":197, "parentid" : 19}
          ,{"title":"04.12.17",  "href":{"link":"http://list.nspk.ru/wa/04.12.17/", "target":"_blank"} ,"id":198, "parentid" : 19}
      ,{"title":"Благотворительность",  "href":{"link":"http://list.nspk.ru/charity/", "target":"_blank"} ,"id":110, "parentid" : 1}
      
  ]}



  mR%mzJUG1qE
  I9grekVmk5g

  http://msk1-vm-ovisp01:443/api/Quiz/Execute/?id=20:60
  http://msk1-vm-ovisp01:80/api/Quiz/Execute/?id=20:60
  http://msk1-vm-ovisp01:443/Quiz/Execute/?id=20:60
  http://msk1-vm-ovisp01:80/Quiz/Execute/?id=20:60

  http://msk1-vm-indb01:2480
  http://msk1-vm-ovisp02:2480
  Intranet
  Quiz


  [

  {"title":"Опросы", "href":{"link":0, "target":"_self"} ,"id":50, "parentid" : 1}


  [
  {"href":{"target":"_self"},"ID":500,"parentID":50}
  ,{"title":"Опросы","href":{"link":"","target":"_self"},"ID":50,"parentID":1}
  ,{"title":"","href":{"target":"_self"},"ID":50,"parentID":0}
  ]


  title QuizName (27...)
  status -> State (!Finished,Published only)
  data->today StartDate  today
  href:link http://my.nspk.ru/Quiz/Execute/
  "target":"_blank"
  id >= 500
  parentid 50

  ]


  select from Quiz
  /*where QuizName like '%зрени%'*/
  where StartDate between '2017-11-10 00:00:00' and '2018-07-11 00:00:00'

  Comment
  News
  Question
  Quiz
  QuizResult
  WeatherInformation

  Answers
  Label
  Profile

  http://my.nspk.ru/Quiz/Execute/19:5

  http://msk1-vm-ovisp01/nspk.ru

  create index Person.Name_sAMAccountName_mail on Person (Name,sAMAccountName,mail) FULLTEXT ENGINE LUCENE;
  create index OFunction.name on OFunction (Name) UNIQUE_HASH_INDEX;
  ALTER PROPERTY VSC.Seed DEFAULT "sequence('intranet_seed').next()";


  Get()
  st <= curdate 
  fn > curdate
  Get(-4)


        }
        
        ,{Db Orient script

  select from Quiz where State ='Published' and StartDate between '2017-12-06 00:00:00' and '2018-07-30 00:00:00'
  create vertex Quiz content {"StartDate":"2017-12-03 17:01:01","State":"Published","EndDate":"2018-12-04 01:01:01","QuizDescription":"test quiz with large finish date for publishing","Title":"Test quiz title2"}

        }
        
      }
    
    ]       

	public class CodeShemas
	{
				
	public Node reference shemas{
			
	Node reference shemas. Minimum and explicit[

		[Simplified overall
		()-[]->()
		Node type,reference types, reference types, directions, node type
		(Nd){-|=|..+}[Rf]-{<|>}(Nd)
		],	
		[Detailed Minimal possible 
		{
		(Node)-[Ref]->(Node)
		One type reference to same type Node
		}
		0->0
		],
		[Detailed Minimal explicit 
		(cannot be complexed with quality only with quantity of directions and types, which leads to pattern)
		{
		[NodeB]=[Ref2]=>NodeO)-[Ref1]->(NodeA)
		one type reference r1 from one type node O 
		to other type node A 
		and other type reference r2 to other type node B 
		with other direction
		}

		0
		-[r1]->A
		<=[r2]=B
		]

	]  
		  
	}
              
			  
	public EntityFrameworkClassShema{
		
		Tests{
	FunctionalTests{
	ProductivitiApi{
		DatabaseInitializationTests.cs
	}
	query{LinqToEntities{
		ContainsTests.cs{}
		}}
	}
	},
	EF{
	core{
	Metadata{
	Edm{
		PrimitiveTypeKind.cs{primitive types}
		,EntityType.cs{fundamental type }
		,ClrEntityType.cs{}
	}
	}
	}
	Query{
		InternalTrees{
		Node.cs
		,Command.cs
		}
		PlainCompiler{CodeGen.cs}
	}

	QueryableExtensions.cs{7763 GetMethod}
	DbContext.cs{

	}

	}

		IV:Miscellaneous ˌmɪs(ə)'leɪnɪəs
	Actual:
	TODO to JSON;
	Revise:
		equality, string, default null init, inheritance, events
	}

	}

]

#endregion