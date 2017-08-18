import web
from web import form

urls = (
  '/', 'Index'
)

app = web.application(urls, globals())

render = web.template.render('templates/')

login = form.Form(
	form.Textbox('username'),
	form.Password('password'),
	form.Button('Login'),
)


class Index:
	def GET(self):
		f=login()
		return render.index2(f)
		
	def POST(self):
		f=login()
		return "Validate insert"
		
		
if __name__ == "__main__":
    app.run()