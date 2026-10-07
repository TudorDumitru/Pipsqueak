
namespace Pipsqueak.App;

public interface IJsonHandler
{
    T Deserialize<T>(string message);
}