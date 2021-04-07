using SmartGenerator.Source;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SmartGenerator.Classes
{
    public class Models
    {
        public int ModelID { get; set; }
        public string Name { get; set; }
        public int NbImplementations { get; set; }
        public string Content { set; get; } // doit etre un fichier rtf
        public List<string> ModelImplementations { set; get; }
        
        public Models() { }
        public Models(int ArgID, string ArgName)
        {
            this.ModelID = ArgID;
            this.Name = ArgName;
            this.Content = "";
            this.ModelImplementations = new List<string>();
        }

        public void AddToModel(string Champs)
        {
            ModelImplementations.Add(Champs);
        }
        
        public override string ToString()
        {
            return "ID:" + this.ModelID + "Name: " + this.Name + "Content:" + this.Content + "ListLen:" + ModelImplementations.Count;
        }

        public void AddToModelsguide()
        {
            int LineNb = 0;
            string StartPath = Directory.GetParent(Application.ExecutablePath).ToString(); //Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            string EndingPath = @"\Resources\src\ModelsGuide.txt";
            string FullCurrentProjectPath = StartPath + EndingPath;
            if (!string.IsNullOrEmpty(this.Name))
            {
                if (!File.Exists(FullCurrentProjectPath))
                {
                    using (StreamWriter sw = File.CreateText(FullCurrentProjectPath))
                    {
                        sw.WriteLine("0" + ';' + this.Name + '\n');
                    }
                }
                else
                {
                    string[] line = File.ReadAllLines(FullCurrentProjectPath);
                    foreach (var l in line)
                    {
                        if (l.Length != 0)
                        {
                            LineNb++;
                        }
                    }
                    using (StreamWriter sw = File.AppendText(FullCurrentProjectPath))
                    {
                        string LineToAdd = LineNb.ToString() + ';' + this.Name + ';' + this.Content + ';';
                        string ListLine = "";
                        for (int i = 0; i < this.ModelImplementations.Count; i++)
                        {
                            if (i == 0)
                            {
                                ListLine = ListLine + this.ModelImplementations[i];
                            }
                            else
                            {
                                ListLine = ListLine + ',' + this.ModelImplementations[i];
                            }

                        }
                        string AllLine = LineToAdd + ListLine;
                        sw.WriteLine(AllLine);
                    }
                }
                this.ModelID = LineNb;
            }
        }

        public void RemoveFromModelsGuide()
        {
            string StartPath = Directory.GetParent(Application.ExecutablePath).ToString(); // Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            //---- File from where to read
            string EndingPath_ModelsGuide = @"\Resources\src\ModelsGuide.txt";
            string ModelsGuideTxt = StartPath + EndingPath_ModelsGuide;

            //--- File where to write temporary
            string EndingPath_Temporary = @"\Resources\src\Temporary.txt";
            string TemporaryTxt = StartPath + EndingPath_Temporary;
            
            try
            {
                Treatments.RemoveModelRewritingFile(this.ModelID, ModelsGuideTxt, TemporaryTxt);
                File.Delete(this.Content);
            }
            catch (Exception Ex)
            {
                Console.WriteLine("Error: " + Ex.Message);
            }
        }
        public void EditFromModelsGuide()
        {
            string StartPath = Directory.GetParent(Application.ExecutablePath).ToString(); //Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            //---- File from where to read
            string EndingPath_ModelsGuide = @"\Resources\src\ModelsGuide.txt";
            string ModelsGuideTxt = StartPath + EndingPath_ModelsGuide;

            //--- File where to write temporary
            string EndingPath_Temporary = @"\Resources\src\Temporary.txt";
            string TemporaryTxt = StartPath + EndingPath_Temporary;

            try
            {
                Treatments.EditModelRewritingFile(this.ModelID, this.Name, this.Content, this.ModelImplementations, ModelsGuideTxt, TemporaryTxt);
                //File.Delete(this.Content);
            }
            catch (Exception Ex)
            {
                Console.WriteLine("Error: " + Ex.Message);
            }
        }
    }
}
