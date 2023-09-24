using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace opakovani6
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.CenterToScreen();
            this.Text = "ukol 6";
            this.ShowIcon = false;
            this.BackColor = Color.DodgerBlue;            

            button2.Visible = false;
            button3.Visible = false;
            button4.Visible = false;            
        }

        StreamReader sr;
        FileStream fs;
        BinaryWriter bw;
        BinaryReader br;
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = Path.GetDirectoryName(Application.ExecutablePath);


            if(ofd.ShowDialog() == DialogResult.OK)
            {
                string file  = ofd.FileName;                
                sr = new StreamReader(file,Encoding.GetEncoding("Windows-1250"));
                fs = new FileStream("cisla.dat", FileMode.Create, FileAccess.ReadWrite);
                bw = new BinaryWriter(fs, Encoding.GetEncoding("Windows-1250"));
                

                string line;

                listBox1.Items.Clear();
                while(!sr.EndOfStream)
                {
                    line = sr.ReadLine();
                    listBox1.Items.Add(line);                        
                    string[] slova = line.Split(';');
                    double nejdelsi = (double)slova.Max(x => x.Length) / 10;
                    bw.Write(nejdelsi);
                }

                button2.Visible = true;
                //bw.Close();
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            br = new BinaryReader(fs, Encoding.GetEncoding("Windows-1250"));

            br.BaseStream.Position = 0;
            while(br.BaseStream.Position < br.BaseStream.Length)
            {
                double cislo = br.ReadDouble();
                listBox2.Items.Add(cislo);
            }
            button3.Visible = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            br.BaseStream.Position = 0;
            while (br.BaseStream.Position < br.BaseStream.Length)
            {                
                double cislo = br.ReadDouble();
                if (cislo < 1) cislo *= 10;
                br.BaseStream.Position -= sizeof(double);
                bw.Write(cislo);                         
                listBox3.Items.Add(cislo);
            }
            button4.Visible = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            double soucet = 0;
            int pocet = 0;
            br.BaseStream.Position = 0;
            while (br.BaseStream.Position < br.BaseStream.Length)
            {
                double cislo = br.ReadDouble();
                listBox4.Items.Add(cislo);
                if (cislo > 2)
                {
                    soucet += cislo;
                    pocet++;
                }
            }
            double prumer = Math.Round(soucet / (double)pocet, 2);
            bw.Write(prumer);
            listBox4.Items.Add(prumer); 
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if(button2.Visible)fs.Close();      //kdyby uzivatel nevybral soubor a zavrel formular, tak se nebude tok zavírat, protoze neni otevreny
        }
    }
}
