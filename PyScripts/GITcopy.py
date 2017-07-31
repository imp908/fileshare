#from urllib.request import urlopen
import urllib.request
import shutil
url = "https://github.com/imp908/Fileshare/raw/master/CODE_TEMPLATES.cs"
file = "C:\\TEMP\\CT.cs"
resp = urllib.request.urlopen(url)
data = resp.read()
file_write = open(file,'wb')
file_write.write(data)
text = data.decode('utf-8')
#shutil.copyfileobj(resp,file_write)

'''
with urllib.request.urlopen(url) as response, open(file,'wb') as file_write:
	shutil.copyfileobj(response,file_write)
'''
