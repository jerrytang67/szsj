using Microsoft.EntityFrameworkCore;
using TTWork.Abp.LaborUnion.Domains;

namespace TTWork.Abp.LaborUnion.EntityFrameworkCore
{
    public interface ILaborUnionDbContext
    {
        public DbSet<CraftsmanRecommend> CraftsmanRecommends { get; set; }
        public DbSet<Craftsman> Craftsman { get; set; }


    }
}