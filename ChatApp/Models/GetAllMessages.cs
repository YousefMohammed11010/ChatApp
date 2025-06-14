﻿namespace ChatApp.Models
{
    public class GetAllMessages
    {
        public int Id { get; set; }
        public string SenderId { get; set; }
        public string RecipientId { get; set; }
        public string Text { get; set; }
        public DateTime SentAt { get; set; }
        public bool IsRead { get; set; }
        public string AttachmentUrl { get; set; }
    }
}
