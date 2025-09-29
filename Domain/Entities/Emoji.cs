using System.ComponentModel.DataAnnotations.Schema;

namespace ChatSystemBackend.Domain.Entities;

[Table("Emojis")]
public class Emoji : BaseEntity
{
    // Mã định danh emoji (ví dụ ":smile:")
    public string Code { get; set; } = null!;

    // URL ảnh hoặc đường dẫn sprite sheet
    public string Url { get; set; } = null!;

    // Phân loại (ví dụ "people", "animals", "food", "custom")
    public string Category { get; set; } = "default";
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}