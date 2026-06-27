namespace ConferenceManager.API.Services.Registrations
{
    using ConferenceManager.API.Data;
    using ConferenceManager.API.DTOs.Registrations;
    using ConferenceManager.API.Models;
    using Microsoft.EntityFrameworkCore;

    public class RegistrationService : IRegistrationService
    {
        private readonly AppDbContext _context;

        public RegistrationService(AppDbContext context)
        {
            _context = context;
        }

        public RegistrationResultDto RegisterUser(int userId, CreateRegistrationDto dto)
        {
            PromoterProfile? promoter = null;

            if (!string.IsNullOrEmpty(dto.ReferralCode))
            {
                promoter = _context.PromoterProfiles
                    .Include(p => p.User)
                    .FirstOrDefault(p => p.ReferralCode == dto.ReferralCode);
            }

            bool invalidPromoCode = false;

            if (!string.IsNullOrEmpty(dto.ReferralCode) && promoter == null)
            {
                invalidPromoCode = true;
            }

            var conference = _context.Conferences
                .FirstOrDefault(c => c.Id == dto.ConferenceId);

            if (conference == null)
            {
                return new RegistrationResultDto
                {
                    Success = false,
                    Warning = "Conference not found"
                };
            }

            var existingRegistration = _context.Registrations
                .FirstOrDefault(r => r.UserId == userId && r.ConferenceId == dto.ConferenceId);

            if (existingRegistration != null)
            {
                return new RegistrationResultDto
                {
                    Success = false,
                    Warning = "Already registered"
                };
            }

            var registration = new Registration
            {
                UserId = userId,
                ConferenceId = dto.ConferenceId,
                RegistrationDate = DateTime.UtcNow,
                IsConfirmed = false,
                PromoterProfileId = promoter?.Id
            };

            _context.Registrations.Add(registration);
            _context.SaveChanges();

            return new RegistrationResultDto
            {
                Success = true,
                HasPromoter = promoter != null,
                PromoterName = promoter?.User?.Username,
                Warning = invalidPromoCode
                    ? "Invalid promo code. Registration completed without promoter."
                    : null
            };
        }
        //public RegistrationResultDto RegisterUser(int userId, CreateRegistrationDto dto)
        //{
        //    PromoterProfile? promoter = null;

        //    if (!string.IsNullOrEmpty(dto.ReferralCode))
        //    {
        //        promoter = _context.PromoterProfiles
        //            .Include(p => p.User)
        //            .FirstOrDefault(p => p.ReferralCode == dto.ReferralCode);
        //    }

        //    bool invalidPromoCode = false;

        //    if (!string.IsNullOrEmpty(dto.ReferralCode) && promoter == null)
        //    {
        //        invalidPromoCode = true;
        //    }

        //    var conference = _context.Conferences
        //            .FirstOrDefault(c => c.Id == dto.ConferenceId);

        //    if (conference == null)
        //    {
        //        return new RegistrationResultDto
        //        {
        //            Success = false,
        //            Warning = "Conference not found"
        //        };
        //    }

        //    var existingRegistration = _context.Registrations
        //        .FirstOrDefault(r => r.UserId == userId && r.ConferenceId == dto.ConferenceId);

        //    if (existingRegistration != null)
        //    {
        //        return new RegistrationResultDto
        //        {
        //            Success = true,
        //            HasPromoter = promoter != null,
        //            PromoterName = promoter == null
        //                ? null
        //                : _context.Users
        //                    .Where(u => u.Id == promoter.UserId)
        //                    .Select(u => u.Username)
        //                    .FirstOrDefault(),
        //                            Warning = invalidPromoCode
        //                ? "Invalid promo code. Registration completed without promoter."
        //                : null
        //        };
        //    }

        //    var registration = new Registration
        //    {
        //        UserId = userId,
        //        ConferenceId = dto.ConferenceId,
        //        RegistrationDate = DateTime.UtcNow,
        //        IsConfirmed = false,
        //        PromoterProfileId = promoter?.Id
        //    };

        //    _context.Registrations.Add(registration);

        //    _context.SaveChanges();

        //    return new RegistrationResultDto
        //    {
        //        Success = true,
        //        HasPromoter = promoter != null,
        //        Warning = invalidPromoCode
        //        ? "Invalid promo code. Registration completed without promoter."
        //        : null
        //    };
        //}

        public List<RegistrationDto> GetUserRegistrations(int userId)
        {
            return _context.Registrations
                .Where(r => r.UserId == userId)
                .Select(r => new RegistrationDto
                {
                    Id = r.Id,
                    ConferenceId = r.ConferenceId,
                    ConferenceTitle = r.Conference.Title,
                    RegistrationDate = r.RegistrationDate,
                    IsConfirmed = r.IsConfirmed
                })
                .ToList();
        }

        public List<AdminRegistrationDto> GetAllRegistrations()
        {
            return _context.Registrations
                .Select(r => new AdminRegistrationDto
                {
                    Id = r.Id,
                    UserId = r.UserId,
                    ConferenceId = r.ConferenceId,
                    ConferenceTitle = r.Conference.Title,
                    UserEmail = r.User.Email,
                    RegistrationDate = r.RegistrationDate,
                    IsConfirmed = r.IsConfirmed
                })
                .ToList();
        }

        public bool ConfirmRegistration(int registrationId, bool isConfirmed, int adminId)
        {
            var registration = _context.Registrations.FirstOrDefault(r => r.Id == registrationId);

            if (registration == null)
            {
                return false;
            }

            registration.IsConfirmed = isConfirmed;
            registration.ConfirmedByAdminId = adminId;
            registration.ConfirmedAt = DateTime.UtcNow;

            _context.SaveChanges();

            return true;
        }

        public List<AdminRegistrationDto> GetAllRegistrations(string? status)
        {
            var query = _context.Registrations.AsQueryable();

            if (status == "confirmed")
            {
                query = query.Where(r => r.IsConfirmed);
            }
            else if (status == "unconfirmed")
            {
                query = query.Where(r => !r.IsConfirmed);
            }

            return query
                .Select(r => new AdminRegistrationDto
                {
                    Id = r.Id,
                    UserId = r.UserId,
                    ConferenceId = r.ConferenceId,
                    ConferenceTitle = r.Conference.Title,
                    UserEmail = r.User.Email,
                    RegistrationDate = r.RegistrationDate,
                    IsConfirmed = r.IsConfirmed
                })
                .ToList();
        }

    }
}
