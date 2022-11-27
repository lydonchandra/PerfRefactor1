

using System.Threading.Tasks;

namespace  PerfRefactor1
{
    using System.Net;
    using System.Net.Sockets;
    using BenchmarkDotNet.Attributes;
    using BenchmarkDotNet.Running;
    public class Program
    {
        static void Main(string[] args)
        {
            BenchmarkSwitcher.FromAssemblies(new[] { typeof(Program).Assembly }).Run(args);
        }

        [MemoryDiagnoser]
        public class MemoryBenchmark
        {
            private NetworkStream _client, _server;
            private byte[] _buffer = new byte[10];

            [GlobalSetup]
            public void Setup()
            {
                using Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                var client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                listener.Bind(new IPEndPoint(IPAddress.Loopback, 0));
                listener.Listen();
                client.Connect(listener.LocalEndPoint);
                _client = new NetworkStream(client);
                _server = new NetworkStream(listener.Accept());
            }

            [Benchmark(Baseline = true)]
            public async Task ReadWrite1()
            {
                byte[] buffer = _buffer;
                for (int i = 0; i < 1000; i++)
                {
                    await _client.WriteAsync(buffer, 0, buffer.Length);
                    await _server.ReadAsync(buffer, 0, buffer.Length); // may not read everything; just for demo purposes
                }
            }

            [Benchmark]
            public async Task ReadWrite2()
            {
                byte[] buffer = _buffer;
                for (int i = 0; i < 1000; i++)
                {
                    await _client.WriteAsync(buffer);
                    await _server.ReadAsync(buffer); // may not read everything; just for demo purposes
                }
            }
        }    
    }

    
}
