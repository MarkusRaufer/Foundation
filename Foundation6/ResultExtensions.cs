namespace Foundation
{
    public static class ResultExtensions
    {
        /// <summary>
        /// If IsOk is true <paramref name="ok"/> is called otherwise <paramref name="error"/> is called.
        /// </summary>
        /// <typeparam name="TOk"></typeparam>
        /// <typeparam name="TError"></typeparam>
        /// <param name="result"></param>
        /// <param name="ok"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static Unit Invoke<TOk, TError>(
            this Result<TOk, TError> result,
            Action<TOk> ok,
            Action<TError> error)
        {
            return result.Match(_ => result.OnOk(ok), _ => result.OnError(error));
        }
            

        /// <summary>
        /// If IsOk is true <paramref name="ok"/> is called otherwise <paramref name="error"/> is called.
        /// </summary>
        /// <typeparam name="TOk"></typeparam>
        /// <typeparam name="TError"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="result"></param>
        /// <param name="ok"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static TResult Match<TOk, TError, TResult>(
            this Result<TOk, TError> result,
            Func<TOk, TResult> ok,
            Func<TError, TResult> error)
            => result.IsOk ? ok(result.Ok) : error(result.Error);

        /// <summary>
        /// <paramref name="error"/> is only called when IsError of <paramref name="result"/> is true.
        /// </summary>
        /// <typeparam name="TOk"></typeparam>
        /// <typeparam name="TError"></typeparam>
        /// <param name="result"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static Unit OnError<TOk, TError>(this Result<TOk, TError> result, Action<TError> error)
        {
            if (result.IsError) error(result.Error);

            return new Unit();
        }

        /// <summary>
        /// <paramref name="ok"/> is only called when IsOk of <paramref name="result"/> is true.
        /// </summary>
        /// <typeparam name="TOk"></typeparam>
        /// <typeparam name="TError"></typeparam>
        /// <param name="result"></param>
        /// <param name="ok"></param>
        /// <returns></returns>
        public static Unit OnOk<TOk, TError>(this Result<TOk, TError> result, Action<TOk> ok)
        {
            if(result.IsOk) ok(result.Ok);

            return new Unit();
        }

        /// <summary>
        /// Converts a result to an optional.
        /// </summary>
        /// <typeparam name="TOk"></typeparam>
        /// <typeparam name="TError"></typeparam>
        /// <param name="result"></param>
        /// <returns></returns>
        public static Option<TOk> ToOption<TOk, TError>(this Result<TOk, TError> result) 
            => result.IsOk ? Option.Some(result.Ok) : Option.None<TOk>();
    }
}
