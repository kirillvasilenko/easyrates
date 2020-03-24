using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EasyRates.WriterApp.AspNet
{
	public class WriterHostedService : IHostedService, IDisposable
	{
		private readonly IWriterApp app;
		private readonly ILogger logger;

		public WriterHostedService(
			IWriterApp app,
			ILogger<WriterHostedService> logger)
		{
			this.app = app;
			this.logger = logger;
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			logger.LogInformation("WriterHostedService is starting...");

			app.Start();

			return Task.CompletedTask;
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			logger.LogInformation("WriterHostedService is stopping...");

			app.Stop();

			return Task.CompletedTask;
		}

		public void Dispose()
		{
			// just in case, StopAsync may be not invoked.
			app.Dispose();
		}
	}
}