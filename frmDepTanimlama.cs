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
    public partial class frmDepTanimlama : Form
    {
        public frmDepTanimlama()
        {
            InitializeComponent();
        }

        SqlConnection baglanti;
        SqlCommand komut;
        DataSet ds,ds1;
        SqlDataAdapter da,da1;
        public void VTbaglan()
        {
            baglanti = new SqlConnection("Data Source=SONERPALTA\\SQLEXPRESS;Initial Catalog=DemirbasTakip;Integrated Security=True");
            baglanti.Open();
        }

        public void VTkapat()
        {
            baglanti.Close();
        }
        private void btnTanimlamaBack_Click(object sender, EventArgs e)
        {
            frmAnaMenu frmanamenu = new frmAnaMenu(this);
            frmanamenu.Show();
            
        }

        void KayıtSil(int departmanid)
        {
            VTbaglan();
            string sql = "DELETE FROM tblDepartman WHERE DepartmanID=@departmanid";
            komut = new SqlCommand(sql, baglanti);
            komut.Parameters.AddWithValue("@departmanid", departmanid);

            komut.ExecuteNonQuery();
            VTkapat();
        }

        private void frmDepTanimlama_Load(object sender, EventArgs e)
        {
            dGWDep.AllowUserToAddRows = false;
            dGWPersonel.AllowUserToAddRows = false;
            VTbaglan();
            da = new SqlDataAdapter("SELECT DepartmanID,DepartmanAdi FROM tblDepartman", baglanti);
            ds = new DataSet();
            da.Fill(ds, "tblDepartman");
            dGWDep.DataSource = ds.Tables["tblDepartman"];
            VTkapat();

            VTbaglan();
            da1 = new SqlDataAdapter("SELECT PersonelID,PersonelAdi,PersonelSoyadi FROM tblPersonel", baglanti);
            ds1 = new DataSet();
            da1.Fill(ds1, "tblPersonel");
            dGWPersonel.DataSource = ds1.Tables["tblPersonel"];
            VTkapat();

            VTbaglan();
            da = new SqlDataAdapter("SELECT O.DepartmanID,DepartmanAdi,ODT.PersonelID,PersonelAdi,PersonelSoyadi FROM tblDepartman O INNER JOIN tbldepDemirbasAtama ODT ON O.DepartmanID=ODT.DepartmanID INNER JOIN tblPersonel P ON ODT.PersonelID=P.PersonelID", baglanti);
            ds = new DataSet();
            da.Fill(ds, "tblDepartman");
            dgwListe.DataSource = ds.Tables["tblDepartman"];
            VTkapat();
        }
        string Personeladi,Departmanadi;

        private void btnSil_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow drow in dgwListe.SelectedRows)  //Seçili Satırları Silme
            {
                int demirbasid = Convert.ToInt32(drow.Cells[0].Value);
                KayıtSil(demirbasid);
            }

            VTbaglan();
            da = new SqlDataAdapter("SELECT O.DepartmanID,DepartmanAdi,ODT.PersonelID,PersonelAdi,PersonelSoyadi FROM tblDepartman O INNER JOIN tbldepDemirbasAtama ODT ON O.DepartmanID=ODT.DepartmanID INNER JOIN tblPersonel P ON ODT.PersonelID=P.PersonelID", baglanti);
            ds = new DataSet();
            da.Fill(ds, "tblDepartman");
            dgwListe.DataSource = ds.Tables["tblDepartman"];
            VTkapat();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow drow in dGWDep.SelectedRows)  //Seçili Satırları Silme
            {
                int departmanid = Convert.ToInt32(drow.Cells[0].Value);
                KayıtSil(departmanid);
            }
            VTbaglan();
            da = new SqlDataAdapter("SELECT DepartmanID,DepartmanAdi FROM tblDepartman", baglanti);
            ds = new DataSet();
            da.Fill(ds, "tblDepartman");
            dGWDep.DataSource = ds.Tables["tblDepartman"];
            VTkapat();


        }

        private void btnOTdepKaydet_Click(object sender, EventArgs e)
        {
            VTbaglan();
            komut = new SqlCommand("insert into tbldepDemirbasAtama(DepartmanID,PersonelId)values(@departmanID,@personelID)", baglanti);
            komut.Parameters.AddWithValue("@departmanID", Departmanadi);
            komut.Parameters.AddWithValue("@personelID", Personeladi);
            komut.ExecuteNonQuery();
            VTkapat();
            MessageBox.Show("Personel Departmana Atandı.");
        }

        private void dGWDep_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            txtOTdepAdi.Text=ds.Tables["tblDepartman"].Rows[e.RowIndex]["DepartmanAdi"].ToString();
            Departmanadi = ds.Tables["tblDepartman"].Rows[e.RowIndex]["DepartmanID"].ToString();
        }

        private void dGWPersonel_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            txtOTDepSorumlusu.Text = ds1.Tables["tblPersonel"].Rows[e.RowIndex]["PersonelAdi"].ToString();
            Personeladi = ds1.Tables["tblPersonel"].Rows[e.RowIndex]["PersonelID"].ToString();
        }     
    }
}
