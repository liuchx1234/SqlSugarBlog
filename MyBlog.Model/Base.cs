
namespace MyBlog.Model
{
    public class Base
    {
        [SugarColumn(IsIdentity = true,IsPrimaryKey = true)]
        public   int BaseId { get; set; }
    }
}