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
using System.Media;
using Nielot.Properties;

namespace Nielot
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        List<int> Rura1 = new List<int>();
        List<int> Rura2 = new List<int>();
        int RuraSzerokosc = 55;
        int RuraRoznicaY = 140; //szerokosc bramki
        int RuraRoznicaX = 180;//odleglosc miedzy bramkami
        bool start = true;
        bool fruwa;
        int krok = 5;
        int OrginalX, OrginalY;
        bool ResetRury = false;
        int punkty;
        bool Gol = false;
        int wynik;
        int WynikRoznica;

        private void Ginie()
        {
            fruwa = false;
            timer2.Enabled = false;
            timer3.Enabled = false;
            button1.Visible = true;
            button1.Enabled = true;
            SprawdzIPokazWynik();
            punkty = 0;
            pictureBox1.Location = new Point(OrginalX, OrginalY);
            ResetRury = true;
            Rura1.Clear();
           
        }

        private void SprawdzIPokazWynik()
        {
            using (StreamReader czytaj = new StreamReader("Wynik.ini"))
            {
                wynik = int.Parse(czytaj.ReadToEnd());
                czytaj.Close();
                if (int.Parse (label1.Text) == 0 | int.Parse (label1.Text) > 0 )
                {
                    WynikRoznica = wynik - int.Parse(label1.Text) + 1;
                }
                
                if (wynik < int.Parse(label1.Text))
                {
                   MessageBox.Show(string.Format("Gratulacje!!! uzyskales recordowy wynik przekraczajacy {0}. Nowy rekord to {1}.", wynik, label1.Text),"Nielot",MessageBoxButtons.OK,MessageBoxIcon.Information);
                   using (StreamWriter pisz = new StreamWriter("Wynik.ini"))
                   {
                       pisz.Write(label1.Text);
                       pisz.Close();
                   }
                }
                
                if (wynik > int.Parse(label1.Text))
                {
                   MessageBox.Show(string.Format("Do pobicia najlepszego wyniku zostalo {0}, rekordowy wynik w grze to {1}.",WynikRoznica, wynik), "Nielot", MessageBoxButtons.OK, MessageBoxIcon.Information);
 
                }
                
                if (wynik == int.Parse(label1.Text))
                {
                    MessageBox.Show(string.Format("Zdobyłes {0} punkty, wyrownales najlepszy dotad wynik!!!", wynik),"Nielot",MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }


 
        }

        private void StartGry()
        {
            ResetRury = false;
            timer1.Enabled = true;
            timer2.Enabled = true;
            timer3.Enabled = true;
            Random random = new Random();
            int num = random.Next(40, this.Height - this.RuraRoznicaY);
            int num1 = num + this.RuraRoznicaY;
            Rura1.Clear();
            Rura1.Add(this.Width);
            Rura1.Add(num);
            Rura1.Add(this.Width);
            Rura1.Add(num1);

            num = random.Next(40, this.Height - this.RuraRoznicaY);
            num1 = num + this.RuraRoznicaY;
            Rura2.Clear();
            Rura2.Add(this.Width + RuraRoznicaX);
            Rura2.Add(num);
            Rura2.Add(this.Width + RuraRoznicaX);
            Rura2.Add(num1);

            button1.Visible = false;
            button1.Enabled = false;
            fruwa = true;
            Focus();

        }


        private void Form1_Load(object sender, EventArgs e)
        {
            OrginalX = pictureBox1.Location.X;
            OrginalY = pictureBox1.Location.Y;
            
            if (!File.Exists("Wynik.ini"))
            {
                File.Create("Wynik.ini").Dispose();
            }

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StartGry();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (Rura1[0] + RuraSzerokosc <= 0 | start == true)
            {
                Random rnd = new Random();
                int rx = this.Width;
                int ry = rnd.Next (40, (this.Height - RuraRoznicaY));
                var r2x = rx;
                var r2y = ry + RuraRoznicaY;
                
                Rura1.Clear();
                Rura1.Add(rx);
                Rura1.Add(ry);
                Rura1.Add(r2x);
                Rura1.Add(r2y);
            }
            else
            {
                Rura1[0] = Rura1[0] - 1;
                Rura1[2] = Rura1[2] - 1;
            }
            
            if (Rura2[0] + RuraSzerokosc <= 0)
            {
                
                Random rnd = new Random();
                int rx = this.Width;
                int ry = rnd.Next (40, (this.Height - RuraRoznicaY));
                var r2x = rx;
                var r2y = ry + RuraRoznicaY;
                int[] r1 = {rx, ry, r2x, r2y} ;
                
                Rura2.Clear();
                Rura2.Add(rx);
                Rura2.Add(ry);
                Rura2.Add(r2x);
                Rura2.Add(r2y);
                
            }
            
            else
            {
                Rura2[0] = Rura2[0] - 1;
                Rura2[2] = Rura2[2] - 1;
            
            }
            
            if(start==true)
            {
                start = false;   
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (!ResetRury && Rura1.Any() && Rura2.Any())
            {
                //pierwsza gorna
                e.Graphics.FillRectangle(Brushes.Red, new Rectangle(Rura1[0], 0, RuraSzerokosc, Rura1[1]));
                e.Graphics.FillRectangle(Brushes.Pink, new Rectangle(Rura1[0] - 10, Rura1[3] - RuraRoznicaY, 75, 15));

                //pierwsza dolna
                e.Graphics.FillRectangle(Brushes.Blue, new Rectangle(Rura1[2],Rura1[3], RuraSzerokosc,this.Height - Rura1[3]));
                e.Graphics.FillRectangle(Brushes.Orange, new Rectangle(Rura1[2] - 10, Rura1[3], 75, 15));
                
                //druga gorna
                e.Graphics.FillRectangle(Brushes.Purple, new Rectangle(Rura2[0], 0, RuraSzerokosc, Rura2[1]));
                e.Graphics.FillRectangle(Brushes.White, new Rectangle(Rura2[0] - 10, Rura2[3] - RuraRoznicaY, 75, 15));
                
                //druga dolna
                e.Graphics.FillRectangle(Brushes.Yellow, new Rectangle(Rura2[2], Rura2[3], RuraSzerokosc, this.Height - Rura2[3]));
                e.Graphics.FillRectangle(Brushes.Green, new Rectangle(Rura2[2] - 10, Rura2[3], 75, 15));

            }
        }

    private void SprawdzPunkt()
    {
        Rectangle rec = pictureBox1.Bounds;
        Rectangle rec1 = new Rectangle(Rura1[2] + 40, Rura1[3] - RuraRoznicaY, 15, RuraRoznicaY);
        Rectangle rec2 = new Rectangle(Rura2[2] + 40, Rura2[3] - RuraRoznicaY, 15, RuraRoznicaY);
        Rectangle intersect1 = Rectangle.Intersect(rec, rec1);
        Rectangle intersect2 = Rectangle.Intersect(rec, rec2);

        if (!ResetRury | start)
        {
            if (intersect1 != Rectangle.Empty | intersect2 != Rectangle.Empty)
            {
                if (!Gol)
                {
                    punkty++;
                    SoundPlayer sp = new SoundPlayer(Nielot.Properties.Resources.punkt);
                    sp.Play();
                    Gol = true;
                }
            }

            else
            {
                Gol = false;
            }
        }
    }
        private void SprawdzKolizje()
        {
            Rectangle rec = pictureBox1.Bounds;
            Rectangle rec1 = new Rectangle(Rura1[0], 0, RuraSzerokosc, Rura1[1]);
            Rectangle rec2 = new Rectangle(Rura1[2], Rura1[3], RuraSzerokosc, this.Height - Rura1[3]);
            Rectangle rec3 = new Rectangle(Rura2[0], 0, RuraSzerokosc, Rura2[1]);
            Rectangle rec4 = new Rectangle(Rura2[2], Rura2[3], RuraSzerokosc, this.Height - Rura2[3]);
            Rectangle intersect1 = Rectangle.Intersect(rec, rec1);
            Rectangle intersect2 = Rectangle.Intersect(rec, rec2);
            Rectangle intersect3 = Rectangle.Intersect(rec, rec3);
            Rectangle intersect4 = Rectangle.Intersect(rec, rec4);
            
            if (!ResetRury | start)
            {
                if(intersect1 != Rectangle.Empty | intersect2 != Rectangle.Empty | intersect3 != Rectangle.Empty | intersect4 != Rectangle.Empty)
                {
                    SoundPlayer sp = new SoundPlayer(Nielot.Properties.Resources.brzekrury);
                    sp.Play();
                    Ginie();
                }
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Space:
                    krok = -3;
                    pictureBox1.Image = Resources.leci;
                    break;
            }
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            pictureBox1.Location = new Point(pictureBox1.Location.X, pictureBox1.Location.Y + krok);
            
            if (pictureBox1.Location.Y < 0)
            {
                pictureBox1.Location = new Point(pictureBox1.Location.X, 0);
            }
            
            if (pictureBox1.Location.Y + pictureBox1.Height > this.ClientSize.Height)
            {
                pictureBox1.Location = new Point(pictureBox1.Location.X, this.ClientSize.Height - pictureBox1.Height);
            }
            
            SprawdzKolizje();
            
            if (fruwa)
            {
                SprawdzPunkt();
            }
            label1.Text = Convert.ToString(punkty);
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Space:
                    krok = 3;
                    pictureBox1.Image = Resources.spada;
                    break;
            }

        }
    }
}
