using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Drawing.Text;
using System.Globalization;
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
        string[] KategoriaFeliratok = Beolvasas("LabelFeliratok.txt");
        Button[] KategoriaGombok = new Button[4];
        Button Vissza;
        Random r = new Random();
        TabControl TabControl;
        TabPage MegszamolasPage, EldontesPage, MasolasPage, MetszetPage, ECSPage, MinMaxPage, LinKerPage, BinKerPage;
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
            CenterToScreen();

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
            TabControl.SelectedIndexChanged += TabControl_SelectedIndexChanged;
            Kodok(Beolvasas("PszpszKodok.txt"));
        }

        private void TabControl_SelectedIndexChanged(object senderr, EventArgs e)
        {
            TabControl sender = senderr as TabControl;
            if (sender.SelectedIndex != -1)
            {
                KodMegj.DataSource = PszKodok[sender.SelectedIndex];
            }else
            {
                KodMegj.DataSource = null;
            }
        }

        private void Kod_Click(object sender, EventArgs e)
        {
            if (KodMegj.Visible == false)
            {
                KodMegj.Visible = true;
                KodMegj.Location = new Point(TabControl.Width, 0);
                Width += 500;
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
            if (KodMegj.Visible)
            {
                Width -= 500;
                KodMegj.Visible = false;
            }
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
                    KodMegj.DataSource = PszKodok[0];
                    Fomenu.Parent = MegszamolasPage;
                    Fomenu.Location = new Point(MegszamolasPage.Width - 80, MegszamolasPage.Height - 80);
                    Kod.Parent = MegszamolasPage;
                    Kod.Location = new Point(MegszamolasPage.Width - 80, MegszamolasPage.Height - 80 - Fomenu.Height);
                }
                TabControl.SelectedIndexChanged += (s, args) =>
                {
                    if (TabControl.SelectedTab == MegszamolasPage)
                    {
                        Megszamolas();
                        KodMegj.DataSource = PszKodok[0];
                        Fomenu.Parent = MegszamolasPage;
                        Fomenu.Location = new Point(MegszamolasPage.Width - 80, MegszamolasPage.Height - 80);
                        Kod.Parent = MegszamolasPage;
                        Kod.Location = new Point(MegszamolasPage.Width - 80, MegszamolasPage.Height - 80 - Fomenu.Height);
                    }
                    else if (TabControl.SelectedTab == EldontesPage)
                    {
                        Eldontes();
                        KodMegj.DataSource = PszKodok[1];
                        Fomenu.Parent = EldontesPage;
                        Fomenu.Location = new Point(EldontesPage.Width - 80, EldontesPage.Height - 80);
                        Kod.Parent = EldontesPage;
                        Kod.Location = new Point(EldontesPage.Width - 80, EldontesPage.Height - 80 - Fomenu.Height);
                    }
                };
            }
            else if (sender.Text == "Összetett programozási tételek")
            {
                TabControl.Visible = true;
                Panel.Visible = false;
                MasolasPage = new TabPage("Másolás");
                TabControl.TabPages.Add(MasolasPage);
                MetszetPage = new TabPage("Metszet");
                TabControl.TabPages.Add(MetszetPage);
                if (TabControl.SelectedTab == MasolasPage)
                {
                    Masolas();
                    KodMegj.DataSource = PszKodok[2];
                    Fomenu.Parent = MasolasPage;
                    Fomenu.Location = new Point(MasolasPage.Width - 80, MasolasPage.Height - 80);
                    Kod.Parent = MasolasPage;
                    Kod.Location = new Point(MasolasPage.Width - 80, MasolasPage.Height - 80 - Fomenu.Height);
                }
                TabControl.SelectedIndexChanged += (s, args) =>
                {
                    if (TabControl.SelectedTab == MasolasPage)
                    {
                        Masolas();
                        KodMegj.DataSource = PszKodok[2];
                        Fomenu.Parent = MasolasPage;
                        Fomenu.Location = new Point(MasolasPage.Width - 80, MasolasPage.Height - 80);
                        Kod.Parent = MasolasPage;
                        Kod.Location = new Point(MasolasPage.Width - 80, MasolasPage.Height - 80 - Fomenu.Height);
                    }
                    else if (TabControl.SelectedTab == MetszetPage)
                    {
                        Metszet();
                        KodMegj.DataSource = PszKodok[3];
                        Fomenu.Parent = MetszetPage;
                        Fomenu.Location = new Point(20, MetszetPage.Height - 80);
                        Kod.Parent = MetszetPage;
                        Kod.Location = new Point(20, MetszetPage.Height - 80 - Fomenu.Height);
                    }
                };
            }
            else if (sender.Text == "Rendezések")
            {
                TabControl.Visible = true;
                Panel.Visible = false;
                ECSPage = new TabPage("Egyszerű cserés rendezés");
                TabControl.TabPages.Add(ECSPage);
                MinMaxPage = new TabPage("MinMax rendezés");
                TabControl.TabPages.Add(MinMaxPage);
                if (TabControl.SelectedTab == ECSPage)
                {
                    ECS();
                    KodMegj.DataSource = PszKodok[4];
                    Fomenu.Parent = ECSPage;
                    Fomenu.Location = new Point(ECSPage.Width - 80, ECSPage.Height - 80);
                    Kod.Parent = ECSPage;
                    Kod.Location = new Point(ECSPage.Width - 80, ECSPage.Height - 80 - Fomenu.Height);
                }
                TabControl.SelectedIndexChanged += (s, args) =>
                {
                    if (TabControl.SelectedTab == ECSPage)
                    {
                        ECS();
                        KodMegj.DataSource = PszKodok[4];
                        Fomenu.Parent = ECSPage;
                        Fomenu.Location = new Point(ECSPage.Width - 80, ECSPage.Height - 80);
                        Kod.Parent = ECSPage;
                        Kod.Location = new Point(ECSPage.Width - 80, ECSPage.Height - 80 - Fomenu.Height);
                    }
                    else if (TabControl.SelectedTab == MinMaxPage)
                    {
                        MinMax();
                        KodMegj.DataSource = PszKodok[5];
                        Fomenu.Parent = MinMaxPage;
                        Fomenu.Location = new Point(MinMaxPage.Width - 80, MinMaxPage.Height - 80);
                        Kod.Parent = MinMaxPage;
                        Kod.Location = new Point(MinMaxPage.Width - 80, MinMaxPage.Height - 80 - Fomenu.Height);
                    }
                };
            }
            else if (sender.Text == "Keresések")
            {
                TabControl.Visible = true;
                Panel.Visible = false;
                LinKerPage = new TabPage("Lineáris keresés");
                TabControl.TabPages.Add(LinKerPage);
                BinKerPage = new TabPage("Bináris keresés");
                TabControl.TabPages.Add(BinKerPage);
                if (TabControl.SelectedTab == LinKerPage)
                {
                    LinKer();
                    KodMegj.DataSource = PszKodok[6];
                    Fomenu.Parent = LinKerPage;
                    Fomenu.Location = new Point(LinKerPage.Width - 80, LinKerPage.Height - 80);
                    Kod.Parent = LinKerPage;
                    Kod.Location = new Point(LinKerPage.Width - 80, LinKerPage.Height - 80 - Fomenu.Height);
                }
                TabControl.SelectedIndexChanged += (s, args) =>
                {
                    if (TabControl.SelectedTab == LinKerPage)
                    {
                        LinKer();
                        KodMegj.DataSource = PszKodok[6];
                        Fomenu.Parent = LinKerPage;
                        Fomenu.Location = new Point(LinKerPage.Width - 80, LinKerPage.Height - 80);
                        Kod.Parent = LinKerPage;
                        Kod.Location = new Point(LinKerPage.Width - 80, LinKerPage.Height - 80 - Fomenu.Height);
                    }
                    else if (TabControl.SelectedTab == BinKerPage)
                    {
                        BinKer();
                        KodMegj.DataSource = PszKodok[7];
                        Fomenu.Parent = BinKerPage;
                        Fomenu.Location = new Point(BinKerPage.Width - 80, BinKerPage.Height - 80);
                        Kod.Parent = BinKerPage;
                        Kod.Location = new Point(BinKerPage.Width - 80, BinKerPage.Height - 80 - Fomenu.Height);
                    }
                };
            }
        }
        #region Megszamolas
        private void Megszamolas()
        {
            NumericUpDown Also, Felso, Keresett, Elemszam;
            Label Eredmeny;

            Label[] Labelek = new Label[4];
            string[] LabelekText = { "Alsó határ:", "Felső határ:", "Elemszám:", "Keresett elem:" };
            for (int i = 0; i < Labelek.Length; i++)
            {
                Labelek[i] = new Label()
                {
                    Parent = MegszamolasPage,
                    Text = LabelekText[i],
                    AutoSize = true,
                    Location = new Point(20, 20 + i * 70)
                };
            }

            Also = new NumericUpDown()
            {
                Parent = MegszamolasPage,
                Location = new Point(20, Labelek[0].Bottom + 10),
                Size = new Size(180, 40),
                Minimum = int.MinValue,
                Maximum = int.MaxValue
            };

            Felso = new NumericUpDown()
            {
                Parent = MegszamolasPage,
                Location = new Point(20, Labelek[1].Bottom + 10),
                Size = new Size(180, 40),
                Minimum = int.MinValue,
                Maximum = int.MaxValue,
                Enabled = false
            };

            Elemszam = new NumericUpDown()
            {
                Parent = MegszamolasPage,
                Location = new Point(20, Labelek[2].Bottom + 10),
                Size = new Size(180, 40),
                Minimum = 1,
                Maximum = 10000000
            };

            Keresett = new NumericUpDown()
            {
                Parent = MegszamolasPage,
                Location = new Point(20, Labelek[3].Bottom + 10),
                Size = new Size(180, 40),
                Enabled = false
            };

            Also.ValueChanged += (s, e) =>
            {
                Felso.Minimum = Also.Value;
                Felso.Enabled = true;
            };
            Felso.ValueChanged += (s, e) =>
            {
                Keresett.Minimum = Also.Value;
                Keresett.Maximum = Felso.Value;
                Keresett.Enabled = true;
            };

            Button OK = new Button()
            {
                Parent = MegszamolasPage,
                Location = new Point(20, Keresett.Bottom + 20),
                Size = new Size(180, 40),
                Text = "OK"
            };

            Eredmeny = new Label()
            {
                Parent = MegszamolasPage,
                Location = new Point(20, OK.Bottom + 20),
                AutoSize = true,
                Text = "Keresett elem előfordulása: "
            };

            OK.Click += (s, e) =>
            {
                Eredmeny.Text = "Keresett elem előfordulása: ";
                if (Felso.Enabled && Keresett.Enabled)
                {
                    Eredmeny.Text += Tetelek.Megszamolas(TombGeneralas((int)Also.Value, (int)Felso.Value, (int)Elemszam.Value), (int)Keresett.Value);
                }
            };
        }
        #endregion
        #region Eldontes
        private void Eldontes()
        {
            NumericUpDown Also, Felso, Keresett, Elemszam;
            Label Eredmeny;

            Label[] Labelek = new Label[4];
            string[] LabelekText = { "Alsó határ:", "Felső határ:", "Elemszám:", "Keresett elem:" };
            for (int i = 0; i < Labelek.Length; i++)
            {
                Labelek[i] = new Label()
                {
                    Parent = EldontesPage,
                    Text = LabelekText[i],
                    AutoSize = true,
                    Location = new Point(20, 20 + i * 70)
                };
            }

            Also = new NumericUpDown()
            {
                Parent = EldontesPage,
                Location = new Point(20, Labelek[0].Bottom + 10),
                Size = new Size(180, 40),
                Minimum = int.MinValue,
                Maximum = int.MaxValue
            };

            Felso = new NumericUpDown()
            {
                Parent = EldontesPage,
                Location = new Point(20, Labelek[1].Bottom + 10),
                Size = new Size(180, 40),
                Minimum = int.MinValue,
                Maximum = int.MaxValue,
                Enabled = false
            };

            Elemszam = new NumericUpDown()
            {
                Parent = EldontesPage,
                Location = new Point(20, Labelek[2].Bottom + 10),
                Size = new Size(180, 40),
                Minimum = 1,
                Maximum = 10000000
            };

            Keresett = new NumericUpDown()
            {
                Parent = EldontesPage,
                Location = new Point(20, Labelek[3].Bottom + 10),
                Size = new Size(180, 40),
                Enabled = false
            };

            Also.ValueChanged += (s, e) =>
            {
                Felso.Minimum = Also.Value;
                Felso.Enabled = true;
            };
            Felso.ValueChanged += (s, e) =>
            {
                Keresett.Minimum = Also.Value;
                Keresett.Maximum = Felso.Value;
                Keresett.Enabled = true;
            };

            Button OK = new Button()
            {
                Parent = EldontesPage,
                Location = new Point(20, Keresett.Bottom + 20),
                Size = new Size(180, 40),
                Text = "OK"
            };

            Eredmeny = new Label()
            {
                Parent = EldontesPage,
                Location = new Point(20, OK.Bottom + 20),
                AutoSize = true,
            };

            OK.Click += (s, e) =>
            {
                if (Felso.Enabled && Keresett.Enabled)
                {
                    bool eredemny = Tetelek.Eldontes(TombGeneralas((int)Also.Value, (int)Felso.Value, (int)Elemszam.Value), (int)Keresett.Value);
                    if (eredemny)
                    {
                        Eredmeny.Text = "Az elem benne van a tömbben.";
                    } else
                    {
                        Eredmeny.Text = "Az elem nincs benne a tömbben.";
                    }
                }
            };
        }
        #endregion
        #region Masolas
        private void Masolas()
        {
            NumericUpDown Also, Felso, Elemszam;
            Label Eredmeny;
            Label[] Labelek = new Label[3];
            string[] LabelekText = { "Alsó határ:", "Felső határ:", "Elemszám:"};
            for (int i = 0; i < Labelek.Length; i++)
            {
                Labelek[i] = new Label()
                {
                    Parent = MasolasPage,
                    Text = LabelekText[i],
                    AutoSize = true,
                    Location = new Point(20, 20 + i * 70)
                };
            }

            Also = new NumericUpDown()
            {
                Parent = MasolasPage,
                Location = new Point(20, Labelek[0].Bottom + 10),
                Size = new Size(180, 40),
                Minimum = int.MinValue,
                Maximum = int.MaxValue
            };

            Felso = new NumericUpDown()
            {
                Parent = MasolasPage,
                Location = new Point(20, Labelek[1].Bottom + 10),
                Size = new Size(180, 40),
                Minimum = int.MinValue,
                Maximum = int.MaxValue,
                Enabled = false
            };

            Elemszam = new NumericUpDown()
            {
                Parent = MasolasPage,
                Location = new Point(20, Labelek[2].Bottom + 10),
                Size = new Size(180, 40),
                Minimum = 1,
                Maximum = 10000000
            };

            Also.ValueChanged += (s, e) =>
            {
                Felso.Minimum = Also.Value;
                Felso.Enabled = true;
            };

            Button OK = new Button()
            {
                Parent = MasolasPage,
                Location = new Point(20, Elemszam.Bottom + 20),
                Size = new Size(180, 40),
                Text = "OK"
            };

            Eredmeny = new Label()
            {
                Parent = MasolasPage,
                Location = new Point(20, OK.Bottom + 20),
                AutoSize = true,
            };
            Label EredetiTombLabel = new Label()
            {
                Parent = MasolasPage,
                Text = "Eredeti tömb:",
                AutoSize = true,
                Location = new Point(Also.Right + 20,Labelek[0].Location.Y ),
            };
            TextBox EredetiTomb = new TextBox()
            {
                Parent = MasolasPage,
                Size  = new Size(350, 100),
                Location = new Point(EredetiTombLabel.Location.X, Also.Location.Y),
                ScrollBars = ScrollBars.Vertical,
                Multiline = true,
            };
            Label MasoltTombLabel = new Label()
            {
                Parent = MasolasPage,
                Text = "Másolt tömb:",
                AutoSize = true,
                Location = new Point(Also.Right + 20, EredetiTomb.Bottom + 10),
            };
            TextBox MasoltTomb = new TextBox()
            {
                Parent = MasolasPage,
                Size = new Size(350, 100),
                Location = new Point(MasoltTombLabel.Location.X, MasoltTombLabel.Bottom + 10),
                ScrollBars = ScrollBars.Vertical,
                Multiline = true,
            };
            OK.Click += (s, e) =>
            {
                if (Felso.Enabled)
                {
                    int[] eredetitomb = TombGeneralas((int)Also.Value, (int)Felso.Value, (int)Elemszam.Value);
                    EredetiTomb.Text = string.Join(",", eredetitomb);
                    MasoltTomb.Text = string.Join(",", Tetelek.Masolas(eredetitomb));
                }
            };
        }
        #endregion
        #region Metszet
        private void Metszet()
        {
            NumericUpDown Also, Felso, Elemszam;
            Label Eredmeny;
            Label[] Labelek = new Label[3];
            string[] LabelekText = { "Alsó határ:", "Felső határ:", "Elemszám:" };
            for (int i = 0; i < Labelek.Length; i++)
            {
                Labelek[i] = new Label()
                {
                    Parent = MetszetPage,
                    Text = LabelekText[i],
                    AutoSize = true,
                    Location = new Point(20, 20 + i * 70)
                };
            }

            Also = new NumericUpDown()
            {
                Parent = MetszetPage,
                Location = new Point(20, Labelek[0].Bottom + 10),
                Size = new Size(180, 40),
                Minimum = int.MinValue,
                Maximum = int.MaxValue
            };

            Felso = new NumericUpDown()
            {
                Parent = MetszetPage,
                Location = new Point(20, Labelek[1].Bottom + 10),
                Size = new Size(180, 40),
                Minimum = int.MinValue,
                Maximum = int.MaxValue,
                Enabled = false
            };

            Elemszam = new NumericUpDown()
            {
                Parent = MetszetPage,
                Location = new Point(20, Labelek[2].Bottom + 10),
                Size = new Size(180, 40),
                Minimum = 1,
                Maximum = 10000000
            };

            Also.ValueChanged += (s, e) =>
            {
                Felso.Minimum = Also.Value;
                Felso.Enabled = true;
            };

            Button OK = new Button()
            {
                Parent = MetszetPage,
                Location = new Point(20, Elemszam.Bottom + 20),
                Size = new Size(180, 40),
                Text = "OK"
            };

            Eredmeny = new Label()
            {
                Parent = MetszetPage,
                Location = new Point(20, OK.Bottom + 20),
                AutoSize = true,
            };
            Label ElsoTombLabel = new Label()
            {
                Parent = MetszetPage,
                Text = "Eredeti tömb:",
                AutoSize = true,
                Location = new Point(Also.Right + 20, Labelek[0].Location.Y),
            };
            TextBox ElsoTomb = new TextBox()
            {
                Parent = MetszetPage,
                Size = new Size(350, 100),
                Location = new Point(ElsoTombLabel.Location.X, Also.Location.Y),
                ScrollBars = ScrollBars.Vertical,
                Multiline = true,
            };
            Label MasodikTombLabel = new Label()
            {
                Parent = MetszetPage,
                Text = "Másolt tömb:",
                AutoSize = true,
                Location = new Point(Also.Right + 20, ElsoTomb.Bottom + 10),
            };
            TextBox MasodikTomb = new TextBox()
            {
                Parent = MetszetPage,
                Size = new Size(350, 100),
                Location = new Point(MasodikTombLabel.Location.X, MasodikTombLabel.Bottom + 10),
                ScrollBars = ScrollBars.Vertical,
                Multiline = true,
            };
            Label MetszetTombLabel = new Label()
            {
                Parent = MetszetPage,
                Text = "Másolt tömb:",
                AutoSize = true,
                Location = new Point(Also.Right + 20, MasodikTomb.Bottom + 10),
            };
            TextBox MetszetTomb = new TextBox()
            {
                Parent = MetszetPage,
                Size = new Size(350, 90),
                Location = new Point(MetszetTombLabel.Location.X, MetszetTombLabel.Bottom + 10),
                ScrollBars = ScrollBars.Vertical,
                Multiline = true,
            };
            OK.Click += (s, e) =>
            {
                if (Felso.Enabled)
                {
                    int[] elsotomb = TombGeneralas((int)Also.Value, (int)Felso.Value, (int)Elemszam.Value);
                    int[] masodiktomb = TombGeneralas((int)Also.Value, (int)Felso.Value, (int)Elemszam.Value);
                    ElsoTomb.Text = string.Join(",", elsotomb);
                    MasodikTomb.Text = string.Join(",", Tetelek.Masolas(masodiktomb));
                    MetszetTomb.Text = string.Join(",", Tetelek.Metszet(elsotomb, masodiktomb));
                }
            };
        }
        #endregion
        #region ECS
        private void ECS()
        {
            NumericUpDown Also, Felso, Elemszam;
            Label Eredmeny;
            Label[] Labelek = new Label[3];
            string[] LabelekText = { "Alsó határ:", "Felső határ:", "Elemszám:" };
            for (int i = 0; i < Labelek.Length; i++)
            {
                Labelek[i] = new Label()
                {
                    Parent = ECSPage,
                    Text = LabelekText[i],
                    AutoSize = true,
                    Location = new Point(20, 20 + i * 70)
                };
            }

            Also = new NumericUpDown()
            {
                Parent = ECSPage,
                Location = new Point(20, Labelek[0].Bottom + 10),
                Size = new Size(180, 40),
                Minimum = int.MinValue,
                Maximum = int.MaxValue
            };

            Felso = new NumericUpDown()
            {
                Parent = ECSPage,
                Location = new Point(20, Labelek[1].Bottom + 10),
                Size = new Size(180, 40),
                Minimum = int.MinValue,
                Maximum = int.MaxValue,
                Enabled = false
            };

            Elemszam = new NumericUpDown()
            {
                Parent = ECSPage,
                Location = new Point(20, Labelek[2].Bottom + 10),
                Size = new Size(180, 40),
                Minimum = 1,
                Maximum = 10000000
            };

            Also.ValueChanged += (s, e) =>
            {
                Felso.Minimum = Also.Value;
                Felso.Enabled = true;
            };

            Button OK = new Button()
            {
                Parent = ECSPage,
                Location = new Point(20, Elemszam.Bottom + 20),
                Size = new Size(180, 40),
                Text = "OK"
            };

            Eredmeny = new Label()
            {
                Parent = ECSPage,
                Location = new Point(20, OK.Bottom + 20),
                AutoSize = true,
            };
            Label ElsoTombLabel = new Label()
            {
                Parent = ECSPage,
                Text = "Eredeti tömb:",
                AutoSize = true,
                Location = new Point(Also.Right + 20, Labelek[0].Location.Y),
            };
            TextBox ElsoTomb = new TextBox()
            {
                Parent = ECSPage,
                Size = new Size(350, 100),
                Location = new Point(ElsoTombLabel.Location.X, Also.Location.Y),
                ScrollBars = ScrollBars.Vertical,
                Multiline = true,
            };
            Label RendezettTombLabel = new Label()
            {
                Parent = ECSPage,
                Text = "Rendezett tömb:",
                AutoSize = true,
                Location = new Point(Also.Right + 20, ElsoTomb.Bottom + 10),
            };
            TextBox RendezettTomb = new TextBox()
            {
                Parent = ECSPage,
                Size = new Size(350, 100),
                Location = new Point(RendezettTombLabel.Location.X, RendezettTombLabel.Bottom + 10),
                ScrollBars = ScrollBars.Vertical,
                Multiline = true,
            };
            OK.Click += (s, e) =>
            {
                if (Felso.Enabled)
                {
                    int[] elsotomb = TombGeneralas((int)Also.Value, (int)Felso.Value, (int)Elemszam.Value);
                    ElsoTomb.Text = string.Join(", ", elsotomb);
                    RendezettTomb.Text = string.Join (", ", Tetelek.egyszeruCseres(elsotomb));
                }
            };
        }
        #endregion
        #region  MinMax
        private void MinMax()
        {
            NumericUpDown Also, Felso, Elemszam;
            Label Eredmeny;
            Label[] Labelek = new Label[3];
            string[] LabelekText = { "Alsó határ:", "Felső határ:", "Elemszám:" };
            for (int i = 0; i < Labelek.Length; i++)
            {
                Labelek[i] = new Label()
                {
                    Parent = MinMaxPage,
                    Text = LabelekText[i],
                    AutoSize = true,
                    Location = new Point(20, 20 + i * 70)
                };
            }

            Also = new NumericUpDown()
            {
                Parent = MinMaxPage,
                Location = new Point(20, Labelek[0].Bottom + 10),
                Size = new Size(180, 40),
                Minimum = int.MinValue,
                Maximum = int.MaxValue
            };

            Felso = new NumericUpDown()
            {
                Parent = MinMaxPage,
                Location = new Point(20, Labelek[1].Bottom + 10),
                Size = new Size(180, 40),
                Minimum = int.MinValue,
                Maximum = int.MaxValue,
                Enabled = false
            };

            Elemszam = new NumericUpDown()
            {
                Parent = MinMaxPage,
                Location = new Point(20, Labelek[2].Bottom + 10),
                Size = new Size(180, 40),
                Minimum = 1,
                Maximum = 10000000
            };

            Also.ValueChanged += (s, e) =>
            {
                Felso.Minimum = Also.Value;
                Felso.Enabled = true;
            };

            Button OK = new Button()
            {
                Parent = MinMaxPage,
                Location = new Point(20, Elemszam.Bottom + 20),
                Size = new Size(180, 40),
                Text = "OK"
            };

            Eredmeny = new Label()
            {
                Parent = MinMaxPage,
                Location = new Point(20, OK.Bottom + 20),
                AutoSize = true,
            };
            Label ElsoTombLabel = new Label()
            {
                Parent = MinMaxPage,
                Text = "Eredeti tömb:",
                AutoSize = true,
                Location = new Point(Also.Right + 20, Labelek[0].Location.Y),
            };
            TextBox ElsoTomb = new TextBox()
            {
                Parent = MinMaxPage,
                Size = new Size(350, 100),
                Location = new Point(ElsoTombLabel.Location.X, Also.Location.Y),
                ScrollBars = ScrollBars.Vertical,
                Multiline = true,
            };
            Label RendezettTombLabel = new Label()
            {
                Parent = MinMaxPage,
                Text = "Rendezett tömb:",
                AutoSize = true,
                Location = new Point(Also.Right + 20, ElsoTomb.Bottom + 10),
            };
            TextBox RendezettTomb = new TextBox()
            {
                Parent = MinMaxPage,
                Size = new Size(350, 100),
                Location = new Point(RendezettTombLabel.Location.X, RendezettTombLabel.Bottom + 10),
                ScrollBars = ScrollBars.Vertical,
                Multiline = true,
            };
            OK.Click += (s, e) =>
            {
                if (Felso.Enabled)
                {
                    int[] elsotomb = TombGeneralas((int)Also.Value, (int)Felso.Value, (int)Elemszam.Value);
                    ElsoTomb.Text = string.Join(", ", elsotomb);
                    RendezettTomb.Text = string.Join(", ", Tetelek.MinMax(elsotomb));
                }
            };
        }
        #endregion
        #region  LinKer
        private void LinKer()
        {
            NumericUpDown Also, Felso, Keresett, Elemszam;
            Label Eredmeny;

            Label[] Labelek = new Label[4];
            string[] LabelekText = { "Alsó határ:", "Felső határ:", "Elemszám:", "Keresett elem:" };
            for (int i = 0; i < Labelek.Length; i++)
            {
                Labelek[i] = new Label()
                {
                    Parent = LinKerPage,
                    Text = LabelekText[i],
                    AutoSize = true,
                    Location = new Point(20, 20 + i * 70)
                };
            }

            Also = new NumericUpDown()
            {
                Parent = LinKerPage,
                Location = new Point(20, Labelek[0].Bottom + 10),
                Size = new Size(180, 40),
                Minimum = int.MinValue,
                Maximum = int.MaxValue
            };

            Felso = new NumericUpDown()
            {
                Parent = LinKerPage,
                Location = new Point(20, Labelek[1].Bottom + 10),
                Size = new Size(180, 40),
                Minimum = int.MinValue,
                Maximum = int.MaxValue,
                Enabled = false
            };

            Elemszam = new NumericUpDown()
            {
                Parent = LinKerPage,
                Location = new Point(20, Labelek[2].Bottom + 10),
                Size = new Size(180, 40),
                Minimum = 1,
                Maximum = 10000000
            };

            Keresett = new NumericUpDown()
            {
                Parent = LinKerPage,
                Location = new Point(20, Labelek[3].Bottom + 10),
                Size = new Size(180, 40),
                Enabled = false
            };

            Also.ValueChanged += (s, e) =>
            {
                Felso.Minimum = Also.Value;
                Felso.Enabled = true;
            };
            Felso.ValueChanged += (s, e) =>
            {
                Keresett.Minimum = Also.Value;
                Keresett.Maximum = Felso.Value;
                Keresett.Enabled = true;
            };

            Button OK = new Button()
            {
                Parent = LinKerPage,
                Location = new Point(20, Keresett.Bottom + 20),
                Size = new Size(180, 40),
                Text = "OK"
            };

            Eredmeny = new Label()
            {
                Parent = LinKerPage,
                Location = new Point(20, OK.Bottom + 20),
                AutoSize = true,
                Text = "Keresett elem indexe: "
            };

            OK.Click += (s, e) =>
            {
                Eredmeny.Text = "Keresett elem indexe: ";
                if (Felso.Enabled && Keresett.Enabled)
                {
                    int szam =  Tetelek.LinearisKereses(TombGeneralas((int)Also.Value, (int)Felso.Value, (int)Elemszam.Value), (int)Keresett.Value);
                    if (szam != -1)
                    {
                        Eredmeny.Text += szam;
                    } else
                    {
                        Eredmeny.Text = "A keresett elem nincs benne a tömbben";
                    }
                }
            };
        }
        #endregion
        #region  BinKer
        private void BinKer()
        {
            NumericUpDown Also, Felso, Keresett, Elemszam;
            Label Eredmeny;

            Label[] Labelek = new Label[4];
            string[] LabelekText = { "Alsó határ:", "Felső határ:", "Elemszám:", "Keresett elem:" };
            for (int i = 0; i < Labelek.Length; i++)
            {
                Labelek[i] = new Label()
                {
                    Parent = BinKerPage,
                    Text = LabelekText[i],
                    AutoSize = true,
                    Location = new Point(20, 20 + i * 70)
                };
            }

            Also = new NumericUpDown()
            {
                Parent = BinKerPage,
                Location = new Point(20, Labelek[0].Bottom + 10),
                Size = new Size(180, 40),
                Minimum = int.MinValue,
                Maximum = int.MaxValue
            };

            Felso = new NumericUpDown()
            {
                Parent = BinKerPage,
                Location = new Point(20, Labelek[1].Bottom + 10),
                Size = new Size(180, 40),
                Minimum = int.MinValue,
                Maximum = int.MaxValue,
                Enabled = false
            };

            Elemszam = new NumericUpDown()
            {
                Parent = BinKerPage,
                Location = new Point(20, Labelek[2].Bottom + 10),
                Size = new Size(180, 40),
                Minimum = 1,
                Maximum = 10000000
            };

            Keresett = new NumericUpDown()
            {
                Parent = BinKerPage,
                Location = new Point(20, Labelek[3].Bottom + 10),
                Size = new Size(180, 40),
                Enabled = false
            };

            Also.ValueChanged += (s, e) =>
            {
                Felso.Minimum = Also.Value;
                Felso.Enabled = true;
            };
            Felso.ValueChanged += (s, e) =>
            {
                Keresett.Minimum = Also.Value;
                Keresett.Maximum = Felso.Value;
                Keresett.Enabled = true;
            };

            Button OK = new Button()
            {
                Parent = BinKerPage,
                Location = new Point(20, Keresett.Bottom + 20),
                Size = new Size(180, 40),
                Text = "OK"
            };

            Eredmeny = new Label()
            {
                Parent = BinKerPage,
                Location = new Point(20, OK.Bottom + 20),
                AutoSize = true,
                Text = "Keresett elem indexe: "
            };

            OK.Click += (s, e) =>
            {
                Eredmeny.Text = "Keresett elem indexe: ";
                if (Felso.Enabled && Keresett.Enabled)
                {
                    int szam = Tetelek.BinarisKereses(TombGeneralas((int)Also.Value, (int)Felso.Value, (int)Elemszam.Value), (int)Keresett.Value);
                    if (szam != -1)
                    {
                        Eredmeny.Text += szam;
                    }
                    else
                    {
                        Eredmeny.Text = "A keresett elem nincs benne a tömbben";
                    }
                }
            };
        }
        #endregion
        int[] TombGeneralas(int also, int felso, int elemszam)
        {
            int[] tomb = new int[elemszam];
            for (int i = 0; i < tomb.Length; i++)
            {
                tomb[i] = r.Next(also, felso + 1);
            }
            return tomb;
        }
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