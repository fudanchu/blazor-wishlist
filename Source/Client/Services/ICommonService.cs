using System;
using System.Threading.Tasks;

namespace Wishlist.Client.Services
{
    public interface ICommonService
    {
        event Func<Task> AsyncRefresh;
        Task CallAsyncRequestRefresh();
    }
}
