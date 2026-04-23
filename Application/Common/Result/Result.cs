namespace Application.Common.Result
{
    public class Result<T>
    {
        public bool IsSuccess { get; }
        public T Value { get; }
        public Error Error { get; }

        private Result(T value)
        {
            IsSuccess = true;
            Value = value;
            Error = Error.None;
        }

        private Result(Error error)
        {
            IsSuccess = false;
            Error = error ?? throw new ArgumentNullException(nameof(error));
        }

        public static Result<T> Success(T value) 
            => new(value);
        public static Result<T> Failure(Error error) 
            => new(error);

        // implict casting sugar
        public static implicit operator Result<T>(T value)
            => Success(value);

        public static implicit operator Result<T>(Error error)
            => Failure(error); 

        // Match pattern (functional magic)
        public TResult Match<TResult>(
            Func<T, TResult> onSuccess,
            Func<Error, TResult> onFailure)
        {
            return IsSuccess ? onSuccess(Value) : onFailure(Error);
        }
    }

        // Non-generic version
        public sealed class Result
        {
            public bool IsSuccess { get; }
            public bool IsFailure => !IsSuccess;
            public Error Error { get; }

            private Result(bool isSuccess, Error error)
            {
                IsSuccess = isSuccess;
                Error = error;
            }
            public static Result Success() => new(true, Error.None);
            public static Result Failure(Error error) => new(false, error);

            public static implicit operator Result(Error error)
                => Failure(error);

        }
}
