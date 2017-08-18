# Import smtplib for the actual sending function
import smtplib

# Import the email modules we'll need
from email.mime.text import MIMEText

# Open a plain text file for reading.  For this example, assume that
# the text file contains only ASCII characters.
with open('test.txt') as fp:
    # Create a text/plain message
    msg = MIMEText(fp.read())

# me == the sender's email address
# you == the recipient's email address
msg['Subject'] = 'The contents of %s' % 'textfile'
msg['From'] = 'ia-neprintsev@rsb.ru'
msg['To'] = 'ia-neprintsev@rsb.ru'

# get username and password. Or hardcode it.
username = 'ia-neprintsev'
password = 'sedrftFTDRSE234'

# Send the message via our own SMTP server.
s = smtplib.SMTP('127.0.0.1', 1026)
s.login(username, password)
s.sendmail('ia-neprintsev@rsb.ru', ['ia-neprintsev@rsb.ru'], msg.as_string())
s.quit()
