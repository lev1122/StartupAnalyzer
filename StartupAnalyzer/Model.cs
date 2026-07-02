using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StartupAnalyzer
{
    public class Model
    {
 
        public string Name { get; set; }

        public string Path { get; set; }

        public string Source { get; set; }

        public bool IsActive { get; set; }

        public int Pid { get; set; }

        public string MemoryUsage { get; set; }

        public string CpuUsage { get; set; }

        public override string ToString()
        {
            string status = IsActive ? "Включена" : "Отключена";
            return $"[{Source}] {Name} - {status}";
        }
    }
}