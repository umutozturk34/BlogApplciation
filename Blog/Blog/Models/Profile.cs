using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Blog.Models
{
    public class Profile
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string ProfilePictureUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UserId { get; set; }
        public IdentityUser User { get; set; }
        public List<Post> Posts { get; set; }
    }

}
