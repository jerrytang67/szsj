using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TtWork.ProjectName.Entities.Users;

namespace TtWork.ProjectName.Events.Queries
{
    public class UserSharedFromWhoQuery : IRequest<long?>
    {
        public UserSharedFromWhoQuery(long? abpSessionUserId)
        {
            AbpSessionUserId = abpSessionUserId;
        }

        public long? AbpSessionUserId { get; }

        public class UserSharedFromWhoQueryHandler : IRequestHandler<UserSharedFromWhoQuery, long?>
        {
            private readonly IRepository<UserEvent, long> _userEventRepository;

            public UserSharedFromWhoQueryHandler(IRepository<UserEvent, long> userEventRepository)
            {
                _userEventRepository = userEventRepository;
            }

            [UnitOfWork]
            public virtual async Task<long?> Handle(UserSharedFromWhoQuery request, CancellationToken cancellationToken)
            {
                var find = await _userEventRepository.GetAll().OrderByDescending(x => x.Id).FirstOrDefaultAsync(x => x.CreatorUserId == request.AbpSessionUserId, cancellationToken);

                return find?.FromUserId;
            }
        }
    }
}