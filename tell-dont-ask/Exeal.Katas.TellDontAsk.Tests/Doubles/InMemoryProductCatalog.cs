using System.Collections.Generic;
using System.Linq;
using Exeal.Katas.TellDontAsk.Domain;
using Exeal.Katas.TellDontAsk.Repository;

namespace Exeal.Katas.TellDontAsk.Tests.Doubles
{
    public class InMemoryProductCatalog : ProductCatalog
    {
        private readonly List<Product> products;

        public InMemoryProductCatalog(List<Product> products)
        {
            this.products = products;
        }

        public Product GetByName(string name)
        {
            return products.FirstOrDefault(p => p.Name.Equals(name));
        }
    }
}