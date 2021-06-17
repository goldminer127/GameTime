using System;
using System.Collections.Generic;
using System.Text;
using DSharpPlus.Entities;

namespace GameTime.Models
{
    public class Mail
    {
        public string Title { get; private set; }
        public string Content { get; private set; }
        public string Sign { get; private set; }
        public int Id { set; get; }
        public bool MailRead { get; set; }
        public Mail (string title, string content)
        {
            Title = title;
            Content = content;
            Sign = $"-From the GameTime team\n{DateTime.Now.Month}/{DateTime.Now.Day}";
            MailRead = false;
        }
    }
}
