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
            var f = Parser.Parse("177");
            Parser.Functions["Hii"] = f.AsFunctionInfo();
            lv.ItemsSource = Parser.Functions.Keys;

        }

        private class test : IFunctionInfo
        {
            public IReadOnlyCollection<int> PreferedParameterCount
            {
                get;
            } = new List<int>() { 1 };

            public Tuple<object, MethodInfo> GetExecutable(int parameterCount)
            {
                if(parameterCount != 1)
                    return null;
                return Tuple.Create((object)this, typeof(test).GetMethod("hellO", BindingFlags.NonPublic | BindingFlags.Instance));
            }

            private double hellO(double v)
            {
                return v * v;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var r = Parser.Parse0(box.Text);
                block.Text = $"{r.Formatted} = {r.Compiled.Invoke()}";
            }
            catch(Exception ex)
            {
                block.Text = $"{ex.GetType()}\n{ex.Message}";
            }
        }
    }
}
