using System;
using System.Collections.Generic;
using static tsl.StdImp;

namespace tsl
{
    class Program
    {
    

        static int Main(string[] args)
        {
            int ret = 0;
            Dictionary<string, long> IntVar = new Dictionary<string, long>();
            Dictionary<string, string> StringVarr = new Dictionary<string, string>();
            Dictionary<string, char> CharVar = new Dictionary<string, char>();
            Dictionary<string, bool> BoolVar = new Dictionary<string, bool>();
            Dictionary<string, double> FloatVar = new Dictionary<string, double>();
            Dictionary<string, string> TypeMap = new Dictionary<string, string>();
            List<string> names = new List<string>();
            string[] keywords = {"int string bool float char vput vputl vget put putl puts ret"};
            //foreach (string keyword in keywords) Put(keyword + '\n');
            names.Add("StringVariableOpenerZ36A");
            if (args.Length != 1)
            {
               return Err(".tslf input file required.", 5);
            }
            string[] infile;
            string fname;
            try
            {
                infile = System.IO.File.ReadAllLines(args[0]);
                fname = args[0];
            }
            catch (Exception Er)
            {
                return Err(Er.Message, 2);
            }

            /*foreach(string line in infile)
            {
                Put(line + "\n");
            }*/
            System.IO.File.WriteAllText("tokens.txt", "");
            foreach (string line in infile)
            {
                line.Trim();
                long lcount = 0;
                string[] words = line.Split(' ');
                lcount++;
                
                foreach (string word in words)
                {
                    
                    System.IO.File.AppendAllText("tokens.txt", word + " ");
                }
                //try { string varname = words[1]; string varval = words[2]; }
                //catch (Exception E) { return Err("Invalid declaration. ", 50); }
                switch (words[0]) {
                    case "int":
                        if (names.Contains(words[1]))
                        {
                            TypeMap.TryGetValue(words[1], out string vartype);
                            return Err("Redefinition of variable " + words[1] + " of type " + vartype, 51, lcount, fname);
                        }
                        if (long.TryParse(words[2], out long var)) { 
                            names.Add(words[1]);
                            IntVar.Add(words[1], var);
                            TypeMap.Add(words[1], "int");
                        }
                        else
                        {
                            return Err("Invalid value for variable type int.", 37, lcount, fname);
                        }
                        
                        break;
                    case "vput":
                        if (names.Contains(words[1]))
                        {
                            TypeMap.TryGetValue(words[1], out string vartype);
                            if (vartype == "int")
                            {
                                IntVar.TryGetValue(words[1], out long varval);
                                Put(varval.ToString());
                            }
                        }
                        else
                        {
                            return Err("Variable " + words[1] + " doesn't exist.", 55, lcount, fname);
                        }
                        break;
                    case "vputl":
                        if (names.Contains(words[1]))
                        {
                            TypeMap.TryGetValue(words[1], out string vartype);
                            if (vartype == "int")
                            {
                                IntVar.TryGetValue(words[1], out long varval);
                                Put('\n' + varval.ToString());
                            }
                        }
                        else
                        {
                            return Err("Variable " + words[1] + " doesn't exist.", 55, lcount, fname);
                        }
                        break;
                    case "ret":
                        if (int.TryParse(words[1], out int retval)) {
                            ret = retval;
                        }
                        else
                        {
                            if (IntVar.TryGetValue(words[1], out long retvale))
                            {
                                ret = (int) retvale;
                            }
                            else
                            {
                                return Err("Invalid return value " + words[1], 90, lcount, fname);
                            }
                        }
                        break;
                    case "put":
                        Put(words[1].ToString());
                        break;
                    case "putl":
                        Put("\n" + words[1].ToString());
                        break;
                    case "vget":
                        if (TypeMap.TryGetValue(words[1], out string vtype))
                        {
                            switch (vtype) {
                                case "int":
                                    if (long.TryParse(Console.ReadLine(), out long vstr)) {
                                        IntVar[words[1]] = vstr;
                                    }
                                    else
                                    {
                                        Err("Invalid input for variable " + words[0] + "of type " + vtype, 71, lcount, fname);
                                    }
                                    break;
                                default:
                                    
                                    break;
                            }
                        }
                        else
                        {
                            Err("Invalid input variable.", 75, lcount, fname);
                        }
                        break;
                    case "puts":
                        int l = words.Length;
                        for (int i = 1; i < l - 1 ; ++i) { Put(words[i] + " "); }
                        Put(words[l - 1]);
                        break;
                    case "putsl":
                        Put("\n");
                        int le = words.Length;
                        for (int i = 1; i < le - 1; ++i) { Put(words[i] + " "); }
                        Put(words[le - 1] + '\n');
                        break;
                    case "putnl":
                        Put("\n");
                        break;
                    default:
                        break;
                }

            }
             
            return ret;
        }
    }
}
