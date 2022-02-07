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

namespace PROJE11
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection(@"Data Source=DESKTOP-493DFJA\SQLEXPRESS;Initial Catalog=TBLVERİMESAJLAŞMA;Integrated Security=True");
        public string numara;
        void gelenkutusu()
        {
            SqlDataAdapter da = new SqlDataAdapter("select MESAJID, (AD +' '+SOYAD) AS GÖNDEREN ,BASLIK,ICERIK from TBLMESAJLAR" +
                " inner join TBLKISILER " +
                " on TBLMESAJLAR.GONDEREREN = TBLKISILER.NUMARA " +
                "WHERE ALICI=" + numara, baglanti);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
           
        }
        void gidenkutusu()
        {
            SqlDataAdapter da2 = new SqlDataAdapter("" +"select MESAJID, (AD +' '+SOYAD) AS ALICI ,BASLIK,ICERIK from TBLMESAJLAR" +
                " inner join TBLKISILER " +
                " on TBLMESAJLAR.ALICI = TBLKISILER.NUMARA " +
                "WHERE GONDEREREN =" + numara, baglanti); 
            DataTable dt2 = new DataTable();
            da2.Fill(dt2);
            dataGridView2.DataSource = dt2;
        }

        private void Form2_Load(object sender, EventArgs e)
        { 
            lblnum.Text = numara;
            gelenkutusu();
            gidenkutusu();
            //ADSOYADÇEKME
            baglanti.Open();
            SqlCommand KMT = new SqlCommand("SELECT AD ,SOYAD FROM TBLKISILER WHERE NUMARA="+numara, baglanti);
            SqlDataReader DR = KMT.ExecuteReader();
            while (DR.Read())
            {
                lbladsoyad.Text = DR[0] + " " + DR[1];
            }
            baglanti.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand kmy = new SqlCommand("insert into TBLMESAJLAR (ALICI,BASLIK,ICERIK,GONDEREREN) values (@p1,@p2,@p3,@p4)", baglanti);
            kmy.Parameters.AddWithValue("@p1", maskedTextBox1.Text);
            kmy.Parameters.AddWithValue("@p2", txtbaslık.Text);
            kmy.Parameters.AddWithValue("@p3", richTextBox1.Text);
            kmy.Parameters.AddWithValue("@p4", lblnum.Text);
            kmy.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("mesajınız iletildi :)");
            //gidenkutusu();
            //gelenkutusu();


        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int secilen = dataGridView1.SelectedCells[0].RowIndex;
           richTextBox1.Text = dataGridView1.Rows[secilen].Cells[3].Value.ToString();

        }

       

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
           int secilen = dataGridView2.SelectedCells[0].RowIndex;
            richTextBox1.Text = dataGridView2.Rows[secilen].Cells[3].Value.ToString();
        }
    }
}
