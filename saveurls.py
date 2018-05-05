import urllib.request
import os.path

file = "D:\\disk\\Files\\Progects\\PY_progects\\SaveUrls\\urls.txt"
folder = "D:\\disk\\Files\\git\\all\\";
plhSt = "\\\\\\\\\\\\\\\\\\\\\\"
plhFn = "///////////"
plhTs = ">>>>>>>>>>>"
plhFn = "<<<<<<<<<<<"
test=False
print(plhSt + "START")
with open(file,'r') as fileread:
	print("File to read:" + str(fileread))
	for line in fileread:
	
		print(plhTs)
		print(line)
		
		tokens = line.split("/")
		filename = tokens[len(tokens)-1]		
		giturl=tokens[5]
		folderPath=""					
		
		folderToWrite=""
		finalPath=""
		
		for tkn in range(6,len(tokens)-1):
			folderPath += tokens[tkn]+"\\" 			
		
		if test:
			print(folder + filename)		
			print("len(tokens): ",len(tokens))
			print("giturl: ",giturl)
			print("folderPath: ",folderPath)
		
		print("final folder: ",folder + folderPath + filename.rstrip())
		
		folderToWrite=folder + folderPath;
		
		if not os.path.exists(folderToWrite):
			os.makedirs(folderToWrite)
		
		finalPath=folder + folderPath + filename.rstrip();
		
		resp = urllib.request.urlopen(line)
		data = resp.read()
		fw = open(finalPath,'wb')
		fw.write(data)
		fw.close()
		
		print(plhFn)
			
	fileread.closed
True
print(plhFn + "FINISH")