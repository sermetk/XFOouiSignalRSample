using System.Threading.Tasks;

namespace OouiSignalRSample.Core.Hubs
{
    public interface IChatHub
    {        
        Task ReceiveMessage(string message);
    }
}
