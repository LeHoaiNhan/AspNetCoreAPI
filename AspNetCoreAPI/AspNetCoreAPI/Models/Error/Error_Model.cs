namespace AspNetCoreAPI.Models.Error
{
    public class Error_Model
    {
        public int status { get; set; }
        public string message { get; set; }
        public object data { get; set; }
    }
}
