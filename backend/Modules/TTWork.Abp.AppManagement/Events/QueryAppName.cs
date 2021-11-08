using System.Threading;
using System.Threading.Tasks;
using Abp.UI;
using Castle.Core.Internal;
using MediatR;
using Microsoft.AspNetCore.Http;
using TTWork.Abp.AppManagement.Applications.TT.Abp.AppManagement.Application;
using TTWork.Abp.AppManagement.Apps;
using TTWork.Abp.Core.Extensions;

namespace TTWork.Abp.AppManagement.Events
{
    public class QueryApp : IRequest<AppDto>
    {
        public string AppName { get; set; }
        public bool FromHeader { get; set; }

        public QueryApp(string appName = "", bool fromHeader = true)
        {
            AppName = appName;
            FromHeader = fromHeader;
        }

        public QueryApp(string appName)
        {
            AppName = appName;
            FromHeader = false;
        }

        public class QueryAppNameHandle : IRequestHandler<QueryApp, AppDto>
        {
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IAppProvider _appProvider;

            public QueryAppNameHandle(
                IHttpContextAccessor httpContextAccessor
                , IAppProvider appProvider
            )
            {
                _httpContextAccessor = httpContextAccessor;
                _appProvider = appProvider;
            }

            public async Task<AppDto> Handle(QueryApp request, CancellationToken cancellationToken)
            {
                if (request.FromHeader)
                    request.AppName = _httpContextAccessor.Get_AppName();

                if (request.AppName.IsNullOrEmpty())
                    throw new UserFriendlyException("get appname from header fail!!");

                var appValues = await _appProvider.GetOrNullAsync(request.AppName);

                return appValues != null
                    ? new AppDto
                    {
                        Value = appValues,
                        Name = request.AppName
                    }
                    : null;
            }
        }
    }
}