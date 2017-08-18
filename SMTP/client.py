import smtplib
import email.utils
from email.mime.text import MIMEText

# Create the message
msg = MIMEText('This is the body of the message.')
msg['To'] = email.utils.formataddr(('Recipient', 'ia-neprintsev@rsb.ru'))
msg['From'] = email.utils.formataddr(('Author', 'ia-neprintsev@rsb.ru'))
msg['Subject'] = 'Simple test message'

# server = smtplib.SMTP('127.0.0.1', 1025)
server = smtplib.SMTP('web.bank.rs.ru', 1025)
server.set_debuglevel(True) # show communication with the server
try:
    server.sendmail('author@example.com', ['ia-neprintsev@rsb.ru'], msg.as_string())
finally:
    server.quit()