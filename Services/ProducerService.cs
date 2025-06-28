using IMDBApi_Assignment4.Models.DB;
using IMDBApi_Assignment4.Models.DTOs.Request;
using IMDBApi_Assignment4.Models.DTOs.Response;
using IMDBApi_Assignment4.Models.Enums;
using IMDBApi_Assignment4.Repository.Interface;
using IMDBApi_Assignment4.Services.Interface;
using IMDBApi_Assignment4.Validations.Interface;

namespace IMDBApi_Assignment4.Services
{
    public class ProducerService : IProducerService
    {
        private readonly IProducerValidation _producerValidation;
        private readonly IProducerRepository _producerRepository;

        public ProducerService(IProducerValidation producerValidation, IProducerRepository producerRepository)
        {
            _producerValidation = producerValidation;
            _producerRepository = producerRepository;
        }

        public async Task<List<PersonResponse>> GetAllAsync()
        {
            var producers = await _producerRepository.GetAllAsync();
            return producers.Select(MapToResponse).ToList();
        }

        public async Task<PersonResponse> GetByIdAsync(int id)
        {
            _producerValidation.ValidateIdAsync(id);

            var producer = await ExistsAsync(id);

            return MapToResponse(producer);
        }

        public async Task<(string Message, int Id)> CreateAsync(PersonRequest request)
        {
            _producerValidation.ValidateRequest(request);

            Enum.TryParse<Gender>(request.Gender, true, out var genderEnum);

            var producer = new Person
            {
                Name = request.Name,
                Bio = request.Bio,
                DOB = request.DOB,
                Gender = genderEnum
            };

            await _producerRepository.CreateAsync(producer);

            return ($"Producer '{producer.Name}' created successfully.", producer.Id);
        }

        public async Task<string> UpdateAsync(int id, PersonRequest request)
        {
            _producerValidation.ValidateIdAsync(id);

            var existingproducer = await ExistsAsync(id);

            _producerValidation.ValidateRequest(request);

            Enum.TryParse<Gender>(request.Gender, true, out var genderEnum);

            existingproducer.Name = request.Name;
            existingproducer.Bio = request.Bio;
            existingproducer.DOB = request.DOB;
            existingproducer.Gender = genderEnum;

            await _producerRepository.UpdateAsync(existingproducer);

            return $"Producer id: '{id}' updated successfully.";
        }

        public async Task DeleteAsync(int id)
        {
            _producerValidation.ValidateIdAsync(id);

            var isDeleted = await _producerRepository.DeleteAsync(id);

            if (!isDeleted)
                throw new KeyNotFoundException($"Producer with ID {id} not found");
        }
        public async Task<Person> ExistsAsync(int id)
        {
            var existingProducer = await _producerRepository.GetByIdAsync(id);

            if (existingProducer == null)
            {
                throw new KeyNotFoundException($"Producer with ID {id} not found");
            }

            return existingProducer;
        }

        private PersonResponse MapToResponse(Person producer)
        {
            return new PersonResponse
            {
                Id = producer.Id,
                Name = producer.Name,
                Bio = producer.Bio,
                DOB = producer.DOB,
                Gender = producer.Gender
            };
        }
    }
}