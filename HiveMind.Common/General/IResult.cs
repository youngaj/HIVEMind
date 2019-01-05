namespace HiveMind.Common.General
{
    public interface IResult
    {
        ResultType Type { get; set; }
        string Message { get; set; }
        bool IsSuccessful { get; }
        object Obj { get; set; }

        void AddMessage(string msg);

        void ClearMessages();
    }
}