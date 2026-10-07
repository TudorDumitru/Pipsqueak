using Pipsqueak.App.Models;

namespace Pipsqueak.App;

public interface IJsonHandler
{
    T Deserialize<T>(string message)
        where T : BinanceBaseData;
}