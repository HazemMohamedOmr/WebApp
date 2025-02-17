using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Infrastructure.Helpers
{
    public class MailSettings
    {
        public string SmtpServer { get; set; }
        public int Port { get; set; }
        public string SenderEmail { get; set; }
        public string DisplayName { get; set; }
        public string SenderPassword { get; set; }
        public bool EnableSSL { get; set; }
    }
}