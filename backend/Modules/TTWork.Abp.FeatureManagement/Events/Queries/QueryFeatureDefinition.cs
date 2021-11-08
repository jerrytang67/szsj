using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TTWork.Abp.FeatureManagement.Features;

namespace TTWork.Abp.FeatureManagement.Events.Queries
{
    public class QueryFeatureDefinition : IRequest<List<FeatureDefinition>>
    {
        public QueryFeatureDefinition()
        {
        }

        public class QueryFeatureDefinitionHandle : IRequestHandler<QueryFeatureDefinition, List<FeatureDefinition>>
        {
            private readonly IFeatureDefinitionManager _definitionManager;

            public QueryFeatureDefinitionHandle(IFeatureDefinitionManager definitionManager)
            {
                _definitionManager = definitionManager;
            }

            public virtual Task<List<FeatureDefinition>> Handle(QueryFeatureDefinition request, CancellationToken cancellationToken)
            {
                return Task.FromResult(_definitionManager.GetAll().ToList());
            }
        }
    }
}