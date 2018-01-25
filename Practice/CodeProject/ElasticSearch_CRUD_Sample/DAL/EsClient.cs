using System;
using Nest;

namespace Elastic_CRUD.DAL
{
    /// <summary>
    /// Elastic client
    /// </summary>
    public class EsClient
    {
        private const string ES_URI = "http://localhost:9200";
        
        private ConnectionSettings _settings;
        
        public ElasticClient Current { get; set; }
        
        public EsClient()
        {
            var node = new Uri(ES_URI);

            _settings = new ConnectionSettings(node);
            _settings.DefaultIndex(DTO.Constants.DEFAULT_INDEX);
            _settings.MapDefaultTypeNames(m => m.Add(typeof(DTO.Customer), DTO.Constants.DEFAULT_INDEX_TYPE));

            Current = new ElasticClient(_settings);
            Current.Map<DTO.Customer>(m => m.AutoMap());
        }
    }
}
