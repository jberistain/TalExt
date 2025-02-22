﻿namespace CommonTools.DTOs
{
    public class EmailSenderOptions
    {
        public string DisplayName { get; set; }
        public int Port { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool EnableSsl { get; set; }
        public string Host { get; set; }
        public bool UseStartTls { get; set; }
    }
}
