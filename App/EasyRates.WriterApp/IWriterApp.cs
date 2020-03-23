using System;

namespace EasyRates.WriterApp
{
    public interface IWriterApp : IDisposable
    {
        void Start();

        void Stop();
    }
}