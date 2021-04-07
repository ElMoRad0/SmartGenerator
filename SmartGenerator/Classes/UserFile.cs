using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGenerator.Classes
{
    class UserFile
    {
        public string Name { get; set; }
        public string Title { set; get; }
        public Models BaseModel { set; get; }
        public Dictionary<string, string> FileImplementationsValue { set; get; }

        public UserFile(string ArgName, string ArgTitle, Models ArgModel)
        {
            Name = ArgName;
            Title = ArgTitle;
            BaseModel = ArgModel;
            InitFieldsDictionary();
        }

        public void InitFieldsDictionary()
        {
            try
            {
                this.FileImplementationsValue = new Dictionary<string, string>();
            
                for (int i = 0; i < this.BaseModel.ModelImplementations.Count; i++)
                {
                    this.FileImplementationsValue.Add(this.BaseModel.ModelImplementations[i], " ");
                }
            }
            catch(Exception)
            {
                throw;
            }
        }


        /*public void SetImplementation()
        {
            this.FileImplementationsValue = this.BaseModel.ModelImplementations; //  je pense petit changement ici
            for (int i = 0; i < this.BaseModel.ModelImplementations.Count ; i++)
            {
                this.FileImplementationsValue.Add(this.BaseModel.ModelImplementations[i], " ");
                //this.FileImplementationsValue[this.BaseModel.ModelImplementations[i]] = " ";
            }
        }*/

        public bool IsAllImplementation()
        {
            if (this.BaseModel.NbImplementations == this.FileImplementationsValue.Count())
                return true;
            else
                return false;
        }

        public void CreateFile(string Content, string Path)
        {
            Microsoft.Office.Interop.Word.Application winword = new Microsoft.Office.Interop.Word.Application();

            winword.ShowAnimation = false;
            winword.Visible = false;

            object missing = System.Reflection.Missing.Value;
            

            Microsoft.Office.Interop.Word.Document document = winword.Documents.Add(ref missing, ref missing, ref missing, ref missing);
            foreach (Microsoft.Office.Interop.Word.Section section in document.Sections)
            {
                //Get the header range and add the header details.
                Microsoft.Office.Interop.Word.Range headerRange = section.Headers[Microsoft.Office.Interop.Word.WdHeaderFooterIndex.wdHeaderFooterPrimary].Range;
                headerRange.Fields.Add(headerRange, Microsoft.Office.Interop.Word.WdFieldType.wdFieldPage);
                headerRange.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                headerRange.Font.ColorIndex = Microsoft.Office.Interop.Word.WdColorIndex.wdBlue;
                headerRange.Font.Size = 22;
                headerRange.Text = this.Title + "\r\n";
            }

            document.Content.SetRange(0, 0);
            document.Content.Text = Content;

            /*
            string StartPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            string EndingPath = @"\Resources\src\Generated\" + this.Name + ".docx";
            string FullCurrentProjectPath = StartPath + EndingPath;
            */


            object filename = Path; /*FullCurrentProjectPath;*/

            document.SaveAs2(ref filename);
            document.Close(ref missing, ref missing, ref missing);
            document = null;
            winword.Quit(ref missing, ref missing, ref missing);
            winword = null;

        }
    }
}
