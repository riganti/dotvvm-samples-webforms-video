using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotVVM.Framework.ViewModel;
using DotVVM.Framework.Hosting;

namespace Altairis.VtipBaze.WebCore.ViewModels
{
    public class TestViewModel : DotvvmViewModelBase
    {

        public int Number1 { get; set; }

        public int Number2 { get; set; }

        public int Result { get; set; }

        public void Calculate()
        {
            Result = Number1 + Number2;
        }
    }
}

