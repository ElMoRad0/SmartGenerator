using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGenerator.Classes
{
    public class User
    {
        private string Password = "root";
        public string GetPwd()
        {
            return this.Password;
        }
        public void SetPwd(string pwd)
        {
            this.Password = pwd;
        }
    }
}
