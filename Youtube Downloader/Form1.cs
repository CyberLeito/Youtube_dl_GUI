using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.IO.Compression;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Youtube_Downloader.Internals;
using Youtube_Downloader.Properties;


namespace Youtube_Downloader
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string SavePath;
        string YouTLink;
      
        private void textBox1_MouseClick(object sender, MouseEventArgs e)
        {
            button1.PerformClick();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrEmpty(fbd.SelectedPath))
                {
                    SavePath = fbd.SelectedPath;
                    textBox1.Text = SavePath;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            YouTLink = Clipboard.GetText();
            textBox2.Text = YouTLink;
            //LinkValidation();
        }
        public static string Between(string src, string findfrom,
                             params string[] findto)
        {
            int start = src.IndexOf(findfrom);
            if (start < 0) return "";
            foreach (string sto in findto)
            {
                int to = src.IndexOf(sto, start + findfrom.Length);
                if (to >= 0) return
                    src.Substring(
                               start + findfrom.Length,
                               to - start - findfrom.Length);
            }
            return "";
        }

        public static string Endfind(string src, string findfrom)
        {
            int start = src.IndexOf(findfrom);
            if (start < 0)
            {
                return "";
            }
            else
            {
                return src.Substring(
                                   start + findfrom.Length,
                                   src.Length - start - findfrom.Length);
            }
            
        }

        string masterget;
        public void getPlayLink(string Input)
        {
            
            string Link = Between(Input, "list=", "&");
            string Link2 = Between(Input, "list=", "#");
            
            //label3.Text = Endfind(textBox3.Text, "list=");
          
            if (!string.IsNullOrEmpty(Link))
            {
                //label4.Text = Link;
                masterget = Link;
            }

            else if (string.IsNullOrEmpty(Link))
            {
                 
                if (!string.IsNullOrEmpty(Link2))
                {
                    //label4.Text = Link2;
                    masterget = Link2;
                }
            }
           
        }

        string videolink1;
        public void getVideoLink(string Input)
        {
            string Link = Between(Input, "watch?v=", "&");
            string Link2 = Between(Input, "watch?v=", "#");

            if (!string.IsNullOrEmpty(Link))
            {
                //label4.Text = Link;
                videolink1 = Link;
                //MessageBox.Show("here1&");
            }

            else if (string.IsNullOrEmpty(Link))
            {
               // MessageBox.Show("empty1");

                if (!string.IsNullOrEmpty(Link2))
                {
                   // MessageBox.Show("here2#");
                    //label4.Text = Link2;
                    videolink1 = Link2;
                }
            }
        }
        string PlaylistH;
        string VideoH;
        string playLink;
        string vidLink;
        private void LinkValidation()
        {
            
            if (!string.IsNullOrEmpty(YouTLink))
            {
                if (YouTLink.Contains("www.youtube.com/watch?v=") || YouTLink.Contains("www.youtube.com/playlist?list="))
                {
                    label2.Text = "Valid";

                    if (YouTLink.Contains("list="))
                    {
                        //MessageBox.Show("playls");
                        label2.Text = "Playlist";
                        // string playLink = getPlayLink(YouTLink);
                        getPlayLink(YouTLink);
                        if (!string.IsNullOrEmpty(masterget))
                        {
                             playLink = masterget;
                        }
                        else
                        {
                            string endyou = Endfind(YouTLink, "list=");
                            if (!string.IsNullOrEmpty(endyou))
                            {
                                playLink = endyou;
                            }
                            else if (string.IsNullOrEmpty(endyou))
                            {
                                playLink = "Link Not found";
                            }
                        }
                            

                        
                        if (playLink.Contains("Link Not found"))
                        {
                            MessageBox.Show("Stooped at playlist");
                            label3.Text = playLink;
                        }
                        else
                        {
                            PlaylistH = "http://www.youtube.com/playlist?list=" + playLink;
                            label3.Text = PlaylistH;
                        }


                        if (YouTLink.Contains("list=") && YouTLink.Contains("watch?v="))
                        {//WATCH VIDEO AND PLAYLIST TOGETHER
                            checkBox1.Enabled = true;
                            checkBox2.Enabled = true;
                            checkBox1.Checked = true;

                        }
                    }
                    //end of playlist
                    if (!(YouTLink.Contains("list=")))
                    {//////++++++++++++++++++


                        IzzVideoPart();
                        //////++++++++++++++++++
                    }







                }
            }
        }


        public void IzzVideoPart()
        {
            label2.Text = "Video";
            //then its a video
            // MessageBox.Show("here");
            getVideoLink(YouTLink);
            //  MessageBox.Show("here2");


            if (!string.IsNullOrEmpty(videolink1))
            {
                vidLink = videolink1;
            }
            else
            {
                string endyou = Endfind(YouTLink, "watch?v=");
                if (!string.IsNullOrEmpty(endyou))
                {
                    vidLink = endyou;
                }
                else if (string.IsNullOrEmpty(endyou))
                {
                    vidLink = "Link Not found";
                }
            }

            if (vidLink.Contains("Link Not found"))
            {
                label3.Text = vidLink;
            }
            else
            {
                VideoH = "http://www.youtube.com/watch?v=" + vidLink;
                label3.Text = VideoH;
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            YouTLink = textBox2.Text;
            LinkValidation();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(label3.Text);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                label3.Text = PlaylistH;
                label2.Text = "Playlist";
                checkBox2.Checked = false;
            }
            else if (!checkBox1.Checked)
            {
                checkBox2.Checked = true;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                checkBox1.Checked = false;
                IzzVideoPart();
                label2.Text = "Video";
            }
            else if (!checkBox2.Checked)
            {
                checkBox1.Checked = true;
            }
        }

        public void Extract2()
        {
            string path = @"C:\ProgramData\WorkDir\Resources";
            if (!Directory.Exists(path))
            {
                DirectoryInfo di = Directory.CreateDirectory(path);
            }

            if (!File.Exists("C:\\ProgramData\\WorkDir\\Resources\\yout.zip"))
            {
                Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Youtube_Downloader.Resources.yout.zip");
                FileStream fileStream = new FileStream("C:\\ProgramData\\WorkDir\\Resources\\yout.zip", FileMode.CreateNew);
                for (int i = 0; i < stream.Length; i++)
                    fileStream.WriteByte((byte)stream.ReadByte());
                fileStream.Close();
            }
        }

        public void unZipp()
        { ///Tiny unzip helper class for .NET 3.5 Client Profile and Mono 2.10, written in pure C#
            //download from nuget
            try
            {
                using (var unzip = new Unzip(@"C:\ProgramData\WorkDir\Resources\yout.zip"))
                    unzip.ExtractToDirectory(@"C:\ProgramData\WorkDir\Resources");
            }
            catch
            {
                MessageBox.Show("Close fucking Windows Defender");
            }
        }


        string defRes = "480";
        string pathSav = @"C:%HOMEPATH%\Videos\YouTube";

        private void Form1_Load(object sender, EventArgs e)
        {
            //CHANGE DEFAULT RES AND SAVE PATH FROM CONFIG

            comboBox1.Text = defRes;
            string fullPath = Environment.ExpandEnvironmentVariables(pathSav);
            textBox1.Text = fullPath;

            string temp = Clipboard.GetText();
            if (temp.Contains("www.youtube.com/watch?v=") || temp.Contains("www.youtube.com/playlist?list="))
            {
                YouTLink = temp;
                textBox2.Text = YouTLink;
            }

            Extract2();
            unZipp();


        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(label3.Text))
            {
                //++====================================
                PathCreate(textBox1.Text); //creating the folder
                Script();
                COmmds();


                //++====================================
            }
            else
            {
                MessageBox.Show("You must enter a valid link");
            }
        }

        public void Script()
        {

            //string str = File.ReadAllText(@"C:\ProgramData\WorkDir\Resources\Blink\Blink.txt");
            //str = str.Replace("XXX", delay.ToString());
            string res = "";
            string lnk = label3.Text;
            if(string.Equals(comboBox1.Text, "480"))
            {
                res = " -f \"bestvideo[height<=480]+bestaudio/best[height<=480]\" ";
            }
            else if (string.Equals(comboBox1.Text, "360"))
            {
                res = " -f \"bestvideo[height<=360]+bestaudio/best[height<=360]\" ";
            }
            else if (string.Equals(comboBox1.Text, "720"))
            {
                res = " -f \"bestvideo[height<=720]+bestaudio/best[height<=720]\" ";
            }
            else if (string.Equals(comboBox1.Text, "1080"))
            {
                res = " -f \"bestvideo[height<=1080]+bestaudio/best[height<=1080]\" ";
            }
            else if (string.Equals(comboBox1.Text, "MAX"))
            {
                res = " -f \"bestvideo+bestaudio/best\" ";
            }


            string str = "youtube-dl -o \""+textBox1.Text+"\\%%(title)s.%%(ext)s\"";
            if (string.Equals(label2.Text, "Playlist"))
            {
                str = "youtube-dl -o \"" + textBox1.Text + "\\%%(playlist)s/%%(playlist_index)s - %%(title)s.%%(ext)s\"";
            }

            //string final = "@echo off \r\n"+ str + res + lnk + "\r\ntimeout /t 2 \r\n" + str + res + lnk + "\r\ntimeout /t -1";
            string final = "@echo off \r\ncd \"C:\\ProgramData\\WorkDir\\Resources\\\"\r\n" + str + res + lnk +  "\r\ntimeout /t -1";

            // MessageBox.Show(final);
            File.WriteAllText(@"C:\ProgramData\WorkDir\Resources\Script.bat", final);

        }


        public void COmmds()
        {
            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            proc.StartInfo.FileName = "C:\\ProgramData\\WorkDir\\Resources\\Script.bat";
            proc.StartInfo.WorkingDirectory = "C:\\ProgramData\\WorkDir\\Resources\\";
            proc.StartInfo.RedirectStandardError = false;
            proc.StartInfo.Verb = "runas";
            //proc.StartInfo.RedirectStandardOutput = true;
            //proc.StartInfo.UseShellExecute = false;
            //proc.StartInfo.CreateNoWindow = true;
            proc.Start();
        }

        public void PathCreate(string path)
        {
            //string path = @"%HOMEPATH%\Videos\YouTube";
            try
            {
                if (!Directory.Exists(path))
                {
                    DirectoryInfo di = Directory.CreateDirectory(path);
                    MessageBox.Show(path + "CREATED");
                }
            }
            catch 
            {
                MessageBox.Show("Stupid directory asshole !!");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Script();
        }

      

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            //if (comboBox1.Text =="420")
            if (comboBox1.Text =="MAX" || comboBox1.Text == "1080" || comboBox1.Text == "720" || comboBox1.Text == "480" || comboBox1.Text == "360")
            {

            }
            else
            {
                MessageBox.Show("Invalid Resolution!!");
                comboBox1.Text = defRes;
            }
        }

       
    }

}
