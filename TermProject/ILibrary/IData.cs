using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILibrary
{
    public interface IData
    {
        string URL { get; set; }
        IList<string> ImageData();
    }
}
