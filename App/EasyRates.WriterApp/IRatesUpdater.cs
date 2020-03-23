using System.Threading;
using System.Threading.Tasks;

namespace EasyRates.WriterApp
{
    public interface IRatesUpdater
    {
        Task UpdateRates(CancellationToken ct);
    }
}