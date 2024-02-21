namespace Questao5.Domain.Enumerators
{
    public class Result<T>
    {
        public bool IsSuccess { get; private set; }
        public T Data { get; private set; }
        public string ErrorMessage { get; private set; }
        public TipoFalha? ErrorType { get; private set; }

        private Result(bool isSuccess, T data, string errorMessage, TipoFalha? errorType)
        {
            IsSuccess = isSuccess;
            Data = data;
            ErrorMessage = errorMessage;
            ErrorType = errorType;
        }

        public static Result<T> Success(T data)
        {
            return new Result<T>(true, data, null, null);
        }
      
        public static Result<T> Failure(string errorMessage, TipoFalha errorType) => new Result<T>(false, default(T), errorMessage, errorType);


    }
}
