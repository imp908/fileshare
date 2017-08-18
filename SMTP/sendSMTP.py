import smtplib
from email.mime.text import MIMEText
from email.mime.multipart import MIMEMultipart

address_book = ['ia-neprintsev@rsb.ru']
msg = MIMEMultipart()    
sender = 'ia-neprintsev@rsb.ru'
subject = "My subject"
body = "This is my email body"

msg['From'] = sender
msg['To'] = ','.join(address_book)
msg['Subject'] = subject
msg.attach(MIMEText(body, 'plain'))
text=msg.as_string()
#print text
# Send the message via our SMTP server
s = smtplib.SMTP('EX-MB-05.rs.ru',8080)
s.sendmail(sender,address_book, text)
s.quit()        