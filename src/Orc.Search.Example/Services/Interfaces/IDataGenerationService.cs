namespace Orc.Search.Example.Services;

using System.Collections.Generic;
using Models;

public interface IDataGenerationService
{
    IReadOnlyList<ISearchable> GenerateSearchables(int objectCount = ExampleDefaults.GeneratedObjectCount);
    ISearchable GenerateSearchable();
    ISearchable GenerateSearchable(Person person);
}
