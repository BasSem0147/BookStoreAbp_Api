
using Acme.BookStore.Enums;
using Acme.BookStore.Models;
using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace Acme.BookStore.SeedData
{
    public class SeedAuthorData : IDataSeedContributor, ITransientDependency
    {
        private readonly IRepository<Author, Guid> _repo;

        public SeedAuthorData(IRepository<Author,Guid> repo)
        {
            this._repo = repo;
        }
        public async Task SeedAsync(DataSeedContext context)
        {
            if (await _repo.GetCountAsync() <= 0)
            {
                await _repo.InsertAsync(
                    new Author
                    {
                        Name = "George",
                        Surname = "Orwell",
                        Bio = "George Orwell was an English novelist, essayist, journalist and critic.",
                        BirthDate = new DateTime(1903, 6, 25),
                        DeathDate = new DateTime(1950, 1, 21)
                    },                   
                    autoSave: true
                ); 
            }
        }
    }
}
