using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intech_software.DTO
{
    internal class AccountsDTO
    {
        private string maTK;
        private string tenTK;
        private string matKhau;

        public string GetMaTK()
        {
            return maTK;
        }
        public void SetMaTK(string maTK)
        {
            this.maTK = maTK;
        }

        public string GetTenTK()
        {
            return tenTK;
        }
        public void SetTenTK(string tenTK)
        {
            this.tenTK = tenTK;
        }
        public string GetMatKhau()
        {
            return matKhau;
        }

        public void SetMatKhau(string matKhau)
        {
            this.matKhau = matKhau;
        }

        public AccountsDTO()
        {
            this.maTK = string.Empty;
            this.tenTK = string.Empty;
            this.tenTK = string.Empty;
        }

        public AccountsDTO(string maTK, string tenTK, string matKhau)
        {
            this.maTK = maTK;
            this.tenTK = tenTK;
            this.matKhau = matKhau;
        }

    }
}
