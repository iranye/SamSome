using System;
using Nest;
using System.Collections.Generic;
using System.Linq;
using MicroMvvm;

namespace Elastic_CRUD.BLL
{
    /// <summary>
    /// BLL for Customer entity
    /// </summary>
    public class Customer : ObservableObject
    {
        private string mStatus = String.Empty;
        public string Status
        {
            get { return mStatus; }
            private set
            {
                if (mStatus != value)
                {
                    mStatus = value;
                    NotifyPropertyChanged("Status");
                }
            }
        }

        DAL.EsClient _EsClientDAL;

        public Customer()
        {
            _EsClientDAL = new DAL.EsClient();
        }

        /// <summary>
        /// Inserting or Updating a doc
        /// </summary>
        /// <param name="customer"></param>
        public void Index(DTO.Customer customer)
        {
            IIndexResponse response = _EsClientDAL.Current.Index(customer, c => c.Type(DTO.Constants.DEFAULT_INDEX_TYPE));

            if (response.ServerError != null)
            {
                throw new Exception(response.ServerError.Error.ToString());
            }

            const int updateSuccess = 200;
            const int createSuccess = 201;

            int httpResponse = response.ApiCall.HttpStatusCode.GetValueOrDefault();
            if (httpResponse != updateSuccess && httpResponse != createSuccess)
            {
                string message = "HttpStatusCode indicates failure";
                message += $"{Environment.NewLine}{response.ApiCall.ToString()}";
                Status = message;
            }

            if (!response.IsValid)
            {
                string message = "Response indicates InValid data";
                message += $"{Environment.NewLine}{response.ApiCall.ToString()}";
                Status = message;
            }
            else
            {
                InspectResponseCode(customer.Id.ToString(), response.Result);
            }
        }

        public void Delete(string id)
        {
            IDeleteResponse response = _EsClientDAL.Current.Delete
                (new Nest.DeleteRequest(DTO.Constants.DEFAULT_INDEX, DTO.Constants.DEFAULT_INDEX_TYPE, id));

            if (response.ServerError != null)
            {
                throw new Exception(response.ServerError.Error.ToString());
            }
            int deleteSuccess = 200;
            if (response.ApiCall.HttpStatusCode.GetValueOrDefault() != deleteSuccess)
            {
                string message = "HttpStatusCode indicates failure";
                message += $"{Environment.NewLine}{response.ApiCall.ToString()}";
                Status = message;
            }
            if (!response.IsValid)
            {
                string message = "Response indicates InValid data";
                message += $"{Environment.NewLine}{response.ApiCall.ToString()}";
                Status = message;
            }
            else
            {
                InspectResponseCode(id, response.Result);
            }
        }

        private void InspectResponseCode(string id, Result result)
        {
            switch (result)
            {
                case Result.Updated:
                    Status = $"Successfully updated doc w/ Id={id}";
                    break;
                case Result.Created:
                    Status = $"Successfully created doc w/ Id={id}";
                    break;
                case Result.Deleted:
                    Status = $"Successfully deleted doc w/ Id={id}";
                    break;
            }
        }

        public void BulkIndex()
        {
            var customers = new List<DTO.Customer>
            {
                new DTO.Customer { Name = "barb", Age = 22, Birthday = "20000101", EnrollmentFee = 233.66, HasChildren = true, Id = 16, Opinion = "Love it a lot" },
                new DTO.Customer { Name = "larb", Age = 28, Birthday = "19950512", EnrollmentFee = 233.66, HasChildren = true, Id = 17, Opinion = "Love it anyway" },
            };
            var bulkResponse = _EsClientDAL.Current.Bulk(b => b
                .IndexMany(customers, (d, doc) => d.Document(doc).Index(DTO.Constants.DEFAULT_INDEX).Type(DTO.Constants.DEFAULT_INDEX_TYPE))
            );
            if (bulkResponse.ServerError != null)
            {
                throw new Exception(bulkResponse.ServerError.Error.ToString());
            }

            const int updateSuccess = 200;
            const int createSuccess = 201;

            int httpResponse = bulkResponse.ApiCall.HttpStatusCode.GetValueOrDefault();
            if (httpResponse != updateSuccess && httpResponse != createSuccess)
            {
                string message = "HttpStatusCode indicates failure";
                message += $"{Environment.NewLine}{bulkResponse.ApiCall.ToString()}";
                Status = message;
            }

            if (!bulkResponse.IsValid)
            {
                string message = "Response indicates InValid data";
                message += $"{Environment.NewLine}{bulkResponse.ApiCall.ToString()}";
                Status = message;
            }
        }

        public List<DTO.Customer> QueryById(string id)
        {
            QueryContainer queryById = new TermQuery() { Field = "_id", Value = id.Trim() };
            var hits = _EsClientDAL.Current
                                   .Search<DTO.Customer>(s => s.Query(q => q.MatchAll() && queryById))
                                   .Hits;
            List<DTO.Customer> typedList = hits.Select(hit => ConvertHitToCustomer(hit)).ToList();
            return typedList; 
        }

        public List<DTO.Customer> QueryByAllFieldsUsingAnd(DTO.Customer customer)
        {
            var query = CreateSimpleQueryUsingAnd(customer);

            var hits = _EsClientDAL.Current
                                   .Search<DTO.Customer>(s => s.Query(q => query))
                                   .Hits;

            List<DTO.Customer> typedList = hits.Select(hit => ConvertHitToCustomer(hit)).ToList();

            return typedList;
        }

        public List<DTO.Customer> QueryByAllFieldsUsingOr(DTO.Customer costumer)
        {
            var query = CreateSimpleQueryUsingOr(costumer);

            var hits = _EsClientDAL.Current
                                   .Search<DTO.Customer>(s => s.Query(q => query))
                                   .Hits;

            List<DTO.Customer> typedList = hits.Select(hit => ConvertHitToCustomer(hit)).ToList();

            return typedList;
        }

        private DTO.Customer ConvertHitToCustomer(IHit<DTO.Customer> hit)
        {
            Func<IHit<DTO.Customer>, DTO.Customer> func = (x) =>
            {
                hit.Source.Id = Convert.ToInt32(hit.Id);
                return hit.Source;
            };

            return func.Invoke(hit);

            #region Notes
            /*
             Its a necessary workaround to get the "_id" property that remains in a upper level.
             Take this json return as sample:             
             
            {
            "_index": "crud_sample",
            "_type": "Customer_Info",
            "_id": "4",                   <- ID of the row
            "_score": 1,
            "_source": {                  <- All other properties are in the "_source" level: 
               "age": 32,
               "birthday": "19830101",
               "enrollmentFee": 100.1,
               "hasChildren": false,
               "name": "Juan",
               "opinion": "¿Qué tal estás?"
            }
             
             */
            #endregion
        }

        private QueryContainer CreateSimpleQueryUsingAnd(DTO.Customer customer)
        {
            QueryContainer queryContainer = null;

            queryContainer &= new TermQuery() { Field = "_id", Value = customer.Id };

            queryContainer &= new TermQuery() { Field = "name", Value = customer.Name };

            queryContainer &= new TermQuery() { Field = "age", Value = customer.Age };

            queryContainer &= new TermQuery() { Field = "birthday", Value = customer.Birthday };

            queryContainer &= new TermQuery() { Field = "hasChildren", Value = customer.HasChildren };

            queryContainer &= new TermQuery() { Field = "enrollmentFee", Value = customer.EnrollmentFee };

            return queryContainer;
        }

        private QueryContainer CreateSimpleQueryUsingOr(DTO.Customer customer)
        {
            QueryContainer queryContainer = null;

            queryContainer |= new TermQuery() { Field = "_id", Value = customer.Id };

            queryContainer |= new TermQuery() { Field = "name", Value = customer.Name };

            queryContainer |= new TermQuery() { Field = "age", Value = customer.Age };

            queryContainer |= new TermQuery() { Field = "birthday", Value = customer.Birthday };

            queryContainer |= new TermQuery() { Field = "hasChildren", Value = customer.HasChildren };

            queryContainer |= new TermQuery() { Field = "enrollmentFee", Value = customer.EnrollmentFee };

            return queryContainer;
        }
        
        public List<DTO.Customer> QueryUsingCombinations(DTO.CombinedFilter filter)
        {
            //Build Elastic "Should" filtering object for "Ages":          
            //FilterContainer[] agesFiltering = new FilterContainer[filter.Ages.Count];
            //for (int i = 0; i < filter.Ages.Count; i++)
            //{
            //    FilterDescriptor<DTO.Customer> clause = new FilterDescriptor<DTO.Customer>();
            //    agesFiltering[i] = clause.Term("age", int.Parse(filter.Ages[i]));                                
            //}

            ////Build Elastic "Must Not" filtering object for "Names":
            //FilterContainer[] nameFiltering = new FilterContainer[filter.Names.Count];
            //for (int i = 0; i < filter.Names.Count; i++)
            //{
            //    FilterDescriptor<DTO.Customer> clause = new FilterDescriptor<DTO.Customer>();
            //    nameFiltering[i] = clause.Term("name", filter.Names[i]);
            //}

            //Run the combined query:
            //var hits = _EsClientDAL.Current.Search<DTO.Customer>(s => s
            //                                                       .Query(q => q
            //                                                           .Filtered(fq => fq
            //                                                           .Query(qq => qq.MatchAll())
            //                                                           .Filter(ff => ff
            //                                                               .Bool(b => b
            //                                                                   .Must(m1 => m1.Term("hasChildren", filter.HasChildren))
            //                                                                   .MustNot(nameFiltering)
            //                                                                   .Should(agesFiltering)
            //                                                               )
            //                                                            )
            //                                                         )
            //                                                      )
            //                                                    ).Hits;

            var hits = _EsClientDAL.Current.Search<DTO.Customer>(s => s).Hits;
            //if (hits == null || hits.Count(n => n) == 0)

            //Translate the hits and return the list
            List<DTO.Customer> typedList = hits.Select(hit => ConvertHitToCustomer(hit)).ToList();
            return typedList;
        }

        public List<DTO.Customer> QueryUsingRanges(DTO.RangeFilter filter)
        {
            throw new NotImplementedException("QueryUsingRanges");
            //FilterContainer[] ranges = new FilterContainer[2];

            ////Build Elastic range filtering object for "Enrollment Fee": 
            //FilterDescriptor<DTO.Customer> clause1 = new FilterDescriptor<DTO.Customer>();
            //ranges[0] = clause1.Range(r => r.OnField(f => 
            //                                            f.EnrollmentFee).Greater(filter.EnrollmentFeeStart)
            //                                                            .Lower(filter.EnrollmentFeeEnd));

            ////Build Elastic range filtering object for "Birthday": 
            //FilterDescriptor<DTO.Customer> clause2 = new FilterDescriptor<DTO.Customer>();
            //ranges[1] = clause2.Range(r => r.OnField(f => f.Birthday)
            //                                .Greater(filter.Birthday.ToString(DTO.Constants.BASIC_DATE)));

            ////Run the combined query:
            //var hits = _EsClientDAL.Current
            //                        .Search<DTO.Customer>(s => s
            //                                               .Query(q => q
            //                                                   .Filtered(fq => fq
            //                                                   .Query(qq => qq.MatchAll())
            //                                                   .Filter(ff => ff
            //                                                       .Bool(b => b
            //                                                           .Must(ranges)
            //                                                       )
            //                                                    )
            //                                                 )
            //                                              )
            //                                            ).Hits;


            ////Translate the hits and return the list
            //List<DTO.Customer> typedList = hits.Select(hit => ConvertHitToCustumer(hit)).ToList();
            //return typedList;
        }

        public Dictionary<string, double> GetAggregations(DTO.Aggregations filter)
        {
            Dictionary<string, double> list = new Dictionary<string, double>();
            string agg_nickname = "customer_agg";

            switch (filter.AggregationType)
            {
                case "Count":
                            ExecuteCountAggregation(filter, list, agg_nickname);                        
                            break;
                case "Avg":
                            ExecuteAvgAggregation(filter, list, agg_nickname);                            
                            break;
                case "Sum":
                            ExecuteSumAggregation(filter, list, agg_nickname);
                            break;
                case "Min":
                        
                            ExecuteMinAggregation(filter, list, agg_nickname);
                            break;
                case "Max":                        
                            ExecuteMaxAggregation(filter, list, agg_nickname);
                            break;
                default:
                    break;
            }
            return list;
        }
        
        private void ExecuteSumAggregation(DTO.Aggregations filter, Dictionary<string, double> list, string agg_nickname)
        {
            throw new NotImplementedException("ExecuteSumAggregation");
            //var response = _EsClientDAL.Current.Search<DTO.Customer>(s => s
            //                                                         .Aggregations(a => a
            //                                                              .Sum(agg_nickname, st => st
            //                                                                  .Field(filter.Field)
            //                                                                    )
            //                                                                )
            //                                                          );

            //list.Add(filter.Field + " Sum", response.Aggs.Sum(agg_nickname).Value.Value);
        }
        
        private void ExecuteAvgAggregation(DTO.Aggregations filter, Dictionary<string, double> list, string agg_nickname)
        {
            var response = _EsClientDAL.Current.Search<DTO.Customer>(s => s
                                                                     .Aggregations(a => a
                                                                          .Average(agg_nickname, st => st
                                                                              .Field(filter.Field)
                                                                                )
                                                                            )
                                                                      );

            list.Add(filter.Field + " Average", response.Aggs.Average(agg_nickname).Value.Value);
        }
        
        private void ExecuteCountAggregation(DTO.Aggregations filter, Dictionary<string, double> list, string agg_nickname)
        {
            throw new NotImplementedException("ExecuteCountAggregation");
            //var response = _EsClientDAL.Current.Search<DTO.Customer>(s => s
            //                                                         .Aggregations(a => a
            //                                                              .Terms(agg_nickname, st => st
            //                                                                  .Field(filter.Field)
            //                                                                  .Size(int.MaxValue)
            //                                                                  .ExecutionHint(TermsAggregationExecutionHint.GlobalOrdinals)
            //                                                                    )
            //                                                                )
            //                                                          );

            //foreach (var item in response.Aggs.Terms(agg_nickname).Items)
            //{
            //    list.Add(item.Key, item.DocCount);
            //}
        }
        
        private void ExecuteMinAggregation(DTO.Aggregations filter, Dictionary<string, double> list, string agg_nickname)
        {
            var response = _EsClientDAL.Current.Search<DTO.Customer>(s => s
                                                                     .Aggregations(a => a
                                                                          .Min(agg_nickname, st => st
                                                                              .Field(filter.Field)
                                                                                )
                                                                            )
                                                                      );

            list.Add(filter.Field + " Min", response.Aggs.Sum(agg_nickname).Value.Value);
        }

        private void ExecuteMaxAggregation(DTO.Aggregations filter, Dictionary<string, double> list, string agg_nickname)
        {
            var response = _EsClientDAL.Current.Search<DTO.Customer>(s => s
                                                                     .Aggregations(a => a
                                                                          .Max(agg_nickname, st => st
                                                                              .Field(filter.Field)
                                                                                )
                                                                            )
                                                                      );

            list.Add(filter.Field + " Max", response.Aggs.Sum(agg_nickname).Value.Value);
        }
    }
}
