using System;
using System.Collections.Generic;
using System.Text;

namespace tsl
{
    public static class Header
    {
        public static int ret = 0;
        public static Dictionary<string, long> IntVar = new Dictionary<string, long>();
        public static Dictionary<string, string> StringVar = new Dictionary<string, string>();
        public static Dictionary<string, char> CharVar = new Dictionary<string, char>();
        public static Dictionary<string, bool> BoolVar = new Dictionary<string, bool>();
        public static Dictionary<string, double> FloatVar = new Dictionary<string, double>();
        public static Dictionary<string, string> TypeMap = new Dictionary<string, string>();
        public static List<string> names = new List<string>();
        public static string[] keywords = { "int string bool float char vput vputl vget put putl puts ret fputs fwrites" };
        //foreach (string keyword in keywords) Put(keyword + '\n');
    }
}
