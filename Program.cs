using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace PreMadeAutoBackUP
{
    class Program
    {

        static int Looooop = 60;
        static string StartTime = DateTime.Now.ToString("d/M/y h:mm:ss");

        const int MF_BYCOMMAND = 0x00000000;
        const int SC_MINIMIZE = 0xF020;
        const int SC_MAXIMIZE = 0xF030;
        const int SC_SIZE = 0xF000;

        [DllImport("user32.dll")]
        public static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();

        static public void Tick(Object stateInfo)
        {
            Looooop--;
            Console.Clear();
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.Yellow;
            string FirstLine = $"Last back up created: {StartTime}";
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (FirstLine.Length / 2)) + "}", FirstLine));

            Console.ForegroundColor = ConsoleColor.Green;
            string SecondtLine = $"{Looooop.ToString()} minutes untill create new backup";
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (SecondtLine.Length / 2)) + "}", SecondtLine));
            if (Looooop == 10)
            {
                string startPath = @"C:\Users\admin\Desktop\LegacyDB";
                string zipPath = @"C:\Users\admin\Desktop\HourlyBackup\DB " + DateTime.Now.ToString("d-M htt") + ".zip"; // :mm:ss <-- removed. Can't use : in file name
                ZipFile.CreateFromDirectory(startPath, zipPath);
            }
            else if (Looooop == 0)
            {
                Process.Start(Assembly.GetExecutingAssembly().Location);
                Environment.Exit(0);
            }
        }

        static void Main()
        {
            Console.Title = "[PreMade] ConquerLegacy backup creator";
            
            Console.WindowHeight = 5;
            Console.WindowWidth = 70;

            DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_MINIMIZE, MF_BYCOMMAND);
            DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_MAXIMIZE, MF_BYCOMMAND);
            DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_SIZE, MF_BYCOMMAND);

            TimerCallback callback = new TimerCallback(Tick);
            Console.Clear();

            // create a one second timer tick
            Timer stateTimer = new Timer(callback, null, 0, 60000);

            // loop here forever
            for (; ; )
            {
                // add a sleep for 100 mSec to reduce CPU usage
                Thread.Sleep(1000);
            }
        }
    }
}
