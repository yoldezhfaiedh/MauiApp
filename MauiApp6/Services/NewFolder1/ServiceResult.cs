namespace MauiApp6.Services.ServiceResultBase
{
    public class ServiceResult<T>
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public T? Data { get; set; }

        // Méthodes pratiques pour créer des résultats
        public static ServiceResult<T> Ok(T data)
        {
            return new ServiceResult<T> { Success = true, Data = data };
        }

        public static ServiceResult<T> Fail(string error)
        {
            return new ServiceResult<T> { Success = false, ErrorMessage = error };
        }
    }
}
