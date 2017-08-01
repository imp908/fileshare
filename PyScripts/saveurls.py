import urllib.request
file = "C:\\TEMP\\GETgit\\urls.txt"
folder = "C:\TEMP\GIT\\all";
print("START--------------------------")
with open(file,'r') as file_read:
	print("File to read" + str(file_read))
	for line in file_read:
		print( "Url:" + str(line))
		tokens = line.split("/")
		filename = tokens[len(tokens)-1]
		filewrite = folder + "\\" + filename.rstrip()	
		print( "File:" + str(filewrite))
		resp = urllib.request.urlopen(line)	
		data = resp.read()
		print("resp readed")
		file_write = open(filewrite,'wb')
		print("write openned")
		file_write.write(data)
		file_write.close()
		print("write finished")
	#resp = urllib.request.urlopen(line)
	file_read.closed
True
print("FINISHED--------------------------")
'''
with urllib.request.urlopen(url) as response, open(file,'wb') as file_write:
	shutil.copyfileobj(response,file_write)
'''
