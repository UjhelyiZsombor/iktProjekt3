using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Text;
using System.Security.AccessControl;

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
        private void Megszamolas()
        {
            NumericUpDown Elemszam = new NumericUpDown()
            {
                Parent = Container.Panel2,
                Minimum = 1,
                Maximum = decimal.MaxValue,
                Location = new Point(20, 20),
                Size = new Size(180, 40)
            };
            Elemszam.ValueChanged += Elemszam_ValueChanged;
        }
        private void Elemszam_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown elemszam = sender as NumericUpDown;
            MostTomb = new double[(int)elemszam.Value];
            for (int i = 0; i < MostTomb.Length; i++)
            {
                MostTomb[i] = r.Next();
            }
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
    }
}
