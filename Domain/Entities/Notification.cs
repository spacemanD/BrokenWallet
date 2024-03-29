﻿using Domain.Enums;

namespace Domain.Entities
{
    public class Notification
    {
        public int Id { get; set; }

        public Guid CoinId { get; set; }

        public Coin Coin { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public NotificationMode Mode { get; set; }
    }
}