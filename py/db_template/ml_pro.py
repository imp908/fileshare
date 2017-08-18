import smtplib
from email.mime.text import MIMEText
 
# Now construct the mail msg
HOST = 'localhost'
message = MIMEText()
FROM = 'ia-neprintsev@rsb.ru'
COMMMASPACE = ', '
TO = ['ia-neprintsev@rsb.ru']
CC = ['ia-neprintsev@rsb.ru']


message["From"] = "dila...@iitb.ac.in"
message["To"] = unicode(TO)
message["Subject"] = "Attached files are very similar. Meet your instructor!"
message["Date"] = formatdate(localtime=True)


f = open('test.txt', 'r')
text = f.read()
text = text + msg
text = text +'\n--\n' \
+'\nThis email is system-generated. You need not reply.' 


message.attach(MIMEText(text))
# attach a file
part = MIMEBase('application', "octet-stream")
part.set_payload( open(tarfile_name,"rb").read() )
Encoders.encode_base64(part)
part.add_header('Content-Disposition', 'attachment; filename="%s"'\
% os.path.basename(tarfile_name))
message.attach(part)

# get username and password. Or hardcode it.
username = os.getenv('ia-neprintsev')
password = os.getenv('sedrftFTDRSE234')


server = smtplib.SMTP(HOST, 25)
server.starttls()
server.set_debuglevel(2)
server.login(username, password) # optional
try:
	print 'Sending email.'
	TO = TO + CC
	failed = server.sendmail(FROM, TO, message.as_string())
	server.close()
except Exception, e: 
	errorMsg = "Unable to send email. Error: %s" % str(e) 
	