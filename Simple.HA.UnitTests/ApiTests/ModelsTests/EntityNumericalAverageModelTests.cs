using Simple.HAApi.Models;
using System;
using Xunit;

namespace Simple.HA.UnitTests.ApiTests.ModelsTests
{
    public class EntityNumericalAverageModelTests
    {
        [Fact]
        public void BuildWeightedAverage_Nulls()
        {
            Assert.Null(EntityNumericalAverageModel.BuildWeightedAverage(null));
            Assert.Null(EntityNumericalAverageModel.BuildWeightedAverage(new EntityStateChangeModel[0]));
        }

        [Fact]
        public void BuildWeightedAverage_SingleValue()
        {
            EntityStateChangeModel[] values = new EntityStateChangeModel[]
            {
                new EntityStateChangeModel { state = "10", last_updated = DateTime.UtcNow }
            };

            EntityNumericalAverageModel result = EntityNumericalAverageModel.BuildWeightedAverage(values);

            Assert.Equal(values[0].state, result.Average.ToString());
            Assert.Equal(values[0].state, result.Min.ToString());
            Assert.Equal(values[0].state, result.Max.ToString());
            Assert.Equal(1, result.Entries);
        }

        [Fact]
        public void BuildWeightedAverage_MultipleValues()
        {
            EntityStateChangeModel[] values = new EntityStateChangeModel[]
            {
                new EntityStateChangeModel { state = "10", last_updated = DateTime.UtcNow },
                new EntityStateChangeModel { state = "20", last_updated = DateTime.UtcNow.AddSeconds(100) },
                new EntityStateChangeModel { state = "30", last_updated = DateTime.UtcNow.AddSeconds(120) }
            };

            EntityNumericalAverageModel result = EntityNumericalAverageModel.BuildWeightedAverage(values);

            Assert.Equal(16.67M, result.Average, 2);
            Assert.Equal(10, result.Min);
            Assert.Equal(30, result.Max);
            Assert.Equal(3, result.Entries);
        }

    }
}
