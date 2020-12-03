namespace ELearn.Services.Data.Choices
{
    using System.Threading.Tasks;

    using AutoMapper;
    using ELearn.Data.Common.Repositories;
    using ELearn.Data.Models;

    public class ChoicesService : IChoicesService
    {
        private readonly IDeletableEntityRepository<Choice> choiceRepository;
        private readonly IMapper mapper;

        public ChoicesService(IDeletableEntityRepository<Choice> choiceRepository, IMapper mapper)
        {
            this.choiceRepository = choiceRepository;
            this.mapper = mapper;
        }

        public async Task AddChoiceAsync<TModel>(TModel model)
        {
            var choice = this.mapper.Map<Choice>(model);

            await this.choiceRepository.AddAsync(choice);
            await this.choiceRepository.SaveChangesAsync();
        }
    }
}
