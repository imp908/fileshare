# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from django.db import models, migrations


class Migration(migrations.Migration):

    dependencies = [
    ]

    operations = [
        migrations.CreateModel(
            name='temp_table_dj_1',
            fields=[
                ('id', models.AutoField(verbose_name='ID', serialize=False, auto_created=True, primary_key=True)),
                ('idd', models.IntegerField()),
                ('insrt_date', models.DateTimeField(verbose_name=b'date inserted')),
                ('body_text', models.TextField()),
            ],
        ),
        migrations.CreateModel(
            name='temp_table_dj_2',
            fields=[
                ('id', models.AutoField(verbose_name='ID', serialize=False, auto_created=True, primary_key=True)),
                ('idd', models.IntegerField()),
                ('insrt_date', models.DateTimeField(verbose_name=b'date inserted')),
                ('body_text2', models.TextField()),
            ],
        ),
    ]
