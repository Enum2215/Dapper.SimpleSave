using System.Collections.Generic;
using System.Text;

namespace Dapper.SimpleSave
{
    public interface IScript {
        StringBuilder Buffer { get; }
        IDictionary<string, object> Parameters { get; }
        object Config { get; }
        object InsertedValue { get; set; }
        DtoMetadata InsertedValueMetadata { get; set; }
    }
}