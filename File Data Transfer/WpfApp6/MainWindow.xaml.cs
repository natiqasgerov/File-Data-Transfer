using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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

namespace WpfApp6
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public OpenFileDialog OpenFileDialog { get; set; }
        public Thread thread { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            progress.Value = 0;
            
        }

        

        private void fromOpen_btn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog = new OpenFileDialog();
            OpenFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            OpenFileDialog.ShowDialog();
            from_textblock.Text = OpenFileDialog.FileName;        
        }

        private void toOpen_btn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog = new OpenFileDialog();
            OpenFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            OpenFileDialog.ShowDialog();
            to_textblock.Text = OpenFileDialog.FileName;
        }

        private void start_btn_Click(object sender, RoutedEventArgs e)
        {
            if (from_textblock.Text != String.Empty && to_textblock.Text != String.Empty) {
                string file1 = from_textblock.Text;
                string text1 = File.ReadAllText(file1);
                start_btn.IsEnabled = false;
                string file2 = to_textblock.Text;
                string text2 = String.Empty;
                if (text1.Length != 0)
                {
                    progress.Maximum = text1.Length;
                    thread = new Thread(() => MyWork(ref text1, ref text2, file2));
                    thread.Start();
                }
                else
                    MessageBox.Show("From File is EMPTY");
                
            }
        }


        private void MyWork(ref string text1,ref string text2,string file2)
        {
            for (int i = 0; i < text1.Length; i++)
            {
                text2 += text1[i];
                Dispatcher.InvokeAsync(() => progress.Value += 1);             
                Thread.Sleep(70);
            }
            File.AppendAllText(file2, text2);
            Dispatcher.InvokeAsync(() => start_btn.IsEnabled = true);
            Dispatcher.Invoke(() => progress.Value = 0);
            Dispatcher.Invoke(() => from_textblock.Text = null) ;
            Dispatcher.Invoke(() => to_textblock.Text = null);
            thread.Abort();
            
        }

        private void resume_btn_Click(object sender, RoutedEventArgs e)
        {
            if(thread!=null && thread.ThreadState == ThreadState.Suspended)
                thread.Resume();     
            
        }

        private void suspend_btn_Click(object sender, RoutedEventArgs e)
        {
            if(thread!=null && thread.IsAlive == true)
                thread.Suspend();                     
        }

        
    }
}
