﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ombi.Core.Engine;
using Ombi.Core.Engine.Interfaces;
using Ombi.Core.Models.Requests;
using Ombi.Core.Models.Search;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ombi.Store.Entities.Requests;
using System.Diagnostics;

namespace Ombi.Controllers
{
    [Authorize]
    public class RequestController : BaseV1ApiController
    {
        public RequestController(IMovieRequestEngine engine, ITvRequestEngine tvRequestEngine)
        {
            MovieRequestEngine = engine;
            TvRequestEngine = tvRequestEngine;
        }

        private IMovieRequestEngine MovieRequestEngine { get; }
        private ITvRequestEngine TvRequestEngine { get; }

        /// <summary>
        /// Gets movie requests.
        /// </summary>
        /// <param name="count">The count of items you want to return.</param>
        /// <param name="position">The position.</param>
        [HttpGet("movie/{count:int}/{position:int}")]
        public async Task<IEnumerable<MovieRequests>> GetRequests(int count, int position)
        {
            return await MovieRequestEngine.GetRequests(count, position);
        }

        /// <summary>
        /// Gets all movie requests.
        /// </summary>
        [HttpGet("movie")]
        public async Task<IEnumerable<MovieRequests>> GetRequests()
        {
            return await MovieRequestEngine.GetRequests();
        }

        /// <summary>
        /// Requests a movie.
        /// </summary>
        /// <param name="movie">The movie.</param>
        /// <returns></returns>
        [HttpPost("movie")]
        public async Task<RequestEngineResult> RequestMovie([FromBody] SearchMovieViewModel movie)
        {
            return await MovieRequestEngine.RequestMovie(movie);
        }

        /// <summary>
        /// Searches for a specific movie request
        /// </summary>
        /// <param name="searchTerm">The search term.</param>
        /// <returns></returns>
        [HttpGet("movie/search/{searchTerm}")]
        public async Task<IEnumerable<MovieRequests>> Search(string searchTerm)
        {
            return await MovieRequestEngine.SearchMovieRequest(searchTerm);
        }

        /// <summary>
        /// Deletes the specified movie request.
        /// </summary>
        /// <param name="requestId">The request identifier.</param>
        /// <returns></returns>
        [HttpDelete("movie/{requestId:int}")]
        public async Task DeleteRequest(int requestId)
        {
            await MovieRequestEngine.RemoveMovieRequest(requestId);
        }

        /// <summary>
        /// Updates the specified movie request.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPut("movie")]
        public async Task<MovieRequests> UpdateRequest([FromBody] MovieRequests model)
        {
            return await MovieRequestEngine.UpdateMovieRequest(model);
        }

        /// <summary>
        /// Gets the tv requests.
        /// </summary>
        /// <param name="count">The count of items you want to return.</param>
        /// <param name="position">The position.</param>
        /// <returns></returns>
        [HttpGet("tv/{count:int}/{position:int}")]
        public async Task<IEnumerable<TvRequests>> GetTvRequests(int count, int position)
        {
            try
            {

                return await TvRequestEngine.GetRequests(count, position);
            }
            catch (System.Exception e)
            {
                Debug.WriteLine(e.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets the tv requests.
        /// </summary>
        /// <returns></returns>
        [HttpGet("tv")]
        public async Task<IEnumerable<TvRequests>> GetTvRequests()
        {
            return await TvRequestEngine.GetRequests();
        }

        /// <summary>
        /// Requests a tv show/episode/season.
        /// </summary>
        /// <param name="tv">The tv.</param>
        /// <returns></returns>
        [HttpPost("tv")]
        public async Task<RequestEngineResult> RequestTv([FromBody] SearchTvShowViewModel tv)
        {
            return await TvRequestEngine.RequestTvShow(tv);
        }

        /// <summary>
        /// Searches for a specific tv request
        /// </summary>
        /// <param name="searchTerm">The search term.</param>
        /// <returns></returns>
        [HttpGet("tv/search/{searchTerm}")]
        public async Task<IEnumerable<TvRequests>> SearchTv(string searchTerm)
        {
            return await TvRequestEngine.SearchTvRequest(searchTerm);
        }

        /// <summary>
        /// Deletes the a specific tv request
        /// </summary>
        /// <param name="requestId">The request identifier.</param>
        /// <returns></returns>
        [HttpDelete("tv/{requestId:int}")]
        public async Task DeleteTvRequest(int requestId)
        {
            await TvRequestEngine.RemoveTvRequest(requestId);
        }

        /// <summary>
        /// Updates the a specific tv request
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPut("tv")]
        public async Task<TvRequests> UpdateRequest([FromBody] TvRequests model)
        {
            return await TvRequestEngine.UpdateTvRequest(model);
        }

        /// <summary>
        /// Gets the count of total requests
        /// </summary>
        /// <returns></returns>
        [HttpGet("count")]
        [AllowAnonymous]
        public RequestCountModel GetCountOfRequests()
        {
            // Doesn't matter if we use the TvEngine or MovieEngine, this method is in the base class
            return TvRequestEngine.RequestCount();
        }

        ///// <summary>
        ///// Gets the specific grid model for the requests (for modelling the UI).
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet("tv/grid")]
        //[ApiExplorerSettings(IgnoreApi = true)]
        //public async Task<RequestGridModel<TvRequests>> GetTvRequestsGrid()
        //{
        //    return await GetGrid(TvRequestEngine);
        //}

        ///// <summary>
        ///// Gets the specific grid model for the requests (for modelling the UI).
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet("movie/grid")]
        //[ApiExplorerSettings(IgnoreApi = true)]
        //public async Task<RequestGridModel<MovieRequests>> GetMovieRequestsGrid()
        //{
        //    return await GetGrid(MovieRequestEngine);
        //}

        //private async Task<RequestGridModel<T>> GetGrid<T>(IRequestEngine<T> engine) where T : BaseRequestModel
        //{
        //    var allRequests = await engine.GetRequests();
        //    var r = allRequests.ToList();
        //    var model = new RequestGridModel<T>
        //    {
        //        Available = r.Where(x => x.Available && !x.Approved),
        //        Approved = r.Where(x => x.Approved && !x.Available),
        //        New = r.Where(x => !x.Available && !x.Approved)
        //    };
        //    return model;
        //}
    }
}