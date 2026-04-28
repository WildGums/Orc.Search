namespace Orc.Search.Example.ViewModels;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Catel.MVVM;
using Catel.Services;
using Services;

public class MainViewModel : ViewModelBase
{
    private readonly IDataGenerationService _dataGenerationService;
    private readonly ISearchService _searchService;

    private readonly Stopwatch _searchStopwatch = new Stopwatch();
    private readonly IUIVisualizerService _uiVisualizerService;

    public MainViewModel(IDataGenerationService dataGenerationService, ISearchService searchService,
        IUIVisualizerService uiVisualizerService, IServiceProvider serviceProvider)
        : base(serviceProvider)
    {
        _dataGenerationService = dataGenerationService;
        _searchService = searchService;
        _uiVisualizerService = uiVisualizerService;

        AllObjects = new List<ISearchable>();
        FilteredObjects = new List<ISearchable>();

        AddPerson = new TaskCommand(serviceProvider, OnAddPersonAsync);
        RemovePerson = new Command(serviceProvider, OnRemovePerson);
    }

    public TaskCommand AddPerson { get; }

    public Command RemovePerson { get; }

    public override string Title => "Orc.Search example";

    public object SelectedObject { get; set; }

    public int IndexedObjectCount { get; private set; }

    public bool IsUpdatingSearch { get; private set; }

    public bool IsSearching { get; private set; }

    public string Filter { get; set; }

    public List<ISearchable> AllObjects { get; private set; }

    public List<ISearchable> FilteredObjects { get; private set; }

    public int FilteredObjectCount { get; private set; }

    public TimeSpan LastSearchDuration { get; private set; }

    protected override async Task InitializeAsync()
    {
        await base.InitializeAsync();

        _searchService.Updating += OnSearchServiceUpdating;
        _searchService.Updated += OnSearchServiceUpdated;

        _searchService.Searching += OnSearchServiceSearching;
        _searchService.Searched += OnSearchServiceSearched;

        var generatedSearchables = (await Task.Run(() => _dataGenerationService.GenerateSearchables())).ToList();

        AllObjects = generatedSearchables.ToList();
        await Task.Run(() => _searchService.AddObjects(generatedSearchables));
    }

    private async Task OnAddPersonAsync()
    {
        var result = await _uiVisualizerService.ShowDialogAsync<AddPersonViewModel>();
        if (result.DialogResult ?? false)
        {
            var person = result.GetViewModel<AddPersonViewModel>().Person;
            var searchable = _dataGenerationService.GenerateSearchable(person);

            _searchService.AddObjects(new[] { searchable });
            AllObjects.Add(searchable);

            _searchService.Search(Filter);
        }
    }

    private void OnRemovePerson()
    {
        if (SelectedObject is not ISearchable selectedSearchable)
        {
            return;
        }

        AllObjects.Remove(selectedSearchable);
    }

    protected override async Task CloseAsync()
    {
        _searchService.Updating -= OnSearchServiceUpdating;
        _searchService.Updated -= OnSearchServiceUpdated;

        _searchService.Searching -= OnSearchServiceSearching;
        _searchService.Searched -= OnSearchServiceSearched;

        await base.CloseAsync();
    }

    private void OnSearchServiceUpdating(object? sender, EventArgs e)
    {
        IsUpdatingSearch = true;
    }

    private void OnSearchServiceUpdated(object? sender, EventArgs e)
    {
        IsUpdatingSearch = false;

        IndexedObjectCount = _searchService.IndexedObjectCount;
    }

    private void OnSearchServiceSearching(object? sender, EventArgs e)
    {
        IsSearching = true;

        _searchStopwatch.Restart();
    }

    private void OnSearchServiceSearched(object? sender, SearchEventArgs e)
    {
        _searchStopwatch.Stop();

        LastSearchDuration = _searchStopwatch.Elapsed;
        FilteredObjects = e.Results.ToList();
        FilteredObjectCount = FilteredObjects.Count;

        IsSearching = false;
    }
}
