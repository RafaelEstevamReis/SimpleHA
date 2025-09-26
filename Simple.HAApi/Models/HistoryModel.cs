namespace Simple.HAApi.Models;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

public class HistoryModel
{
    public EntityStateChangeModel[][] Items { get; set; }

    public IEnumerable<EntityNumericalAverageModel> BuildAverage()
        => Items.Select(EntityNumericalAverageModel.BuildWeightedAverage);

}
public class EntityStateChangeModel
{
    public string entity_id { get; set; }
    public string state
    {
        get => _state; set
        {
            _state = value;
            dval = null;
        }
    }
    public Dictionary<string, string> attributes { get; set; }
    public DateTime last_changed { get; set; }
    public DateTime last_updated { get; set; }

    decimal? dval;
    private string _state;

    public bool GetDecimalState(out decimal dState)
    {
        if (dval != null)
        {
            dState = dval.Value;
            return true;
        }

        var result = decimal.TryParse(state, NumberStyles.Number, CultureInfo.InvariantCulture, out dState);
        dval = dState;
        return result;
    }

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

    public static EntityNumericalAverageModel BuildAverage(EntityStateChangeModel[] entity)
    {
        var e = new EntityNumericalAverageModel()
        {
            EntityId = null,
        };

        decimal sum = 0;
        foreach (var entry in entity)
        {
            if (entry.entity_id != null && e.EntityId == null) e.EntityId = entry.entity_id;

            decimal value;
            if (!decimal.TryParse(entry.state, NumberStyles.Number, CultureInfo.InvariantCulture, out value)) continue;

            if (e.Entries == 0)
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

    public static EntityNumericalAverageModel BuildWeightedAverage(EntityStateChangeModel[] values)
    {
        if (values == null || values.Length == 0) return null;

        EntityNumericalAverageModel result = null;
        EntityStateChangeModel last = null;

        double totalSeconds = 0;
        decimal totalValues = 0;

        foreach (var curr in values.OrderBy(o => o.last_updated))
        {
            if (!curr.GetDecimalState(out decimal val)) continue;

            if (result == null) // first
            {
                result = new EntityNumericalAverageModel()
                {
                    EntityId = curr.entity_id,
                    Average = val,
                    Min = val,
                    Max = val,
                    Entries = 1,
                };

                last = curr;
                continue;
            }

            if (result.EntityId == null && curr.entity_id != null) result.EntityId = curr.entity_id;

            if (result.Max < val) result.Max = val;
            if (result.Min > val) result.Min = val;

            last.GetDecimalState(out decimal lastVal);

            var timeDiff = curr.last_updated - last.last_updated;

            totalSeconds += timeDiff.TotalSeconds;
            var weightedValue = (lastVal + val) / 2;
            totalValues += weightedValue * (decimal)timeDiff.TotalSeconds;

            result.Entries++;
            last = curr;
        }

        if (totalSeconds > 0) result.Average = totalValues / (decimal)totalSeconds;

        return result;
    }

}
