using System;

namespace TodoService.Business.Repositories
{
    [Flags]
    public enum ResultStatus
    {
        OK,
        NotFound,
        BadRequest,
        NoContent
    }
}
