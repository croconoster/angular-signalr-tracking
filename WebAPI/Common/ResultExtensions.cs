using Microsoft.AspNetCore.Mvc;
using MyTransfer.Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Common
{
    public static class ResultExtensions
    {
        public static async Task<ActionResult<TData>> ToActionResultAsync<TData>(this Task<TData> resultTask)
        {
            var result = await resultTask;

            if (result == null)
            {
                return new NotFoundResult();
            }

            return result;
        }

        public static async Task<ActionResult> ToActionResultAsync(this Task<Result> resultTask)
        {
            var result = await resultTask;

            if (!result.Succeeded)
            {
                return new BadRequestObjectResult(result.Errors);
            }

            return new OkResult();
        }

        public static async Task<ActionResult<TData>> ToActionResultAsync<TData>(this Task<Result<TData>> resultTask)
        {
            var result = await resultTask;

            if (!result.Succeeded)
            {
                return new BadRequestObjectResult(result.Errors);
            }

            return result.Data;
        }
    }
}
