import web
import MySQLdb



db=MySQLdb.connect(
host="10.14.16.48",
port=3306,
user="test_user2",
passwd="QwErT234",
db="temp_db"
)

cur = db.cursor()
cur.execute("SHOW TABLES")

def get_todos():
	return
	#cur.execute("select * from temp_table")
	#SELECT DATABASE()
	#SHOW TABLES;	
	data = cur.fetchall()
	
