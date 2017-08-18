from django.db import models

# Create your models here.
class temp_table_dj_1(models.Model):
	idd = models.IntegerField()
	insrt_date = models.DateTimeField('date inserted')
	body_text = models.TextField()
	
	def __str__(self):              # __unicode__ on Python 1
		return self.body_text
	
class temp_table_dj_2(models.Model):
	idd = models.IntegerField()
	insrt_date = models.DateTimeField('date inserted')
	body_text2 = models.TextField()
	
	def __str__(self):              # __unicode__ on Python 2
		return self.body_text2