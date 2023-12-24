using Exeal.Katas.TellDontAsk.Domain;

namespace Exeal.Katas.TellDontAsk.Repository
{
    public interface ProductCatalog
    {
        Product GetByName(string name);
    }
}
