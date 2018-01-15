using Nest;
using System;

namespace Elastic_CRUD.DTO
{
    /// <summary>
    /// Customer entity
    /// </summary>
    [ElasticType(Name = "Customer_Info")]
    public class Customer
    {
        [ElasticProperty(Name="_id", NumericType = NumberType.Long)]
        public int Id { get; set; }

        [ElasticProperty(Name = "name", Index = FieldIndexOption.NotAnalyzed)]
        public string Name { get; set; }

        [ElasticProperty(Name = "age", NumericType = NumberType.Integer)]
        public int Age { get; set; }

        [ElasticProperty(Name = "birthday", Type = FieldType.Date, DateFormat = "basic_date")]
        public string Birthday { get; set; }

        [ElasticProperty(Name = "hasChildren")]
        public bool HasChildren { get; set; }

        [ElasticProperty(Name = "enrollmentFee", NumericType = NumberType.Double)]
        public double EnrollmentFee { get; set; }
        
        [ElasticProperty(Name = "opinion", Index = FieldIndexOption.NotAnalyzed)]
        public string Opinion { get; set; }
    }
}

