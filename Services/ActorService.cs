using IMDBApi_Assignment4.Models.DB;
using IMDBApi_Assignment4.Models.DTOs.Request;
using IMDBApi_Assignment4.Models.DTOs.Response;
using IMDBApi_Assignment4.Models.Enums;
using IMDBApi_Assignment4.Repository.Interface;
using IMDBApi_Assignment4.Services.Interface;
using IMDBApi_Assignment4.Validations.Interface;

namespace IMDBApi_Assignment4.Services
{
    public class ActorService : IActorService
    {
        private readonly IActorRepository _actorRepository;
        private readonly IActorValidation _actorValidation;

        public ActorService(IActorRepository actorRepository, IActorValidation actorValidation)
        {
            _actorRepository = actorRepository;
            _actorValidation = actorValidation;
        }

        public async Task<List<PersonResponse>> GetAllAsync()
        {
            var actors = await _actorRepository.GetAllAsync();
            return actors.Select(MapToResponse).ToList();
        }

        public async Task<PersonResponse> GetByIdAsync(int id)
        {
            _actorValidation.ValidateIdAsync(id);

            var actor = await ExistsAsync(id);
            return MapToResponse(actor);
        }

        public async Task<(string Message, int Id)> CreateAsync(PersonRequest request)
        {
            _actorValidation.ValidateRequest(request);
            Enum.TryParse<Gender>(request.Gender, true, out var genderEnum);

            var actor = new Person
            {
                Name = request.Name,
                Bio = request.Bio,
                DOB = request.DOB,
                Gender = genderEnum
            };

            await _actorRepository.CreateAsync(actor);
            return ($"Actor '{actor.Name}' created successfully.", actor.Id);
        }

        public async Task<string> UpdateAsync(int id, PersonRequest request)
        {
            _actorValidation.ValidateIdAsync(id);
            _actorValidation.ValidateRequest(request);

            var existingActor = await ExistsAsync(id);

            Enum.TryParse<Gender>(request.Gender, true, out var genderEnum);

            existingActor.Name = request.Name;
            existingActor.Bio = request.Bio;
            existingActor.DOB = request.DOB;
            existingActor.Gender = genderEnum;

            await _actorRepository.UpdateAsync(existingActor);
            return $"Actor id: '{existingActor.Id}' updated successfully.";
        }

        public async Task DeleteAsync(int id)
        {
            _actorValidation.ValidateIdAsync(id);
            var isDeleted = await _actorRepository.DeleteAsync(id);
            if (!isDeleted)
                throw new KeyNotFoundException($"Actor with ID {id} not found");
        }

        public async Task<Person> ExistsAsync(int id)
        {
            var existingActor = await _actorRepository.GetByIdAsync(id);

            if (existingActor == null)
            {
                throw new KeyNotFoundException($"Actor with ID {id} not found");
            }

            return existingActor;
        }
        private PersonResponse MapToResponse(Person actor)
        {
            return new PersonResponse
            {
                Id = actor.Id,
                Name = actor.Name,
                Bio = actor.Bio,
                DOB = actor.DOB,
                Gender = actor.Gender
            };
        }
    }
}