using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using MathExpression;
using System.Reflection;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace Caculator
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            var f = Parser.Parse("sin(x)^2");
            Parser.Functions["Sin2"] = f;
            records.Add(new Record());
            var p = Windows.Storage.ApplicationData.Current.LocalFolder.Path;
        }

        private ObservableCollection<Record> records = new ObservableCollection<Record>();

        private void box_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            var data = (Record)sender.Tag;
            try
            {
                var r = Parser.Parse(data.Input);
                data.Input = r.Formatted;
                data.Output = $"Ans = {r.Compiled.DynamicInvoke()}";
            }
            catch(Exception ex)
            {
                data.Output = $"{ex.GetType()}\n{ex.Message}";
            }
            records.Add(new Record());
        }

        private async void box_Loaded(object sender, RoutedEventArgs e)
        {
            ((Control)sender).Focus(FocusState.Programmatic);
            await Task.Delay(50);
            ((Control)sender).Focus(FocusState.Programmatic);
            lv.ScrollIntoView(records.Last());
        }
    }
}
