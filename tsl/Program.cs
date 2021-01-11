using System;
using System.Collections.Generic;
using static tsl.StdImp;
using static tsl.Header;

namespace tsl
{
    class Program
    {  

        static int Main(string[] args)
        {
            names.Add("StringVariableOpenerZ36A@$#>DefaultString?_");
            if (args.Length < 1 || args.Length > 2)
            {
               return Err(".tslf input file required.", 5);
            }
            if (args.Length == 2)
            {
                if (args[1][0] != '-') {
                    return Err("Usage: tslc [filename] [flag]\n", 1);
                }

                string flagstr = "";
                for(int i = 1; i < args[1].Length; i++) flagstr+= args[1][i];

                //Put(flagstr.ToLower());
                switch (flagstr.ToLower())
                {
                    
                    case "v":
                    case "version":
                    case "ver":
                        try
                        {
                            string dateString = System.DateTime.Now.ToString("yyyy");
                            Put("TSLC TSL Compiler Version " + VersionString + "\nCopyright Sekhara Pramod " + dateString + ".\n\n\n");
                        }
                        catch (Exception FileMissingE)
                        {
                            return Err("Couldn't find config file.", 2);
                        }
                        break;
                    default:
                        Err("Invalid flag.", 3);
                        break;
                }
            }
            string[] infile;
            string fname;
            bool SpaceTerminated = false;
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
            long lcount = 1;
            foreach (string line in infile)
            {
                if (line.Length != 0 && line[^1] == ' ') SpaceTerminated = true;
                line.Trim();
                
                string[] words = line.Split(' ');
                
                
                foreach (string word in words)
                {
                    
                    System.IO.File.AppendAllText("tokens.txt", word + " ");
                }
                //try { string varname = words[1]; string varval = words[2]; }
                //catch (Exception E) { return Err("Invalid declaration. ", 50); }
                if (line.Length != 0) switch (words[0])
                    {
                        case "titleset":
                            if (words.Length < 2) Err("Expected title string.", 125, lcount, fname);
                            else
                            {
                                try
                                {
                                    string instr = "";
                                    for (int i = 1; i < words.Length; i++)
                                    {
                                        instr += words[i];
                                        if (i != words.Length - 1) instr += " ";

                                    }
                                    Console.Title = instr;
                                }
                                catch
                                {
                                    Err("Invalid console title.", 126, lcount, fname);
                                }
                            }
                            break;
                        case "vtitleset":
                            if (words.Length < 2) Err("Expected title variable", 125, lcount, fname);
                            else
                            {
                                if (TypeMap.TryGetValue(words[1], out string vtType))
                                {
                                    if (vtType == "int")
                                    {
                                        Console.Title = IntVar[words[1]].ToString();
                                    }
                                    if (vtType == "float")
                                    {
                                        Console.Title = FloatVar[words[1]].ToString();
                                    }
                                    if (vtType == "string")
                                    {
                                        Console.Title = StringVar[words[1]];
                                    }
                                    if (vtType == "bool")
                                    {
                                        Console.Title = BoolVar[words[1]].ToString();
                                    }
                                }
                                else
                                {
                                    Err("Invalid variable.", 124, lcount, fname);
                                }
                            }
                            break;
                        case "int":
                            if (words.Length < 2) return Err("Expected variable identifier.", 30, lcount, fname);
                            if (names.Contains(words[1]))
                            {
                                TypeMap.TryGetValue(words[1], out string vartype);
                                return Err("Redefinition of variable " + words[1] + " of type " + vartype, 51, lcount, fname);
                            }
                            if (CheckArr(keywords, words[1]) != -1)
                            {
                                return Err("Usage of reserved TSL keyword '" + words[1] + "' as variable name", 102, lcount, fname);
                            }
                            if (long.TryParse(words[2], out long var))
                            {
                                names.Add(words[1]);
                                IntVar.Add(words[1], var);
                                TypeMap.Add(words[1], "int");
                            }
                            else if (names.Contains(words[2]))
                            {
                                if (TypeMap.TryGetValue(words[2], out string vType) && vType == "int")
                                {
                                    names.Add(words[1]);
                                    IntVar.TryGetValue(words[2], out long varVal);
                                    IntVar.Add(words[1], varVal);
                                    TypeMap.Add(words[1], "int");
                                }
                            }
                            else
                            {
                                return Err("Invalid value for variable type int.", 37, lcount, fname);
                            }

                            break;
                        case "string":
                            if (words.Length < 2) return Err("Expected variable identifier.", 30, lcount, fname);
                            if (names.Contains(words[1]))
                            {
                                TypeMap.TryGetValue(words[1], out string vartype);
                                return Err("Redefinition of variable " + words[1] + " of type " + vartype, 51, lcount, fname);
                            }
                            if (CheckArr(keywords, words[1]) != -1)
                            {
                                return Err("Usage of reserved TSL keyword '" + words[1] + "' as variable name", 102, lcount, fname);
                            }
                            string outString = "";
                            try
                            {
                                int strLen = words.Length;
                                for (int i = 2; i < strLen; i++)
                                {
                                    outString += words[i];
                                    if (i != strLen - 1) outString += " ";
                                }
                                if (SpaceTerminated) outString += " ";
                            }
                            catch (Exception es)
                            {
                                return Err("Invalid string assignment", 53, lcount, fname);
                            }

                            names.Add(words[1]);
                            StringVar.Add(words[1], outString);
                            TypeMap.Add(words[1], "string");
                            break;

                        case "float":
                            if (words.Length < 2) return Err("Expected variable identifier.", 30, lcount, fname);
                            if (names.Contains(words[1]))
                            {
                                TypeMap.TryGetValue(words[1], out string vartype);
                                return Err("Redefinition of variable " + words[1] + " of type " + vartype, 51, lcount, fname);
                            }
                            if (CheckArr(keywords, words[1]) != -1)
                            {
                                return Err("Usage of reserved TSL keyword '" + words[1] + "' as variable name", 102, lcount, fname);
                            }
                            if (double.TryParse(words[2], out double vVal))
                            {
                                names.Add(words[1]);
                                FloatVar.Add(words[1], vVal);
                                TypeMap.Add(words[1], "float");
                            }
                            else if (names.Contains(words[2]))
                            {
                                string vType;
                                if (TypeMap.TryGetValue(words[2], out vType) && vType == "int")
                                {
                                    names.Add(words[1]);
                                    IntVar.TryGetValue(words[2], out long varVal);
                                    double vDouble = varVal;
                                    FloatVar.Add(words[1], vDouble);
                                    TypeMap.Add(words[1], "float");
                                }
                                else if (TypeMap.TryGetValue(words[2], out vType) && vType == "float")
                                {
                                    names.Add(words[1]);
                                    FloatVar.TryGetValue(words[2], out double varVal);
                                    FloatVar.Add(words[1], varVal);
                                    TypeMap.Add(words[1], "float");
                                }
                            }
                            else
                            {
                                return Err("Invalid value for variable type float.", 37, lcount, fname);
                            }

                            break;
                        case "bool":
                            if (words.Length < 2) return Err("Expected variable identifier.", 30, lcount, fname);
                            if (names.Contains(words[1]))
                            {
                                TypeMap.TryGetValue(words[1], out string vartype);
                                return Err("Redefinition of variable " + words[1] + " of type " + vartype, 51, lcount, fname);
                            }
                            if (CheckArr(keywords, words[1]) != -1)
                            {
                                return Err("Usage of reserved TSL keyword '" + words[1] + "' as variable name", 102, lcount, fname);
                            }
                            if (bool.TryParse(words[2], out bool bVal))
                            {
                                names.Add(words[1]);
                                TypeMap.Add(words[1], "bool");
                                BoolVar.Add(words[1], bVal);
                            }
                            else if (long.TryParse(words[2], out long pVal))
                            {
                                names.Add(words[1]);
                                TypeMap.Add(words[1], "bool");
                                bool boolVal = (pVal != 0);
                                BoolVar.Add(words[1], boolVal);
                            }
                            else if (names.Contains(words[2]))
                            {
                                string vType;
                                if (TypeMap.TryGetValue(words[2], out vType))
                                {
                                    if (vType == "bool")
                                    {
                                        names.Add(words[1]);
                                        TypeMap.Add(words[1], "bool");
                                        BoolVar.TryGetValue(words[2], out bool boolVal);
                                        BoolVar.Add(words[1], boolVal);
                                    }
                                    if (vType == "int")
                                    {
                                        if (IntVar.TryGetValue(words[2], out long intVal))
                                        {
                                            names.Add(words[1]);
                                            TypeMap.Add(words[1], "bool");
                                            bool boolVal = (intVal != 0);
                                            BoolVar.Add(words[1], boolVal);
                                        }
                                    }
                                    if (vType == "float")
                                    {
                                        if (FloatVar.TryGetValue(words[2], out double flVal))
                                        {
                                            names.Add(words[1]);
                                            TypeMap.Add(words[1], "bool");
                                            bool boolVal = (flVal != 0);
                                            BoolVar.Add(words[1], boolVal);
                                        }
                                    }
                                    else
                                    {
                                        return Err("Invalid value for variable type float.", 37, lcount, fname);
                                    }
                                }
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
                                if (vartype == "string")
                                {
                                    StringVar.TryGetValue(words[1], out string varval);
                                    Put(varval);
                                }
                                if (vartype == "float")
                                {
                                    FloatVar.TryGetValue(words[1], out double varval);
                                    Put(varval.ToString());
                                }
                                if (vartype == "bool")
                                {
                                    BoolVar.TryGetValue(words[1], out bool varval);
                                    Put(varval.ToString());
                                }
                            }
                            else
                            {
                                return Err("Variable " + words[1] + " doesn't exist.", 55, lcount, fname);
                            }
                            break;
                        case "vputl":
                            if (vputlcount == 0)
                            {
                                if (names.Contains(words[1]))
                                {
                                    TypeMap.TryGetValue(words[1], out string vartype);
                                    if (vartype == "int")
                                    {
                                        IntVar.TryGetValue(words[1], out long varval);
                                        Put(varval.ToString());
                                    }
                                    if (vartype == "string")
                                    {
                                        StringVar.TryGetValue(words[1], out string varval);
                                        Put(varval);
                                    }
                                    if (vartype == "float")
                                    {
                                        FloatVar.TryGetValue(words[1], out double varval);
                                        Put(varval.ToString());
                                    }
                                    if (vartype == "bool")
                                    {
                                        BoolVar.TryGetValue(words[1], out bool varval);
                                        Put(varval.ToString());
                                    }
                                }
                                else
                                {
                                    return Err("Variable " + words[1] + " doesn't exist.", 55, lcount, fname);
                                }
                                break;
                            }
                            if (names.Contains(words[1]))
                            {
                                TypeMap.TryGetValue(words[1], out string vartype);
                                if (vartype == "int")
                                {
                                    IntVar.TryGetValue(words[1], out long varval);
                                    Put('\n' + varval.ToString());
                                }
                                if (vartype == "string")
                                {
                                    StringVar.TryGetValue(words[1], out string varval);
                                    Put('\n' + varval);
                                }
                                if (vartype == "float")
                                {
                                    FloatVar.TryGetValue(words[1], out double varval);
                                    Put('\n' + varval.ToString());
                                }
                                if (vartype == "bool")
                                {
                                    BoolVar.TryGetValue(words[1], out bool varval);
                                    Put('\n' + varval.ToString());
                                }
                            }
                            else
                            {
                                return Err("Variable " + words[1] + " doesn't exist.", 55, lcount, fname);
                            }
                            break;
                        case "ret":
                            if (int.TryParse(words[1], out int retval))
                            {
                                ret = retval;
                            }
                            else
                            {
                                if (IntVar.TryGetValue(words[1], out long retvale))
                                {
                                    ret = (int)retvale;
                                }
                                else
                                {
                                    return Err("Invalid return value " + words[1], 90, lcount, fname);
                                }
                            }
                            break;
                        case "put":
                            Put(words[1].ToString());
                            vputlcount++;
                            break;
                        case "putl":
                            if (vputlcount != 0)
                            {
                                Put("\n" + words[1].ToString());
                            }
                            else
                            {
                                Put("\n" + words[1].ToString());
                            }
                            vputlcount++;
                            break;
                        case "vget":
                            if (TypeMap.TryGetValue(words[1], out string vtype))
                            {
                                switch (vtype)
                                {
                                    case "int":
                                        if (long.TryParse(Console.ReadLine(), out long vstr))
                                        {
                                            IntVar[words[1]] = vstr;
                                            vputlcount = 0;
                                        }
                                        else
                                        {
                                            Err("Invalid input for variable '" + words[1] + "' of type '" + vtype + "'.", 71, lcount, fname);
                                        }
                                        break;
                                    case "string":
                                        StringVar[words[1]] = Console.ReadLine();
                                        vputlcount = 0;
                                        break;
                                    case "float":
                                        if (double.TryParse(Console.ReadLine(), out double vDouble))
                                        {
                                            FloatVar[words[1]] = vDouble;
                                            vputlcount = 0;
                                        }
                                        else
                                        {
                                            Err("Invalid input for variable '" + words[1] + "' of type '" + vtype + "'.", 71, lcount, fname);
                                        }
                                        break;
                                    case "bool":
                                        if (bool.TryParse(Console.ReadLine(), out bool vBool))
                                        {
                                            BoolVar[words[1]] = vBool;
                                            vputlcount = 0;
                                        }
                                        else
                                        {
                                            Err("Invalid input for variable '" + words[1] + "' of type '" + vtype + "'.", 71, lcount, fname);
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
                            for (int i = 1; i < l - 1; ++i) { Put(words[i] + " "); }
                            Put(words[l - 1]);
                            vputlcount++;
                            if (SpaceTerminated) Put(" ");
                            break;
                        case "putsl":
                            if (vputlcount != 0) Put("\n");
                            int le = words.Length;
                            for (int i = 1; i < le - 1; ++i) { Put(words[i] + " "); }
                            Put(words[le - 1]);
                            if (SpaceTerminated) Put(" ");
                            vputlcount++;
                            break;
                        case "putnl":
                            Put("\n");
                            break;
                        case "fwrites":
                            int lstren = words.Length;
                            try
                            {
                                string outstr = "";
                                for (int iles = 2; iles < lstren; iles++)
                                {
                                    outstr += words[iles];
                                    if (iles != lstren - 1)
                                    {
                                        outstr += " ";
                                    }
                                }
                                if (SpaceTerminated) outstr += " ";
                                System.IO.File.WriteAllText(words[1], outstr);
                            }
                            catch (Exception FErr)
                            {
                                Err("Error writing to file " + words[1] + ".", 69, lcount, fname);
                            }
                            break;
                        case "fputs":
                            int lstrenb = words.Length;
                            try
                            {
                                string outstrb = "";
                                for (int iles = 2; iles < lstrenb; iles++)
                                {
                                    outstrb += words[iles];
                                    if (iles != lstrenb - 1)
                                    {
                                        outstrb += " ";
                                    }
                                }
                                if (SpaceTerminated) outstrb += " ";
                                System.IO.File.AppendAllText(words[1], outstrb);
                            }
                            catch (Exception FErr)
                            {
                                Err("Error writing to file " + words[1] + ".", 69, lcount, fname);
                            }
                            break;
                        case "fgets":
                            int lstrenc = words.Length;
                            string outstrc = "";
                            try
                            {
                                outstrc = System.IO.File.ReadAllText(words[1]);
                            }
                            catch (Exception FErr)
                            {
                                Err("Error reading from file " + words[1] + ".", 68, lcount, fname);
                            }
                            Put(outstrc + "\n");
                            break;
                        case "fgetsl":
                            int lstrend = words.Length;
                            string[] outstrd = { "" };
                            try
                            {
                                outstrd = System.IO.File.ReadAllLines(words[1]);
                            }
                            catch (Exception FErr)
                            {
                                return Err("Error reading from file " + words[1] + ".", 68, lcount, fname);
                            }
                            foreach (string outline in outstrd)
                            {
                                Put(outline);
                                if (outline[^1] == '\n') Put("\n");
                                else Put(" ");
                            }
                            break;
                        case "awaitkey":
                            Put("\n");
                            Console.ReadKey();
                            break;
                        default:
                            if (words[0] == "com" || words[0] == "//")
                            {
                                break;
                            }
                            else if (TypeMap.TryGetValue(words[0], out string vType))
                            {
                                if (words.Length != 2)
                                {
                                    return Err("Unidentified variable assignment.", 82, lcount, fname);
                                }
                                if (vType == "int")
                                {
                                }
                            }
                            else
                            {
                                return Err("Unidentified statement.", 81, lcount, fname);
                            }
                            break;
                    }
                else;
                lcount++;
                SpaceTerminated = false;
            }
             
            return ret;
        }
    }
}
