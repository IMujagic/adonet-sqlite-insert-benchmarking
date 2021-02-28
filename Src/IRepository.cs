using System;
using System.Collections.Generic;
using System.Text;

namespace Sqlite.Benchmarking
{
    public interface IRepository
    {
        void Insert(List<SampleDataModel> data);
    }
}
