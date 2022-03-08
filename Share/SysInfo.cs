using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share
{
    public class SysInfo
    {
        public List<SysInfoObject> GetInfo()
        {
            string output = "";
            var proc = new ProcessStartInfo("powershell.exe", "/c Get-WmiObject -Class Win32_Process")
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                WorkingDirectory = @"C:\Windows\System32\"
            };
            Process p = Process.Start(proc);
            p.OutputDataReceived += (sender, args1) => { output += args1.Data + Environment.NewLine; };
            p.BeginOutputReadLine();
            p.WaitForExit();

            List<SysInfoObject> OutputList = new List<SysInfoObject>();
            var sisinfo = output.Split('\n');
            foreach (var item in sisinfo)
            {
                string[] Line = item.Trim().Split(':');

                if (Line[0] == null) continue;
                if (Line.Length == 1) continue;

                OutputList.Add(new SysInfoObject { Name = Line[0].Trim(), Value = Line[1].Trim() });
            }

            return OutputList;
        }
        public string GetInfoJson()
        {
            List<SysInfoObject> OutputList = GetInfo();

            string json = System.Text.Json.JsonSerializer.Serialize(OutputList);

            return json;
        }

    }
}
