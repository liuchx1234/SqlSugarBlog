namespace 开始学习SqlSugar.Helper;

public class ApiResult
{
    public int Code { get; set; }
    public string Msg { get; set; }
    public int Total { get; set; }
    public dynamic Data { get; set; }
}