using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace WEBtest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            textBox1.Text = "URL";

        }

        private void button1_Click(object sender, EventArgs e)
        {
            getdata();
        }

        async void getdata(){

            //ダウンロード元のURL
            string url = textBox1.Text;
            richTextBox1.Text = "";

            string source;

            //WebClientを作成
            using (var wc = new System.Net.WebClient())
            {
                //文字コードを指定
                wc.Encoding = System.Text.Encoding.UTF8;
                //データを文字列としてダウンロードする
                source = wc.DownloadString(url);
                //後始末
                wc.Dispose();
            }
            //richTextBox1.Text = source;

            // パースして配列の空の部分を削除
            string[] parse = source.Split('\n').Where(x => x != "").ToArray();

            string[] parse2 = parse.Where(x => x.Contains(".png") || x.Contains(".jpg")).ToArray();

            string sampleStr = @"<title>逆引きサンプル コード</title>";
            string patternStr = @"http[s]?://(?:[a-zA-Z]|[0-9]|[$-_@.&+]|[!*\(\),]|(?:%[0-9a-fA-F][0-9a-fA-F]))+";
            string newStr = Regex.Replace(sampleStr, patternStr, string.Empty);

            for (int i = 0; i < parse2.Length; i++)
            {
                //parse2[i] = Regex.Matches(parse2[i], patternStr, System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Singleline);
            }

            MatchCollection mc = Regex.Matches(source, patternStr, System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Singleline);

            var aa = mc.Cast<Match>()
                        .Select(m => m.Value)
                        .Where(x => x.Contains(".png") || x.Contains(".jpg"))
                        .ToArray();

            //var aa = mc.OfType<string>

            //.Where(x => x.Contains(".png") || x.Contains(".jpg")).ToArray();

            // タグの除去
            Regex r = new Regex("<.*?>", RegexOptions.Singleline);
            foreach (var mm in aa)
            {
                string m = r.Replace(mm, "");

                //正規表現に一致したグループを表示
                richTextBox1.AppendText(m.ToString() + "\n");
                //richTextBox1.AppendText("テキスト:{0}", m.Groups["text"].Value);
            }

            System.IO.Directory.CreateDirectory(@".\temp");
            //using (var wc = new System.Net.WebClient())
            {
                foreach (var mm in aa)
                {
                    try
                    {
                        int cnt = 0;
                         string m = r.Replace(mm, "");
                        string file = System.IO.Path.GetFileName(m);
                        while (System.IO.File.Exists(@".\temp\" + file))
                        {
                            file = System.IO.Path.GetFileNameWithoutExtension(m) + "_" + cnt.ToString() + System.IO.Path.GetExtension(m);
                            cnt++;

                        }

                            using (var wc = new System.Net.WebClient())
                            {
                                wc.DownloadFileCompleted += check;
                                wc.QueryString.Add("file", file); // here you can add values
                                //wc.DownloadFile(new UriBuilder(m).Uri, @".\temp\" + file);
                                wc.DownloadFileAsync(new UriBuilder(m).Uri, @".\temp\" + file);
                                wc.Dispose();
                            }

                            //if (new System.IO.FileInfo(@".\temp\" + file).Length <= 50000)
                            //{
                            //    System.IO.File.Open(@".\temp\" + file,System.IO.FileMode.Append).Close();
                            //    System.IO.File.Delete(@".\temp\" + file);
                            //}
                            

                    }
                    catch (Exception e)
                    {
                        richTextBox1.Text += "error\n";
                    };
                }
            }



            //string[] parse3 = parse2.

        
        }

        void check(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            string file= ((System.Net.WebClient)(sender)).QueryString["file"];

            try
            {
                if (new System.IO.FileInfo(@".\temp\" + file).Length <= 30000)
                {
                    System.IO.File.Delete(@".\temp\" + file);
                }

            }
            catch (Exception)
            {
                
                //throw;
            }

                            //if (new System.IO.FileInfo(@".\temp\" + file).Length <= 50000)
                            //{
                            //    System.IO.File.Delete(@".\temp\" + file);
                            //}
            
            if (e.Error != null)
            {
                richTextBox1.Text += "ok\n";
                return;

            }
        }

    }
}
