namespace HR_Managemnet.PL.ErrorsHandle
{
    public class ApiValidationErrorResponce : ApiResponce
    {
        public IEnumerable<string> Errors { get; set; }
        public ApiValidationErrorResponce() :base(400)
        { 
            Errors = new List<string>();
        }
    }
}
