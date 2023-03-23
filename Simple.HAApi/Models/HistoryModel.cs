using System;
using System.Collections.Generic;
using System.Globalization;

namespace Simple.HAApi.Models
{
    public class HistoryModel
    {
        public EntityStateChangeModel[][] Items { get; set; }

        public IEnumerable<EntityNumericalAverageModel> BuildAverage()
        {
            foreach (var entity in Items)
            {
                yield return EntityNumericalAverageModel.Build(entity);
            }
        }

    }
    public class EntityStateChangeModel
    {
        public string entity_id { get; set; }
        public string state { get; set; }
        public Dictionary<string, string> attributes { get; set; }
        public DateTime last_changed { get; set; }
        public DateTime last_updated { get; set; }

        public override string ToString()
        {
            return $"{entity_id} {state} on {last_changed}";
        }
    }
    public class EntityNumericalAverageModel
    {
        public string EntityId { get; set; }
        public decimal Average { get; set; }
        public decimal Min { get; set; }
        public decimal Max { get; set; }
        public int Entries { get; set; }

        public override string ToString()
        {
            return $"[{Entries}] {EntityId} {Average} [{Min}/{Max}]";
        }

        internal static EntityNumericalAverageModel Build(EntityStateChangeModel[] entity)
        {
            var e = new EntityNumericalAverageModel()
            {
                EntityId = null,
            };

            decimal sum = 0;
            foreach(var entry in entity)
            {
                if(entry.entity_id != null && e.EntityId == null ) e.EntityId = entry.entity_id;

                decimal value;
                if (!decimal.TryParse(entry.state, NumberStyles.Number, CultureInfo.InvariantCulture, out value)) continue;

                if(e.Entries == 0)
                {
                    e.Min = e.Max = value;
                }

                e.Entries++;
                sum += value;

                if (value > e.Max) e.Max = value;
                if (value < e.Min) e.Min = value;
            }

            if (e.Entries > 0) e.Average = Math.Round(sum / e.Entries, 6);

            return e;
        }
    }
}
