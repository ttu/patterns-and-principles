using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace PatternsAndPinciples.Principles.SOLID
{
    public class CollectionUse
    {
        private List<int> _items = new List<int>();

        public void SearchItems(IEnumerable<int> collection)
        {
            var current = collection.Where(i => i == 2);
        }

        public void ModifyCollection(ICollection<int> collection)
        {
            collection.Add(2);
        }

        public void NeedToUseIndex(IList<int> collection)
        {
            collection[0] = 2;
        }
    }

    public class DataItem
    {
        public string Text { get; set; }
    }

    public interface IDataUpdateService
    {
        void AddData(DataItem newItem);
    }

    public interface IDataFetchService
    {
        DataItem GetData(Func<DataItem, bool> predicate);
    }

    public interface IDataService : IDataFetchService, IDataUpdateService
    { }

    public class DataService : IDataService
    {
        private readonly List<DataItem> _list = new List<DataItem>();

        public void AddData(DataItem newItem)
        {
            _list.Add(newItem);
        }

        public DataItem GetData(Func<DataItem, bool> predicate)
        {
            return _list.Single(predicate);
        }
    }

    public class InterfaceSegregationTest
    {
        [Fact]
        public void Test()
        {
            var service = new DataService();

            void AddExample(IDataUpdateService updater)
            {
                updater.AddData(new DataItem { Text = "Hello" });
            }

            void ReadExample(IDataFetchService fetcher)
            {
                var item = fetcher.GetData(i => i.Text == "Hello");
            }

            AddExample(service);
            ReadExample(service);
        }
    }
}