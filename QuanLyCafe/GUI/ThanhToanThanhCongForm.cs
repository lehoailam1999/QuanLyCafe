using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ReaLTaiizor.Forms;
using System.Runtime.InteropServices;
using System.Data.SqlClient;
using ReaLTaiizor.Controls;
using ReaLTaiizor.Util;
using ReaLTaiizor.Manager;
using ReaLTaiizor.Enum.Material;
using ReaLTaiizor.Colors;
using System.IO;
using QuanLyCafe.BLL;
using QuanLyCafe.DTO;
namespace QuanLyCafe.GUI
{
    public partial class ThanhToanThanhCongForm : MaterialForm
    {
        HoaDonBLL hoaDonBLL = new HoaDonBLL();
        VoucherBLL voucherBLL = new VoucherBLL();
        public ThanhToanThanhCongForm()
        {
            InitializeComponent();
        }

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect,
            int nTopRect,
            int nRightRect,
            int nBottomRect,
            int nWidthEllipse,
            int nHeightEllipse
        );

        private void ThanhToanThanhCongForm_Load(object sender, EventArgs e)
        {
            CreateBorderRadius();
            HienThiThongTinHoaDonThanhCong();
        }

        #region Các hàm khởi tạo
        void CreateBorderRadius()
        {
            pnlThanhToanThanhCong.Anchor = AnchorStyles.None;
            pnlThanhToanThanhCong.Region = Region.FromHrgn(
                CreateRoundRectRgn(
                    0,
                    0,
                    pnlThanhToanThanhCong.Width,
                    pnlThanhToanThanhCong.Height,
                    15,
                    15
                )
            );
        }

        void HienThiThongTinHoaDonThanhCong()
        {
          
            if (ControlForm.BanDangChon != null && ControlForm.BanDatDangChon != null)
            {
                HoaDon getHoaDon = hoaDonBLL.LayThongTinHoaDonByIDDatBan(
                     ControlForm.BanDatDangChon.ID
                 );

                lblTongTien.Text = getHoaDon.ThanhTien.ToString();
                lblTongTien.Text = string.Format("{0:#,##0} VNĐ", double.Parse(lblTongTien.Text));
                if (!string.IsNullOrEmpty(getHoaDon.VoucherHoaDon))
                {
                    Voucher getVoucher = voucherBLL.LayThongTinVoucher(getHoaDon.VoucherHoaDon);
                    lblGiamGia.Text =
                        $"{getVoucher.GiamGia}% ({getHoaDon.VoucherHoaDon})";
                }
                else
                {
                    lblGiamGia.Text = $"0%";
                }
                lblThanhTien.Text =
                    getHoaDon.ThanhTienGiamGia.ToString();
                lblThanhTien.Text = string.Format("{0:#,##0} VNĐ", double.Parse(lblThanhTien.Text));
                lblKhachTra.Text = getHoaDon.TienKhachTra.ToString();
                lblKhachTra.Text = string.Format("{0:#,##0} VNĐ", double.Parse(lblKhachTra.Text));

                lblTienThua.Text = getHoaDon.TienThua.ToString();
                lblTienThua.Text = string.Format("{0:#,##0} VNĐ", double.Parse(lblTienThua.Text));
            }
            ControlForm.BanDatDangChon = null;
            ControlForm.FormChiTietBan.HienThiThongTinBan();
        }

        #endregion

        #region Các hàm sự kiện
        private void ThanhToanThanhCongForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!ControlForm.ConfirmForm("Hãy xuất hóa đơn trước khi thoát nhé!"))
            {

                e.Cancel = true;
            }
        }

        private void btnXuatHoaDon_Click(object sender, EventArgs e)
        {
            if (ControlForm.BanDangChon != null)
            {
                GUI.XuatHoaDonForm fXuatHoaDon = new GUI.XuatHoaDonForm();
                fXuatHoaDon.ShowDialog();
            }
        }
        #endregion
    }
}
