using System.Collections.Generic;

namespace Intranet.Models
{
    public interface IHelper<out T>
    {
        IEnumerable<T> Select();
        void Insert();
        void SendToArchive(string rid);
        void Update();

    }

}
