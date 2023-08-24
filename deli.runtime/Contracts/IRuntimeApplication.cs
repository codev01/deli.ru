using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace deli.runtime.Contracts
{
    public interface IRuntimeApplication
    {
        /// <summary>
        /// Допускаемое количество запросов в секунду
        /// </summary>
        int RateLimit { get; }
    }
}
