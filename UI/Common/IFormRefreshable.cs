namespace UI.Common
{
    /// <summary>
    /// Interface để form có thể tự động refresh/reload dữ liệu
    /// </summary>
    public interface IFormRefreshable
    {
        /// <summary>
        /// Refresh/reload dữ liệu của form
        /// </summary>
        void RefreshData();
    }
}

