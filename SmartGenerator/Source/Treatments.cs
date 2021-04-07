using SmartGenerator.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace SmartGenerator.Source
{
    public class Treatments
    {
        static public bool CheckCredentials(string Passwd, User U)
        {
            bool IsCorrect;
            if (Passwd == U.GetPwd())
            {
                IsCorrect = true;
            }
            else
            {
                IsCorrect = false;
            }
            return IsCorrect;
        }

        static public User CreateSession()
        {
            User Admin = new User();
            return Admin;
        }
        
        static public List<Models> InitModelsCB()
        {
            try
            {
                string ModelName;
                int ModelID;
               // string StartPath  = ; //Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
                string StartPath = Directory.GetParent(Application.ExecutablePath).ToString();
                string EndingPath = @"\Resources\src\ModelsGuide.txt";
                string FullCurrentProjectPath = StartPath + EndingPath;
                List<Models> ModelsList = new List<Models>();
                string[] line = File.ReadAllLines(FullCurrentProjectPath);
                if (File.Exists(FullCurrentProjectPath))
                {
                    foreach (var l in line)
                    {
                        if (l.Length != 0)
                        {
                            string[] DataInLine = l.Split(';');
                            if(DataInLine.Count() == 4)
                            {
                                ModelName = DataInLine[1];
                                ModelID = int.Parse(DataInLine[0]);
                                Models MyModel = new Models(ModelID, ModelName);
                                MyModel.Content = DataInLine[2];
                                string[] ListOfImplementation = DataInLine[3].Split(',');
                                for (int i = 0; i < ListOfImplementation.Length; i++)
                                {
                                    MyModel.ModelImplementations.Add(ListOfImplementation[i]);
                                }
                                ModelsList.Add(MyModel);
                            }
                        }
                    }
                }
                return ModelsList;
            }
            catch (Exception MyEx)
            {
                Console.WriteLine(MyEx.Message);
                throw;
            }
        }
                

        public static void RemoveModelRewritingFile(int id, string Principal, string Temporary)
        {
            try
            {
                string[] lineRead = File.ReadAllLines(Principal);
                foreach (var rline in lineRead)
                {
                    if (rline.Length != 0)
                    {
                        string[] DataInLine = rline.Split(';');
                        if (DataInLine.Count() == 4)
                        {
                            using (StreamWriter sw = File.AppendText(Temporary))
                            {
                                int readID = int.Parse(DataInLine[0]);
                                if (readID != id)
                                {
                                    sw.WriteLine(rline);
                                }
                            }
                        }
                    }
                }
                using (StreamWriter sw = File.CreateText(Principal))
                {
                    sw.WriteLine();
                }
                lineRead = File.ReadAllLines(Temporary);
                foreach (var rline in lineRead)
                {
                    if (rline.Length != 0)
                    {
                        string[] DataInLine = rline.Split(';');
                        if (DataInLine.Count() == 4)
                        {
                            using (StreamWriter sw = File.AppendText(Principal))
                            {
                                int readID = int.Parse(DataInLine[0]);
                                if (readID != id)
                                {
                                    sw.WriteLine(rline);
                                }
                            }
                        }
                    }
                }
                File.Delete(Temporary);
            }
            catch(Exception MyEx)
            {
                Console.WriteLine(MyEx.Message);
                throw;
            }
        }

        public static void EditModelRewritingFile(int argModelID, string argName, string argContent, List<string> argModelImplementations, string Principale, string Temporary)
        {
            try
            {
                string[] lineRead = File.ReadAllLines(Principale);
                foreach (var rline in lineRead)
                {
                    if (rline.Length != 0)
                    {
                        string[] DataInLine = rline.Split(';');
                        if (DataInLine.Count() == 4)
                        {
                            using (StreamWriter sw = File.AppendText(Temporary))
                            {
                                int readID = int.Parse(DataInLine[0]);
                                if (readID != argModelID)
                                {
                                    sw.WriteLine(rline);
                                }
                                else
                                {
                                    string LineToAdd = argModelID.ToString() + ';' + argName + ';' + argContent + ';';
                                    string ListLine = "";
                                    for (int i = 0; i < argModelImplementations.Count; i++)
                                    {
                                        if (i == 0)
                                        {
                                            ListLine = ListLine + argModelImplementations[i];
                                        }
                                        else
                                        {
                                            ListLine = ListLine + ',' + argModelImplementations[i];
                                        }

                                    }
                                    sw.WriteLine(LineToAdd+ListLine);
                                }
                            }
                        }
                    }
                }
                using (StreamWriter sw = File.CreateText(Principale))
                {
                    sw.WriteLine();
                }
                lineRead = File.ReadAllLines(Temporary);
                foreach (var rline in lineRead)
                {
                    if (rline.Length != 0)
                    {
                        string[] DataInLine = rline.Split(';');
                        if (DataInLine.Count() == 4)
                        {
                            using (StreamWriter sw = File.AppendText(Principale))
                            {
                                sw.WriteLine(rline);
                            }
                        }
                    }
                }
                File.Delete(Temporary);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}