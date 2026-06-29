using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StartupAnalyzer
{
    public class StartupItem
    {
 
        public string Name { get; set; }

        public string Path { get; set; }

        public string Source { get; set; }

        public bool IsActive { get; set; }

        public override string ToString()
        {
            string status = IsActive ? "Включена" : "Отключена";
            return $"[{Source}] {Name} - {status}";
        }
    }
}