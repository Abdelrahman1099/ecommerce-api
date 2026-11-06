using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace E_Commerce.Application.Common
{
    public class Result
    {
        public Result(bool isSuccess, IReadOnlyList<Error> errors)
        {
            IsSuccess = isSuccess;
            Errors = errors;
        }

        public bool IsSuccess { get;}
        public IReadOnlyList<Error> Errors { get;}

        public static Result Ok() => new Result(true, Array.Empty<Error>());
        public static Result Fail(Error error) => new Result(false, new[] { error });
        public static Result Fail(IReadOnlyList<Error> errors) => new Result(false, errors);
    }

    public class Result<TValue> : Result
    {
        private readonly TValue _value;

        public TValue Data => IsSuccess ? _value : throw new InvalidOperationException("");

        public Result(TValue value) : base(true, Array.Empty<Error>())
        {
            _value = value;
        }

        private Result(Error error) : base(false, new[] { error })
        {
            _value = default;
        }
        private Result(IReadOnlyList<Error> errors) : base(false, errors)
        {
            _value = default;
        }
        public static Result<TValue> Ok(TValue value) => new (value);
        public static Result<TValue> Fail(Error error) => new (error);
        public static Result<TValue> Fail(IReadOnlyList<Error> errors) => new (errors);
    }


}
