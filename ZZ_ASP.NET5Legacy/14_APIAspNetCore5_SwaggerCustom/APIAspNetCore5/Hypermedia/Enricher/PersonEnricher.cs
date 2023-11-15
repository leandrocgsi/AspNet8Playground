using APIAspNetCore5.Hypermedia.Constants;
using APIAspNetCore5.Data.VO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text;

namespace APIAspNetCore5.Hypermedia.Enricher
{
    public class PersonEnricher : ContentResponseEnricher<PersonVO>
    {
        private readonly object _lock = new object();
        protected override Task EnrichModel(PersonVO content, IUrlHelper urlHelper)
        {

            var path = "api/persons/v1";
            string link = getLink(content, urlHelper, path);

            content.Links.Add(new HyperMediaLink()
            {
                Action = HttpActionVerb.GET,
                Href = link,
                Rel = RelationType.self,
                Type = ResponseTypeFormat.DefaultGet
            });
            content.Links.Add(new HyperMediaLink()
            {
                Action = HttpActionVerb.POST,
                Href = link,
                Rel = RelationType.self,
                Type = ResponseTypeFormat.DefaultPost
            });
            content.Links.Add(new HyperMediaLink()
            {
                Action = HttpActionVerb.PUT,
                Href = link,
                Rel = RelationType.self,
                Type = ResponseTypeFormat.DefaultPost
            });
            content.Links.Add(new HyperMediaLink()
            {
                Action = HttpActionVerb.PATCH,
                Href = link,
                Rel = RelationType.self,
                Type = ResponseTypeFormat.DefaultPost
            });
            content.Links.Add(new HyperMediaLink()
            {
                Action = HttpActionVerb.DELETE,
                Href = link,
                Rel = RelationType.self,
                Type = "int"
            });
            return null;
        }

        private string getLink(PersonVO content, IUrlHelper urlHelper, string path)
        {
            lock (_lock)
            {
                var url = new { controller = path, id = content.Id };
                return new StringBuilder(urlHelper.Link("DefaultApi", url)).Replace("%2F", "/").ToString();
            }
        }
    }
}