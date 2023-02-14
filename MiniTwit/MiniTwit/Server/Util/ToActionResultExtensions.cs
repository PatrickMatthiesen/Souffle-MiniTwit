using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MiniTwit.Shared.Response;

namespace MiniTwit.Shared.Util;
public static class ToActionResultExtensions
{
    public static IActionResult ToActionResult(this Response response)
    {
        return response switch
        {
            Updated => new NoContentResult(),
            Deleted => new NoContentResult(),
            NotFound => new NotFoundResult(),
            Conflict => new ConflictResult(),
            //Success => new OkResult(),
            _ => throw new NotSupportedException($"{response} not supported")
        };
    }

    public static ActionResult<T> ToActionResult<T>(this Option<T> option) where T : class
    {
        return option.IsSome ? option.Value : new NotFoundResult();
    }

    public static async Task<ActionResult<T>> ToActionResult<T>(this Task<Option<T>> optionTask) where T : class
    {
        var option = await optionTask;
        return option.IsSome ? option.Value : new NotFoundResult();
    }
}
