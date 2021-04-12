namespace MyTwse
{
    public class ApiResponseModel<T> : ApiResponseModel
    {
        public T Data { get; set; }
    }

    public class ApiResponseModel
    {
        public MyTwseExceptionEnum Code { get; set; }

        public string Message { get; set; }
    }

}
