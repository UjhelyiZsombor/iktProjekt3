using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Projekt_3
{
    public partial class Form1 : Form
    {
        string[] GombFeliratok = Beolvasas("GombFeliratok.txt");
        string[] KategoriaFeliratok = Beolvasas("LabelFeliratok.txt");
        Button[] KategoriaGombok = new Button[4];
        Button Vissza;
        Button[] Gombok = new Button[8];
        double[] MostTomb;
        Random r = new Random();
        TabControl TabControl;
        TabPage MegszamolasPage;
        TabPage EldontesPage;
        Panel Panel;
        ListBox KodMegj;
        string[][] PszKodok = new string[8][]; 

        Button Fomenu;
        Button Kod;
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            Text = "Alaptetelek";
            Font = new Font("Segoe UI", 12f);
            Size = new Size(620, 500);
            FormBorderStyle = FormBorderStyle.Fixed3D;
            MaximizeBox = false;

            Panel = new Panel()
            {
                Parent = this,
                Location = new Point(0, 0),
                Size = new Size(Width, Height),
            };
            KodMegj = new ListBox()
            {
                Parent = this,
                Size = new Size(500, Height),
                BackColor = Color.LightGray,
                Visible = false
            };
            for (int i = 0; i < KategoriaGombok.Length; i++)
            {
                KategoriaGombok[i] = new Button()
                {
                    Parent = Panel,
                    Text = $"{KategoriaFeliratok[i]}",
                    Size = new Size(200, 50),
                    Location = new Point(Panel.Width / 2 - 100, Panel.Height / 2 - 160 + i * 60),
                    TextAlign = ContentAlignment.MiddleCenter,
                };
                KategoriaGombok[i].Click += KategoriaGombok_Click;
            }
            Vissza = new Button()
            {
                Parent = Panel,
                Text = "Kilépés",
                Size = new Size(200, 50),
                Location = new Point(Panel.Width / 2 - 100, KategoriaGombok[3].Location.Y + 60),
                TextAlign = ContentAlignment.MiddleCenter,
            };
            Vissza.Click += (senderr, args) => Application.Exit();

            Fomenu = new Button()
            {
                Text = "Főmenü",
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter
            };
            Fomenu.Click += Fomenu_Click;

            Kod = new Button()
            {
                Text = "Kód",
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter
            };
            Kod.Click += Kod_Click;

            TabControl = new TabControl()
            {
                Parent = this,
                Size = new Size(600, Height),
                Location = new Point(0, 0),
                Visible = false,
            };
        }
        private void Kod_Click(object sender, EventArgs e)
        {
            if (KodMegj.Visible == false)
            {
                KodMegj.Visible = true;
                KodMegj.Location = new Point(TabControl.Width, 0);
                Width += 500;
                Kodok(Beolvasas("PszpszKodok.txt"));
                KodMegj.DataSource = PszKodok[7];
            }
            else
            {
                KodMegj.Visible = false;
                Width -= 500;
            }
        }
        private void Fomenu_Click(object sender, EventArgs e)
        {
            Panel.Visible = true;
            TabControl.Visible = false;
            TabControl.TabPages.Clear();
        }
        private void KategoriaGombok_Click(object senderr, EventArgs e)
        {
            Button sender = senderr as Button;
            if (sender.Text == "Elemi programozási tételek")
            {
                TabControl.Visible = true;
                Panel.Visible = false;
                MegszamolasPage = new TabPage("Megszámolás");
                TabControl.TabPages.Add(MegszamolasPage);
                EldontesPage = new TabPage("Eldöntés");
                TabControl.TabPages.Add(EldontesPage);
                if (TabControl.SelectedTab == MegszamolasPage)
                {
                    Megszamolas();
                    Fomenu.Parent = MegszamolasPage;
                    Fomenu.Location = new Point(MegszamolasPage.Width - 80, MegszamolasPage.Height - 80);
                    Kod.Parent = MegszamolasPage;
                    Kod.Location = new Point(MegszamolasPage.Width - 80, MegszamolasPage.Height - 80 - Fomenu.Height);
                }
            }
        }
        #region Megszamolas
        TextBox Keresett;
        TextBox Also;
        TextBox Felso;
        Label Eredmeny;
        private void Megszamolas()
        {
            Also = new TextBox()
            {
                Parent = MegszamolasPage,
                Location = new Point(20, 20),
                Size = new Size(180, 40),
                Text = "Számok alsó határa",
            };
            Also.Tag = "Also";
            Also.Enter += TextBox_Enter;
            Also.Leave += TextBox_Leave;

            Felso = new TextBox()
            {
                Parent = MegszamolasPage,
                Location = new Point(20, Also.Location.Y + Also.Size.Height + 20),
                Size = new Size(180, 40),
                Text = "Számok felső határa"
            };
            Felso.Tag = "Felso";
            Felso.Enter += TextBox_Enter;
            Felso.Leave += TextBox_Leave;

            NumericUpDown Elemszam = new NumericUpDown()
            {
                Parent = MegszamolasPage,
                Minimum = 1,
                Maximum = 10000000,
                Location = new Point(20, Felso.Location.Y + Felso.Size.Height + 20),
                Size = new Size(180, 40),
            };
            Elemszam.ValueChanged += Elemszam_ValueChanged;

            Keresett = new TextBox()
            {
                Parent = MegszamolasPage,
                Location = new Point(20, Elemszam.Location.Y + Elemszam.Size.Height + 20),
                Size = new Size(180, 40),
                Text = "Keresett elem",
                Enabled = false,
            };
            Keresett.Tag = "Keresett";
            Keresett.Enter += TextBox_Enter;
            Keresett.Leave += Keresett_Leave;

            Button OK = new Button()
            {
                Parent = MegszamolasPage,
                Location = new Point(20, Keresett.Location.Y + Keresett.Size.Height + 20),
                Size = new Size(180, 40),
                Text = "OK",
            };
            OK.Click += OK_Click;
            Eredmeny = new Label()
            {
                Parent = MegszamolasPage,
                Location = new Point(20, OK.Location.Y + OK.Size.Height + 20),
                Size = new Size(500, 40),
                Text = "Keresett elem előfordulása: ",
            };
        }

        int? also, felso, elemszam;
        double? keresett;

        private void OK_Click(object sender, EventArgs e)
        {
            if (also.HasValue && felso.HasValue && keresett.HasValue && elemszam.HasValue)
            {
                if (also > felso)
                {
                    int? temp = felso;
                    felso = also;
                    also = temp;
                }
                MostTomb = new double[(int)elemszam];
                for (int i = 0; i < elemszam; i++)
                {
                    MostTomb[i] = r.Next((int)also, (int)felso) * Math.Round(r.NextDouble() + 1, 1);
                }
                double keres = (double)keresett;
                int talalat = Tetelek.Megszamolas(MostTomb, keres);
                Eredmeny.Text = $"Keresett elem előfordulása: {talalat}db";
            }
        }
        private void Keresett_Leave(object senderr, EventArgs e)
        {
            TextBox sender = senderr as TextBox;
            if (!int.TryParse(sender.Text, out int alsoHatar) && !string.IsNullOrWhiteSpace(sender.Text))
            {
                sender.Text = "Érvénytelen érték!";
            }
            else if (string.IsNullOrWhiteSpace(sender.Text))
            {
                sender.Text = "Keresett elem";
            }
            else if (int.Parse(sender.Text) <= int.MaxValue || int.Parse(sender.Text) >= int.MinValue || int.Parse(sender.Text) > felso || int.Parse(sender.Text) < also)
            {
                keresett = int.Parse(sender.Text);
            }
        }
        private void TextBox_Leave(object senderr, EventArgs e)
        {
            string text;
            TextBox sender = senderr as TextBox;
            if (sender.Tag.ToString() == "Also")
            {
                text = "Számok alsó határa";
            }
            else
            {
                text = "Számok felső határa";
            }


            if (!int.TryParse(sender.Text, out int hatar) && !string.IsNullOrWhiteSpace(sender.Text))
            {
                sender.Text = "Érvénytelen érték!";
            }
            else if (string.IsNullOrWhiteSpace(sender.Text))
            {
                sender.Text = text;
            }
            else if (int.Parse(sender.Text) <= int.MaxValue || int.Parse(sender.Text) >= int.MinValue)
            {
                if (sender.Tag.ToString() == "Also")
                {
                    also = int.Parse(sender.Text);
                }
                else
                {
                    felso = int.Parse(sender.Text);
                }
            }
            if (also != null && felso != null)
            {
                Keresett.Enabled = true;
            }
            else
            {
                Keresett.Enabled = false;
            }
        }
        private void TextBox_Enter(object senderr, EventArgs e)
        {
            TextBox sender = senderr as TextBox;
            if (sender.Text == "Számok alsó határa" || sender.Text == "Érvénytelen érték!" || sender.Text == "Számok felső határa" || sender.Text == "Keresett elem")
            {
                sender.Text = "";
            }
        }
        private void Elemszam_ValueChanged(object senderr, EventArgs e)
        {
            NumericUpDown sender = senderr as NumericUpDown;
            elemszam = (int)sender.Value;
        }
        #endregion
        static string[] Beolvasas(string FileNev)
        {
            List<string> szoveg = new List<string>();
            var adatok = File.ReadAllLines(FileNev, Encoding.UTF8);
            for (int i = 0; i < adatok.Length; i++)
            {
                szoveg.Add(adatok[i]);
            }
            return szoveg.ToArray();
        }
        void Kodok(string[] adat)
        {
            int j = 0;

            for (int i = 0; i < adat.Length; i++)
            {
                if (adat[i] == "SEP")
                {
                    i++;
                    List<string> most = new List<string>();
                    while (i < adat.Length && adat[i] != "SEP")
                    {
                        most.Add(adat[i]);
                        i++;
                    }
                    if (most.Count > 0)
                    {
                        PszKodok[j] = most.ToArray();
                        j++;
                    }
                    i--;
                }
            }
        }
    }
}