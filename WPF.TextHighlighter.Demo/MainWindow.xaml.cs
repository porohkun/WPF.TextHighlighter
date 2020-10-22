using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF.TextHighlighter.Demo
{
    public class MainWindowViewModel
    {
        public string ExampleText { get; set; } = "System.NotImplementedException: Метод или операция не реализована.\n   в MyNLog.Services.LogFileService.OpenFile(String fileName) в C:\\Projects\\Desktop\\MyNLog\\MyNLog\\Services\\LogFileService.cs:строка 14\n   в MyNLog.Commands.OpenLogFileCommand.ExecuteInternal(Object parameter) в C:\\Projects\\Desktop\\MyNLog\\MyNLog\\Commands\\OpenLogFileCommand.cs:строка 32";
    }

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
    }
}
