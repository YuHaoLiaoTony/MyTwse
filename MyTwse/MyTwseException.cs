namespace MyTwse
{
    public class MyTwseException : System.Exception
    {
        public MyTwseExceptionEnum ErrorCode { get; set; }
        public object ExceptionDetail { get; set; }


        public override string Message
        {
            get
            {
                return "Exception：" + this.ErrorCode.ToString();
            }
        }

        public MyTwseException()
        {

        }

        public MyTwseException(MyTwseExceptionEnum errorCode)
        {
            this.ErrorCode = errorCode;
        }


        public MyTwseException(MyTwseExceptionEnum errorCode, string detail)
        {
            this.ErrorCode = errorCode;
            this.ExceptionDetail = detail;
        }

    }
}
