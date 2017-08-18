import web
import MySQLdb
from web import form


#db = web.database(dbn="mysql", db="temp_db",user="test_user", pw="QwErT234")


db=MySQLdb.connect(
host="10.14.16.48",
port=3306,
user="test_user2",
passwd="QwErT234",
db="temp_db"
)

cur = db.cursor()
#cur.execute("select * from temp_table")
#SELECT DATABASE()
#SHOW TABLES;
cur.execute("select * from temp_table")
data = cur.fetchall() 


for row in cur.description :
	print row[0]

urls = (
  '/', 'Index'
)

app = web.application(urls, globals())

render = web.template.render('templates/')

#sends form to template to render
login = form.Form(
	form.Textbox("username",
	form.notnull, class_ = "texttoEnter",description="enter your name",id="nameID"
	),
	form.Password('password',
	form.notnull, class_ = "texttoEnter",description="enter your password",id="passID"
	),
	form.Button('Login'),
)

class Index:
	def GET(self):
		greeting = "Hello World"
		f=login()
		d=data		
		c=cur
		return render.index(greeting,f,d,c)
		
	def POST(self):
		greeting = "Hello " + login.textbox("username")
		f=login()
		return render.index(greeting,f,d,c)
		

if __name__ == "__main__":
    app.run()
	