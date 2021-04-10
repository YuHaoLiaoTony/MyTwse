using System.ComponentModel;
using System.Net;

namespace MyTwse
{
    public enum MyTwseExceptionEnum
    {
        [HttpStatusCode(HttpStatusCode.NotFound)]
        [Description("查無資料")]
        NotFount = 404,
        [HttpStatusCode(HttpStatusCode.InternalServerError)]
        [Description("系統錯誤")]
        InternalServerError = 500,
    }
}
