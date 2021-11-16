using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Kur_Verisi_Cekme_Ve_Hesaplama
{
    public partial class Form1 : Form
    {
        double dolar, euro, sterlin;
        public Form1()
        {
            InitializeComponent();
        }
        public void DovizGoster()
        {
            try
            {
                XmlDocument xmlVerisi = new XmlDocument();
                xmlVerisi.Load("https://www.tcmb.gov.tr/kurlar/today.xml");

                dolar = Convert.ToDouble(xmlVerisi.SelectSingleNode(string.Format("Tarih_Date/Currency[@Kod='{0}']/ForexSelling", "USD")).InnerText.Replace('.', ','));
                euro = Convert.ToDouble(xmlVerisi.SelectSingleNode(string.Format("Tarih_Date/Currency[@Kod='{0}']/ForexSelling", "EUR")).InnerText.Replace('.', ','));
                sterlin = Convert.ToDouble(xmlVerisi.SelectSingleNode(string.Format("Tarih_Date/Currency[@Kod='{0}']/ForexSelling", "GBP")).InnerText.Replace('.', ','));

                lblDolar.Text = "Dolar = " + dolar.ToString("n2") + " TL";
                lblEuro.Text = "Euro = " + euro.ToString("n2") + " TL";
                lblSterlin.Text = "Sterlin = " + sterlin.ToString("n2") + " TL";
            }
            catch (XmlException xml)
            {
                timer1.Stop();
                MessageBox.Show(xml.ToString());
            }
        }

        private void btnHesapla_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtKurAdet.Text))
            {
                var kursecim = cboKur.SelectedIndex;
                var hesaplama = 0D;
                switch (kursecim)
                {
                    case 0:
                        hesaplama = dolar * (Convert.ToDouble(txtKurAdet.Text));
                        break;
                    case 1:
                        hesaplama = euro * (Convert.ToDouble(txtKurAdet.Text));
                        break;
                    case 2:
                        hesaplama = sterlin * (Convert.ToDouble(txtKurAdet.Text));
                        break;
                }
                label2.Text = txtKurAdet.Text + " Adet " + cboKur.Text + " " + hesaplama.ToString("n2") + " TL Yapıyor";
            }
            else
            {
                MessageBox.Show("Kur Adet Boş Olamaz!", "Hata");
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Start();
            cboKur.Items.Add("Dolar");
            cboKur.Items.Add("Euro");
            cboKur.Items.Add("Sterlin");
            cboKur.SelectedIndex = 0;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Interval = 5000;
            DovizGoster();
        }
    }
}
