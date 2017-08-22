using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WcfService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IWCF2" in both code and config file together.
    [ServiceContract]
    public interface IWCF2
    {
        [OperationContract]
        void SetCurrentUser(int id);
        [OperationContract]
        List<Model.SQLmodel.KEY_CLIENTS_SQL> GetKKByUserId();
    }
   
}
