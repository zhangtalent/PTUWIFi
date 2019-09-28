using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public string u = "";
        public string p = "";
        public string name = "";
        public string ti = "";
        public string url_canSurface = "http://192.168.116.8/1.htm";
        public string url_limit = "http://192.168.116.8/usermsg?url=1";
        public string url_connect = "";
        System.Timers.Timer timer = new System.Timers.Timer();
        public MainWindow()
        {
            InitializeComponent();
            if (System.IO.File.Exists(@"D:\ptu_wifi_data.txt"))
            {
                string[] line_all = System.IO.File.ReadAllLines(@"D:\ptu_wifi_data.txt");
                if (line_all.Length > 3)
                {
                    username_box.Text = line_all[0];
                    password_box.Text = line_all[1];
                    name_box.Text = line_all[2];
                    time_box.Text = line_all[3];
                }
                
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            u = username_box.Text;
            p = password_box.Text;
            name = name_box.Text;
            ti = time_box.Text;
            url_connect = "http://192.168.116.8/drcom/login?callback=dr1567554170962&DDDDD="+u+"&upass="+p+"&0MKKey=123456&R1=0&R3=0&R6=0&para=00&v6ip=&_=1567554113391";
            string[] lines = { u, p, name, "dd" };
            //using (FileStream fsWrite = new FileStream(@"D:\ptu_wifi_data.txt", FileMode.OpenOrCreate, FileAccess.Write, FileShare.None)) ;
            //System.IO.File.WriteAllLines(@"C:\ptu_wifi_data.txt", lines);
            string data_1 = GetHttp(url_limit);
            Console.WriteLine(data_1);
            //byte[] buffer = Encoding.GetEncoding("gb2312").GetBytes(data_1);
            //byte[] buffer = Encoding.GetEncoding("GBK").GetBytes(txt);
            //data_1 = gb2312_utf8(data_1);
            //data_1 = Encoding..GetString(Encoding.GetEncoding("gb2312").GetBytes(data_1));
            //System.IO.File.WriteAllText(@"D:\ptu_wifi.txt", data_1);
            //Console.WriteLine(data_1);

            fisrt_run();


        }
        public static string gb2312_utf8(string text)
        {
            //声明字符集   
            System.Text.Encoding utf8, gb2312;
            //gb2312   
            gb2312 = System.Text.Encoding.GetEncoding("gb2312");
            //utf8   
            utf8 = System.Text.Encoding.Default;
            byte[] gb;
            gb = gb2312.GetBytes(text);
            gb = System.Text.Encoding.Convert(gb2312, utf8, gb);
            //返回转换后的字符   
            return utf8.GetString(gb);
        }
        private void check(object source, ElapsedEventArgs e)
        {
            string data = GetHttp(url_canSurface);
            int ri = data.IndexOf("注销页");
            //可以上外网
            if (ri > 0)
            {
                string d = "";
                string data_1 = GetHttp(url_limit);
                Console.WriteLine(data_1);
                int ri_1 = data_1.IndexOf("2分钟");
                if (ri_1 > 0)
                {
                    string data_2 = GetHttp(url_connect);
                    d = "状态：自动重连,消除检测-时间" + DateTime.Now.ToLongTimeString().ToString();
                }
                else
                {
                    d = "状态：正常联网-时间"+ DateTime.Now.ToLongTimeString().ToString(); 
                }
                this.result.Dispatcher.Invoke(
                   new Action(
                        delegate
                        {
                            result.Text = d;
                        }
                   )
                );
            }
            else
            {
                string h = "";
                string data_3 = GetHttp(url_connect);
                int ri_3 = data_3.IndexOf(name);
                if (ri_3 > 0)
                {
                    h = "状态：重连成功-时间" + DateTime.Now.ToLongTimeString().ToString();

                }
                else
                {
                    h = "状态：正在重连-时间" + DateTime.Now.ToLongTimeString().ToString();
                }
                this.result.Dispatcher.Invoke(
                   new Action(
                        delegate
                        {
                            result.Text = h;
                        }
                   )
                );


            }
            //Console.WriteLine(ri);
            
            
        }

        private void fisrt_run()
        {
            string data = GetHttp(url_canSurface);
            int ri = data.IndexOf("注销页");
            //可以上外网
            if (ri > 0)
            {
                string d = "";
                
                string data_1 = GetHttp(url_limit);
                int ri_1 = data_1.IndexOf("2分钟");
                if (ri_1 > 0)
                {
                    string data_2 = GetHttp(url_connect);
                    d = "状态：自动重连,消除检测-时间" + DateTime.Now.ToLongTimeString().ToString();
                }
                else
                {
                    //d = "状态：成功运行脚本-时间" + DateTime.Now.ToLongTimeString().ToString();
                    if (timer != null)
                    {
                        int i = int.Parse(ti);
                        timer.Enabled = true;
                        timer.Interval = i * 1000; //执行间隔时间,单位为毫秒; 这里实际间隔为10分钟  
                        timer.Start();
                        timer.Elapsed += new System.Timers.ElapsedEventHandler(check);

                        

                        string[] lines = { u, p, name, ti };
                        if (System.IO.File.Exists(@"D:\ptu_wifi_data.txt"))
                        {
                            System.IO.File.WriteAllLines(@"D:\ptu_wifi_data.txt", lines); 
                        }
                        else
                        {
                            //MessageBox.Show("Nothing。。。");
                            //System.IO.File.Create(@"D:\ptu_wifi_data.txt");
                            System.IO.File.WriteAllLines(@"D:\ptu_wifi_data.txt", lines); ;

                        }
                        
                        d = "状态：成功运行脚本-时间" + DateTime.Now.ToLongTimeString().ToString();
                    }
                    else
                    {
                        d = "状态：启动失败，请重启软件-时间" + DateTime.Now.ToLongTimeString().ToString();
                        MessageBox.Show("启动失败，请重启软件");
                    }
                }
                
                result.Text = d;
                
            }
            else
            {
                string h = "";
                string data_3 = GetHttp(url_connect);
                //Console.WriteLine(url_connect);
                int ri_3 = data_3.IndexOf(name);
                if (ri_3 > 0)
                {
                    if (timer != null)
                    {
                        int i = int.Parse(ti);
                        timer.Enabled = true;
                        timer.Interval = i * 1000; //执行间隔时间,单位为毫秒; 这里实际间隔为10分钟  
                        timer.Start();
                        timer.Elapsed += new System.Timers.ElapsedEventHandler(check);
                        string[] lines = { u, p, name,ti };
                        if (System.IO.File.Exists(@"D:\ptu_wifi_data.txt"))
                        {
                            System.IO.File.Delete(@"D:\ptu_wifi_data.txt");
                        }
                        else
                        {
                            System.IO.File.Create(@"D:\ptu_wifi_data.txt");
                        }
                        System.IO.File.WriteAllLines(@"D:\ptu_wifi_data.txt", lines);
                        h = "状态：成功运行脚本-时间" + DateTime.Now.ToLongTimeString().ToString();
                    }
                    else
                    {
                        
                        h = "状态：启动失败，请重启软件-时间" + DateTime.Now.ToLongTimeString().ToString();
                        MessageBox.Show("启动失败，请重启软件");
                    }

                }
                else
                {
                    h = "状态：考虑学号密码等信息有误-时间" + DateTime.Now.ToLongTimeString().ToString();
                    MessageBox.Show("启动失败->学号密码等信息有误");
                }
                
                result.Text = h;
                    


            }
            Console.WriteLine(ri);


        }


        public static string GetHttp(string url)
        {


            string responseContent = "No";
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);

                //httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "GET";
                httpWebRequest.Timeout = 2000;
                httpWebRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/77.0.3865.90 Safari/537.36";
                //byte[] btBodys = Encoding.UTF8.GetBytes(body);
                //httpWebRequest.ContentLength = btBodys.Length;
                //httpWebRequest.GetRequestStream().Write(btBodys, 0, btBodys.Length);

                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.Default);
                responseContent = streamReader.ReadToEnd();

                httpWebResponse.Close();
                streamReader.Close();
            }catch(Exception e)
            {

            }
            return responseContent;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (timer != null) { timer.Stop(); }
        }
    }
}
