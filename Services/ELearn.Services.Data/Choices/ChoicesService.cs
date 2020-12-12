namespace ELearn.Services.Data.Choices
{
    using System.Threading.Tasks;

    using AutoMapper;
    using ELearn.Data.Common.Repositories;
    using ELearn.Data.Models;
    using ELearn.Services.Mapping;
    using ELearn.Web.ViewModels.Choices;
    using Microsoft.EntityFrameworkCore;

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

        public async Task EditChoiceAsync(EditChoiceViewModel model)
        {
            var choice = await this.choiceRepository.All()
                .FirstOrDefaultAsync(x => x.Id == model.Id);

            choice.Text = model.Text;
            choice.IsTrue = model.IsTrue;

            this.choiceRepository.Update(choice);
            await this.choiceRepository.SaveChangesAsync();
        }

        public async Task DeleteChoiceAsync(string choiceId)
        {
            var choice = await this.choiceRepository.All()
                .FirstOrDefaultAsync(x => x.Id == choiceId);

            this.choiceRepository.Delete(choice);
            await this.choiceRepository.SaveChangesAsync();
        }

        public async Task<ChoiceViewModel> GetChoiceByIdAsync(string choiceId)
        {
            var choice = await this.choiceRepository.All()
                .To<ChoiceViewModel>()
                .FirstOrDefaultAsync(x => x.Id == choiceId);

            return choice;
        }
    }
}
