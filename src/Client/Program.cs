using Grpc.Net.Client;

namespace Client;

internal sealed class Program
{
    static async Task Main(string[] args)
    {
        using var channel = GrpcChannel.ForAddress("https://localhost:7017");

        var client = new Greeter.GreeterClient(channel);
        var reply = await client.SayHelloAsync(new HelloRequest { Name = "GreeterClient" });

        Console.WriteLine("Greeting: " + reply.Message);

        Console.WriteLine();


        var streamingClient = new StockTicker.StockTickerClient(channel);
        var serverStream = streamingClient.GetStockStream(new());

        while (await serverStream.ResponseStream.MoveNext(CancellationToken.None))
        {
            Console.WriteLine($"Stock price: {serverStream.ResponseStream.Current.Value:F} at {serverStream.ResponseStream.Current.Timestamp.ToDateTime()}");
        }

        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}