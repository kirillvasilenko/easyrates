using System.Threading.Tasks;

namespace ErrorMessages
{
    public interface IErrorMessages
    {
        Task<string> GetMessage(string code, string defaultMessage, params object[] args);
    }
}