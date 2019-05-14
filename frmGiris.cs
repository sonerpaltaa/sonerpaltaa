using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace DemirbasTakip
{
    public partial class frmGiris : Form
    {
        public frmGiris()
        {
            InitializeComponent();
        }

        SqlConnection baglanti;
        SqlDataAdapter sda;
        DataTable dt;



        public void VTbaglan()
        {
            baglanti = new SqlConnection("Data Source=SONERPALTA\\SQLEXPRESS;Initial Catalog=DemirbasTakip;Integrated Security=True");
            baglanti.Open();
        }

        public void VTkapat()
        {
            baglanti.Close();
        }

        public static string kadi, sifre;
        private void btnGiris_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Giriş Başarısız.");
            }

            kadi = txtUsername.Text;
            sifre = txtPassword.Text;

            try
            {
                VTbaglan();
                string sql = "SELECT * FROM tblKullanicilar WHERE KullaniciAdi='" + kadi + "' AND Sifre='" + sifre + "'";
                SqlCommand cmd = new SqlCommand(sql, baglanti);
                dt = new DataTable();
                sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);


                if (dt.Rows.Count > 0)
                {
                    frmAnaMenu anaMenu = new frmAnaMenu(this);
                    anaMenu.Show();

                }

                else
                {
                    MessageBox.Show("Yanlış Kullanıcı ve ya şifre.");
                }
            }
            catch
            {
                MessageBox.Show("Hatalı Gİriş.");
            }
            VTkapat();
        }
    }
}
