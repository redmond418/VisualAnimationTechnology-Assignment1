namespace VAT
{
    public interface IProcessElement
    {
        public void ResetTime(float currentTime);
        public void ProcessUV(ProcessContext context);
    }
}
