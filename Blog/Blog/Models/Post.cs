using System;
using System.Collections.Generic;

namespace Blog.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }

        public string ProfileId { get; set; }
        public Profile Profile { get; set; } 

        public List<Comment> Comments { get; set; } = new List<Comment>(); 
    }
}
