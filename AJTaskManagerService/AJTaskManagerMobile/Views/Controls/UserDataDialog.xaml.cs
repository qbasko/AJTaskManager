using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Pickers.Provider;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using AJTaskManagerMobile.Common;

// The Content Dialog item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace AJTaskManagerMobile.Views.Controls
{
    public sealed partial class UserDataDialog : BindablePage
    {
        public UserDataDialog()
        {
            this.InitializeComponent();
        }
    }
}
