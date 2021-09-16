using System;
using System.Threading.Tasks;

namespace Wishlist.Client.Services
{
    public class CommonService : ICommonService
    {
        public event Func<Task> AsyncRefresh;
        public async Task CallAsyncRequestRefresh() {
            await AsyncRefresh();
        }
    }
}