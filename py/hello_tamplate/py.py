import web

render = web.template.render('templates/')

urls = (
  '/.*', 'Index'
)

app = web.application(urls, globals())



class Index(object):
    def GET(self):
        return render.hello()

    def POST(self):
        form = web.input(name="Nobody", greet="Hello")
        greeting = "%s, %s" % (form.greet, form.name)
        return render.index(greeting = greeting)

if __name__ == "__main__":
    app.run()