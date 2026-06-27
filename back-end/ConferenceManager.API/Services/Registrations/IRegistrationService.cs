namespace ConferenceManager.API.Services.Registrations
{
    using ConferenceManager.API.DTOs.Registrations;

    public interface IRegistrationService
    {
        RegistrationResultDto RegisterUser(int userId, CreateRegistrationDto dto);

        List<RegistrationDto> GetUserRegistrations(int userId);

        List<AdminRegistrationDto> GetAllRegistrations();

        bool ConfirmRegistration(int registrationId, bool isConfirmed, int adminId);

        List<AdminRegistrationDto> GetAllRegistrations(string? status);    
    }
}
