using Simple.API;
using Simple.HAApi.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using System.Web;

namespace Simple.HAApi.Sources
{
    public class History : SourceBase
    {
        public History(ClientInfo info)
           : base(info)
        { }

        /// <summary>
        /// Returns an array of state changes in the past. Each object contains further details for the entities.
        /// </summary>
        /// <param name="startDate">It determines the beginning of the period.</param>
        /// <param name="endDate">It determines the end of the period</param>
        /// <param name="minimalResponse">Only return last_changed and state for states other than the first and last state (much faster)</param>
        /// <param name="includeAttributes">Determines whenever return attributes from the database (much faster)</param>
        /// <param name="significantChangesOnly">Returns significant state changes</param>
        /// <param name="filterIds">Filter on one or more entities, use NULL to return all entities</param>
        public async Task<HistoryModel> GetPeriodAsync(DateTime startDate, DateTime endDate,
                                         bool minimalResponse = true, bool includeAttributes = false,
                                         bool significantChangesOnly = true,
                                         string[] filterIds = null)
        {
            if (filterIds != null && filterIds.Length == 0)
            {
                throw new ArgumentException("filterIds should be null or greater than zero");
            }

            string timestamp = startDate.ToString("yyyy-MM-ddTHH:mm:sszzz", CultureInfo.InvariantCulture);
            string strEndDate = HttpUtility.UrlEncode(endDate.ToString("yyyy-MM-ddTHH:mm:sszzz", CultureInfo.InvariantCulture));

            var parameters = new List<string>()
            {
                $"end_time={strEndDate}",
            };
            if (filterIds != null)
            {
                parameters.Add($"filter_entity_id={string.Join(",", filterIds)}");
            }
            if (significantChangesOnly) parameters.Add("minimal_response");
            if (minimalResponse) parameters.Add("significant_changes_only");
            if (!includeAttributes) parameters.Add("no_attributes");

            string url = $"/api/history/period/{timestamp}?{string.Join("&", parameters)}";
            var result = await GetAsync<EntityStateChangeModel[][]>(url);

            return new HistoryModel()
            {
                Items = result
            };
        }

    }
}
