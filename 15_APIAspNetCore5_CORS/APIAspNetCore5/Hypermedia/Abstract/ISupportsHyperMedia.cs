using System.Collections.Generic;

namespace APIAspNetCore5.Hypermedia.Abstract
{
    public interface ISupportsHyperMedia
    {
        List<HyperMediaLink> Links { get; set; }

    }
}