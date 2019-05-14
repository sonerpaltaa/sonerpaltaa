using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Mail;
using System.Data.SqlClient;

namespace DemirbasTakip
{
    public partial class ParolaHatirla : Form
    {
        public ParolaHatirla()
        {
            InitializeComponent();
        }

        SqlConnection baglanti;
        SqlCommand com;
        DataTable dt;
        SqlDataReader oku;
        private string sifre;

        public void VTbaglan()
        {
            baglanti = new SqlConnection("Data Source=SONERPALTA\\SQLEXPRESS;Initial Catalog=DemirbasTakip;Integrated Security=True");
            baglanti.Open();
        }

        public void VTkapat()
        {
            baglanti.Close();
        }

        public bool Gonder(string konu, string icerik)
        {
            MailMessage ePosta = new MailMessage();
            ePosta.From = new MailAddress("sonerpaltaa@gmail.com");//buraya kendi gmail hesabınız
            //
            ePosta.To.Add(textBox2.Text);//bura şifre unutanın maili textboxdan çekdiniz.
            ;

            ePosta.Subject = konu; //butonda veri tabanı çekdikden sonra aldımız değer konu değeri
            //
            ePosta.Body = icerik;  // buda şifremiz
            //
            SmtpClient smtp = new SmtpClient();
            //
            smtp.Credentials = new System.Net.NetworkCredential("sonerpaltaa@gmail.com", "b1b2b1b2");
            //kendi gmail hesabiniz var şifresi
            smtp.Port = 587;
            smtp.Host = "smtp.gmail.com";
            smtp.EnableSsl = true;
            object userState = ePosta;
            bool kontrol = true;
            try
            {
                smtp.SendAsync(ePosta, (object)ePosta);
            }
            catch (SmtpException ex)
            {
                kontrol = false;
                System.Windows.Forms.MessageBox.Show(ex.Message, "Mail Gönderme Hatasi");
            }
            return kontrol;

        }

        private void button1_Click(object sender, EventArgs e)
        {

            VTbaglan();
            SqlCommand com = new SqlCommand("Select * from tblKullanicilar where KullaniciAdi='" + textBox1.Text.ToString() +
                "'and e_mail='" + textBox2.Text.ToString() + "'", baglanti);
            //burada veritabanina kodlayarak yazdımız şifrelerin kodlarını karşılaştırdık
            SqlDataReader oku = com.ExecuteReader();
            if (oku.Read())
            {
                //burada verdiği tc ve mail doğruysa gireceği için şifreyi veritabanından çekip gönder fonksiyonunu kullanarak göndereceğiz

                sifre = oku["Sifre"].ToString();
                //veritabanından çekdik            
                MessageBox.Show("Girmiş Oldunuz Bilgiler Uyuşuyor Şifreniz Mail adresinize yollanıyor");
                Gonder("Unutmuş Olduğunuz Şifreniz Ektedir", sifre);
                //gönder paremetresi ile içeriğe 2 string değer yolladık biri mesajımız öbürü şifresi
                baglanti.Close();
            }
            else
            {
                MessageBox.Show("Bilgileriniz Uyuşmadı");
            }
            VTkapat();
        }
    }
}
