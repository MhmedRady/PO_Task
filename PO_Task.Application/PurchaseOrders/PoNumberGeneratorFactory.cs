using Microsoft.Extensions.DependencyInjection;
using PO_Task.Domain.BuildingBlocks;
using PO_Task.Domain.PurchaseOrders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PO_Task.Application.PurchaseOrders
{
    public class PoNumberGeneratorFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public PoNumberGeneratorFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IPoNumberGenerator GetGenerator(PoNumberGeneratorType generatorType)
        {
            return generatorType switch
            {
                PoNumberGeneratorType.OldPoNumberGenerator => _serviceProvider.GetRequiredService<OldPoNumberGenerator>(),
                PoNumberGeneratorType.NewPoNumberGenerator => _serviceProvider.GetRequiredService<NewPoNumberGenerator>(),
                _ => throw new ArgumentException($"Unknown generator type: {generatorType}")
            };
        }
    }

}
