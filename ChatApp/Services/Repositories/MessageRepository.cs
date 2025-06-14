using ChatApp.Data;
using ChatApp.Data.Entities;
using ChatApp.Models;
using ChatApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Services.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileRepository _fileRepository;

        public MessageRepository(ApplicationDbContext context, IFileRepository fileRepository)
        {
            _context = context;
            _fileRepository = fileRepository;
        }

        public async Task<IEnumerable<GetAllMessages>> GetChatHistoryAsync(string senderId, string recipientId)
        {
            var data = await _context.Messages
                .Where(m => (m.SenderId == senderId && m.RecipientId == recipientId) ||
                            (m.SenderId == recipientId && m.RecipientId == senderId))
                .OrderBy(m => m.SentAt)
                .ToListAsync();

            var result = data.Select(m => new GetAllMessages
            {
                Id = m.Id,
                SenderId = m.SenderId,
                RecipientId = m.RecipientId,
                Text = m.Text,
                SentAt = m.SentAt,
                IsRead = m.IsRead,
                AttachmentUrl = m.AttachmentUrl
            });

            return result;
        }


        public async Task MarkMessageAsReadAsync(int messageId)
        {
            var message = await _context.Messages.FindAsync(messageId);
            if (message != null)
            {
                message.IsRead = true;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<CreateMessage> SendMessageAsync(CreateMessage message, string userId)
        {
            string? uploadedPath = null;

            if (message.AttachmentUrl != null)
            {
                uploadedPath = await _fileRepository.UploadFile("Images", message.AttachmentUrl);
            }

            // ✅ نتاكد ان الـ Id موجود فعلاً
            var sender = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (sender == null)
                throw new Exception("Sender user not found.");

            Message data = new()
            {
                IsRead = false,
                RecipientId = message.RecipientId,
                SenderId = sender.Id,
                SentAt = message.SentAt == default ? DateTime.UtcNow : message.SentAt,
                Text = message.Text,
                AttachmentUrl = uploadedPath
            };

            await _context.Messages.AddAsync(data);
            await _context.SaveChangesAsync();

            return message;
        }



    }
}
