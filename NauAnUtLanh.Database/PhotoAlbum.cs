using System;
using System.ComponentModel.DataAnnotations;

namespace NauAnUtLanh.Database
{
    public class PhotoAlbum
    {
        [Key]
        public Guid Id { get; set; }

        public string AlbumName { get; set; }
        public bool Activated { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}
