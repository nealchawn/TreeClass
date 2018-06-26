using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    class Tree
    {
        private String RootSource;
        private String SourcePath;
        private int RootSourceIndex;
        private String DestPath;

        private List<String> AllDirs = new List<string>();
        private List<String> AllFiles = new List<string>();

        public String getRootSource() { return RootSource; }

        public void setRootSource(String InputDir)
        {
            this.RootSource = Path.GetFileName(InputDir);
            this.RootSourceIndex = InputDir.IndexOf(RootSource);
        }

        public Tree(String InputDir,String OutputDir)
        {
            this.SourcePath = InputDir;
            this.RootSource = Path.GetFileName(InputDir);
            this.RootSourceIndex = InputDir.IndexOf(RootSource);
            this.DestPath = OutputDir;
            Directory.CreateDirectory(OutputDir+"\\"+this.RootSource);
            loadPaths();
            CreateDirs();
        }

        //when this is done two lists AllDirs and AllFiles will be filled with complete paths
        //for all files and subdirectories
        private void loadPaths(String InputDir)
        {

            //step 2 get path for all directories
            String[] subDirs = Directory.GetDirectories(InputDir);
            String[] files = Directory.GetFiles(InputDir);
            foreach (String sub in subDirs)
            {
                this.AllDirs.Add(sub);
            }
            //step 3 get path for all files
            foreach (String file in files)
            {
                this.AllFiles.Add(file);
            }
            //step 4 or each subDirectory
            foreach (String sub in subDirs)
            {
                loadPaths(sub);
            }

        }

        //each overloaded function assumes to take values from global paths 
        //in short it The class was initialized with the Input and Output
        private void loadPaths()
        {

            //step 2 get path for all directories
            String[] subDirs = Directory.GetDirectories(this.SourcePath);
            String[] files = Directory.GetFiles(this.SourcePath);
            foreach (String sub in subDirs)
            {
                this.AllDirs.Add(sub);
            }
            //step 3 get path for all files
            foreach (String file in files)
            {
                this.AllFiles.Add(file);
            }
            //step 4 or each subDirectory
            foreach (String sub in subDirs)
            {
                loadPaths(sub);
            }

        }

        public void CreatePaths()
        {
            loadPaths();
            CreateDirs();
            CreateFiles();
        }

        private void CreateDirs(List<String> InputDirs, String OutputDir)
        {
            foreach(String dir in InputDirs)
            {
                String tempOutput = OutputDir + "\\" + dir.Substring(this.RootSourceIndex);
                Directory.CreateDirectory(tempOutput);
            } 
        }

        //each overloaded function assumes to take values from global lists
        //in short it The class was initialized with the Input and Output
        private void CreateDirs()
        {
            foreach (String dir in this.AllDirs)
            {
                String tempOutput = this.DestPath + "\\" + dir.Substring(this.RootSourceIndex);
                Directory.CreateDirectory(tempOutput);
            }
        }
        //Older may not be needed but so far not conflicting should review this
        private void CreateDirs(String OutputDir)
        {
            List<String> InputDirs = this.AllFiles;
            foreach (String dir in InputDirs)
            {
                String tempOutput = OutputDir + "\\" + dir.Substring(this.RootSourceIndex);
                Directory.CreateDirectory(tempOutput);
            }
        }

        public void CreateFiles()
        {
            foreach (String filepath in this.AllFiles)
            {
                StreamReader reader;
                String line;
                int i = 1;
                reader = new StreamReader(@filepath);

                String tempOutput = this.DestPath + "\\" + filepath.Substring(this.RootSourceIndex);

                //write to temp then overwrite the original
                StreamWriter sw = new StreamWriter(tempOutput);
                do
                {
                    line = reader.ReadLine();
                    sw.WriteLine(line);
                } while (!reader.EndOfStream);
                sw.Close();
                reader.Close();
                reader.Dispose();
            }
        }

        //this one sends the entire file to be read and modified and send back a string 
        //this one will assume the string path of the modified file
        public void CreateFiles(String InputDir, String OutputDir, Func<String, string> modfile)
        {
            loadPaths(InputDir);
            CreateDirs(OutputDir);
            foreach (String filepath in this.AllFiles)
            {
                StreamReader reader;
                String line;
                int i = 1;
                string newmodfile = modfile(filepath);
                reader = new StreamReader(@newmodfile);

                String tempOutput = this.DestPath + "\\" + filepath.Substring(this.RootSourceIndex);

                //write to temp then overwrite the original
                StreamWriter sw = new StreamWriter(tempOutput);
                do
                {
                    line = reader.ReadLine();
                    sw.WriteLine(line);
                } while (!reader.EndOfStream);
                sw.Close();
                reader.Close();
                reader.Dispose();
            }

        }

        public void CreateFiles(Func<String, string> modfile)
        {
            foreach (String filepath in this.AllFiles)
            {
                StreamReader reader;
                String line;
                int i = 1;
                string newmodfile = modfile(filepath);
                reader = new StreamReader(@newmodfile);

                String tempOutput = this.DestPath + "\\" + filepath.Substring(this.RootSourceIndex);

                //write to temp then overwrite the original
                StreamWriter sw = new StreamWriter(tempOutput);
                do
                {
                    line = reader.ReadLine();
                    sw.WriteLine(line);
                } while (!reader.EndOfStream);
                sw.Close();
                reader.Close();
                reader.Dispose();
            }
        }
        public void CopyAll(String InputDir, String OutputDir)
        {
            loadPaths(InputDir);
            CreateDirs(OutputDir);
            foreach (String filepath in this.AllFiles)
            {
                StreamReader reader;
                String line;
                int i = 1;
                reader = new StreamReader(@filepath);

                String tempOutput = OutputDir + "\\" + filepath.Substring(this.RootSourceIndex);

                //write to temp then overwrite the original
                StreamWriter sw = new StreamWriter(tempOutput);
                do
                {
                    line = reader.ReadLine();
                    sw.WriteLine(line);
                } while (!reader.EndOfStream);
                sw.Close();
                reader.Close();
                reader.Dispose();
            }

        }
    }
}
