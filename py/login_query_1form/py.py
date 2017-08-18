import web
import MySQLdb
from web import form

 

render = web.template.render('templates/')

urls = (
  '/', 'Index'
  ,'/hello','Hello'
  ,'/query*','Query'
)

app = web.application(urls, globals())

query = form.Form(
	form.Textbox("SQLquery",
	form.notnull, class_ = "texttoEnter"
	),
	form.Button('Execute')
)

query2 = form.Form(
	form.Textarea("SQLquery2",
	form.notnull, class_ = "texttoEnter",description="enter your sql request",id="queryField"
	),
	form.Button('Execute')
)

class ConnectStr:
	host="10.14.16.48"
	port=3306
	db="temp_db"
	user="test_user2"
	passwd="QwErT234"


class Index(object):
    def GET(self):
        return render.index()
		
'''
no need as doubles post block from hello
    def POST(self):
        form = web.input(name="Nobody", greet="Hello")
        greeting = "%s, %s" % (form.greet, form.name)
        return render.hello(greeting = greeting)
'''

class Hello(object):
	def POST(self):
		form = web.input(Login="Nobody", Password="Hello")
		greeting = "%s, %s" % (form.Login, form.Password)
		f=query()
		
		try:
			db=MySQLdb.connect(
			host=ConnectStr.host,
			port=ConnectStr.port,
			db=ConnectStr.db,
			user="test_user2",
			passwd="QwErT234"
			)
			
			cursor = db.cursor()        
			cursor.execute("SELECT VERSION()")
			results = cursor.fetchone()
			if results:				
				return render.hello(greeting,f,ConnectStr)
			else :
				greeting = "%s, %s" % ("fake", "page")
				return render.hello(greeting,f,ConnectStr)
		except MySQLdb.Error:
			print "ERROR IN CONNECTION"
			greeting = "%s, %s" % ("fake2", "page2")
			return render.hello(greeting,f,ConnectStr)

		
class Query(object):
			
	def GET(self):
		f=query()
		
		db=MySQLdb.connect(
		host=ConnectStr.host,
		port=ConnectStr.port,
		db=ConnectStr.db,
		user="test_user2",
		passwd="QwErT234"
		)
				
		cursor = db.cursor()
		'''ipt = web.input('main')'''
		'''cursor.execute(f.d.SQLquery)'''
		qr = web.input()
		qrtext = qr.message
		cursor.execute(qrtext)
		data = cursor.fetchall()
		'''f['SQLquery'].value'''
		'''"select * from temp_table"'''
		return render.query(f,ConnectStr,data,cursor)
	
	
		
'''
defines if standalone Hello page  is needed
def GET(self):	
greeting = "%s, %s" % ('aaa','bbb')
return render.hello(greeting = greeting)
'''
		
if __name__ == "__main__":
    app.run()