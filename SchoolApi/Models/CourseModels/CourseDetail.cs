using System.ComponentModel.DataAnnotations.Schema;
using MessagePack;

namespace SchoolApi.Models.CourseModels;

public class CourseDetail
{
    [Key("Id"), DatabaseGenerated(DatabaseGeneratedOption.None), ForeignKey("Course")]
    public int Id { get; set; }

    public int Price { get; set; }

    public virtual Course Course { get; set; }
}