﻿using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel
{
    public class Message
    {
        public List<MailboxAddress> To { get; set; } 
        public string? Subject { get; set; } 
        public string? Body { get; set; } 

        //constructor
        public Message(IEnumerable<string>to,string subject,string body)
        {
            To = new List<MailboxAddress>();
            To.AddRange(to.Select(x => new MailboxAddress("email", x)));
            Subject = subject;
            Body = body;
        }
    }
}
