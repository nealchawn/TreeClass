using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private String parentDir = "";
        private bool filter = true; // assume that it is not function

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //one_file(textBox1.Text.ToString());
            Tree tree = new Tree(textBox1.Text.ToString(), textBox3.Text.ToString());
            tree.CreateFiles(ModFile);
            //scan_dir(textBox1.Text.ToString());
            //**/

        }
        //debug method
        private void one_file(String dirPath)
        {
            String[] subDirs = Directory.GetDirectories(dirPath);
            String[] files = Directory.GetFiles(dirPath);
            //to view file directory
            //for (int i=0; i < dir.Length; i++)
            //{
            //    textBox2.Text += dir[i] + Environment.NewLine;
            //}
            //end view file directory
            foreach (String sub in subDirs)
            {
                //Directory.CreateDirectory(textBox3.Text.ToString() + "\\" + Path.GetDirectoryName(sub));
                textBox2.Text += Path.GetFileName(sub) + Environment.NewLine;
                //this.parentDir = Path.GetDirectoryName(sub);
                //scan_dir(sub);
            }
            //c_skein.c
            //ModFile(files[14], Path.GetFileName(files[14]));

        }

        //old start
        private void scan_files(String dirPath)
        {
            String[] subDirs = Directory.GetDirectories(dirPath);
            String[] files = Directory.GetFiles(dirPath);

            //ModFile(dir[0], Path.GetFileName(dir[0]));
            //textBox2.Text = dir[6];

            //**
            for (int i = 0; i < files.Length; i++)
            {
                string ext = Path.GetExtension(files[i]) + Environment.NewLine;

                //    This isolates the files of the directory that are source files c, h, cpp
                // || ext.StartsWith(".h")
                if ((ext.StartsWith(".c") || ext.StartsWith(".cpp")) && !(Path.GetFileName(files[i])  == "Job.cpp") && !(Path.GetFileName(files[i]) == "recog_intel.c") && !(Path.GetFileName(files[i]) == "recog_amd.c") && !(Path.GetFileName(files[i]) == "doubleworker.cpp") && !(Path.GetFileName(files[i]) == "cpu_win.cpp") && !(Path.GetFileName(files[i]) == "cpu.cpp"))
                {
                    //textBox2.Text += Path.GetFileName(dir[i]) + Environment.NewLine;
                    //ModFile(files[i], Path.GetFileName(files[i]));
                    ModFile(files[i]);
                    //       read a file and then write to a temp file affter every change
                    //var txtLines = File.ReadAllLines(fileName).ToList();
                }
                else
                {
                    CopyFile(files[i], Path.GetFileName(files[i]));
                }

            }
            foreach(String sub in subDirs){
                if (sub.Contains(this.parentDir)) {
                    this.parentDir += "\\" + Path.GetFileName(sub);
                    Directory.CreateDirectory(textBox3.Text.ToString() + "\\" + this.parentDir);
                }
                else {
                    this.parentDir = Path.GetFileName(sub);
                    Directory.CreateDirectory(textBox3.Text.ToString() + "\\" + Path.GetFileName(sub));
                }
                
                scan_files(sub);
            }
        }

        private void scan_dir(String dir)
        {
            //scan_files(dir);
            this.parentDir = textBox3.Text+"\\"+Path.GetFileName(dir);

            String[] subDirs = Directory.GetDirectories(dir);
            //Create all sub directories under current directory
            foreach (string sub in subDirs)
            {

                Directory.CreateDirectory(this.parentDir+"\\"+sub);
            }
            scan_files(dir);
            foreach (string sub in subDirs)
            {
                String tempdir = this.parentDir;
                tempdir.IndexOf(sub);
                tempdir += "\\" + sub;
                scan_files(sub);
            }
        }

        private void CopyFile(string filepath, string filename)
        {
            //textBox2.Text += filepath + Environment.NewLine;
            StreamReader reader;
            String line;
            int i = 1;
            reader = new StreamReader(@filepath);
            //write to temp then overwrite the original
            StreamWriter sw = new StreamWriter(textBox3.Text + "\\" + filename);
            do
            {
                line = reader.ReadLine();
                sw.WriteLine(line);
            } while (!reader.EndOfStream);
            sw.Close();
            reader.Close();
            reader.Dispose();

        }

        //old end

        private String ModFile(string filepath)
        {
            textBox2.Text += filepath+Environment.NewLine;
            String filestuff ="";
            StreamReader reader;
            char ch;
            String line;
            String prevline ="";
            int i = 1;
            reader = new StreamReader(@filepath);
            StreamWriter sw;
            //write to temp then overwrite the original

            // 5/18 old
            //apply the subdirectory names, should run linearly and be fine
            //if (this.parentDir == "") { sw = new StreamWriter(textBox3.Text + "\\" + filename); }
            //else { sw = new StreamWriter(textBox3.Text + "\\" +this.parentDir+"\\"+ filename); }
            //5/18 old end
            String tempfile = Path.GetTempFileName();
            FileInfo fileInfo = new FileInfo(tempfile);
            fileInfo.Attributes = FileAttributes.Temporary;

            sw = new StreamWriter(tempfile);

            do
            {
                //read by char
                //ch = (char)reader.Read();
                //sw.Write(ch);
                //filestuff += ch;

                //read by line
                line = reader.ReadLine();

                //this does the page number
                //textBox2.Text += i+".";

                if(prevline == "")
                {
                    prevline = line;
                }
                sw.WriteLine(line);
                if (line.Contains('{'))
                {
                    Filter(line, prevline);
                }
                //filter non functions
                if (!filter)
                {
                    sw.WriteLine(dummyCode());
                    this.filter = true;
                }
                prevline = line;
                //this prints the lines works adjacent to the page numbers for testing output
                //textBox2.Text += Environment.NewLine;

                i++;
            } while (!reader.EndOfStream);
            sw.Close();
            reader.Close();
            reader.Dispose();
            return tempfile;
        }



        private string dummyCode()
        {
            Random random = new Random();
            int rnd = random.Next(0, 4);
            //textBox2.Text += rnd + Environment.NewLine;
            switch (rnd)
            {
                case 0:
                    return ("//This is a test Hello\n for (int test=0;test<5;test++) {\n int testx =5; \n int testy =8; \n printf(\"%d\",testx+testy); \n } \n//End test Hello");

                case 1:
                    return ("//This is a test Hello\n for (int test=0;test<5;test++) {\n int testx =5; \n int testy =8; \n printf(\"%d\",testy*testx); \n } \n//End test Hello");

                case 2:
                    return ("//This is a test Hello\n int test=0;\n while (test<5) {\n int testx =5; \n int testy =8; \n printf(\"%d\",testx+testy + test); \n test++; \n } \n//End test Hello");

                case 3:
                    return ("//This is a test Hello\n for (int test=0;test<5;test++) {\n int testx =5; \n int testy =8; \n printf(\"%d\",testx+testy*test); \n } \n//End test Hello");

                default:
                    return ("//This is a test Hello\n for (int test=0;test<5;test++) {\n int testx =5; \n int testy =8; \n printf(\"%d\",test*testx+testy); \n } \n//End test Hello");

            }
            //throw new NotImplementedException();
        }

        private void Filter(string currline, string prevline)
        {
            char[] trim = " \f\n\r\t\v".ToCharArray();
            //currline.

            //is it the case where the  { is on the following line

            //currline.Trim(trim).Substring(0, currline.Trim(trim).IndexOf('{')+1)
            if (currline.Trim(trim).Substring(0, currline.Trim(trim).IndexOf('{') + 1).Length < 2)
            {
                //works with printing lines for testing output
                //textBox2.Text += prevline  + currline.Trim(trim) + Environment.NewLine;

                Filter(prevline);
            }
            // the logic and { are on the same line
            else
            {
                Filter(currline);
            }
            //throw new NotImplementedException();
        }


        private void Filter(String line)
        {
            //char[] trim = " \f\n\r\t\v".ToCharArray();
            
            if (line.Contains("switch")) { }
            else if (line.StartsWith("if")) { }
            else if (line.Contains(" else")) { }
            else if (line.Contains(" for")) { }
            else if (line.Contains(" while")) { }

            else if (line.Contains(" if")) { }
            else if (line.Contains("enum")) { }
            else if (line.Contains("}")) { }
            else if (line.Contains("//")) { }
            else if (line.Contains("typedef")) { }
            else if (line.Contains("extern")) { }
            else if (line.Contains("class")) { }

            else if (line.Contains("\\")) { }
            else if (line.Contains("define")) { }
            else if (line.Contains("FORCE_INLINE")) { }
            else if (line.Contains("#ifdef")) { }
            else if (line.Contains("#endif")) { }
            else if (line.Contains("union")) { }
            else if (line.Contains(" do")) { }

            //perhaps starts with
            //careful with starts with
            else if (line.StartsWith("const")) { }
            else if (line.StartsWith("struct")) { }

            else if (line.Contains('>')) { }
            else if (line.Contains('<')) { }
            else if (line.Contains('=')) { }
            
            //otherwise must be a function
            else
            {
                this.filter = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
           // this.test = false;
           // this.test1 = true;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
