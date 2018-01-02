using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace RebarEdit
{
    public class RebarPro
    {



        static public int CountChar(string str, char s)
        {
            return str.Split(s).Length - 1;
        }

        static public string ReplaceRebar(string str)
        {

            str = str.Replace("", "a");
            str = str.Replace("", "d");
            str = str.Replace("", "f");
            str = str.Replace("", "e");
            return str;
        }

        static public string ReplaceChr(string str)
        {

            str = str.Replace("a", "");
            str = str.Replace("d", "");
            str = str.Replace("f", "");
            str = str.Replace("e", "");
            return str;
        }


        static public string Rebar2Vaule(string RebarStr)
        {
            try
            {
                if (RebarStr.Contains(" "))
                {
                    RebarStr = RebarStr.Substring(0, RebarStr.IndexOf(" "));
                }
                if (RebarStr.Contains("@"))
                {

                    RebarStr = RebarStr.Replace("a", "");
                    RebarStr = RebarStr.Replace("d", "");
                    RebarStr = RebarStr.Replace("e", "");
                    RebarStr = RebarStr.Replace("f", "");
                    double[] PeiJin = new double[2];
                    string[] strArray = RebarStr.Split(new char[] { '@', '/', '(', ')' });
                    string res;

                    if (2 == CountChar(RebarStr, '@'))
                    {
                        PeiJin[0] = Math.Pow(Convert.ToDouble(strArray[0]), 2) / 4 * Math.PI / Convert.ToDouble(strArray[1]) * Convert.ToDouble(strArray[2]) * 100;
                        PeiJin[1] = Math.Pow(Convert.ToDouble(strArray[4]), 2) / 4 * Math.PI / Convert.ToDouble(strArray[5]) * Convert.ToDouble(strArray[6]) * 100;
                        PeiJin[0] = Convert.ToInt16(PeiJin[0]);
                        PeiJin[1] = Convert.ToInt16(PeiJin[1]);
                        res = PeiJin[0].ToString() + "/" + PeiJin[1].ToString();
                    }
                    else if (1 == CountChar(RebarStr, '/'))
                    {

                        PeiJin[0] = Math.Pow(Convert.ToDouble(strArray[0]), 2) / 4 * Math.PI / Convert.ToDouble(strArray[1]) * Convert.ToDouble(strArray[3]) * 100;
                        PeiJin[1] = Math.Pow(Convert.ToDouble(strArray[0]), 2) / 4 * Math.PI / Convert.ToDouble(strArray[2]) * Convert.ToDouble(strArray[3]) * 100;
                        PeiJin[0] = Convert.ToInt16(PeiJin[0]);
                        PeiJin[1] = Convert.ToInt16(PeiJin[1]);
                        res = PeiJin[0].ToString() + "/" + PeiJin[1].ToString();
                    }
                    else
                    {

                        PeiJin[0] = Math.Pow(Convert.ToDouble(strArray[0]), 2) / 4 * Math.PI / Convert.ToDouble(strArray[1]) * Convert.ToDouble(strArray[2]) * 100;
                        PeiJin[0] = Convert.ToInt16(PeiJin[0]);
                        res = PeiJin[0].ToString();
                    }

                    return res;
                }
                else
                {
                    string res;
                    RebarStr = RebarStr.Replace("N", "");
                    RebarStr = RebarStr.Replace("a", "*");
                    RebarStr = RebarStr.Replace("d", "*");
                    RebarStr = RebarStr.Replace("f", "*");
                    RebarStr = RebarStr.Replace("e", "*");

                    RebarStr = Regex.Replace(RebarStr, @"\+[^\+]*\)", "");
                    RebarStr = Regex.Replace(RebarStr, @"(?<=\s)\S+$", "");

                    string[] strArray = RebarStr.Split(new char[] { '*', '/', '+' });

                    double PeiJin = 0;
                    //int i = 0;
                    int max = strArray.Count() / 2;
                    if (max == 1)
                    {
                        PeiJin = Math.Pow(Convert.ToDouble(strArray[1]), 2) / 4 * Math.PI * Convert.ToDouble(strArray[0]);

                    }
                    if (max == 1)
                    {
                        PeiJin = Math.Pow(Convert.ToDouble(strArray[1]), 2) / 4 * Math.PI * Convert.ToDouble(strArray[0]);

                    }
                    if (max == 2)
                    {
                        PeiJin = Math.Pow(Convert.ToDouble(strArray[1]), 2) / 4 * Math.PI * Convert.ToDouble(strArray[0])
                            + Math.Pow(Convert.ToDouble(strArray[3]), 2) / 4 * Math.PI * Convert.ToDouble(strArray[2]);

                    }
                    if (max == 3)
                    {
                        PeiJin = Math.Pow(Convert.ToDouble(strArray[1]), 2) / 4 * Math.PI * Convert.ToDouble(strArray[0])
                            + Math.Pow(Convert.ToDouble(strArray[3]), 2) / 4 * Math.PI * Convert.ToDouble(strArray[2])
                        + Math.Pow(Convert.ToDouble(strArray[5]), 2) / 4 * Math.PI * Convert.ToDouble(strArray[4]);
                    }
                    //for (i = 0; i <= max + 1; i = i + 2)
                    //{
                    //    PeiJin = PeiJin + Math.Pow(Convert.ToDouble(strArray[i + 1]), 2) / 4 * Math.PI * Convert.ToDouble(strArray[i]);
                    //}

                    PeiJin = Convert.ToInt16(PeiJin);
                    res = PeiJin.ToString();

                    return res;

                }




            }
            catch
            {
                return "error";
            }

        }

        static public int[] RebarSelect(string RebarStr)
        {

            List<int> pos = new List<int>();

            for (int i = 0; i < RebarStr.Length; i++)
            {
                if ((RebarStr.IndexOf('+', i)) != -1)
                {
                    pos.Add(RebarStr.IndexOf('+', i) - 1);
                    pos.Add(RebarStr.IndexOf('+', i) + 1);

                }
                if ((RebarStr.IndexOf('/', i)) != -1)
                {
                    pos.Add(RebarStr.IndexOf('/', i) - 1);
                    pos.Add(RebarStr.IndexOf('/', i) + 1);

                }
                if ((RebarStr.IndexOf(' ', i)) != -1)
                {
                    pos.Add(RebarStr.IndexOf(' ', i) - 1);
                    pos.Add(RebarStr.IndexOf(' ', i) + 1);

                }

            }
            pos.Add(0);
            pos.Add(RebarStr.Length - 1);
            int[] res = pos.ToArray();
            Array.Sort(res);
            res = res.Distinct().ToArray();

            return res;
        }

        static public int[] PosProcess(int start, int end, int[] pos)
        {
            int[] res = new int[2];

            try
            {


                for (int i = 0; i < pos.Length; i++)
                {
                    if (end > pos[i])
                    {
                        res[0] = pos[i + 1];
                        res[1] = pos[i + 2];
                    }

                }

            }
            catch
            {
                res[0] = pos[0];
                res[1] = pos[1];
            }
            return res;
        }

        static public string ReplaceXing(string RebarStr)
        {
            RebarStr = RebarStr.Replace("N", "");
            RebarStr = RebarStr.Replace("a", "*");
            RebarStr = RebarStr.Replace("d", "*");
            RebarStr = RebarStr.Replace("f", "*");
            RebarStr = RebarStr.Replace("e", "*");
            RebarStr = RebarStr.Replace("+", "*");
            RebarStr = RebarStr.Replace("/", "*");
            RebarStr = RebarStr.Replace("(", "*");
            RebarStr = RebarStr.Replace(")", "*");
            return RebarStr;
        }

        static public string ReplaceLo(string RebarStr)
        {
            RebarStr = RebarStr.Replace("N", "");
            RebarStr = RebarStr.Replace("a", "*");
            RebarStr = RebarStr.Replace("d", "*");
            RebarStr = RebarStr.Replace("f", "*");
            RebarStr = RebarStr.Replace("e", "*");
            RebarStr = RebarStr.Replace("+", "*");
            return RebarStr;
        }

        static public List<string> layoutdata(string RebarStr)
        {
            List<string> data = new List<string>();
            if (RebarStr.Contains(" "))
            {
                string lonum = RebarStr.Substring(RebarStr.IndexOf(" "), RebarStr.Length - RebarStr.IndexOf(" "));
                string rbstr = RebarStr.Substring(0, RebarStr.IndexOf(" "));
                lonum = lonum.Replace(" ", "");

                string[] lonumlst = lonum.Split(new char[] { '/' });

                foreach (string lostr in lonumlst)
                {
                    data.Add(lostr + rblounion(rbstr));
                }



            }
            else if (RebarStr.Contains("/"))
            {
                string[] rbstrlist = RebarStr.Split(new char[] { '/' });
                foreach (string rbstr in rbstrlist)
                {
                    data.Add(rbstr);
                }
            }
            else
            {

                data.Add(RebarStr);

            }


            return data;
        }

        static public string rblounion(string Rebarstr)
        {
            string[] strArray = Rebarstr.Split(new char[] { '*' });
            int numstr = strArray.Count();

            int rbnum = 0;
            int i;
            int max = 0;
            for (i = 0; i < numstr; i = i + 2)
            {
                rbnum = rbnum + Convert.ToInt32(strArray[i]);
                if (Convert.ToInt32(strArray[i + 1]) > max)
                { max = Convert.ToInt32(strArray[i + 1]); }
            }
            return "*" + max.ToString();
        }
        static public double lobool(string Rebarstr, double cover, bool booltop)
        {
            double lth, klth;
            if (booltop)
            {
                lth = 30;
                klth = 1.5;
            }
            else
            {
                lth = 25;
                klth = 1;
            }

            string[] strArray = Rebarstr.Split(new char[] { '*' });
            int numstr = strArray.Count();

            double totallth = 0;
            int i;
            for (i = 0; i < numstr; i = i + 2)
            {
                totallth = cover * 2 + totallth 
                    +( Convert.ToInt32(strArray[i])-1) * Math.Max(klth * Convert.ToInt32(strArray[i + 1]), lth)
                    + Convert.ToInt32(strArray[i]) * Convert.ToInt32(strArray[i + 1]);
            }
          
            return totallth;
        }
    }
}
