using System;
using System.ComponentModel.DataAnnotations;

namespace NauAnUtLanh.Database
{
    public class Article
    {
        [Key]
        public Guid Id { get; set; }

        public DateTime CreatedTime { get; set; }
        public bool Activated { get; set; }
        public bool Hot { get; set; }
        public string ArticleTitle { get; set; }
        public string ShortDescription { get; set; }
        public string ArticleContent { get; set; }
        public string ArticleAvatar { get; set; }
        public string Keywords { get; set; }
    }
}
