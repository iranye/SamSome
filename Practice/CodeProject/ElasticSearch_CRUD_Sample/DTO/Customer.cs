using Nest;

namespace Elastic_CRUD.DTO
{
    [ElasticsearchType(Name = "Customer_Info", IdProperty = nameof(Id))]
    public class Customer
    {
        // Attribute "hints" are largely ignored (at least in v6.x) so mapping init should be done prior to inserts.
        [Text(Ignore = true)]
        public int Id { get; set; }

        //[ElasticProperty(Name = "name", Index = FieldIndexOption.NotAnalyzed)]
        public string Name { get; set; }

        //[ElasticProperty(Name = "age", NumericType = NumberType.Integer)]
        public int Age { get; set; }

        //[ElasticProperty(Name = "birthday", Type = FieldType.Date, DateFormat = "basic_date")]
        public string Birthday { get; set; }

        //[ElasticProperty(Name = "hasChildren")]
        public bool HasChildren { get; set; }

        //[ElasticProperty(Name = "enrollmentFee", NumericType = NumberType.Double)]
        public double EnrollmentFee { get; set; }

        //[ElasticProperty(Name = "opinion", Index = FieldIndexOption.NotAnalyzed)]
        public string Opinion { get; set; }
    }
}

