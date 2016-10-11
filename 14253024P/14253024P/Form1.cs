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
using System.Collections;

namespace _14253024P
{
    public partial class Form1 : Form
    {
        int silme_kontrol = 0;
        int M_no = 0;
        int Hesap_no = 0;
        int Musteri_no = 0;
        int mus_no = 0;
        int kullanici_ID=0;
        int ID = 0;
        int admin_guncelle_kont = 0,admin_kayıt_kont=0;
        int mus_kayit_kont = 0,mus_sifre_gunclee_kont=0,mus_bil_gunclelle=0;
        string mevcut_admin_ad = "";
        string mevcut_admin_sifre = "";
        static string str = "Data Source=.;Initial Catalog=14253024P;Integrated Security=True";
        
        SqlConnection baglanti = new SqlConnection(str);
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
         
            try
            {
                baglanti.Open();
            }
            catch (Exception)
            {
                MessageBox.Show("Veritabanı Bağlantı Problemi!");
            }
            SqlCommand komut = new SqlCommand();
            komut.Connection = baglanti;
            komut.CommandText = "select * from Islem_turu_tablosu ";
            SqlDataAdapter adpt = new SqlDataAdapter(komut);
            DataTable data = new DataTable();
            adpt.Fill(data);
            dataGridView1.DataSource = data;
            dataGridView10.DataSource = data;
            baglanti.Close();
            tabControl1.TabPages.Remove(islemler_tab);
            tabControl1.TabPages.Remove(hesap_ac_kapa_tab);
            tabControl1.TabPages.Remove(para_cek_yatir_tab);
            tabControl1.TabPages.Remove(kayit_tabPage1);
            tabControl1.TabPages.Remove(guncelle_tabPage1);
            tabControl1.TabPages.Remove(havale_tabPage1);
            tabControl1.TabPages.Remove(Musteri_hesap_ozeti_tabPage1);
            tabControl1.TabPages.Remove(rapor_tabPage1);
            tabControl1.TabPages.Remove(Kullanici_silme_tabPage1);
            tabControl1.TabPages.Remove(islem_ekle_tab);
        }
       
        public void Hesaplar_fillgrid()//müsteriye göre
        {

            if (baglanti.State == ConnectionState.Closed)
                baglanti.Open();
            SqlCommand komut = new SqlCommand();
            komut.Connection = baglanti;
            komut.CommandText = "select * from Musteri_tablosu where ID='" + kullanici_ID + "'";
            SqlDataAdapter adpt = new SqlDataAdapter(komut);
            DataTable data = new DataTable();
            adpt.Fill(data);
            dataGridView2.DataSource = data;
            baglanti.Close();
        }
      public void admin_hesap_ac_kapa_fill()//admin e göre
        {
            if (baglanti.State == ConnectionState.Closed)
                baglanti.Open();
            SqlCommand komut = new SqlCommand();
            komut.Connection = baglanti;
            komut.CommandText = "select * from hesap_tablosu where [Musteri No]="+mus_no+"";
            SqlDataAdapter adpt = new SqlDataAdapter(komut);
            DataTable data = new DataTable();
            adpt.Fill(data);
            dataGridView3.DataSource = data;
            komut.CommandText = "select * from hesap_tablosu where [Musteri No]=" + mus_no +"";
            SqlDataReader reader = komut.ExecuteReader();
            hesap_kapa_comboBox1.Items.Clear();
            while (reader.Read())
            {
                hesap_kapa_comboBox1.Items.Add(reader.GetInt32(0));
            }
            reader.Close();
            baglanti.Close();
        }
        public void hesap_ac_kapa_fill()//müsteriye göre
        {
            if (baglanti.State == ConnectionState.Closed)
                baglanti.Open();
            SqlCommand komut = new SqlCommand();
            komut.Connection = baglanti;
            komut.CommandText = "select * from hesap_tablosu where [Musteri No]=(select [Musteri No] from Musteri_tablosu where ID='" + kullanici_ID + "')";
            SqlDataAdapter adpt = new SqlDataAdapter(komut);
            DataTable data = new DataTable();
            adpt.Fill(data);
            dataGridView3.DataSource = data;
            komut.CommandText = "select * from hesap_tablosu where [Musteri No]=(select [Musteri No] from Musteri_tablosu where ID='" + kullanici_ID + "')";
            SqlDataReader reader = komut.ExecuteReader();
            hesap_kapa_comboBox1.Items.Clear();
            while (reader.Read())
            {
                hesap_kapa_comboBox1.Items.Add(reader.GetInt32(0));
            }
            reader.Close();
            baglanti.Close();
        }
       
        private void button3_Click(object sender, EventArgs e)
        {
            Hesaplar_fillgrid();
            tabControl1.SelectedTab = hesap_ac_kapa_tab; ;         
            tabControl1.TabPages.Add(hesap_ac_kapa_tab);
            tabControl1.TabPages.Remove(islemler_tab);
            hesap_ac_kapa_fill();
        }

        private void hesap_ac_button1_Click(object sender, EventArgs e)//hesap acma yeri
        {
            if (baglanti.State == ConnectionState.Closed)
                baglanti.Open();
            SqlCommand komut = new SqlCommand();
            komut.Connection = baglanti;
            decimal ek_hesap = 0;
            decimal bakiye = 0;
            try
            {
                ek_hesap = Convert.ToDecimal(ek_hesap_textboc.Text);
                int musteri_no = 0;
                try
                {
                    if (kullanici_ID != 0)//eger musteri acarsa hesap acma işlemi burdan yapılacak
                    {
                       
                        komut.CommandText = "select * from Musteri_tablosu";
                        SqlDataReader reader = komut.ExecuteReader();

                        while (reader.Read())
                        {
                            if (kullanici_ID == reader.GetInt32(6))
                            {
                                musteri_no = reader.GetInt32(0);
                            }
                        }
                        reader.Close();
                        komut.CommandText = "insert into Hesap_tablosu values('" + musteri_no + "','"
                                   + ek_hesap + "','" + bakiye + "','" + (bakiye + ek_hesap) + "')";
                        komut.ExecuteNonQuery();
                        hesap_kapa_comboBox1.Items.Clear();
                        hesap_ac_kapa_fill();
                    }
                    else//admin hesap açma işlemi icin buradaki kodlar çalısacak              
                    {
                                                          
                        komut.CommandText = "insert into Hesap_tablosu values('" + mus_no + "','"
                                   + ek_hesap + "','" + bakiye + "','" + (bakiye + ek_hesap) + "')";
                        komut.ExecuteNonQuery();
                        hesap_kapa_comboBox1.Items.Clear();
                        admin_hesap_ac_kapa_fill();
                    }
                             
                    MessageBox.Show("Yeni Hesap Olusturulmustur");
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }
                baglanti.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Gecersiz ücret formatı girdiniz");
               
            }
            baglanti.Close();
        }

        private void Mus_kayit_button4_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = kayit_tabPage1;
            tabControl1.TabPages.Remove(ana_tab);
            tabControl1.TabPages.Add(kayit_tabPage1);
            groupBox3.Visible = true;
            groupBox4.Visible = true;
            groupBox5.Visible = false;
            button10.Visible = false;
            giris_kayit_iptal_btn.Visible = true;
            Musteri_groupBox3.Visible = true;
            admin_groupBox4.Visible = false;
        }
        public void admin_silme_para_cek_yatir_fil()
        {
            if (baglanti.State == ConnectionState.Closed)
                baglanti.Open();

            SqlCommand komut = new SqlCommand();
            komut.Connection = baglanti;
            komut.CommandText = "select * from hesap_tablosu where [Hesap No]="+Hesap_no+" ";
            SqlDataAdapter adpt = new SqlDataAdapter(komut);
            DataTable data = new DataTable();
            adpt.Fill(data);
            dataGridView4.DataSource = data;
            baglanti.Close();
        }
        public void admin_para_cek_yatir_fill()
        {
            if (baglanti.State == ConnectionState.Closed)
                baglanti.Open();
           
            SqlCommand komut = new SqlCommand();
            komut.Connection = baglanti;
            komut.CommandText = "select * from hesap_tablosu ";
            SqlDataAdapter adpt = new SqlDataAdapter(komut);
            DataTable data = new DataTable();
            adpt.Fill(data);
            dataGridView4.DataSource = data;
            baglanti.Close();
        }

        public void para_cek_yatir_fill()
        {
            if (baglanti.State == ConnectionState.Closed)
                baglanti.Open();
            para_cek_comboBox1.Items.Clear();
            SqlCommand komut = new SqlCommand();
            komut.Connection = baglanti;
            komut.CommandText = "select * from hesap_tablosu where [Musteri No]=(select [Musteri No] from Musteri_tablosu where ID='" + kullanici_ID + "')";
            SqlDataAdapter adpt = new SqlDataAdapter(komut);
            DataTable data = new DataTable();
            adpt.Fill(data);
            dataGridView4.DataSource = data;
            komut.CommandText = "select * from hesap_tablosu where [Musteri No]=(select [Musteri No] from Musteri_tablosu where ID='" + kullanici_ID + "')";
            SqlDataReader reader = komut.ExecuteReader();

            while (reader.Read())
            {
                para_cek_comboBox1.Items.Add(reader.GetInt32(0));
            }
            reader.Close();
            baglanti.Close();
        }

        private void para_cekmebutton4_Click(object sender, EventArgs e)
        {
            para_cek_comboBox1.Items.Clear();
            tabControl1.TabPages.Remove(islemler_tab);
            tabControl1.TabPages.Add(para_cek_yatir_tab);
            tabControl1.SelectedTab = para_cek_yatir_tab;
            para_cek_yatir_fill();
        }

        private void para_cek_button1_Click(object sender, EventArgs e)
        {

            if (baglanti.State == ConnectionState.Closed)
                baglanti.Open();
            SqlCommand komut = new SqlCommand();
            komut.Connection = baglanti;
            int secilen_hesap = 0;
            decimal cekilecek_para = 0;
            try
            {
                secilen_hesap= Convert.ToInt32(para_cek_comboBox1.Text);
                cekilecek_para = Convert.ToDecimal(para_cek_textBox1.Text);
                decimal cekilmis_para = 0, cekilinebilecek_para = 750;
                decimal yatirilacak_para = 0;
                int kontrol = 0;
                SqlDataReader reader1, reader;

                try
                {
                    komut.CommandText = "select * from Islemler_tablosu where Tarih in('" + dateTimePicker1.Value.Date.ToString("yyyy-MM-dd HH:mm") + "')and [Hesap No]='" + secilen_hesap + "'";//ilk önce belirlenen gün icin belirlenen hesaptan ne kadar para cekilmiş onun kontrolu
                    reader1 = komut.ExecuteReader();
                    while (reader1.Read())
                    {
                        if (reader1.GetInt32(2) == 2)
                            cekilmis_para += reader1.GetDecimal(3);

                        cekilinebilecek_para = 750 - cekilmis_para;
                        if (cekilmis_para >= 750 && reader1.GetInt32(2) == 2)
                            kontrol = 1;
                    }
                    reader1.Close();

                    if (cekilecek_para > 750 || kontrol == 1)
                    {
                        MessageBox.Show("Bir gün içerisinde 750 TL den fazla para çekemezsiniz!\n" + MessageBoxIcon.Warning);
                    }
                    else
                    {
                        //bir gün icerisinde cekilecek maximun paranın müsteriye bildirilip onayını aldım
                        DialogResult dialog = MessageBox.Show("Bu tarih icin cekebileceginiz maximun para :  " + Convert.ToString(cekilinebilecek_para) + "\nEğer cekmek istdediginiz miktar:   " + Convert.ToString(cekilinebilecek_para) + " 'dan fazla ise cekilecek miktar çekilinebilecek miktara eşitlenecektir", "ONAYLIYOR MUSUNUZ?", MessageBoxButtons.YesNo);
                        if (dialog == DialogResult.Yes)
                        {
                            if (cekilecek_para > cekilinebilecek_para)
                                cekilecek_para = cekilinebilecek_para;
                            MessageBox.Show("Çekeilecek Miktar= " + Convert.ToString(cekilecek_para) + "");

                            decimal kullanılbilir_bakiye = 0;
                            decimal ek_hesap = 0;
                            decimal bakiye = 0;
                            komut.CommandText = "select * from Hesap_Tablosu where [Hesap No]='" + secilen_hesap + "'";
                            reader = komut.ExecuteReader();
                            if (reader.Read())
                            {
                                Musteri_no = reader.GetInt32(1);
                                ek_hesap = reader.GetDecimal(2);
                                bakiye = reader.GetDecimal(3);
                                kullanılbilir_bakiye = reader.GetDecimal(4);
                            }
                            reader.Close();


                            if ((bakiye - cekilecek_para) >= 0)//bakiye cekilecek miktardan fazla ise
                            {

                                try
                                {
                                    komut.CommandText = "insert into Islemler_tablosu ([Hesap No],[Islem Turu],Miktar,Tarih) values('" + para_cek_comboBox1.Text + "',2,'" + cekilecek_para + "','" + dateTimePicker1.Value.Date.ToString("yyyy-MM-dd HH:mm") + "')";
                                    komut.ExecuteNonQuery();
                                    komut.CommandText = "insert into MYCO_tablosu ([Musteri No],[Yatirilan Para],[Cekilen Para],Tarih,[Hesap No]) values('" + Musteri_no + "','" + yatirilacak_para + "','" + cekilecek_para + "','" + dateTimePicker1.Value.Date.ToString("yyyy-MM-dd HH:mm") + "','" + secilen_hesap + "') ";
                                    komut.ExecuteNonQuery();
                                    bakiye = bakiye - cekilecek_para;
                                    kullanılbilir_bakiye = kullanılbilir_bakiye - cekilecek_para;
                                    MessageBox.Show("para cekilmiştir");

                                }
                                catch (Exception ex)
                                {

                                    MessageBox.Show(ex.Message);
                                }

                            }
                            else
                            {
                                if (ek_hesap - (cekilecek_para - bakiye) >= 0)//burada cekilecek parar bakiyeden fazla oldugunda ek hesaptan cekilecek paranın ek hesabın miktarını aşmamaması için kontrol
                                {
                                    try
                                    {
                                        komut.CommandText = "insert into Islemler_tablosu ([Hesap No],[Islem Turu],Miktar,Tarih) values('" + para_cek_comboBox1.Text + "',2,'" + cekilecek_para + "','" + dateTimePicker1.Value.Date.ToString("yyyy-MM-dd HH:mm") + "')";
                                        komut.ExecuteNonQuery();
                                        komut.CommandText = "insert into MYCO_tablosu ([Musteri No],[Yatirilan Para],[Cekilen Para],Tarih,[Hesap No]) values('" + Musteri_no + "','" + yatirilacak_para + "','" + cekilecek_para + "','" + dateTimePicker1.Value.Date.ToString("yyyy-MM-dd HH:mm") + "','" + secilen_hesap + "') ";
                                        komut.ExecuteNonQuery();
                                        MessageBox.Show("para cekilmiştir");

                                        bakiye = bakiye - cekilecek_para;

                                        kullanılbilir_bakiye = ek_hesap + bakiye;
                                    }
                                    catch (Exception ex)
                                    {

                                        MessageBox.Show(ex.Message);
                                    }

                                }
                                else
                                    MessageBox.Show("Hesabınızda çekebileceginiz kadar para bulunmamaktadır");

                            }

                            try
                            {
                                komut.CommandText = "update Hesap_tablosu set [EK Hesap(TL)]='" + ek_hesap + "',[Bakiye(TL)]='" + bakiye + "',[Kullanilabilir Bakiye(TL)]='" + kullanılbilir_bakiye + "' where[Hesap No]='" + secilen_hesap + "' ";
                                komut.ExecuteNonQuery();
                                if (kullanici_ID != 0)
                                    para_cek_yatir_fill();
                                else if (silme_kontrol == 1)
                                    admin_silme_para_cek_yatir_fil();
                                else
                                    admin_para_cek_yatir_fill();
                            }
                            catch (Exception ex)
                            {

                                MessageBox.Show(ex.Message);
                            }
                        }
                        else
                            MessageBox.Show("Yeni Ücret Miktarını girin");
                    }
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            
            baglanti.Close();
        }

        private void Hesap_kapa_button2_Click(object sender, EventArgs e)//hesap kapama yeri
        {
            if (baglanti.State == ConnectionState.Closed)
                baglanti.Open();
            SqlCommand komut = new SqlCommand();
            komut.Connection = baglanti;
           
            decimal mevcut_bakiye = 0;
            try
            {
                int hesap_no = Convert.ToInt32(hesap_kapa_comboBox1.Text);
                komut.CommandText = "select [Bakiye(TL)] from Hesap_tablosu where [Hesap No]='" + hesap_no + "' ";
                SqlDataReader reader = komut.ExecuteReader();
                if (reader.Read())
                {
                    mevcut_bakiye = reader.GetDecimal(0);
                }
                reader.Close();
                if (mevcut_bakiye != 0)//kapatılacak hesapta para varsa yada hesabın borcu varsa ona göre yapılacak işlem kontrolu
                {
                    DialogResult dialog = MessageBox.Show("Bu hesabı kapatmanız icin bakiyeniz 0 olmalıdır ve ya hesapta borc olmamalıdır hesabınızı Kapatmak istiyormusunuz", "UYARI", MessageBoxButtons.YesNo);
                    if (dialog == DialogResult.Yes)
                    {
                        tabControl1.SelectedTab = para_cek_yatir_tab;
                        tabControl1.TabPages.Add(para_cek_yatir_tab);
                        tabControl1.TabPages.Remove(hesap_ac_kapa_tab);
                        MessageBox.Show("Silmek istediginiz hesap:   " + hesap_no + "");
                        this.hesap_kapa_geri_don_button1.Visible = true;
                        if (kullanici_ID != 0)
                            para_cek_yatir_fill();
                        else
                            admin_para_cek_yatir_fill();
                        
                           
                    }
                }
                else
                {
                    komut.CommandText = "delete from Hesap_tablosu where [Hesap No]='" + hesap_no + "'";
                    komut.ExecuteNonQuery();
                    MessageBox.Show("hesap silinmiştir");
                    hesap_kapa_comboBox1.Items.RemoveAt(hesap_kapa_comboBox1.SelectedIndex);
                    hesap_kapa_comboBox1.Items.Clear();
                    if (kullanici_ID != 0)
                        hesap_ac_kapa_fill();
                    else
                        admin_hesap_ac_kapa_fill();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            baglanti.Close();
        }

        private void Kayit_button1_Click(object sender, EventArgs e)//müşteri kayıt yeri
        {
            
            if (baglanti.State == ConnectionState.Closed)
                baglanti.Open();
            musteri_kayit_kontrol();
            if (mus_kayit_kont == 1)
            {
                try
                {
                    SqlCommand komut = new SqlCommand();
                    komut.Connection = baglanti;
                    int id = 0;
                    komut.CommandText = "insert into Kullanici_tablosu values('"
                       + ad_kayit_textBox1.Text + "','"
                       + sifre_kayit_textBox2.Text + "')";
                    komut.ExecuteNonQuery();
                    komut.CommandText = "select * from Kullanici_tablosu";
                    SqlDataReader reader = komut.ExecuteReader();
                    while (reader.Read())//müsterinin kullanıcı kaydını yaptıktan sonra müsteri bilgilerini olusturmak icin ID yi aldıgım yer
                    {
                        if (ad_kayit_textBox1.Text == reader.GetString(0) && sifre_kayit_textBox2.Text == reader.GetString(1))
                            id = reader.GetInt32(2);
                    }
                    reader.Close();
                    // MessageBox.Show(id.ToString());
                    komut.CommandText = "insert into Musteri_tablosu values('" + ad_textBox2.Text + "','" + soyad_textBox4.Text +
                        "','" + adres_textBox5.Text + "','" + telefon_textBox6.Text + "','" + e_mail_textBox7.Text + "','" + id + "')";
                    komut.ExecuteNonQuery();
                    mus_kayit_kont = 0;
                    if (mevcut_admin_ad == "" && mevcut_admin_sifre == "")
                    {
                        tabControl1.SelectedTab = ana_tab;
                        tabControl1.TabPages.Add(ana_tab);
                        tabControl1.TabPages.Remove(kayit_tabPage1);
                    }
                    else
                        button10.Visible = true;
                    MessageBox.Show(" Kayıt basarılı");
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }
                baglanti.Close();
            }
            else
                MessageBox.Show("BOŞ ALANLARI DOLDURUN");
        }

        private void mus_giris_button1_Click(object sender, EventArgs e)//müşteri giriş
        {

            if (baglanti.State == ConnectionState.Closed)
                baglanti.Open();
            SqlCommand komut = new SqlCommand();
            komut.Connection = baglanti;
            int kontrol = 0;
            string ad =(Mus_kul_ad_textBox1.Text);
            ad = ad.Trim();
            string sifre = mus_sifre_textBox2.Text;
            sifre = sifre.Trim();
            komut.CommandText = "select * from Kullanici_tablosu";
            SqlDataReader reader = komut.ExecuteReader();
            while (reader.Read())
            {

                if (ad == reader.GetString(0) && sifre == reader.GetString(1))
                {

                    kullanici_ID = reader.GetInt32(2);
                    Musteri_groupBox3.Visible = true;
                    admin_groupBox4.Visible = false;
                    tabControl1.SelectedTab = islemler_tab;
                    tabControl1.TabPages.Remove(ana_tab);
                    tabControl1.TabPages.Add(islemler_tab);
                 
                    kontrol = 1;
                    break;
                }

            }
            reader.Close();
            if (kontrol == 0)
                MessageBox.Show("Geçersiz Giriş");
            baglanti.Close();
        }

        private void para_yatir_button3_Click(object sender, EventArgs e)
        {
            para_cek_comboBox1.Items.Clear();
            tabControl1.SelectedTab = para_cek_yatir_tab;
            tabControl1.TabPages.Remove(islemler_tab);
            tabControl1.TabPages.Add(para_cek_yatir_tab);
            para_cek_yatir_fill();
        }

        private void para_yatir_button2_Click(object sender, EventArgs e)//para yatırma yeri
        {
            if (baglanti.State == ConnectionState.Closed)
                baglanti.Open();
            SqlCommand komut = new SqlCommand();
            komut.Connection = baglanti;
            int secilen_hesap = 0;
            decimal yatirilacak_para = 0, cekilecek_para = 0;
            try
            {
                secilen_hesap = Convert.ToInt32(para_cek_comboBox1.Text);
                yatirilacak_para = Convert.ToDecimal(para_yatir_textBox1.Text);
                komut.CommandText = "select * from Hesap_Tablosu where [Hesap No]='" + secilen_hesap + "'";
                decimal kullanılbilir_bakiye = 0;
                decimal ek_hesap = 0;
                decimal bakiye = 0;
                try
                {
                    SqlDataReader reader = komut.ExecuteReader();

                    if (reader.Read())
                    {
                        Musteri_no = reader.GetInt32(1);
                        ek_hesap = reader.GetDecimal(2);
                        bakiye = reader.GetDecimal(3);
                        kullanılbilir_bakiye = reader.GetDecimal(4);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }

                bakiye = bakiye + yatirilacak_para;
                kullanılbilir_bakiye = bakiye + ek_hesap;


                try
                {
                    komut.CommandText = "insert into Islemler_tablosu ([Hesap No],[Islem Turu],Miktar,Tarih) values('" + para_cek_comboBox1.Text + "',1,'" + para_yatir_textBox1.Text + "','" + dateTimePicker1.Value.Date.ToString("yyyy-MM-dd HH:mm") + "')";
                    komut.ExecuteNonQuery();
                    komut.CommandText = "insert into MYCO_tablosu ([Musteri No],[Yatirilan Para],[Cekilen Para],Tarih,[Hesap No]) values('" + Musteri_no + "','" + yatirilacak_para + "','" + cekilecek_para + "','" + dateTimePicker1.Value.Date.ToString("yyyy-MM-dd HH:mm") + "','" + secilen_hesap + "') ";
                    komut.ExecuteNonQuery();
                    komut.CommandText = "update Hesap_tablosu set [EK Hesap(TL)]='" + ek_hesap + "',[Bakiye(TL)]='" + bakiye + "',[Kullanilabilir Bakiye(TL)]='" + kullanılbilir_bakiye + "' where[Hesap No]='" + secilen_hesap + "' ";
                    komut.ExecuteNonQuery();
                    if (kullanici_ID != 0)
                        para_cek_yatir_fill();
                    else if (silme_kontrol == 1)
                        admin_silme_para_cek_yatir_fil();
                    else
                        admin_para_cek_yatir_fill();
                    MessageBox.Show("para yatırılmıstır");
                }
                catch (Exception ex)
                {

                    MessageBox.Show("para yatırma basarısız\n" + ex.Message);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
           
            baglanti.Close();
        }

        private void Mus_guncelle_button1_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = guncelle_tabPage1;
            tabControl1.TabPages.Add(guncelle_tabPage1);
            tabControl1.TabPages.Remove(islemler_tab);
            groupBox8.Visible = false;
        }

        private void guncelle_sifre_button1_Click(object sender, EventArgs e)//müsteri sifresi günelleme yeri
        {
            if (baglanti.State == ConnectionState.Closed)
                baglanti.Open();
            muster_sifre_guncelle();
            if (mus_sifre_gunclee_kont == 1)
            {
                SqlCommand komut = new SqlCommand();
                komut.Connection = baglanti;
                int eski_kontrol = 0, yeni_kontrol = 0;
                try
                {

                    komut.CommandText = "select Sifre from Kullanici_tablosu where [Kullanici ID]='" + kullanici_ID + "'";
                    SqlDataReader reader = komut.ExecuteReader();
                    if (reader.Read())
                    {
                        if (eski_sifretextBox1.Text != reader.GetString(0))
                        {
                           
                            MessageBox.Show("Geçersiz Şifre");
                        }
                        else
                        {
                            
                            eski_kontrol = 1;
                        }
                    }
                    reader.Close();


                    if (yeni_sifre_textBox2.Text != sifre_dogrula_textBox4.Text || yeni_sifre_textBox2.Text == "")
                    {
                      
                        MessageBox.Show("Sifreler Uyumlu degil");
                    }
                    else
                    {
                        yeni_kontrol = 1;
                    }
                    if (yeni_kontrol == 1 && eski_kontrol == 1)
                    {
                        komut.CommandText = "update Kullanici_tablosu set Sifre='" + yeni_sifre_textBox2.Text + "'where [Kullanici ID]='" + kullanici_ID + "' ";
                        komut.ExecuteNonQuery();
                        MessageBox.Show("Sifre Başarıyla Güncellendi");
                    }

                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }
                baglanti.Close();
            }
            else
                MessageBox.Show("BOŞ ALAN BIRAKMAYINIZ1");
        }

        private void guncelle_bilgi_button3_Click(object sender, EventArgs e)//müsteri bilgileri güncelleme yeri
        {
            if (baglanti.State == ConnectionState.Closed)
                baglanti.Open();
            mus_bilgi_guncelle();
            if (mus_bil_gunclelle == 1)
            {
                SqlCommand komut = new SqlCommand();
                komut.Connection = baglanti;
                try
                {
                    komut.CommandText = "update Musteri_tablosu set Ad='" + ad_guncelle_textBox5.Text + "',Soyad='" + soyad_guncellle_textBox6.Text + "',Adres='" + adres_guncelle_textBox7.Text + "',Telefon='" + tele_guncelle_textBox8.Text + "',[E-mail]='" + mail_guncelle_textBox9.Text + "' where ID='" + kullanici_ID + "'";

                    komut.ExecuteNonQuery();
                    MessageBox.Show("guncellendi");
                }
                catch (Exception)
                {

                    MessageBox.Show("basarısız");

                }
                baglanti.Close();
            }
            else
                MessageBox.Show("BOŞ ALAN BIRAKMAYINIZ!");
        }

        private void hesap_kapa_geri_don_button1_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = hesap_ac_kapa_tab;
            tabControl1.TabPages.Add(hesap_ac_kapa_tab);
            tabControl1.TabPages.Remove(para_cek_yatir_tab);
            if (kullanici_ID != 0)
                hesap_ac_kapa_fill();
            else
                admin_hesap_ac_kapa_fill();
            this.hesap_kapa_geri_don_button1.Visible = false;
        }

        private void havale_button3_Click(object sender, EventArgs e)//havale yeri
        {
            if (baglanti.State == ConnectionState.Closed)
                baglanti.Open();

            SqlCommand komut = new SqlCommand();
            komut.Connection = baglanti;
       
            decimal havale_miktari = 0,bakiye=0;
            string ad1="",ad2="", soyad1="",soyad2="";
            int Mno1 = 0,Mno2=0,havale_kontrol=0;
            try
            {
                do
                {
                    havale_miktari = Convert.ToDecimal(havale_textBox1.Text);
                    if (havale_miktari <= 0)
                    {
                        MessageBox.Show("havale miktarı 0 ve 0 küçük olamaz");
                        havale_kontrol = 1;
                        break;
                    }
                    else
                    {
                        komut.CommandText = "select [Bakiye(TL)] from Hesap_tablosu where [Hesap No]='" + Convert.ToInt32(gondere_comboBox1.Text) + "'";
                        SqlDataReader reader = komut.ExecuteReader();
                        while (reader.Read())
                        {
                            bakiye = reader.GetDecimal(0);
                        }
                        reader.Close();
                        if (bakiye <= 0)
                        {
                            MessageBox.Show("Bu hesabın bakiyesi 0 ve ya 0 dan az oldugu için bu hesaptan havale gönderemezsiniz!");
                            havale_kontrol = 1;
                        }
                        else if (bakiye < havale_miktari)
                        {
                            DialogResult dialog = MessageBox.Show("Havale yapmak istedigin miktar bu hesabın bakiyesinden yüksektir eğer havale yapmak istereniz havale miktarı bakiye miktarınıza eşitlenecektir", " ONAYLIYOR MUSUNUZ!!", MessageBoxButtons.YesNo);
                            if (dialog == DialogResult.Yes)
                            {
                                havale_miktari = bakiye;
                                MessageBox.Show("havale edilecek miktar="+ bakiye.ToString()+"");
                                havale_textBox1.Text =bakiye.ToString();
                            }
                            else
                            {
                                MessageBox.Show("Yeni havala miktarı girin!");
                                havale_miktari = 0;
                                havale_textBox1.Text = "0";
                            }
                        }
                        if (havale_miktari >0)
                            break;
                    }
                } while (true);
                if (gondere_comboBox1.Text == alan_comboBox2.Text)
                {
                    MessageBox.Show("Gönderen Hesapla Alan Hesap Aynı Olamaz");
                }
                else
                {
                    if (havale_kontrol == 0)
                    {
                        try
                        {

                            komut.CommandText = "select * from Musteri_tablosu where [Musteri No] in (select [Musteri No] from Hesap_tablosu where [Hesap No]='" + Convert.ToInt32(gondere_comboBox1.Text) + "') ";
                            SqlDataReader reader = komut.ExecuteReader();
                            while (reader.Read())
                            {
                                Mno1 = reader.GetInt32(0);
                                ad1 = reader.GetString(1);
                                soyad1 = reader.GetString(2);
                            }
                            reader.Close();
                        }
                        catch (Exception ex)
                        {

                            MessageBox.Show(ex.Message);
                        }

                        try
                        {

                            komut.CommandText = "select * from Musteri_tablosu where [Musteri No] in (select [Musteri No] from Hesap_tablosu where [Hesap No]='" + Convert.ToInt32(alan_comboBox2.Text) + "') ";
                            SqlDataReader reader = komut.ExecuteReader();
                            while (reader.Read())
                            {
                                Mno2 = reader.GetInt32(0);
                                ad2 = reader.GetString(1);
                                soyad2 = reader.GetString(2);
                            }
                            reader.Close();
                        }
                        catch (Exception ex)
                        {

                            MessageBox.Show(ex.Message);
                        }
                        try
                        {
                            komut.CommandText = "insert into Havale_tablosu ([Gonderen Hesap],[Alan Hesap],Tarih) values('" + Convert.ToInt32(gondere_comboBox1.Text) + "','" + Convert.ToInt32(alan_comboBox2.Text) + "','" + dateTimePicker2.Value.Date.ToString("yyyy-MM-dd HH:mm") + "') ";
                            komut.ExecuteNonQuery();
                            komut.CommandText = "insert into Havale_giden ([Hesap No],[Musteri No],Ad,Soyad,[Miktar(TL)],Tarih) values('" + Convert.ToInt32(gondere_comboBox1.Text) + "'," + Mno1 + ",'" + ad1 + "','" + soyad1 + "'," + havale_miktari + ",'" + dateTimePicker2.Value.Date.ToString("yyyy-MM-dd HH:mm") + "')";
                            komut.ExecuteNonQuery();
                            komut.CommandText = "insert into Havale_gelen ([Hesap No],[Musteri No],Ad,Soyad,[Miktar(TL)],Tarih) values('" + Convert.ToInt32(alan_comboBox2.Text) + "'," + Mno2 + ",'" + ad2 + "','" + soyad2 + "'," + havale_miktari + ",'" + dateTimePicker2.Value.Date.ToString("yyyy-MM-dd HH:mm") + "')";
                            komut.ExecuteNonQuery();
                            komut.CommandText = "update Hesap_tablosu set [Bakiye(TL)]=[Bakiye(TL)]-" + havale_miktari + ",[Kullanilabilir Bakiye(TL)]=[kullanilabilir Bakiye(TL)]-" + havale_miktari + " where [Hesap No]=" + Convert.ToInt32(gondere_comboBox1.Text) + " ";
                            komut.ExecuteNonQuery();
                            komut.CommandText = "update Hesap_tablosu set [Bakiye(TL)]=[Bakiye(TL)]+" + havale_miktari + ",[Kullanilabilir Bakiye(TL)]=[kullanilabilir Bakiye(TL)]+" + havale_miktari + " where [Hesap No]=" + Convert.ToInt32(alan_comboBox2.Text) + " ";
                            komut.ExecuteNonQuery();
                            MessageBox.Show("havale gercekleşmiştir");
                            havale_alan_fill();
                            havale_gonder_fill();
                        }
                        catch (Exception ex)
                        {

                            MessageBox.Show(ex.Message);
                        }
                    }
                }
                baglanti.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("ücret girmediniz");
            }
            
            baglanti.Close();
        }

        private void gondere_comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            havale_gonder_fill();
        }
        private void havale_gonder_fill()
        {
            if (baglanti.State == ConnectionState.Closed)
                baglanti.Open();

            SqlCommand komut = new SqlCommand();
            komut.Connection = baglanti;
            komut.CommandText = "select * from Hesap_tablosu where [Hesap No]=('" + Convert.ToInt32(gondere_comboBox1.Text) + "')";
            SqlDataAdapter adpt = new SqlDataAdapter(komut);
            DataTable data = new DataTable();
            adpt.Fill(data);
            dataGridView5.DataSource = data;
            baglanti.Close();
        }

        private void alan_comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            havale_alan_fill();
        }
        private void havale_alan_fill()
        {
            if (baglanti.State == ConnectionState.Closed)
                baglanti.Open();

            SqlCommand komut = new SqlCommand();
            komut.Connection = baglanti;
            komut.CommandText = "select * from Hesap_tablosu where [Hesap No]=('" + Convert.ToInt32(alan_comboBox2.Text) + "')";
            SqlDataAdapter adpt = new SqlDataAdapter(komut);
            DataTable data = new DataTable();
            adpt.Fill(data);
            dataGridView6.DataSource = data;
            baglanti.Close();
        }

        private void Hesap_ozeti_button1_Click(object sender, EventArgs e)//hesap özeti
        {
            if (baglanti.State == ConnectionState.Closed)
                baglanti.Open();
            tabControl1.TabPages.Add(Musteri_hesap_ozeti_tabPage1);
            tabControl1.TabPages.Remove(islemler_tab);
            SqlCommand komut = new SqlCommand();
            komut.Connection = baglanti;
            try
            {
                komut.CommandText = "select [Hesap No] from Hesap_tablosu ";
                SqlDataReader redaer = komut.ExecuteReader();
                while (redaer.Read())
                {
                    Ozet_comboBox1.Items.Add(Convert.ToString(redaer.GetInt32(0)));
                }
                redaer.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            tabControl1.SelectedTab = Musteri_hesap_ozeti_tabPage1;
        }

        private void goster_button1_Click(object sender, EventArgs e)//secilen hesabın belirlenen tarihteki hesap dökümü
        {
            if (baglanti.State == ConnectionState.Closed)
                baglanti.Open();

            SqlCommand komut = new SqlCommand();
            komut.Connection = baglanti;

            try
            {
                komut.CommandText = "select * from MYCO_tablosu where [Hesap No]='" + Convert.ToInt32(Ozet_comboBox1.Text) + "' and (Tarih) between '" + dateTimePicker3.Value.Date.ToString("yyyy-MM-dd HH:mm") + "' and '" + dateTimePicker4.Value.Date.ToString("yyyy-MM-dd HH:mm") + "'";
                SqlDataAdapter adpt = new SqlDataAdapter(komut);
                DataTable data = new DataTable();
                adpt.Fill(data);
                dataGridView7.DataSource = data;
                komut.CommandText = "select * from Havale_gelen where [Hesap No]='" + Convert.ToInt32(Ozet_comboBox1.Text) + "' and (Tarih) between '" + dateTimePicker3.Value.Date.ToString("yyyy-MM-dd HH:mm") + "' and '" + dateTimePicker4.Value.Date.ToString("yyyy-MM-dd HH:mm") + "'";
                SqlDataAdapter adpt1 = new SqlDataAdapter(komut);
                DataTable data1 = new DataTable();
                adpt.Fill(data1);
                dataGridView8.DataSource = data1;
                komut.CommandText = "select * from Havale_giden where [Hesap No]='" + Convert.ToInt32(Ozet_comboBox1.Text) + "' and (Tarih) between '" + dateTimePicker3.Value.Date.ToString("yyyy-MM-dd HH:mm") + "' and '" + dateTimePicker4.Value.Date.ToString("yyyy-MM-dd HH:mm") + "'";
                SqlDataAdapter adpt2 = new SqlDataAdapter(komut);
                DataTable data2 = new DataTable();
                adpt.Fill(data2);
                dataGridView9.DataSource = data2;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            baglanti.Close();
        }

        private void havale_button5_Click(object sender, EventArgs e)//müsteri havale yapmak isteriginde bilgilerin dökümü
        {
            tabControl1.SelectedTab = havale_tabPage1;         
            tabControl1.TabPages.Add(havale_tabPage1);
            tabControl1.TabPages.Remove(islemler_tab);
            if (baglanti.State == ConnectionState.Closed)
                baglanti.Open();
           
            SqlCommand komut = new SqlCommand();
            komut.Connection = baglanti;
            try
            {
                komut.CommandText = "select [Hesap No] from hesap_tablosu where[Musteri No] = (select[Musteri No] from Musteri_tablosu where ID = '" + kullanici_ID + "')";
                SqlDataReader reader = komut.ExecuteReader();
                gondere_comboBox1.Items.Clear();
                while (reader.Read())
                {
                    gondere_comboBox1.Items.Add(reader.GetInt32(0));
                }
                reader.Close();

                komut.CommandText = "select [Hesap No] from hesap_tablosu";
                SqlDataReader reader1 = komut.ExecuteReader();
                alan_comboBox2.Items.Clear();
                while (reader1.Read())
                {
                    alan_comboBox2.Items.Add(reader1.GetInt32(0));
                }
                reader1.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
           
            baglanti.Close();
        }

        private void rapor_button1_Click(object sender, EventArgs e)//hesap raporu
        {
            if (baglanti.State == ConnectionState.Closed)
                baglanti.Open();
            decimal giden = 0, gelen = 0, toplam = 0;
            SqlCommand komut = new SqlCommand();
            SqlCommand komut1 = new SqlCommand();
            SqlCommand komut2 = new SqlCommand();
            komut2.Connection = baglanti;
            komut1.Connection = baglanti;
            komut.Connection = baglanti;
            try
            {
                komut1.CommandText = "select ([Kullanilabilir Bakiye(TL)]) from Hesap_tablosu";
                SqlDataReader reader1 = komut1.ExecuteReader();
                while(reader1.Read())
                {
                    toplam += reader1.GetDecimal(0);
                }
                reader1.Close();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            try
            {
                komut2.CommandText = "select Miktar,[Islem Turu] from Islemler_tablosu where Tarih > '" + dateTimePicker5.Value.Date.ToString("yyyy-MM-dd HH:mm") + "'";
                SqlDataReader reader2 = komut2.ExecuteReader();
                while (reader2.Read())
                {
                    if (reader2.GetInt32(1) == 1)
                    {
                        toplam -= reader2.GetDecimal(0);
                    }
                    if (reader2.GetInt32(1) == 2)
                    {
                        toplam += reader2.GetDecimal(0);
                    }
                }
                reader2.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            try
            {
                komut.CommandText = "select Miktar,[Islem Turu],Tarih from Islemler_tablosu where Tarih='" + dateTimePicker5.Value.Date.ToString("yyyy-MM-dd HH:mm") + "' ";
                SqlDataReader reader = komut.ExecuteReader();
                while (reader.Read())
                {
                    if (reader.GetInt32(1) == 1)
                    {
                        gelen += reader.GetDecimal(0);                                               
                    }
                    if (reader.GetInt32(1) == 2)
                    {
                        giden += reader.GetDecimal(0);                     
                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            banka_giden_textBox1.Text = Convert.ToString(giden);
            banka_gelen_textBox2.Text = Convert.ToString(gelen);
            banka_toplam_textBox5.Text = Convert.ToString(toplam);
        }
        private void Gelir_gider_button2_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = rapor_tabPage1;
            tabControl1.TabPages.Add(rapor_tabPage1);
            tabControl1.TabPages.Remove(islemler_tab);
        }

        private void admin_kayit_button_Click(object sender, EventArgs e)//admin kayıt
        {
            if (baglanti.State == ConnectionState.Closed)
                baglanti.Open();
            admin_kayit_kontrol();
            if (admin_kayıt_kont == 1)
            {
                SqlCommand komut = new SqlCommand();
                komut.Connection = baglanti;
                try
                {
                    komut.CommandText = "insert into Admin_kullanici_adi values( '" + admin_ad_kayittextBox6.Text + "','" + admin_sifre_kayit_textBox2.Text + "')";
                    komut.ExecuteNonQuery();
                    admin_guncelle_kont = 0;
                    MessageBox.Show("kayıt başarılı");
                    if (mevcut_admin_ad == "" && mevcut_admin_sifre == "")
                    {
                        tabControl1.SelectedTab = ana_tab;
                        tabControl1.TabPages.Add(ana_tab);
                        tabControl1.TabPages.Remove(kayit_tabPage1);
                    }
                    else
                        button10.Visible = true;
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }
            }
            else
                MessageBox.Show("bos alan bırakmayın");
        }

        private void adm_kayit_button5_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = kayit_tabPage1;
            tabControl1.TabPages.Remove(ana_tab);
            tabControl1.TabPages.Add(kayit_tabPage1);
            groupBox5.Visible = true;
            groupBox3.Visible = false;
            groupBox4.Visible = false;
            button10.Visible = false;
            giris_kayit_iptal_btn.Visible = true;
            Musteri_groupBox3.Visible = false;
            admin_groupBox4.Visible = true;
        }

        private void adm_giris_button2_Click(object sender, EventArgs e)//admin giris
        {
            if (baglanti.State == ConnectionState.Closed)
                baglanti.Open();
            SqlCommand komut = new SqlCommand();
            int kontrol = 0;
            komut.Connection = baglanti;
            komut.CommandText = "select * from Admin_kullanici_adi";
            SqlDataReader reader = komut.ExecuteReader();
            while(reader.Read())
            {
                if(admin_ad_giris_textBox4.Text.Trim()==reader.GetString(0)&& admin_sifre_giris_textBox3.Text.Trim()==reader.GetString(1))
                {
                    mevcut_admin_ad = admin_ad_giris_textBox4.Text;
                    mevcut_admin_sifre = admin_sifre_giris_textBox3.Text;
                    tabControl1.SelectedTab = islemler_tab;
                    tabControl1.TabPages.Remove(ana_tab);
                    tabControl1.TabPages.Add(islemler_tab);
                    Musteri_groupBox3.Visible = false;
                    admin_groupBox4.Visible = true;

                    kontrol = 1;
                    break;
                }
            }
            reader.Close();
            if (kontrol == 0)
                MessageBox.Show("geçersiz giriş");
            baglanti.Close();
        }

        private void admin_para_cek_button5_Click(object sender, EventArgs e)//admin in para cekmesi icin bilgi dökümü
        {
            para_cek_comboBox1.Items.Clear();
            tabControl1.SelectedTab = para_cek_yatir_tab;
            tabControl1.TabPages.Remove(islemler_tab);
            tabControl1.TabPages.Add(para_cek_yatir_tab);
            if (baglanti.State == ConnectionState.Closed)
                baglanti.Open();      
            SqlCommand komut = new SqlCommand();
            komut.Connection = baglanti;
            komut.CommandText = "select * from hesap_tablosu ";
            SqlDataAdapter adpt = new SqlDataAdapter(komut);
            DataTable data = new DataTable();
            adpt.Fill(data);
            dataGridView4.DataSource = data;
            komut.CommandText = "select [Hesap No] from Hesap_tablosu";
            SqlDataReader reader = komut.ExecuteReader();

            while (reader.Read())
            {
                para_cek_comboBox1.Items.Add(reader.GetInt32(0));
            }
            reader.Close();
            baglanti.Close();
        }

        private void admin_para_yatirbutton4_Click(object sender, EventArgs e)//admin para yatırması için bilgi dökümü
        {
            para_cek_comboBox1.Items.Clear();
            tabControl1.TabPages.Remove(islemler_tab);
            tabControl1.TabPages.Add(para_cek_yatir_tab);
            tabControl1.SelectedTab = para_cek_yatir_tab;
            if (baglanti.State == ConnectionState.Closed)
                baglanti.Open();
            SqlCommand komut = new SqlCommand();
            komut.Connection = baglanti;
            komut.CommandText = "select * from hesap_tablosu ";
            SqlDataAdapter adpt = new SqlDataAdapter(komut);
            DataTable data = new DataTable();
            adpt.Fill(data);
            dataGridView4.DataSource = data;
            komut.CommandText = "select [Hesap No] from Hesap_tablosu";
            SqlDataReader reader = komut.ExecuteReader();

            while (reader.Read())
            {
                para_cek_comboBox1.Items.Add(reader.GetInt32(0));
            }
            reader.Close();
            baglanti.Close();
        }

        private void admin_havale_button6_Click(object sender, EventArgs e)//admin havale yapmak isteriginde bilgilerin dökümü
        {
            tabControl1.SelectedTab = havale_tabPage1;
            tabControl1.TabPages.Add(havale_tabPage1);
            tabControl1.TabPages.Remove(islemler_tab);
            if (baglanti.State == ConnectionState.Closed)
                baglanti.Open();

            SqlCommand komut = new SqlCommand();
            komut.Connection = baglanti;
            try
            {
                komut.CommandText = "select [Hesap No] from hesap_tablosu";
                SqlDataReader reader = komut.ExecuteReader();
                gondere_comboBox1.Items.Clear();
                while (reader.Read())
                {
                    gondere_comboBox1.Items.Add(reader.GetInt32(0));
                }
                reader.Close();

                komut.CommandText = "select [Hesap No] from hesap_tablosu";
                SqlDataReader reader1 = komut.ExecuteReader();
                alan_comboBox2.Items.Clear();
                while (reader1.Read())
                {
                    alan_comboBox2.Items.Add(reader1.GetInt32(0));
                }
                reader1.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

            baglanti.Close();
        }

        private void admin_hesap_ac_kapa_button7_Click(object sender, EventArgs e)//admin hesap acma için bilgi dökümü
        {
            tabControl1.SelectedTab = hesap_ac_kapa_tab; ;
            tabControl1.TabPages.Add(hesap_ac_kapa_tab);
            tabControl1.TabPages.Remove(islemler_tab);
            if (baglanti.State == ConnectionState.Closed)
                baglanti.Open();
         

            SqlCommand komut1 = new SqlCommand();
         
         
            komut1.Connection = baglanti;
            komut1.CommandText = "select * from Musteri_tablosu";
            SqlDataAdapter adpt1 = new SqlDataAdapter(komut1);
            DataTable data1 = new DataTable();
            adpt1.Fill(data1);
            dataGridView2.DataSource = data1;
            admin_hesap_ac_kapa_fill();
        
            baglanti.Close();
        }

        private void Hesap_sec_btn_Click(object sender, EventArgs e)//admin hesap acma ve kapama icin hangi müsteri icin acacaksa onun bilgileri icin gerekli methot
        {

            if (baglanti.State == ConnectionState.Closed)
                baglanti.Open();
         
            // MessageBox.Show(mus_no);
            try
            {
                int satir = dataGridView2.SelectedCells[0].RowIndex;
                mus_no = (int)dataGridView2.Rows[satir].Cells[0].Value;
                SqlCommand komut = new SqlCommand();
                komut.Connection = baglanti;
                komut.CommandText = "select * from hesap_tablosu where [Musteri No]='" + mus_no + "' ";
                SqlDataAdapter adpt = new SqlDataAdapter(komut);
                DataTable data = new DataTable();
                adpt.Fill(data);
                dataGridView3.DataSource = data;
                komut.CommandText = "select * from hesap_tablosu where [Musteri No]='" + mus_no + "' ";
                SqlDataReader reader = komut.ExecuteReader();
                hesap_kapa_comboBox1.Items.Clear();
                while (reader.Read())
                {
                    hesap_kapa_comboBox1.Items.Add(reader.GetInt32(0));
                }
                reader.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void Adm_guncelle_button1_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = guncelle_tabPage1;
            tabControl1.TabPages.Add(guncelle_tabPage1);
            tabControl1.TabPages.Remove(islemler_tab);
            groupBox8.Visible = true;
        }

        private void Kullanici_silmebutton1_Click(object sender, EventArgs e)//kullanucu silme icin bilgi dökümü
        {
            tabControl1.SelectedTab = Kullanici_silme_tabPage1;
            tabControl1.TabPages.Add(Kullanici_silme_tabPage1);
            tabControl1.TabPages.Remove(islemler_tab);
            if (baglanti.State == ConnectionState.Closed)
                baglanti.Open();
            SqlCommand komut = new SqlCommand();
            komut.Connection = baglanti;
           
            komut.CommandText = "select * from Admin_kullanici_adi where ([Kullanici Adi]<>'admin' and Sifre<>'admin') and ([Kullanici Adi]<>'"+mevcut_admin_ad+"' and Sifre<>'"+mevcut_admin_sifre+"')";
            SqlDataAdapter adpt = new SqlDataAdapter(komut);
            DataTable data = new DataTable();
            adpt.Fill(data);
            dataGridView11.DataSource = data;
            komut.CommandText = "select * from Kullanici_tablosu";
            SqlDataAdapter adpt1 = new SqlDataAdapter(komut);
            DataTable data1 = new DataTable();
            adpt1.Fill(data1);
            dataGridView12.DataSource = data1;
            baglanti.Close();
        }

        private void Mus_kayit_button1_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = kayit_tabPage1;
            tabControl1.TabPages.Add(kayit_tabPage1);
            tabControl1.TabPages.Remove(islemler_tab);
            groupBox3.Visible = true;
            groupBox4.Visible = true;
            kayit_iptal_btn.Visible = true;
        }

        private void Islem_ekle_btn_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = islem_ekle_tab;
            tabControl1.TabPages.Add(islem_ekle_tab);
            tabControl1.TabPages.Remove(islemler_tab);
            
        }

        private void islem_ekle_button1_Click(object sender, EventArgs e)//işlem ekleme yeri
        {
            if (baglanti.State == ConnectionState.Closed)
                baglanti.Open();
            SqlCommand komut = new SqlCommand();
            komut.Connection = baglanti;
            komut.CommandText = "insert into Islem_turu_tablosu values('"+islem_ekle_textbox.Text+"')";
            komut.ExecuteNonQuery();         
            komut.CommandText = "select * from Islem_turu_tablosu ";
            SqlDataAdapter adpt = new SqlDataAdapter(komut);        
            DataTable data = new DataTable();        
            adpt.Fill(data);
            dataGridView1.DataSource = data;
            dataGridView10.DataSource = data;
            baglanti.Close();
        }

        private void islem_sil_button1_Click(object sender, EventArgs e)//işlem silme yeri
        {

            if (baglanti.State == ConnectionState.Closed)
                baglanti.Open();
            SqlCommand komut = new SqlCommand();
            komut.Connection = baglanti;
          
            int satir = dataGridView10.SelectedCells[0].RowIndex;
            int islem_no =(int)dataGridView10.Rows[satir].Cells[0].Value;
            if(islem_no==1|| islem_no==2|| islem_no==3)//ilk 3 işlemi sildirmedim
            {
               // MessageBox.Show(islem_no.ToString());
                MessageBox.Show("BU işlemleri Silemezsinizi");
            }
            else
            {
                komut.CommandText = "delete from Islem_turu_tablosu where [Islem ID#]=" + islem_no + "";
                komut.ExecuteNonQuery();
                MessageBox.Show("İşlem Türü Slinmiştir");
                komut.CommandText = "select * from Islem_turu_tablosu ";
                SqlDataAdapter adpt = new SqlDataAdapter(komut);
                DataTable data = new DataTable();
                adpt.Fill(data);
                dataGridView1.DataSource = data;
                dataGridView10.DataSource = data;
                baglanti.Close();
            }

        }
        public void mus_sec()//kullanıcı silme yerinde musterinin tüm bilgilerini için bilgi dökümü
        {
            if (baglanti.State == ConnectionState.Closed)
                baglanti.Open();
            SqlCommand komut = new SqlCommand();
            komut.Connection = baglanti;
            int sayac = 0;
            try
            {
                komut.CommandText = "select * from Kullanici_tablosu";
                SqlDataReader reader = komut.ExecuteReader();
                while(reader.Read())
                {
                    sayac++;
                }
                if (sayac > 0)
                {
                    int satir = dataGridView12.SelectedCells[0].RowIndex;
                    ID = (int)dataGridView12.Rows[satir].Cells[2].Value;
                }
                reader.Close();
                komut.CommandText = "select * from Musteri_tablosu where ID=" + ID + "";
                SqlDataAdapter adpt = new SqlDataAdapter(komut);
                DataTable data = new DataTable();
                adpt.Fill(data);
                dataGridView13.DataSource = data;
                komut.CommandText = "select [Musteri No] from Musteri_tablosu where ID=" + ID + "";
                reader = komut.ExecuteReader();
                if (reader.Read())
                    M_no = reader.GetInt32(0);
                reader.Close();
                komut.CommandText = "select * from Hesap_tablosu where [Musteri No]=" + M_no + "";
                SqlDataAdapter adpt1 = new SqlDataAdapter(komut);
                DataTable data1 = new DataTable();
                adpt1.Fill(data1);
                dataGridView14.DataSource = data1;
                dataGridView12.Enabled = false;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            baglanti.Close();
           
          
        }
        private void mus_Sec_btn_Click(object sender, EventArgs e)
        {
            mus_sec();

        }

        private void admin_sil_btn_Click(object sender, EventArgs e)//admin bilgilerini silme yeri
        {
            if (baglanti.State == ConnectionState.Closed)
                baglanti.Open();
            SqlCommand komut = new SqlCommand();
            komut.Connection = baglanti;
            int satir = dataGridView11.SelectedCells[0].RowIndex;
            string ad = dataGridView11.Rows[satir].Cells[0].Value.ToString();
            //MessageBox.Show(ad);
            try
            {
                komut.CommandText = "delete from Admin_kullanici_adi where [Kullanici Adi]='" + ad + "'";
                komut.ExecuteNonQuery();
                komut.CommandText = "select * from Admin_kullanici_adi where ([Kullanici Adi]<>'admin' and Sifre<>'admin') and ([Kullanici Adi]<>'" + mevcut_admin_ad + "' and Sifre<>'" + mevcut_admin_sifre + "')";
                SqlDataAdapter adpt = new SqlDataAdapter(komut);
                DataTable data = new DataTable();
                adpt.Fill(data);
                dataGridView11.DataSource = data;
                MessageBox.Show("Kullanici Silinmiştir");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            baglanti.Close();
        }

       private void mus_sil_btn_Click(object sender, EventArgs e)//meüsterinin bütün hesaplarını silem yeri 
        {
            if (baglanti.State == ConnectionState.Closed)
                baglanti.Open();
            SqlCommand komut = new SqlCommand();
            komut.Connection = baglanti;
            int satir = 0;
            decimal bakiye = 0;
            try
            {
                 satir = dataGridView14.SelectedCells[0].RowIndex;
                Hesap_no = (int)dataGridView14.Rows[satir].Cells[0].Value;
                 bakiye = (decimal)dataGridView14.Rows[satir].Cells[3].Value;
            }
            catch (Exception)
            {

                MessageBox.Show("HEsap Seçin");
            }
            //MessageBox.Show(bakiye.ToString());
            try
            {
                if (bakiye == 0)
                {
                    komut.CommandText = "delete from Hesap_tablosu where [Hesap No]=" + Hesap_no + "";
                    komut.ExecuteNonQuery();
                    komut.CommandText = "select * from Hesap_tablosu where [Musteri No]=" + M_no + "";
                    SqlDataAdapter adpt1 = new SqlDataAdapter(komut);
                    DataTable data1 = new DataTable();
                    adpt1.Fill(data1);
                    dataGridView14.DataSource = data1;
                    MessageBox.Show("Hesap Silinmiştir");
                    silme_kontrol = 0;
                }
                else
                {
                    DialogResult dialog = MessageBox.Show("Bu hesabı kapatmanız icin bakiyeniz 0 olmalıdır hesabınızı Kapatmak istiyormusunuz", "UYARI", MessageBoxButtons.YesNo);
                    if (dialog == DialogResult.Yes)
                    {
                        tabControl1.SelectedTab = para_cek_yatir_tab;
                        tabControl1.TabPages.Add(para_cek_yatir_tab);
                        tabControl1.TabPages.Remove(Kullanici_silme_tabPage1);
                        komut.CommandText = "select * from Hesap_tablosu where [Hesap No]=" + Hesap_no + "";
                        SqlDataAdapter adpt = new SqlDataAdapter(komut);
                        DataTable data = new DataTable();
                        adpt.Fill(data);
                        dataGridView4.DataSource = data;
                        para_cek_comboBox1.Items.Clear();
                        para_cek_comboBox1.Items.Add(Hesap_no);
                        silme_kontrol = 1;
                        admin_mus_hesap_sil_btn.Visible = true;

                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            baglanti.Close();
        }

        private void mu_sec_iptal_btn_Click(object sender, EventArgs e)
        {
            dataGridView12.Enabled = true;
            
        }

        private void admin_mus_hesap_sil_btn_Click(object sender, EventArgs e)//müsteri hesap silme yeri
        {
            if (baglanti.State == ConnectionState.Closed)
                baglanti.Open();
            tabControl1.SelectedTab = Kullanici_silme_tabPage1;
            tabControl1.TabPages.Add(Kullanici_silme_tabPage1);
            tabControl1.TabPages.Remove(para_cek_yatir_tab);
            SqlCommand komut = new SqlCommand();
            komut.Connection = baglanti;
            komut.CommandText = "select * from Hesap_tablosu where [Musteri No]=" + M_no + "";
            SqlDataAdapter adpt1 = new SqlDataAdapter(komut);
            DataTable data1 = new DataTable();
            adpt1.Fill(data1);
            dataGridView14.DataSource = data1;
            baglanti.Close();
            admin_mus_hesap_sil_btn.Visible = false;
        }

        private void Musteri_sil_btn_Click(object sender, EventArgs e)//müsteriyi tamemn silmek içim gerekli methot müsterini hesabının kalıp kalmadıgı kontrol etirdim
        {
            if (baglanti.State == ConnectionState.Closed)
                baglanti.Open();
            SqlCommand komut = new SqlCommand();
            komut.Connection = baglanti;
            int count = 0;
           
            try
            {
                komut.CommandText = "select  * from  Hesap_tablosu where [Musteri NO]=" + M_no + "";
                SqlDataReader reader = komut.ExecuteReader();
                while (reader.Read())
                {
                    count++;
                }
                reader.Close();
                if(count!=0)
                {
                    MessageBox.Show("Bütün Hesapları Silmelisiniz");
                }
                else
                {
                    komut.CommandText = "delete from Musteri_tablosu where [Musteri No]="+M_no+" ";
                    komut.ExecuteNonQuery();
                    komut.CommandText = "delete from Kullanici_tablosu where [Kullanici ID]="+ID+"";
                    komut.ExecuteNonQuery();
                    count = 0;
                    
                    komut.CommandText = "select * from Kullanici_tablosu";
                    SqlDataAdapter adpt1 = new SqlDataAdapter(komut);
                    DataTable data1 = new DataTable();
                    adpt1.Fill(data1);
                    dataGridView12.DataSource = data1;
                    mus_sec();
                }
              
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            baglanti.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            kullanici_ID = 0;
            mevcut_admin_ad = "";
            mevcut_admin_sifre = "";
            tabControl1.SelectedTab = ana_tab;
            tabControl1.TabPages.Add(ana_tab);
            tabControl1.TabPages.Remove(islemler_tab);
        }

        private void ana_sayfa_button2_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = islemler_tab;
            tabControl1.TabPages.Add(islemler_tab);
            tabControl1.TabPages.Remove(kayit_tabPage1);
            groupBox4.Visible = false;
            groupBox5.Visible = false;
            groupBox3.Visible = false;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = islemler_tab;
            tabControl1.TabPages.Add(islemler_tab);
            tabControl1.TabPages.Remove(para_cek_yatir_tab);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = islemler_tab;
            tabControl1.TabPages.Add(islemler_tab);
            tabControl1.TabPages.Remove(hesap_ac_kapa_tab);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = islemler_tab;
            tabControl1.TabPages.Add(islemler_tab);
            tabControl1.TabPages.Remove(guncelle_tabPage1);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = islemler_tab;
            tabControl1.TabPages.Add(islemler_tab);
            tabControl1.TabPages.Remove(havale_tabPage1);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = islemler_tab;
            tabControl1.TabPages.Add(islemler_tab);
            tabControl1.TabPages.Remove(Musteri_hesap_ozeti_tabPage1);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = islemler_tab;
            tabControl1.TabPages.Add(islemler_tab);
            tabControl1.TabPages.Remove(rapor_tabPage1);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = islemler_tab;
            tabControl1.TabPages.Add(islemler_tab);
            tabControl1.TabPages.Remove(Kullanici_silme_tabPage1);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = islemler_tab;
            tabControl1.TabPages.Add(islemler_tab);
            tabControl1.TabPages.Remove(islem_ekle_tab);
        }

        private void kayit_iptal_btn_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = islemler_tab;
            tabControl1.TabPages.Add(islemler_tab);
            tabControl1.TabPages.Remove(kayit_tabPage1);
            kayit_iptal_btn.Visible = false;
        }

        private void giris_kayit_iptal_btn_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = ana_tab;
            tabControl1.TabPages.Add(ana_tab);
            tabControl1.TabPages.Remove(kayit_tabPage1);
            giris_kayit_iptal_btn.Visible = false;
        }
        public void admin_kayit_kontrol()
        {
            int a = 0, b = 0;
            string ad = "", sifre = "";
            ad = admin_ad_kayittextBox6.Text;
            ad=kontrol(ad);
            if (ad.Length > 0)
                a = 1;
            sifre = admin_sifre_kayit_textBox2.Text;
            sifre = kontrol(sifre);
            if (sifre.Length > 0)
                b = 1;
            if (a == 1 && b == 1)
               admin_kayıt_kont = 1;

        }
        public void musteri_kayit_kontrol()
        {
            int a = 0, b = 0, c = 0, d = 0, e = 0, f = 0, g = 0;
            string kul_ad = "", kul_sif = "", ad = "", soyad = "", adres = "", tel = "", mail = "";    
                kul_ad = ad_kayit_textBox1.Text;           
                kul_ad = kontrol(kul_ad);
                if (kul_ad.Length > 0)
                    a = 1;
                kul_sif = sifre_kayit_textBox2.Text;
                kul_sif = kontrol(kul_sif);
                if (kul_sif.Length > 0)
                    b = 1;
                ad = ad_textBox2.Text;
                ad = kontrol(ad);
                if (ad.Length > 0)
                    c = 1;
                soyad = soyad_textBox4.Text;
                soyad = kontrol(soyad);
                if (soyad.Length > 0)
                    d = 1;
                adres = adres_textBox5.Text;
                adres = kontrol(adres);
                if (adres.Length > 0)
                    e = 1;
                tel = telefon_textBox6.Text;
                tel = kontrol(tel);
                if (tel.Length > 0)
                    f = 1;
                mail = e_mail_textBox7.Text;
                mail = kontrol(mail);
                if (mail.Length > 0)
                    g = 1;
        
            if (a == 1 && b == 1 && c == 1 && d == 1 && e == 1 && f == 1 && g == 1)
                mus_kayit_kont = 1;
        }
        public void admin_gunclee()
        {
            int a = 0, b = 0,c=0;
            string eski_sifre = "", yeni_sifre = "",dogrula="";
            eski_sifre = adm_eski_sifretextBox1.Text;
            eski_sifre = kontrol(eski_sifre);
            if (eski_sifre.Length > 0)
                a = 1;
            yeni_sifre = adm_yeni_sifre_textBox2.Text;
            yeni_sifre = kontrol(yeni_sifre);
            if (yeni_sifre.Length > 0)
                b = 1;
            dogrula = adm_sifre_dogrula_textBox4.Text;
            dogrula = kontrol(dogrula);
            if (dogrula.Length > 0)
                c = 1;
            if (a == 1 && b == 1&&c==1)
                admin_guncelle_kont = 1;
        }

        private void adm_guncelle_sifre_button1_Click(object sender, EventArgs e)
        {
            if (baglanti.State == ConnectionState.Closed)
                baglanti.Open();
            admin_gunclee();
            if (admin_guncelle_kont == 1)
            {
                SqlCommand komut = new SqlCommand();
                komut.Connection = baglanti;
                int eski_kontrol = 0, yeni_kontrol = 0;
                try
                {

                    komut.CommandText = "select Sifre from Admin_kullanici_adi where [Kullanici Adi]='" + mevcut_admin_ad + "' and Sifre='" + mevcut_admin_sifre + "'";
                    SqlDataReader reader = komut.ExecuteReader();
                    if (reader.Read())
                    {
                        if (adm_eski_sifretextBox1.Text != reader.GetString(0))
                        {
                            
                            MessageBox.Show("Geçersiz Şifre");
                        }
                        else
                        {
                           
                            eski_kontrol = 1;
                        }
                    }
                    reader.Close();


                    if (adm_yeni_sifre_textBox2.Text != adm_sifre_dogrula_textBox4.Text || adm_yeni_sifre_textBox2.Text == "")
                    {
                       
                        MessageBox.Show("Sifreler Uyumlu degil");
                    }
                    else
                    {
                        
                        yeni_kontrol = 1;
                    }
                    if (yeni_kontrol == 1 && eski_kontrol == 1)
                    {
                        komut.CommandText = "update Admin_kullanici_adi set Sifre='" + adm_yeni_sifre_textBox2.Text + "'where [Kullanici Adi]='" + mevcut_admin_ad + "'and Sifre='" + mevcut_admin_sifre + "' ";
                        komut.ExecuteNonQuery();
                        MessageBox.Show("Sifre Başarıyla Güncellendi");
                    }

                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }
                baglanti.Close();
            }
            else
                MessageBox.Show("BOŞ ALAN BIRAKMAYINIZ!");
        }

        public void  muster_sifre_guncelle()
        {
            int a = 0, b = 0, c = 0;
            string eski = "",yeni="",dogrula="";
           eski = eski_sifretextBox1.Text;
            eski = kontrol(eski);
            if (eski.Length > 0)
                a = 1;
            yeni = yeni_sifre_textBox2.Text;
            yeni = kontrol(yeni);
            if (yeni.Length > 0)
                b = 1;
            dogrula= sifre_dogrula_textBox4.Text;
            dogrula = kontrol(dogrula);
            if (dogrula.Length > 0)
                c = 1;
       
            if (a == 1 && b == 1 && c == 1)
                mus_sifre_gunclee_kont = 1;
        }
        public void mus_bilgi_guncelle()
        {
            int a = 0, b = 0, c = 0, d = 0, e = 0, f = 0, g = 0, h = 0;
            string   ad = "", soyad = "", adres = "", tel = "", mail = "";
                                       
            ad = ad_guncelle_textBox5.Text;
            ad = kontrol(ad);
            if (ad.Length > 0)
                a = 1;
            soyad = soyad_guncellle_textBox6.Text;
            soyad = kontrol(soyad);
            if (soyad.Length > 0)
                b = 1;
            adres = adres_guncelle_textBox7.Text;
            adres = kontrol(adres);
            if (adres.Length > 0)
                c= 1;
            tel = tele_guncelle_textBox8.Text;
            tel = kontrol(tel);
            if (tel.Length > 0)
              d = 1;
            mail = mail_guncelle_textBox9.Text;
            mail = kontrol(mail);
            if (mail.Length > 0)
               e = 1;

            if (a == 1 && b == 1 && c == 1 && d == 1 && e == 1)
                mus_bil_gunclelle = 1;
               
        }
        public string kontrol(string str)
        {
            string str1 = "";
            
            string[] ayır = str.Split(' ');
            for (int i = 0; i < ayır.Length; i++)
            {
                str1 += ayır[i];
               
            }
           
            return str1;
        }
    }
}
