using System;

namespace TodoService.Business.Repositories
{
    [Flags]
    public enum ResultStatus : short
    {
        OK,
        NotFound,
        BadRequest,
        NoContent
    }
}
