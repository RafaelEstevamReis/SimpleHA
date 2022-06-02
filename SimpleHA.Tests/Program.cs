// Put yout token in a text file

using Simple.HAApi;

string haToken = File.ReadAllLines("token.txt").First();

var instance = new Instance(new Uri("http://127.0.0.1:8213"), haToken);


Console.WriteLine($"IsOnline: { await instance.CheckRunningAsync() }" );
