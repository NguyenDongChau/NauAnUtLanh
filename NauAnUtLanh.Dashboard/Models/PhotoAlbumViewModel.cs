using System.ComponentModel.DataAnnotations;

namespace NauAnUtLanh.Dashboard.Models
{
    public class PhotoAlbumViewModel
    {
        [Required(ErrorMessage = "Chưa cung cấp tên album ảnh")]
        public string AlbumName { get; set; }

        public bool Activated { get; set; }
    }
}