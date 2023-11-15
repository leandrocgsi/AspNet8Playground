using System.Collections.Generic;
using APIAspNetCore5.Hypermedia.Abstract;

namespace APIAspNetCore5.Filters
{
    public class HyperMediaFilterOptions
    {
        public List<IResponseEnricher> ContentResponseEnricherList { get; set; } = new List<IResponseEnricher>();
    }
}