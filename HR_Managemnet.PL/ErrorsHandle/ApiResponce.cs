namespace HR_Managemnet.PL.ErrorsHandle
{
    public class ApiResponce
    {
        public int StatusCode { get; set; }
        public string Massage { get; set; }
        public ApiResponce(int statusCode, string? massage = null)
        {
            StatusCode = statusCode;
            Massage = massage ?? GetDefaultMessageForStatusCode(statusCode);
        }

        private string? GetDefaultMessageForStatusCode(int? statusCode)
        {
            //500 => Internal Server Error
            //400 => Bad Request
            //401 => Unauthorized
            //404 => Not Found
            return statusCode switch
            {
                500 => "Internal Server Error",
                400 => "Bad Request",
                401 => "Unauthorized",
                404 => "Not Found",
                _ => null
            };
        }
    }
}
