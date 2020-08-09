using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ATM
{
    public interface IMediator
    {
        void Notify(object page, String _event);
    }
}
