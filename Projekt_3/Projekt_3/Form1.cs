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
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Projekt_3
{
    public partial class Form1 : Form
    {
        SplitContainer Container;
        string[] GombFeliratok = Beolvasas("GombFeliratok.txt");
        string[] LabelFeliratok = Beolvasas("LabelFeliratok.txt");
        Label[] Labelek = new Label[4];
        Button[] Gombok = new Button[8];
        double[] MostTomb;
        Random r = new Random();
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            Text = "Alaptetelek";
            Font = new Font("Segoe UI", 12f);
            Size = new Size(1000, 630);
            FormBorderStyle = FormBorderStyle.Fixed3D;

            Container = new SplitContainer()
            {
                Parent = this,
                Dock = DockStyle.Fill,
                IsSplitterFixed = true,
            };
            Container.SplitterDistance = 200;
            Container.Panel1.BackColor = Color.Beige;
            Container.Panel2.BackColor = Color.LightCoral;

            for (int i = 0; i < Gombok.Length; i++)
            {
                Gombok[i] = new Button()
                {
                    Parent = Container.Panel1,
                    Size = new Size(180, 40),
                    Text = GombFeliratok[i]
                };

                if (i % 2 == 0 && i != 0)
                {
                    Gombok[i].Location = new Point(10, Gombok[i - 1].Location.Y + 40 + 70);
                } else if (i % 2 != 0)
                {
                    Gombok[i].Location = new Point(10, Gombok[i - 1].Location.Y + 40);
                } else
                {
                    Gombok[i].Location = new Point(10, 40);
                }
                Gombok[i].Click += Gomb_Click;
            }
            for (int i = 0; i < Labelek.Length; i++)
            {
            
                Labelek[i] = new Label()
                {
                    Parent = Container.Panel1,
                    Size = new Size(180, 30),
                    Text = LabelFeliratok[i]
                };
            }
            Labelek[0].Location = new Point(10, 10);
            Labelek[1].Location = new Point(10, Gombok[2].Location.Y - Labelek[1].Height);
            Labelek[2].Location = new Point(10, Gombok[4].Location.Y - Labelek[2].Height);
            Labelek[3].Location = new Point(10, Gombok[6].Location.Y - Labelek[3].Height);
        }

        private void Gomb_Click(object sender, EventArgs e)
        {
            Container.Panel2.Controls.Clear();
            if (sender == Gombok[0])
            {
                Megszamolas();
            }

        }
        #region Megszamolas
        private void Megszamolas()
        {
            TextBox Also = new TextBox()
            {
                Parent = Container.Panel2,
                Location = new Point(20, 20),
                Size = new Size(180, 40),
                Text = "Számok alsó határa",
                Tag = "Also"
            };
            Also.Enter += Keresett_Enter;
            Also.Leave += Keresett_Leave;

            TextBox Felso = new TextBox()
            {
                Parent = Container.Panel2,
                Location = new Point(20, Also.Location.Y + Also.Size.Height + 20),
                Size = new Size(180, 40),
                Text = "Számok felső határa",
                Tag = "Felso"
            };

            Felso.Enter += Keresett_Enter;
            Felso.Leave += Keresett_Leave;

            NumericUpDown Elemszam = new NumericUpDown()
            {
                Parent = Container.Panel2,
                Minimum = 1,
                Maximum = 10000000,
                Location = new Point(20, Felso.Location.Y + Felso.Size.Height + 20),
                Size = new Size(180, 40),
            };
            Elemszam.ValueChanged += Elemszam_ValueChanged;

            TextBox Keresett = new TextBox()
            {
                Parent = Container.Panel2,
                Location = new Point(20, Elemszam.Location.Y + Elemszam.Size.Height + 20),
                Size = new Size(180, 40),
                Text = "Keresett elem",
                Tag = "Keresett"
            };
            Keresett.Enter += Keresett_Enter;
            Keresett.Leave += Keresett_Leave;

            Button OK = new Button()
            {
                Parent = Container.Panel2,
                Location = new Point(20, Keresett.Location.Y + Keresett.Size.Height + 20),
                Size = new Size(180, 40),
                Text = "OK",
            };
            OK.Click += OK_Click;

            Label Eredmeny = new Label()
            {
                Parent = Container.Panel2,
                Location = new Point(20, OK.Location.Y + OK.Size.Height + 20),
                Size = new Size(180, 40),
                Text = "Keresett elem előfordulása: "
            };
            OK.Tag = Tuple.Create(Keresett, Eredmeny);
        }
        private void OK_Click(object sender, EventArgs e)
        {
            Button ok = sender as Button;
            var tag = ok.Tag as Tuple<TextBox, Label>;
            TextBox Keresett = tag.Item1;
            Label Eredmeny = tag.Item2;


            double szam;
            if (double.TryParse(Keresett.Text, out szam))
            {
                Eredmeny.Text = $"Keresett elem előfordulása: {Tetelek.Megszamolas(MostTomb, szam)}";
            }
            else
            {
                Keresett.Text = "Számot adjon meg!";
            }
        }

        private void Keresett_Leave(object sender, EventArgs e)
        {
            TextBox kuldo = sender as TextBox;
            string nev = kuldo.Tag as string;
            if (string.IsNullOrWhiteSpace(kuldo.Text))
            {
                if (nev == "Also")
                {
                    kuldo.Text = "Számok alsó határa";
                } else if (nev == "Felso")
                {
                    kuldo.Text = "Számok felső határa";
                }
                else
                {
                    kuldo.Text = "Keresett elem";
                }
            }
        }

        private void Keresett_Enter(object sender, EventArgs e)
        {
            TextBox Keresett = sender as TextBox;
            Keresett.Text = "";
        }
        private void Elemszam_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown elemszam = sender as NumericUpDown;
            if (elemszam.Value <= elemszam.Maximum)
            {
                MostTomb = new double[(int)elemszam.Value];
                for (int i = 0; i < MostTomb.Length; i++)
                {
                    MostTomb[i] = r.NextDouble() * int.MaxValue;
                }
            }
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
    }
}
