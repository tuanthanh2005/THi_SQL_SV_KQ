using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BT07_MainSub_SV_KQ
{
    public partial class Form1 : Form
    {
  
        DSSV ds = new DSSV();
        DSSVTableAdapters.KHOATableAdapter adpKhoa = new DSSVTableAdapters.KHOATableAdapter();
        DSSVTableAdapters.SINHVIENTableAdapter adpSinhVien = new DSSVTableAdapters.SINHVIENTableAdapter();
        DSSVTableAdapters.KETQUATableAdapter adpKetQua = new DSSVTableAdapters.KETQUATableAdapter();
        DSSVTableAdapters.MONHOCTableAdapter adpMonHoc = new DSSVTableAdapters.MONHOCTableAdapter();
        BindingSource bsSV = new BindingSource();
        BindingSource bsKQ = new BindingSource();
      

        public Form1()
        {
            InitializeComponent();
            bsSV.CurrentChanged += bsSV_currentchange;
        }

        private void bsSV_currentchange(object sender, EventArgs e)
        {
            bdnSinhVien.BindingSource = bsSV;
            lblSTT.Text = (bsSV.Position + 1) + "/" + bsSV.Count;
            txtTongDiem.Text = TongDiem(txtMasv.Text).ToString();
            btTruot.Enabled = bsSV.Position > 0;
            //btTruot.Enabled = btSau.Enabled;
            btSau.Enabled = bsSV.Position < bsSV.Count - 1;
            //btSau.Enabled = btSau.Enabled;
        }

        private object TongDiem(string MSV)
        {
            double kq = 0;
            Object td = ds.Tables["KETQUA"].Compute("sum(Diem)", "Masv='" + MSV + "'");
        
                if (td == DBNull.Value)
                kq = 0;
            else
                kq = Convert.ToDouble(td);
            return kq;

            
        }

        private void btSau_Click(object sender, EventArgs e)
        {
            bsSV.MoveNext();
        }

        private void btTruot_Click(object sender, EventArgs e)
        {

            bsSV.MovePrevious();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Doc_Du_Lieu();
            Lien_Ket_Dieu_Khien();
            txtTongDiem.Text = TongDiem(txtMasv.Text).ToString();


        }

        private void Lien_Ket_Dieu_Khien()
        {
            foreach (Control ctl in this.Controls)
                if (ctl is TextBox && ctl.Name != "txtTongDiem")
                    ctl.DataBindings.Add("text", bsSV, ctl.Name.Substring(3), true);
                else if (ctl is ComboBox)
                    ctl.DataBindings.Add("Selectedvalue", bsSV, ctl.Name.Substring(3), true);
                else if (ctl is DateTimePicker)
                    ctl.DataBindings.Add("value", bsSV, ctl.Name.Substring(3), true);
                else if (ctl is CheckBox)
                    ctl.DataBindings.Add("checked", bsSV, ctl.Name.Substring(3), true);





        }

        private void Doc_Du_Lieu()
        {
            adpKhoa.Fill(ds.KHOA);
            adpSinhVien.Fill(ds.SINHVIEN);
            adpMonHoc.Fill(ds.MONHOC);
            adpKetQua.Fill(ds.KETQUA);
            // 2 nạp nguồn
            cboMaKH.DisplayMember = "TenKH";
            cboMaKH.ValueMember = "MaKH";
            cboMaKH.DataSource = ds.KHOA;
            //2.1
            bsSV.DataSource = ds.SINHVIEN;
            // 3 nạp nguồn bindingsoure
            bsKQ.DataSource = bsSV;
            bsKQ.DataMember = "SINHVIENKETQUA";

            dgvKetQua.DataSource = bsKQ;

            // 6 
            dgvKetQua.Columns["MaSV"].Visible = false;


        }
    }
}
