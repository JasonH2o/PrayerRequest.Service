using System;
using System.Linq;
using System.Web.Http;
using PrayerRequest.Service.DataContext;
using PrayerRequest.Service.Models;
using System.Web.Http.Cors;
using PrayerRequest.Service.GenericClass;
using PrayerRequest.Service.Extention;


namespace PrayerRequest.Service.Controllers
{
    // TODO: modify the origin when deploy to server
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class PrayerRequestController : ApiController
    {
        private DatabaseContext dbContext = new DatabaseContext();
        private const int _defaultPageSize = 10;
        private const int _defaultPageNo = 1;

        /// <summary>
        /// End point to get all the prayer requests
        /// http://localhost:54011/api/PrayerRequest
        /// </summary>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        //[EnableQuery(PageSize = 10)]
        //[ResponseType(typeof(IQueryable<PrayerRequestDetail>))]
        [Route("api/PrayerRequest/{pageNo}/{isCurrent}")]
        public IHttpActionResult GetPrayer(int pageNo = 0, bool isCurrent = true)
        {
            try
            {                
                // Determine the number of records to skip
                int skip = (pageNo - 1) * _defaultPageSize;

                var result = dbContext.PrayerRequests.Where(x => x.IsCurrent == isCurrent).OrderByDescending(y => y.Id)
                                    .Skip(skip)
                                    .Take(_defaultPageSize)
                                    .ToList();

                // Group by open prayer first then order by Id
                //var result = dbContext.PrayerRequests.ToList().GroupBy(b => b.IsCurrent).Select(grouping => grouping.OrderByDescending(b => b.Id))
                //            .OrderByDescending(grouping => grouping.First().IsCurrent)
                //            .SelectMany(grouping => grouping)
                //            .Skip(skip)
                //            .Take(pageSize)
                //            .ToList();
                var total = dbContext.PrayerRequests.Count(x => x.IsCurrent == isCurrent);

                if (!result.Any())
                {
                    return NotFound();
                }
                return Ok(new PagedResult<PrayerRequestDetail>(result, pageNo, _defaultPageSize, total));
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        /// <summary>
        /// End point to create a prayer request
        /// http://localhost:54011/api/PrayerRequest 
        /// </summary>
        /// <param name="request">info for the new object</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult PostPrayer(PrayerRequestDetail request)
        {
            try
            {                                
                var isCurrent = request.IsCurrent;
                dbContext.PrayerRequests.Add(request);
                dbContext.SaveChanges();

                var result = dbContext.PrayerRequests.Where(x => x.IsCurrent == isCurrent).OrderByDescending(y => y.Id)                                    
                                    .Take(_defaultPageSize)
                                    .ToList();
                //var result = dbContext.PrayerRequests.Select(x => x).OrderByDescending(y => y.Id);
                //var result = dbContext.PrayerRequests.ToList().GroupBy(b => b.IsCurrent).Select(grouping => grouping.OrderByDescending(b => b.Id))
                //            .OrderByDescending(grouping => grouping.First().IsCurrent)
                //            .SelectMany(grouping => grouping)
                //            .Skip(skip)
                //            .Take(pageSize)
                //            .ToList();
                var total = dbContext.PrayerRequests.Where(x => x.IsCurrent == isCurrent).Count();                

                return Ok(new PagedResult<PrayerRequestDetail>(result, _defaultPageNo, _defaultPageSize, total));
            }
            catch (Exception e)
            {                
                return InternalServerError();
            }
        }

        /// <summary>
        /// Delete prayer
        /// http://localhost:54011/api/PrayerRequests?id=20
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IHttpActionResult DeletePrayer(int id)
        {
            try
            {                                
                var requestToBeDeleted = dbContext.PrayerRequests.FirstOrDefault(x => x.Id == id);
                var isCurrent = requestToBeDeleted.IsCurrent;

                if (requestToBeDeleted != null)
                {
                    dbContext.Entry(requestToBeDeleted).State = System.Data.Entity.EntityState.Deleted;
                    dbContext.SaveChanges();

                    var result = dbContext.PrayerRequests.Where(x => x.IsCurrent == isCurrent).OrderByDescending(y => y.Id)
                                    .Take(_defaultPageSize)
                                    .ToList();
                    
                    var total = dbContext.PrayerRequests.Where(x => x.IsCurrent == isCurrent).Count();

                    return Ok(new PagedResult<PrayerRequestDetail>(result, _defaultPageNo, _defaultPageSize, total));
                }

                return NotFound();
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        /// <summary>
        /// End point to update the object
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updatedRequest"></param>
        /// <returns></returns>
        public IHttpActionResult PutPrayer(int id, PrayerRequestDetail updatedRequest)
        {
            // TODO: need to refactor this once adding the edit feature
            try
            {                                
                PrayerRequestDetail existingRequest;
                bool isCurrent;
                // the negative id object is the object that contains the updated info  
                // this is to handle check/uncheck pray button
                if (updatedRequest.IsModified())
                {
                    existingRequest = dbContext.PrayerRequests.FirstOrDefault(x => x.Id == id);
                    isCurrent = existingRequest.IsCurrent;
                    existingRequest.IsCurrent = !existingRequest.IsCurrent;
                    dbContext.SaveChanges();

                    var result = dbContext.PrayerRequests.Where(x => x.IsCurrent == isCurrent).OrderByDescending(y => y.Id)
                                    .Take(_defaultPageSize)
                                    .ToList();                    
                    var total = dbContext.PrayerRequests.Count();
                    return Ok(new PagedResult<PrayerRequestDetail>(result, _defaultPageNo, _defaultPageSize, total));

                }

                // the following is for edit the prayer request
                // Not support the edit feature yet
                existingRequest = dbContext.PrayerRequests.FirstOrDefault(x => x.Id == updatedRequest.Id);
                isCurrent = existingRequest.IsCurrent;
                if (existingRequest != null)
                {
                    existingRequest.Name = updatedRequest.Name;
                    existingRequest.Request = updatedRequest.Request;
                    existingRequest.IsCurrent = updatedRequest.IsCurrent;
                    dbContext.SaveChanges();

                    var result = dbContext.PrayerRequests.Where(x => x.IsCurrent == isCurrent).OrderByDescending(y => y.Id)
                                    .Take(_defaultPageSize)
                                    .ToList();

                    var total = dbContext.PrayerRequests.Count();
                    return Ok(new PagedResult<PrayerRequestDetail>(result, _defaultPageNo, _defaultPageSize, total));
                }

                return NotFound();
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }



        //----------------------------------------------the codes below may be the refactored end point. save it for later-----------------------
        ///// <summary>
        ///// End point to update the object
        ///// </summary>        
        ///// <param name="updatedRequest"></param>
        ///// <returns></returns>
        //public IHttpActionResult PutPrayer(PrayerRequestDetail updatedRequest)
        //{
        //    try
        //    {
        //        var isCurrent = updatedRequest.IsCurrent;

        //        var existingRequest = dbContext.PrayerRequests.FirstOrDefault(x => x.Id == updatedRequest.Id);

        //        if(existingRequest == null)
        //        {
        //            return NotFound();
        //        }

        //        existingRequest.Id = updatedRequest.Id;
        //        existingRequest.Name = updatedRequest.Name;
        //        existingRequest.Request = updatedRequest.Request;
        //        existingRequest.IsCurrent = updatedRequest.IsCurrent;
        //        existingRequest.Date = updatedRequest.Date;
        //        dbContext.SaveChanges();

        //        var result = dbContext.PrayerRequests.Where(x => x.IsCurrent == isCurrent).OrderByDescending(y => y.Id)
        //                            .Take(_defaultPageSize)
        //                            .ToList();

        //        var total = dbContext.PrayerRequests.Where(x => x.IsCurrent == isCurrent).Count();

        //        return Ok(new PagedResult<PrayerRequestDetail>(result, _defaultPageNo, _defaultPageSize, total));
        //    }
        //    catch (Exception)
        //    {
        //        return InternalServerError();
        //    }
        //}
    }
}
