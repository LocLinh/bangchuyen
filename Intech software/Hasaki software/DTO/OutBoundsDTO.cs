using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intech_software.DTO
{
    internal class OutBoundsDTO
    {
        private string ngay;
        private string thoiGian;
        private string maKienHang;
        private string tenTK;
        public OutBoundsDTO() { 
            this.ngay = string.Empty;
            this.thoiGian = string.Empty;
            this.maKienHang = string.Empty;
            this.tenTK = string.Empty;
        }

        public OutBoundsDTO(string ngay, string thoiGian, string maKienHang, string tenTK)
        {
            this.ngay = ngay;
            this.thoiGian = thoiGian;
            this.maKienHang = maKienHang;
            this.tenTK = tenTK;
        }

        public string GetNgay()
        {
            return this.ngay;
        }
        public void SetNgay(string ngay)
        {
            this.ngay = ngay;
        }

        public string GetThoiGian()
        {
            return this.thoiGian;
        }

        public void SetThoiGian(string thoiGian)
        {
            this.thoiGian = thoiGian;
        }

        public string GetMaKienHang()
        {
            return this.maKienHang;
        }
        public void SetMaKienHang(string maKienHang)
        {
            this.maKienHang= maKienHang;    
        }
        public string GetTenTK()
        {
            return this.tenTK;
        }
        public void SetTenTK(string tenTK)
        {
            this.tenTK = tenTK;
        }
    }
}
