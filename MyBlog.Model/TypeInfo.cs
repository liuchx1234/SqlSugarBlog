namespace MyBlog.Model;

public class TypeInfo:Base
{
    [SugarColumn(ColumnDataType = "nvarchar(12)")]
    public string Name { get; set; }

}