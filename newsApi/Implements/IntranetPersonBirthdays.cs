using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using NewsAPI.Interfaces;
using NewsAPI.Helpers;
using System.Threading;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Configuration;

namespace NewsAPI.Implements
{
    public class IntranetPersonBirthdays : IPersonBirhtdays
    {
        public IHttpActionResult GetActualPersonBirthdays()
        {
            var query = "select GUID as id, ifnull( if( eval(\"@class = 'Person'\"),in(\"MainAssignment\").Name[0],Name[0]) ,'0' ) as division , ifnull( if( eval(\"@class = 'Person'\"),in(\"MainAssignment\").GUID[0],PGUID[0]) ,'0' ) as parent, ifnull(telephoneNumber, ' ') as phone, ifnull( mail,'&nbsp')as mail, Name as title, Name as label, LastName.append(' ').append(FirstName.substring( 0, 1 )).append('. ').append(MiddleName.substring( 0, 1 )).append('.') as shortFName, ifnull((inE('MainAssignment').Name[0]), Name) as description , GetDepartmentColor(GUID)[color][0] as itemTitleColor , if( eval(\"@class = 'Person'\"),'PersonTemplate','UnitTemplate' ) as templateName , if(eval('inE().ExpDate[0] is not null'), inE().State[0],null)  as state,  if( eval('inE().ExpDate[0] is not null'),inE().ExpDate[0].format('dd.MM.YYYY'),null) as expDate, GetPositionBar(InE().Name[0].replace('\\\"',''))['groupTitle'][0] as groupTitle  , Birthday.format('dd.MM') as birthday , GetDepartmentColorClass(GUID)[colorClass][0] as colorClass, GetDepartmentName(GUID)[departmentName][0] as departmentName, sAMAccountName as login from Person WHERE ((in(\"MainAssignment\")[0].Disabled is null or in(\"MainAssignment\")[0].Disabled >= sysdate()) and inE(\"MainAssignment\").size() != 0 ) and (inE(\"MainAssignment\")[0].Disabled is null) and (Disabled is null) and (inE(\"MainAssignment\").State != 'Отпуск по уходу за ребенком' and inE(\"MainAssignment\").State != 'Отпуск по беременности и родам') and (sAMAccountName is not null) and (out (\"CommonSettings\")[0].showBirthday = true) and (Birthday.format('MM') * 30 + Birthday.format('dd') <= DATE().format('MM') * 30 + 14 + DATE().format('dd')) and (Birthday.format('MM') * 30 + Birthday.format('dd') >= DATE().format('MM') * 30 + DATE().format('dd'))";

            string batch = OrientBatchBuilder.CreateBatch(query);

            return new OrientDB_HttpManager.PostBatch(
                ConfigurationManager.AppSettings["orient_batch_host"],
                batch
                );

        }
    }
}