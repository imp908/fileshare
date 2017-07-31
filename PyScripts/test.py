#from urllib.request import urlopen
import urllib.request
import shutil
url = "https://github.com/imp908/Fileshare/raw/master/CODE_TEMPLATES.cs"
file = "C:\\TEMP\\CT.cs"
s=[1, 2, 3, 4,5,6,7,8,9,10,11,12,13,14,15]
#print(x/(2**x))
b=sum((x/(2**x)) for x in s)
print(b)
print(7000000000/100/100)
resp = urllib.request.urlopen(url)
data = resp.read()
file_write = open(file,'wb')
file_write.write(data)
text = data.decode('utf-8')
#shutil.copyfileobj(resp,file_write)
import os
os.system('G:')
os.system('cd G:\\disk\\Files\\git\\test')
'''
with urllib.request.urlopen(url) as response, open(file,'wb') as file_write:
	shutil.copyfileobj(response,file_write)
'''
