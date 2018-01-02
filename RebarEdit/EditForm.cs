using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace RebarEdit
{
    public partial class EditForm : Form
    {
        public double cover = 20;


        public string res
        {
            get
            {
                return ResLab.Text;
            }
            set
            {
                ResLab.Text = value;
            }
        }

        private Data mdata;
        public EditForm(Data data)
        {
            mdata = data;

            InitializeComponent();
        }

        private void EditForm_Load(object sender, EventArgs e)
        {
            BHlabel.Text = mdata.beamdata["bnth"] + "   " + mdata.beamdata["bb"] + "X" + mdata.beamdata["bh"];
            OrginLab.Text = "原:" + mdata.strRebar;
            OrginValueLab.Text = "原:" + RebarPro.Rebar2Vaule(RebarPro.ReplaceRebar(mdata.strRebar));
            ResLab.Text = "现:" + mdata.strRebar;
            ResValueLab.Text = "现:" + RebarPro.Rebar2Vaule(RebarPro.ReplaceRebar(mdata.strRebar));
            RebarBox.Text = RebarPro.ReplaceRebar(mdata.strRebar);



            string rbtype = RebarPro.ReplaceRebar(mdata.strRebar);
            this.MouseWheel += new System.Windows.Forms.MouseEventHandler(all_MouseWheel);
            getadfe(rbtype);

            getrate();



            makelayout();
            checklayout();
            RebarBox.Focus();
            RebarBox.SelectAll();


        }


        private void getrate()
        {
            try
            {
                double beamarea = (Convert.ToInt32(mdata.beamdata["bb"]) - cover)
                    * Convert.ToInt32(mdata.beamdata["bh"]);
                double leftarea = Convert.ToInt32(RebarPro.Rebar2Vaule(RebarPro.ReplaceRebar(mdata.beamdata["bleft"])));
                double rightarea = Convert.ToInt32(RebarPro.Rebar2Vaule(RebarPro.ReplaceRebar(mdata.beamdata["bright"])));
                double bomarea = Convert.ToInt32(RebarPro.Rebar2Vaule(RebarPro.ReplaceRebar(mdata.beamdata["bbj"])));
                double toparea = Convert.ToInt32(RebarPro.Rebar2Vaule(RebarPro.ReplaceRebar(mdata.beamdata["btj"])));

                string tagname = mdata.tagName;
                switch (tagname)
                {
                    case "单梁支座上部纵筋（左）":
                        leftarea = Convert.ToInt32(RebarPro.Rebar2Vaule(RebarBox.Text));
                        break;
                    case "单梁支座上部纵筋（右）":
                        rightarea = Convert.ToInt32(RebarPro.Rebar2Vaule(RebarBox.Text));
                        break;
                    case "单梁下部纵筋":
                        bomarea = Convert.ToInt32(RebarPro.Rebar2Vaule(RebarBox.Text));
                        break;
                }


                try
                {
                    double leftrate = leftarea / beamarea;
                    double rightrate = rightarea / beamarea;
                    double bomrate = bomarea / beamarea;

                    TopLeftRatelabel.Text = leftrate.ToString("P");
                    TopRightRatelabel.Text = rightrate.ToString("P");
                    BomRatelabel.Text = bomrate.ToString("P");

                    if (leftrate > 0.02)
                    { TopLeftRatelabel.ForeColor = Color.MediumVioletRed; if (leftrate > 0.025) { TopLeftRatelabel.ForeColor = Color.Red; } }
                    else
                    { TopLeftRatelabel.ForeColor = Color.Black; }
                    if (rightrate > 0.02)
                    { TopRightRatelabel.ForeColor = Color.MediumVioletRed; if (rightrate > 0.025) { TopRightRatelabel.ForeColor = Color.Red; } }
                    else
                    { TopRightRatelabel.ForeColor = Color.Black; }
                    if (bomrate > 0.02)
                    { BomRatelabel.ForeColor = Color.MediumVioletRed; if (bomrate > 0.025) { BomRatelabel.ForeColor = Color.Red; } }
                    else
                    { BomRatelabel.ForeColor = Color.Black; }
                }
                catch { }
                try
                {
                    double left6332 = bomarea / leftarea;
                    double right6332 = bomarea / rightarea;

                    KG6332label.Text = "抗规6.3.3.2:\r\n" + "左:" + left6332.ToString("f2") + "     右:" + right6332.ToString("f2");

                    if (left6332 < 0.5 || right6332 < 0.5)
                    {
                        KG6332label.ForeColor = Color.OrangeRed; if (left6332 < 0.3 || right6332 < 0.3) { KG6332label.ForeColor = Color.Red; }
                    }
                    else
                    { KG6332label.ForeColor = Color.Black; }
                }

                catch { }
                try
                {
                    double top6431 = Math.Max(leftarea, rightarea) / toparea;
                    KG6341label.Text = "抗规6.3.4.1:\r\n1/" + top6431.ToString("f2");
                    if (top6431 > 4.05)
                    { KG6341label.ForeColor = Color.Red; }
                    else
                    { KG6341label.ForeColor = Color.Black; }
                }
                catch { }

            }
            catch { }


        }

        private void getadfe(string rbtype)
        {

            try
            {
                if (rbtype.Contains('@'))
                {

                    rbtype = rbtype.Substring(0, 1) + rbtype.Substring(rbtype.Length - 2, 1);

                    guLab(rbtype);
                }
                else
                {
                    rbtype = Regex.Replace(rbtype, "[0-9/() ]", "", RegexOptions.IgnoreCase);

                    rbtype = rbtype.Substring(rbtype.Length - 1, 1);
                    zongLab(rbtype);
                }
            }
            catch
            {
                rbtype = "e";
                zongLab(rbtype);
            }
        }



        private void guLab(string rbtype)
        {
            this.gjpanel.Controls.Clear();
            gjpanel.Name = "gupanel";
            string limb;
            limb = rbtype.Substring(1);
            rbtype = rbtype.Substring(0, 1);

            Label[,] gulabs = new Label[9, 11];
            int[] rbdiam = { 6, 8, 10, 12 };
            string[] gap = { "100", "120", "150", "200", "100/150", "150/200", "100/200" };
            for (int j = 0; j < 7; j++)
            {
                for (int i = 0; i < 4; i++)
                {
                    gulabs[i, j] = new Label(); //这一句往往为初学者忽视，须知要创建对象的实例！
                    gulabs[i, j].Location = new System.Drawing.Point(119 * i, 24 * j);
                    gulabs[i, j].Name = "guLab";
                    gulabs[i, j].Size = new System.Drawing.Size(120, 25);

                    gulabs[i, j].BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
                    gulabs[i, j].BackColor = Color.LightGray;

                    gulabs[i, j].TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                    gulabs[i, j].Font = new Font("Arial", 10, FontStyle.Regular);
                    gulabs[i, j].Text = rbtype + rbdiam[i] + "@" + gap[j] + "(" + limb + ")";
                    //+(j + 1).ToString() + rbtype + rbdiam[i];

                    gulabs[i, j].Click += new System.EventHandler(this.labs_Click); //统一的事件处理
                    gulabs[i, j].MouseEnter += new System.EventHandler(this.labs_MouseEnter); //统一的事件处理
                    gulabs[i, j].MouseLeave += new System.EventHandler(this.labs_MouseLeave); //统一的事件处理
                    gjpanel.Controls.Add(gulabs[i, j]); //在窗体上呈现控件        

                }
            }
        }

        private void zongLab(string rbtype)
        {
            this.gjpanel.Controls.Clear();
            gjpanel.Name = "zongpanel";
            Label[,] zonglabs = new Label[10, 11];
            int[] rbdiam = { 10, 12, 14, 16, 18, 20, 22, 25, 28, 32 };

            for (int j = 0; j < 11; j++)
            {
                for (int i = 0; i < 10; i++)
                {
                    zonglabs[i, j] = new Label(); //这一句往往为初学者忽视，须知要创建对象的实例！
                    zonglabs[i, j].Location = new System.Drawing.Point(49 * i, 24 * j);
                    zonglabs[i, j].Name = "zongLab";
                    zonglabs[i, j].Size = new System.Drawing.Size(50, 25);

                    zonglabs[i, j].BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
                    zonglabs[i, j].BackColor = Color.LightGray;

                    zonglabs[i, j].TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                    zonglabs[i, j].Font = new Font("Arial", 10, FontStyle.Regular);
                    zonglabs[i, j].Text = (j + 1).ToString() + rbtype + rbdiam[i];

                    zonglabs[i, j].Click += new System.EventHandler(this.labs_Click); //统一的事件处理
                    zonglabs[i, j].MouseEnter += new System.EventHandler(this.labs_MouseEnter); //统一的事件处理
                    zonglabs[i, j].MouseLeave += new System.EventHandler(this.labs_MouseLeave); //统一的事件处理

                    gjpanel.Controls.Add(zonglabs[i, j]); //在窗体上呈现控件        

                }
            }

        }


        private void labs_Click(object sender, System.EventArgs e)
        {
            MouseEventArgs Mouse_e = (MouseEventArgs)e;

            if (Mouse_e.Button == MouseButtons.Left)
            {
                string rbstr = RebarBox.Text;
                string instr = ((Label)sender).Text;
                if (((Label)sender).Name == "lyLab")
                {
                    if (RebarBox.Text.Contains(" "))
                    { RebarBox.Text = RebarBox.Text.Substring(0, RebarBox.Text.IndexOf(" ")); }
                    RebarBox.Text = RebarBox.Text + " " + instr;
                }
                else
                {
                    if (this.gjpanel.Name == "zongpanel")
                    {

                        int oldstart = RebarBox.SelectionStart;
                        int oldlength = RebarBox.SelectionLength;
                        string newstr = rbstr.Insert(oldstart + oldlength, instr);
                        newstr = newstr.Remove(oldstart, oldlength);
                        RebarBox.Text = newstr; //通过sender判断激发事件的控件
                        RebarBox.SelectionStart = oldstart;
                        RebarBox.SelectionLength = instr.Length;
                    }
                    else
                    {
                        RebarBox.Text = instr;
                        RebarBox.SelectionStart = 0;
                        RebarBox.SelectionLength = instr.Length;
                    }
                }
            }


            if (Mouse_e.Button == MouseButtons.Right)
            {
                this.DialogResult = DialogResult.OK;
            }

        }

        private void all_MouseWheel(object sender, MouseEventArgs e)
        {
            if (this.gjpanel.Name == "zongpanel")
            {
                RebarBox.Focus();

                int[] allpos = RebarPro.RebarSelect(RebarBox.Text);
                int start = RebarBox.SelectionStart;
                int end = RebarBox.SelectionStart + RebarBox.SelectionLength;
                if (e.Delta > 0)
                {
                    RebarBox.Select(allpos[0], allpos[1] - allpos[0] + 1);
                }
                else
                {
                    //string str = string.Join(",", RebarPro.RebarSelect(RebarBox.Text));


                    int[] newpos = RebarPro.PosProcess(start, end, allpos);
                    RebarBox.Select(newpos[0], newpos[1] - newpos[0] + 1);
                }


            }
        }


        private void labs_MouseEnter(object sender, System.EventArgs e)
        {
            ((Label)sender).BackColor = Color.LightGreen; //通过sender判断激发事件的控件
        }

        private void labs_MouseLeave(object sender, System.EventArgs e)
        {
            ((Label)sender).BackColor = Color.LightGray; //通过sender判断激发事件的控件
        }

        private void RebarBox_TextChanged(object sender, EventArgs e)
        {

            try
            {
                makelayout();
                checklayout();
                getrate();
                ResLab.Text = RebarPro.ReplaceChr(RebarBox.Text);
                ResValueLab.Text = RebarPro.Rebar2Vaule(RebarBox.Text);
                if (this.gjpanel.Name == "gupanel")
                {
                    string rbtype = RebarBox.Text;

                    foreach (Control control in this.gjpanel.Controls)
                    {
                        rbtype = rbtype.Substring(rbtype.Length - 2, 1) + ")";
                        control.Text = control.Text.Substring(0, control.Text.Length - 2) + rbtype;

                    }
                }


            }

            catch
            {
                ResLab.Text = "现:";
                ResValueLab.Text = "现:";
            }


        }


        private void EditForm_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right )
            {
                this.DialogResult = DialogResult.OK;
            }
        }

        private void zjbutton_Click(object sender, EventArgs e)
        {

            getadfe("2e18");
        }

        private void gjbutton_Click(object sender, EventArgs e)
        {

            getadfe("f8@100(2)");

        }

        private void limbplusbutton_Click(object sender, EventArgs e)
        {

            if (this.gjpanel.Name == "gupanel")
            {
                string rbtype = RebarBox.Text;
                int limb = Convert.ToInt32(rbtype.Substring(rbtype.Length - 2, 1)) + 1;
                rbtype = limb + ")";
                RebarBox.Text = RebarBox.Text.Substring(0, RebarBox.Text.Length - 2) + rbtype;

            }
        }

        private void limbminusbutton_Click(object sender, EventArgs e)
        {
            if (this.gjpanel.Name == "gupanel")
            {
                string rbtype = RebarBox.Text;
                int limb = Convert.ToInt32(rbtype.Substring(rbtype.Length - 2, 1)) - 1;
                rbtype = limb + ")";
                RebarBox.Text = RebarBox.Text.Substring(0, RebarBox.Text.Length - 2) + rbtype;

            }

        }

        private void jiabutton_Click(object sender, EventArgs e)
        {
            RebarBox.Text = RebarBox.Text + "+";
            RebarBox.Focus();
            RebarBox.SelectionStart = RebarBox.Text.Length;
        }

        private void fenbutton_Click(object sender, EventArgs e)
        {
            RebarBox.Text = RebarBox.Text + "/";
            RebarBox.Focus();
            RebarBox.SelectionStart = RebarBox.Text.Length;
        }

        private void kuobutton_Click(object sender, EventArgs e)
        {
            string rbstr = RebarBox.Text;
            string instr = "(" + RebarBox.SelectedText + ")";
            int oldstart = RebarBox.SelectionStart;
            int oldlength = RebarBox.SelectionLength;
            string newstr = rbstr.Insert(oldstart + oldlength, instr);
            newstr = newstr.Remove(oldstart, oldlength);
            RebarBox.Text = newstr; //通过sender判断激发事件的控件
            RebarBox.SelectionStart = oldstart;
            RebarBox.SelectionLength = instr.Length;

        }

        private void makelayout()
        {
            try
            {
                if (!RebarBox.Text.Contains("@") && !RebarBox.Text.Contains("("))
                {


                    this.layoutpanel.Controls.Clear();

                    string rbstr = RebarBox.Text;
                    try { rbstr = rbstr.Substring(0, rbstr.IndexOf(" ")); }
                    catch { }
                    rbstr = RebarPro.ReplaceXing(rbstr);

                    string[] strArray = rbstr.Split(new char[] { '*' });
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


                    if (rbnum > 3)
                    {
                        int rbnum1 = rbnum - 2;
                        int rbnum2 = rbnum / 2;

                        List<string> lylist = new List<string>();

                        if (rbnum1 - rbnum2 >= 1)
                        {
                            for (i = 2; i <= rbnum2; i++)
                            {
                                if (mdata.tagName.Contains("下部纵筋"))
                                {
                                    lylist.Add(i.ToString() + "/" + (rbnum - i).ToString());
                                }
                                else
                                {
                                    lylist.Add((rbnum - i).ToString() + "/" + i.ToString());
                                }
                            }
                        }

                        else
                        {
                            lylist.Add(rbnum1.ToString() + "/" + rbnum2.ToString());
                        }
                        Label[] lylablist = new Label[lylist.Count];

                        try
                        {
                            int iwidth = 0;
                            int ihight = 0;
                            for (i = 0; i < lylablist.Count(); i++)
                            {
                                if (lylablist.Count() > 3 && i > 3)
                                {
                                    iwidth = 1;
                                    ihight = -96;
                                }
                                lylablist[i] = new Label(); ;
                                lylablist[i].Name = "lyLab";
                                lylablist[i].Size = new System.Drawing.Size(35, 25);
                                lylablist[i].Text = lylist[i];
                                lylablist[i].Location = new System.Drawing.Point(10 + 35 * iwidth, 24 * i + ihight);
                                lylablist[i].BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
                                lylablist[i].BackColor = Color.LightGray;

                                lylablist[i].TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                                lylablist[i].Font = new Font("Arial", 10, FontStyle.Regular);

                                lylablist[i].Click += new System.EventHandler(this.labs_Click); //统一的事件处理
                                lylablist[i].MouseEnter += new System.EventHandler(this.labs_MouseEnter); //统一的事件处理
                                lylablist[i].MouseLeave += new System.EventHandler(this.labs_MouseLeave); //统一的事件处理

                                layoutpanel.Controls.Add(lylablist[i]); //在窗体上呈现控件        

                            }


                        }
                        catch { }

                    }
                }
            }
            catch { }
        }

        private void checklayout()
        {

            try
            {
                double beamb = Convert.ToInt32(mdata.beamdata["bb"]);
                string rbleft = RebarPro.ReplaceLo(RebarPro.ReplaceRebar(mdata.beamdata["bleft"]));
                string rbright = RebarPro.ReplaceLo(RebarPro.ReplaceRebar(mdata.beamdata["bright"]));
                string rbbom = RebarPro.ReplaceLo(RebarPro.ReplaceRebar(mdata.beamdata["bbj"]));
                string tagname = mdata.tagName;
                switch (tagname)
                {
                    case "单梁支座上部纵筋（左）":
                        rbleft = RebarPro.ReplaceLo(RebarBox.Text);
                        break;
                    case "单梁支座上部纵筋（右）":
                        rbright = RebarPro.ReplaceLo(RebarBox.Text);
                        break;
                    case "单梁下部纵筋":
                        rbbom = RebarPro.ReplaceLo(RebarBox.Text);
                        break;
                }


                string resleft = "";
                string resright = "";
                string resbom = "";
                bool boolleft = true;
                bool boolright = true;
                bool booltbom = true;

                foreach (string t1 in RebarPro.layoutdata(rbleft))
                {
                    resleft = resleft + RebarPro.lobool(t1, cover, true) + "/";
                    boolleft = boolleft & (RebarPro.lobool(t1, cover, true) < beamb);
                }
                foreach (string t2 in RebarPro.layoutdata(rbright))
                {
                    resright = resright + RebarPro.lobool(t2, cover, true) + "/";
                    boolright = boolright & (RebarPro.lobool(t2, cover, true) < beamb);

                }
                foreach (string t3 in RebarPro.layoutdata(rbbom))
                {
                    resbom = resbom + RebarPro.lobool(t3, cover, false) + "/";
                    booltbom = booltbom & (RebarPro.lobool(t3, cover, false) < beamb);

                }
                resleft = resleft.Substring(0, resleft.Length - 1);
                resright = resright.Substring(0, resright.Length - 1);
                resbom = resbom.Substring(0, resbom.Length - 1);

                Loleftlabel.Text = resleft.ToString();
                Lorightlabel.Text = resright.ToString();
                Lobomlabel.Text = resbom.ToString();



                if (!boolleft)
                { Loleftlabel.ForeColor = Color.Red; }
                else
                { Loleftlabel.ForeColor = Color.Black; }
                if (!boolright)
                { Lorightlabel.ForeColor = Color.Red; }
                else
                { Lorightlabel.ForeColor = Color.Black; }
                if (!booltbom)
                { Lobomlabel.ForeColor = Color.Red; }
                else
                { Lobomlabel.ForeColor = Color.Black; }
            }
            catch { }



        }

        private void gjpanel_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                this.DialogResult = DialogResult.OK;
            }
        }
    }
}
