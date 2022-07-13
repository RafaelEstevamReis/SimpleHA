﻿using System.Collections.Generic;
using System.Linq;

namespace Simple.HAApi
{
    public static class EntitiesExtensions
    {
        public static IEnumerable<Models.StateModel> OfDomain(this IEnumerable<Models.StateModel> source, string domain)
            => source.Where(o => o.Domain == domain);

        public static IEnumerable<string> GetIds(this IEnumerable<Models.StateModel> source)
            => source.Select(o => o.EntityId);

        public static IEnumerable<(string Key, Models.StateModel[])> GroupByDomain(this IEnumerable<Models.StateModel> source)
            => source.GroupBy(o => o.Domain).Select(g => (g.Key, g.ToArray()));

    }
}
