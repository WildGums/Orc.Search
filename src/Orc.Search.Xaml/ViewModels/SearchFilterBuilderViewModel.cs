namespace Orc.Search;

using System;
using Catel.MVVM;

public class SearchFilterBuilderViewModel : ViewModelBase
{
    public SearchFilterBuilderViewModel(IServiceProvider serviceProvider)
        : base(serviceProvider)
    {
    }

    public string? Filter { get; set; }
}
