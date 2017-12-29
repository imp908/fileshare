﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quartz;
using Quartz.Impl;

namespace NewsAPI.Jobs
{
    public class DailyBirthdayNotificationScheduler
    {
        public static void Start()
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();

            IJobDetail job = JobBuilder.Create<DailyBirthdayNotificationSender>().Build();

            //ITrigger trigger = TriggerBuilder.Create()  // создаем триггер
            //    .WithIdentity("trigger1", "group1")     // идентифицируем триггер с именем и группой
            //    .StartNow()                            // запуск сразу после начала выполнения
            //    .WithSimpleSchedule(x => x            // настраиваем выполнение действия
            //        .WithIntervalInMinutes(1)          // через 1 минуту
            //        .RepeatForever())                   // бесконечное повторение
            //    .Build();                               // создаем триггер

            ITrigger trigger = TriggerBuilder.Create()  // создаем триггер
                .WithIdentity("trigger1", "group1")     // идентифицируем триггер с именем и группой
               .WithCronSchedule("0 40 19 ? * *")
                
                .Build();                               // создаем триггер

            scheduler.ScheduleJob(job, trigger);        // начинаем выполнение работы
        }
    }
}