using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBlog.Common.Options;

public class AvatarOptions : BaseApplicationOptions
{
    public new static string Config => "Avatar";
    public Sizing Max { get; set; }
    public Sizing Min { get; set; }
    public struct Sizing
    {
        public double Height { get; set; }
        public double Width { get; set; }
    }
    [Required]
    public TimeSpan CacheRetention { get; set; }
}