namespace Orc.Search
{
    public interface ISearchHighlightProvider
    {
        void ResetHighlight();

        void HighlightSearchable(object searchable);
    }
}
