using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using GuardX.Enums;

namespace GuardX.Interfaces
{
    public interface IEmailService
    {
        public EResult SendOtpEmail(String name, String email, int otpValue, EEmailPurpose emailPurpose);
    }
}