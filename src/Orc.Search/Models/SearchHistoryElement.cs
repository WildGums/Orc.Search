namespace Orc.Search
{
    using Catel.Data;

    public class SearchHistoryElement : ModelBase
    {
        public SearchHistoryElement()
        {
            Filter = string.Empty;
            FilterLowerCase = string.Empty;
        }

        public string Filter { get; set; }

        public string FilterLowerCase { get; private set; }

        public bool EverFoundResults { get; set; }
        public int Count { get; set; }

        private void OnFilterChanged()
        {
            var filter = Filter;

            if (string.IsNullOrWhiteSpace(filter))
            {
                FilterLowerCase = filter;
            }
            else
            {
                FilterLowerCase = filter.ToLower();
            }
        }

        protected override void OnDeserialized()
        {
            base.OnDeserialized();

            OnFilterChanged();
        }
    }
}
