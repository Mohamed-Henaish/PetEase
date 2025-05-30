using PetEase.Core.Models;
using PetEase.Core.Repositories;
using Microsoft.AspNetCore.Identity.UI.Services;
using PetEase.Services.Interface;

namespace PetEase.Infrastructure.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailSender _emailSender;

        public DoctorService(IUnitOfWork unitOfWork, IEmailSender emailSender)
        {
            _unitOfWork = unitOfWork;
            _emailSender = emailSender;
        }
        public async Task<(IEnumerable<Doctor>, int)> GetDoctorsAsync(string? searchTerm, int pageNumber, int pageSize)
        {
            return await _unitOfWork.DoctorRepository.GetPagedDoctorsAsync(searchTerm, pageNumber, pageSize);
        }

        public async Task<Doctor> GetDoctorByIdAsync(int id)
        {
            return await _unitOfWork.DoctorRepository.
                GetFirstorDefaultAsync(d => d.Id == id , IncludeWord: "Address,TimeSlots");
        }

        public async Task<Doctor> CreateDoctorAsync(Doctor doctor)
        {
            await _unitOfWork.DoctorRepository.AddAsync(doctor);
            await _unitOfWork.CompleteAsync();
            return doctor;
        }

        public async Task<bool> UpdateDoctorAsync(Doctor doctor)
        {
            var exDoctor = await _unitOfWork.DoctorRepository.GetFirstorDefaultAsync(x=>x.Id==doctor.Id);
            if (exDoctor == null) return false;

            exDoctor.Name = doctor.Name;
            exDoctor.ConsultationPrice = doctor.ConsultationPrice;
            exDoctor.PhotoUrl = doctor.PhotoUrl;
            exDoctor.ContactNumber = doctor.ContactNumber;
            exDoctor.Email = doctor.Email;

           
            if (exDoctor.Address == null && doctor.Address != null)
            {
                exDoctor.Address = doctor.Address;
            }
            else if (exDoctor.Address != null && doctor.Address != null)
            {
               
                exDoctor.Address.Location = doctor.Address.Location;
                exDoctor.Address.City = doctor.Address.City;
                exDoctor.Address.State = doctor.Address.State;
                exDoctor.Address.Country = doctor.Address.Country;
            }
            else if (doctor.Address == null)
            {
                exDoctor.Address = null; 
            }

            var timeSlotsToRemove = exDoctor.TimeSlots
                .Where(ts => !doctor.TimeSlots.Any(dts => dts.Id == ts.Id))
                .ToList();

            foreach (var tsRemove in timeSlotsToRemove)
            {
                exDoctor.TimeSlots.Remove(tsRemove);
            }

           
            foreach (var ts in doctor.TimeSlots)
            {
                var existingTs = exDoctor.TimeSlots.FirstOrDefault(t => t.Id == ts.Id);
                if (existingTs != null)
                {
                   
                    existingTs.StartTime = ts.StartTime;
                    existingTs.EndTime = ts.EndTime;
                    existingTs.IsAvailable = ts.IsAvailable;
                }
                else
                {
                   
                    exDoctor.TimeSlots.Add(ts);
                }
            }

            await _unitOfWork.CompleteAsync();
            return true;
        }


        public async Task DeleteDoctorAsync(int id)
        {
            var doctor = await GetDoctorByIdAsync(id);
            if (doctor == null)
                throw new Exception("Doctor not found");

            await _unitOfWork.DoctorRepository.RemoveAsync(doctor);
            await _unitOfWork.CompleteAsync();
        }
        public async Task<DoctorRequest> ReserveDoctorAsync(DoctorRequest request)
        {
            var doctor = await _unitOfWork.DoctorRepository.GetFirstorDefaultAsync(d => d.Id == request.DoctorId);
            if (doctor == null)
                throw new Exception("Doctor not found");

            await _unitOfWork.DoctorRequestRepository.AddAsync(request);
            await _unitOfWork.CompleteAsync();

            // Email styles (inline CSS)
            string emailStyle = @"
        body { font-family: Arial, sans-serif; color: #333; font-size: 14px; }
        .container { padding: 20px; }
        h2 { color: #2a9df4; }
        p { line-height: 1.6; }
        .highlight { font-weight: bold; color: #444; }
        .footer { margin-top: 30px; font-size: 12px; color: #888; }
    ";

            // Vendor (Doctor) email body
            string vendorBody = $@"
    <html>
    <head><style>{emailStyle}</style></head>
    <body>
      <div class='container'>
        <h2>New Doctor Reservation</h2>
        <p>You have a new reservation for Dr. <span class='highlight'>{doctor.Name}</span>.</p>
        <p><strong>Patient Details:</strong></p>
        <p>
          Name: {request.UserName}<br/>
          Phone: {request.UserPhone}<br/>
          Email: {request.UserEmail}<br/>
          Reserved Time: {request.ReservedTime:yyyy-MM-dd HH:mm}
        </p>
        <p>Please review and confirm the reservation at your earliest convenience.</p>
        <p class='footer'>This is an automated notification from PetEase system.</p>
      </div>
    </body>
    </html>
    ";

            // User (Patient) email body
            string userBody = $@"
    <html>
    <head><style>{emailStyle}</style></head>
    <body>
      <div class='container'>
        <h2>Reservation Received</h2>
        <p>Dear <span class='highlight'>{request.UserName}</span>,</p>
        <p>Your reservation with Dr. <strong>{doctor.Name}</strong> at <strong>{request.ReservedTime:yyyy-MM-dd HH:mm}</strong> has been received.</p>
        <p>We will notify you once your reservation is confirmed.</p>
        <p>Thank you for choosing PetEase!</p>
        <p class='footer'>If you have any questions, reply to this email or contact support.</p>
      </div>
    </body>
    </html>
    ";

            if (!string.IsNullOrEmpty(doctor.Email))
                await _emailSender.SendEmailAsync(doctor.Email, "New Doctor Reservation", vendorBody);

            await _emailSender.SendEmailAsync(request.UserEmail, "Reservation Received", userBody);

            return request;
        }

    }
}
