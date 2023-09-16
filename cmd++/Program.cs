using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading;

namespace cmd__
{
    internal class Program
    {
        static SQLiteConnection con = new SQLiteConnection("DataSource=cmd.db");
        static void Main(string[] args)
        {
            con.Open();

            Console.WriteLine("to see your command history type 'historyx' with number like this: historyx 10");
            while(true)
            {
                string cmd = Console.ReadLine();
                try
                {
                    if (cmd.Split(' ')[0] == "historyx")
                    {
                        try
                        {
                            if (int.Parse(cmd.Split(' ')[1]) > 0)
                            {
                                SQLiteDataAdapter Adapter = new SQLiteDataAdapter($"Select command from history order by ID Asc Limit {cmd.Split(' ')[1]}", con);
                                DataTable DT = new DataTable();
                                Adapter.Fill(DT);

                                foreach (DataRow Row in DT.Rows)
                                {
                                    Console.WriteLine(Row.ItemArray[0]);
                                }
                            }
                        }
                        catch
                        {
                            SQLiteDataAdapter Adapter = new SQLiteDataAdapter("Select command from history order by ID Asc", con);
                            DataTable DT = new DataTable();
                            Adapter.Fill(DT);

                            foreach (DataRow Row in DT.Rows)
                            {
                                Console.WriteLine(Row.ItemArray[0]);
                            }
                        }
                    }
                    continue;
                }
                catch
                {
                    
                }
                Load(cmd);
                new SQLiteCommand($"Insert Into history (command) Values ('{cmd}')", con).ExecuteNonQuery();
            }
        }
        static void Load(string cmd)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.FileName = "oldcmd.exe";
            startInfo.Arguments = "/c " + cmd;
            process.StartInfo = startInfo;
            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            Console.WriteLine(output);
            process.WaitForExit();

        }
    }
}
