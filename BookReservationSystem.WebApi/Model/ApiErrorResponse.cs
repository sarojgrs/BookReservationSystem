namespace BookReservationSystem.WebApi.Model
{
    public class ApiErrorResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string Detail { get; set; }

        public ApiErrorResponse(int statusCode, string message, string detail)
        {
            StatusCode = statusCode;
            Message = message;
            Detail = detail;
        }
    }
}
