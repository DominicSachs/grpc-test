using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcService.Protos;

namespace GrpcService.Services;

public sealed class StockService : StockTicker.StockTickerBase
{
    private readonly ILogger<GreeterService> _logger;

    public StockService(ILogger<GreeterService> logger)
    {
        _logger = logger;
    }

    public override async Task GetStockStream(StockRequest request, IServerStreamWriter<StockResponse> responseStream, ServerCallContext context)
    {
        var rand = new Random();

        while (true)
        {
            var value = (float) rand.NextDouble();
            value = value * 100.0f / 2;

            if (context.CancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation("GetStockStream was cancelled manually.");

                break;
            }

            await responseStream.WriteAsync(new StockResponse { Value = value, Timestamp = Timestamp.FromDateTime(DateTime.UtcNow) });
            await Task.Delay(1000);
        }
    }
}