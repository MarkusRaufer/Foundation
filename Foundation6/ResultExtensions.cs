namespace Foundation
{
    public static class ResultExtensions
    {
        /// <summary>
        /// Converts a result to an optional.
        /// </summary>
        /// <typeparam name="TOk"></typeparam>
        /// <typeparam name="TError"></typeparam>
        /// <param name="result"></param>
        /// <returns></returns>
        public static Opt<TOk> ToOpt<TOk, TError>(this Result<TOk, TError> result) 
            => result.IsOk ? Opt.Some(result.Ok) : Opt.None<TOk>();
    }
}
