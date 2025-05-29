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
            return await _unitOfWork.DoctorRepository.GetFirstorDefaultAsync(d => d.Id == id);
        }

        public async Task<Doctor> CreateDoctorAsync(Doctor doctor)
        {
            await _unitOfWork.DoctorRepository.AddAsync(doctor);
            await _unitOfWork.CompleteAsync();
            return doctor;
        }

        public async Task UpdateDoctorAsync(Doctor doctor)
        {
            await _unitOfWork.DoctorRepository.UpdateAsync(doctor);
            await _unitOfWork.CompleteAsync();
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

            string vendorSubject = "New Doctor Reservation";
            string vendorBody = $@"
                New reservation for Dr. {doctor.Name}.<br/>
                Patient Name: {request.UserName}<br/>
                Phone: {request.UserPhone}<br/>
                Email: {request.UserEmail}<br/>
                Time: {request.ReservedTime}
            ";

            string userSubject = "Reservation Received";
            string userBody = $@"
                Dear {request.UserName},<br/>
                Your reservation with Dr. {doctor.Name} at {request.ReservedTime} is received.<br/>
                We will notify you once confirmed.<br/>Thanks!
            ";

            if (!string.IsNullOrEmpty(doctor.Email))
                await _emailSender.SendEmailAsync(doctor.Email, vendorSubject, vendorBody);

            await _emailSender.SendEmailAsync(request.UserEmail, userSubject, userBody);

            return request;
        }
    }
}
