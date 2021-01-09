using System;
using System.Collections.Generic;
using System.Text;

namespace tsl
{
    public static class StdImp
    {
        public static void Put(string arg)
        {
            Console.Write(arg);
        }
        public static int Err(string arg, int errcode)
        {
            ConsoleColor def = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine("\n{0} Error Code: {1}. ", arg, errcode);
            Console.ForegroundColor = def;
            return errcode;
        }

        public static int Err(string arg, int errcode, long ln, string fname)
        {
            ConsoleColor def = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine("\nTSLC \"{2}\" line {3} {0} Error Code: {1}. ", arg, errcode, fname, ln);
            Console.ForegroundColor = def;
            return errcode;
        }

        public static int CheckArr(string[] arr, string chk)
        {
            int len = arr.Length;
            for(int i = 0; i < len; i++)
            {
                if (arr[i] == chk) return i;
            }
            return -1;
        }
    }
}
