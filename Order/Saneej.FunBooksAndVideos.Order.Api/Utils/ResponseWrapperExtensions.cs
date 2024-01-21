using Microsoft.AspNetCore.Mvc;
using Saneej.FunBooksAndVideos.Service.Models;

namespace Saneej.FunBooksAndVideos.Order.Api.Utils
{
    public static class ResponseWrapperExtensions
    {
        public static ActionResult ToHttpResponse(this ResponseWrapper response)
        {
            if (response.IsNotFound)
            {
                return new NotFoundObjectResult(response);
            }

            if (response.IsClientError)
            {
                return new BadRequestObjectResult(response);
            }

            if (response.IsUnAuthorized)
            {
                return new UnauthorizedObjectResult(response.ErrorMessage);
            }

            return new OkResult();
        }

        public static ActionResult ToHttpResponse<T>(this ResponseWrapper<T> response)
        {
            if (response.IsNotFound)
            {
                return new NotFoundObjectResult(response);
            }

            if (response.IsClientError)
            {
                return new BadRequestObjectResult(response);
            }

            if (response.IsUnAuthorized)
            {
                return new UnauthorizedObjectResult(response.ErrorMessage);
            }

            return new OkObjectResult(response.Data);
        }
    }
}
