using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Sqlite.Benchmarking
{
    public class SampleDataModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public bool IsActive { get; set; }
        public string LongText { get; set; }
        public int RandomNumber { get; set; }

        internal static List<SampleDataModel> Generate(int numberOfSampleItems)
        {
            var rand = new Random();
            var list = new List<SampleDataModel>();

            for (var i = 0; i < numberOfSampleItems; i++)
            {
                list.Add(new SampleDataModel
                {
                    Id = i + 1,
                    IsActive = i % 2 == 0,
                    LongText = new string(Enumerable.Range(1, 1000).Select(x => (char)rand.Next('a', 'z')).ToArray()),
                    RandomNumber = rand.Next(),
                    Date = DateTime.Now
                });
            }

            return list;
        }
    }
}
