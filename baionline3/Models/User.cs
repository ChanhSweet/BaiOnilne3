﻿namespace baionline3.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string? FullName { get; set; }
        public DateTime? DateOfBirth { get; set; }

        public int RoleId { get; set; }
        public Role? Role { get; set; }
    }
}
