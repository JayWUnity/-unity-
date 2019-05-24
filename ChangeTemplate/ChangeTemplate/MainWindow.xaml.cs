using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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

namespace ChangeTemplate
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        static string ConfigPath=@"C:\\Users\\Unity\\Desktop\\Config.txt";
        static string PathConfigPath = @"C:\\Users\\Unity\\Desktop\\Config.txt";
        static string HaveNameSpacePath = @"C:\\Users\\Unity\\Desktop\\HaveNPTemplate.txt";
        static string NoNameSpacePath = @"C:\\Users\\Unity\\Desktop\\NoNPTemplate.txt";
        static string TemplatePath = @"C:\\Users\\Unity\\Desktop\\TemplateScrip.txt";

        public MainWindow()
        {
            InitializeComponent();
            
            
            string path = Environment.CurrentDirectory;
            ConfigPath = path + "\\Config.txt";
            PathConfigPath = path + "\\PathConfig.txt";
            HaveNameSpacePath= path + "\\HaveNPTemplate.txt";
            NoNameSpacePath= path + "\\NoNPTemplate.txt";

            if (File.Exists(ConfigPath))
            {
                string[] str = File.ReadAllLines(ConfigPath);
                foreach (var s in str)
                {
                    comboBox.Items.Add(s);
                }

                comboBox.SelectedIndex = 0;
            }

            if (File.Exists(PathConfigPath))
            {
                string[] paths = File.ReadAllLines(PathConfigPath);
                TemplatePath = paths[0];
            }
            
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void 确认_Click(object sender, RoutedEventArgs e)
        {
            string name = comboBox.SelectedValue.ToString();
            string templateStr = "";

            if (name.Equals("None"))
            {
                StreamReader sr = new StreamReader(NoNameSpacePath);
                templateStr = sr.ReadToEnd();
            }
            else
            {
                StreamReader sr = new StreamReader(HaveNameSpacePath);
                templateStr = sr.ReadToEnd();
                Regex r = new Regex("(?<=(%)).*?(?=(%))", RegexOptions.IgnoreCase);

                templateStr = r.Replace(templateStr, name);
                templateStr = templateStr.Replace("%", "");
            }
            File.WriteAllText(TemplatePath, templateStr);

            Thread thread = new Thread(new ThreadStart(Tip));
            thread.Start();
        }

        void Tip()
        {
            this.LogText.Dispatcher.Invoke(new Action(() => { this.LogText.Content = "设置成功"; }));
            Thread.Sleep(1000);
            this.LogText.Dispatcher.Invoke(new Action(() => { this.LogText.Content = ""; }));
        }
       

    }
}
