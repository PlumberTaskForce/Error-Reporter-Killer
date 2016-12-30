using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading;

namespace JohnCena.CrypticErrorKiller
{
    internal static class Program
    {
        internal static AutoResetEvent are = new AutoResetEvent(false);
        internal static bool run = true;

        internal static void Main(string[] args)
        {
            var a = Assembly.GetExecutingAssembly();
            var n = a.GetName();

            Console.WriteLine("CrypticError killer");
            Console.WriteLine("Version: {0}", n.Version);
            Console.WriteLine("===================");
            Console.WriteLine();
            Console.WriteLine("Standing guard...");
            Console.WriteLine("Press any key to exit");
            Console.WriteLine();

            ThreadPool.QueueUserWorkItem(Guard);

            Console.ReadKey();
            run = false;
            are.WaitOne();
        }

        internal static void Guard(object o)
        {
            while (run)
            {
                var pl = Process.GetProcesses();
                foreach (var p in pl)
                {
                    if (p.ProcessName.ToLowerInvariant().Contains("crypticerror"))
                    {
                        Console.WriteLine("Identified {0}:{1}", p.Id, p.ProcessName);
                        try
                        {
                            p.Kill();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Unable to kill {0}: {1}", p.Id, ex.Message);
                        }

                        break;
                    }
                }
            }

            are.Set();
        }
    }
}
