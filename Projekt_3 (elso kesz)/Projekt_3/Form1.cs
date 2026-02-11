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
        TextBox Keresett;
        TextBox Also;
        TextBox Felso;
        Label Eredmeny;
        private void Megszamolas()
        {
            Also = new TextBox()
            {
                Parent = Container.Panel2,
                Location = new Point(20, 20),
                Size = new Size(180, 40),
                Text = "Számok alsó határa",
            };
            Also.Tag = "Also";
            Also.Enter += TextBox_Enter;
            Also.Leave += TextBox_Leave;
            
            Felso = new TextBox()
            {
                Parent = Container.Panel2,
                Location = new Point(20, Also.Location.Y + Also.Size.Height + 20),
                Size = new Size(180, 40),
                Text = "Számok felső határa"
            };
            Felso.Tag = "Felso";
            Felso.Enter += TextBox_Enter;
            Felso.Leave += TextBox_Leave;

            NumericUpDown Elemszam = new NumericUpDown()
            {
                Parent = Container.Panel2,
                Minimum = 1,
                Maximum = 10000000,
                Location = new Point(20, Felso.Location.Y + Felso.Size.Height + 20),
                Size = new Size(180, 40),
            };
            Elemszam.ValueChanged += Elemszam_ValueChanged;

            Keresett = new TextBox()
            {
                Parent = Container.Panel2,
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
                Parent = Container.Panel2,
                Location = new Point(20, Keresett.Location.Y + Keresett.Size.Height + 20),
                Size = new Size(180, 40),
                Text = "OK",
            };
            OK.Click += OK_Click;
            Eredmeny = new Label()
            {
                Parent = Container.Panel2,
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
                for (int i = 0;i < elemszam;i++)
                {
                    MostTomb[i] = r.Next((int)also, (int)felso) * Math.Round(r.NextDouble() + 1,1);
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
            } else
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
                } else
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
    }
}
