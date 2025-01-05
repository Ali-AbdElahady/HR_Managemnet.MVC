namespace HR_Managemnet.PL.ErrorsHandle
{
    public class ApiExceptionResponse : ApiResponce
    {
        public string? Details { get; set; }
        public ApiExceptionResponse(int StatusCode, string? Message = null, string? Details = null) : base(StatusCode, Message)
        {
            this.Details = Details;
        }
    }
}
