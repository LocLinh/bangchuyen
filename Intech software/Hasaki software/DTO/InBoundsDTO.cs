using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intech_software.DTO
{
    class InBoundsDTO
    {
        private string ngay;
        private string thoiGian;
        private string maKienHang;
        private string khoiLuong;
        private string chieuDai;
        private string chieuRong;
        private string chieuCao;
        private string maZone;
        private string trangThai;
        private string tenTK;

        public InBoundsDTO()
        {
            this.ngay = string.Empty;
            this.thoiGian = string.Empty;
            this.maKienHang = string.Empty; 
            this.khoiLuong = string.Empty;
            this.chieuDai = string.Empty;
            this.chieuRong = string.Empty;
            this.chieuCao = string.Empty;
            this.maZone = string.Empty;
            this.trangThai = string.Empty;
            this.tenTK = string.Empty;
        }

        public InBoundsDTO(string ngay, string thoiGian, string maKienHang, string khoiLuong, string chieuDai, string chieuRong, string chieuCao, string maZone, string trangThai, string tenTK)
        {
            this.ngay = ngay;
            this.thoiGian = thoiGian;
            this.maKienHang = maKienHang;
            this.khoiLuong = khoiLuong;
            this.chieuDai = chieuDai;
            this.chieuRong = chieuRong;
            this.chieuCao = chieuCao;
            this.maZone = maZone;
            this.trangThai = trangThai;
            this.tenTK = tenTK;
        }

        public string GetNgay() { return ngay; }
        public void SetNgay(string ngay)
        {
            this.ngay = ngay;
        }

        public string GetThoiGian() { return ngay; }
        public void SetThoiGian(string thoiGian)
        {
            this.thoiGian = thoiGian;
        }

        public string GetMaKienHang() { return maKienHang; }
        public void SetMaKienHang(string maKienHang)
        {
            this.maKienHang = maKienHang;
        }

        public string GetKhoiLuong() { return khoiLuong; }
        public void SetKhoiLuong(string khoiLuong)
        {
            this.khoiLuong = khoiLuong;
        }

        public string GetChieuDai() { return khoiLuong; }
        public void SetChieuDai(string chieuDai)
        {
            this.chieuDai = chieuDai;
        }

        public string GetChieuRong() { return chieuRong; }
        public void SetChieuRong(string chieuRong)
        {
            this.chieuRong = chieuRong;
        }

        public string GetChieuCao() { return chieuCao; }
        public void SetChieuCao(string chieuCao)
        {
            this.chieuCao = chieuCao;
        }

        public string GetMaZone() { return maZone; }
        public void SetMaZone(string maZone)
        {
            this.maZone = maZone;
        }

        public string GetTrangThai() { return trangThai; }
        public void SetTrangThai(string trangThai)
        {
            this.trangThai = trangThai;
        }

        public string GetTenTK() { return tenTK; }
        public void SetTenTK(string tenTK)
        {
            this.tenTK = tenTK;
        }
    }
}
