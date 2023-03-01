using Microsoft.AspNetCore.Mvc;
using MiniTwit.Shared;
using static MiniTwit.Shared.Response;

namespace MiniTwit.Server.Util;
public static class ToActionResultExtensions {
    public static IActionResult ToActionResult(this Response response) {
        return response switch {
            Updated => new NoContentResult(),
            Deleted => new NoContentResult(),
            NotFound => new NotFoundResult(),
            Conflict => new ConflictResult(),
            NoContent => new NoContentResult(),
            Created => new CreatedResult("", null),
            //Success => new OkResult(),
            _ => throw new NotSupportedException($"{response} not supported")
        };
    }

    public static ActionResult<T> ToActionResult<T>(this Option<T> option) where T : class {
        return option.IsSome ? option.Value : new NotFoundResult();
    }

    public static async Task<ActionResult<T>> ToActionResult<T>(this Task<Option<T>> optionTask) where T : class {
        var option = await optionTask;
        return option.IsSome ? option.Value : new NotFoundResult();
    }
}
