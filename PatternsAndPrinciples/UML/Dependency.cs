using System.Collections.Generic;

namespace PatternsAndPinciples.UML
{
    public interface ISearchService
    {
        ICollection<dynamic> Get(string searchText);
    }

    public class SearchController
    {
        private readonly ISearchService _service;

        public SearchController(ISearchService service) => _service = service;

        public ICollection<dynamic> Get(string searchText) => _service.Get(searchText);
    }
}