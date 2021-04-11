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
        [HttpStatusCode(HttpStatusCode.BadRequest)]
        [Description("錯誤請求")]
        BadRequest = 400000,
        [HttpStatusCode(HttpStatusCode.BadRequest)]
        [Description("輸入的日期不能為假日")]
        BadRequestIsHoliday = 400001,
        [HttpStatusCode(HttpStatusCode.BadRequest)]
        [Description("輸入值只能是數字")]
        BadRequestIsNotNumber = 400002,
    }
}
