namespace Zermelo.App.UWP.Helpers
{
    public delegate void LoadingChangedEventHandler(object sender, bool isLoading);

    public class MultiOpLoadingStatus
    {
        int _loadingOps = 0;

        public bool IsLoading { get; private set; }

        public event LoadingChangedEventHandler LoadingChanged;

        protected virtual void OnLoadingChanged() => LoadingChanged?.Invoke(this, IsLoading);

        public void AddLoadingOperation()
        {
            if (++_loadingOps > 0 && IsLoading != true)
            {
                IsLoading = true;
                OnLoadingChanged();
            }
        }

        public void FinishLoadingOperation()
        {
            if (--_loadingOps < 1 && IsLoading != false)
            {
                IsLoading = false;
                OnLoadingChanged();
            }
        }
    }
}
