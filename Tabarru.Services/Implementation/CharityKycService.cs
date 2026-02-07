using Microsoft.AspNetCore.Http;
using System.Net;
using Tabarru.Common.Enums;
using Tabarru.Common.Models;
using Tabarru.Repositories.DatabaseContext;
using Tabarru.Repositories.IRepository;
using Tabarru.Repositories.Models;
using Tabarru.Services.IServices;
using Tabarru.Services.Models;

namespace Tabarru.Services.Implementation
{
    public class CharityKycService : ICharityKycService
    {
        private readonly ICharityKycRepository charityKycRepository;
        private readonly ICharityRepository charityRepository;
        private readonly IEmailMessageService emailMessageService;
        private readonly IFileStoringService fileStoringService;
        private readonly DbStorageContext dbContext;

        public CharityKycService(ICharityKycRepository charityKycRepository,
            ICharityRepository charityRepository,
            IEmailMessageService emailMessageService,
            IFileStoringService fileStoringService,
            DbStorageContext dbContext)
        {
            this.charityKycRepository = charityKycRepository;
            this.charityRepository = charityRepository;
            this.emailMessageService = emailMessageService;
            this.fileStoringService = fileStoringService;
            this.dbContext = dbContext;
        }

        public async Task<Response> SubmitKycAsync(string charityId, CharityKycDto dto)
        {
            await using var transaction = await dbContext.Database.BeginTransactionAsync();

            try
            {
                var charity = await charityRepository.GetByIdAsync(charityId);
                if (charity == null)
                    return new Response(HttpStatusCode.NotFound, "Charity not found");

                //Get existing active KYC, pending, 
                var existingKyc = await charityKycRepository.GetActiveByCharityIdAsync(charityId);

                if (existingKyc != null && existingKyc.Status == CharityKycStatus.Approved)
                {
                    return new Response(HttpStatusCode.BadRequest, "Charity KYC cannot submit, KYC is Approved .");
                }

                if (existingKyc != null)
                {
                    //delete old KYC
                    var kycDeleted = await charityKycRepository.DeleteAsync(existingKyc);

                    if (kycDeleted)
                    {
                        var kycDocumentDeleted = await charityKycRepository.DeleteKycDocumentAsync(existingKyc.CharityKycDocuments);
                    }
                }

                //Create new KYC
                var newKyc = new CharityKycDetails
                {
                    CharityId = charityId,
                    Status = CharityKycStatus.Pending,
                    IsCharityDocumentUploaded =
                        !string.IsNullOrWhiteSpace(dto.IncorporationCertificate),

                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    CharityName = dto.CharityName,
                    CountryCode = dto.CountryCode,
                    CharityNumber = dto.CharityNumber,
                    CharityKycDocuments = new CharityKycDocuments
                    {
                        Logo = dto.Logo,
                        IncorporationCertificate = dto.IncorporationCertificate,
                        UtilityBill = dto.UtilityBill,
                        TaxExemptionCertificate = dto.TaxExemptionCertificate,
                        BankStatement = dto.BankStatement
                    }
                };

                charity.KycStatus = CharityKycStatus.Pending;
                charity.IsKycVerified = false;

                await charityRepository.UpdateAsync(charity);
                await charityKycRepository.AddAsync(newKyc);

                //Commit transaction
                await transaction.CommitAsync();

                return new Response(
                    HttpStatusCode.OK,
                    existingKyc == null
                        ? "Charity KYC submitted successfully"
                        : "Charity KYC resubmitted successfully"
                );
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new Response(
                    HttpStatusCode.InternalServerError,
                    "An error occurred while submitting KYC"
                );
            }
        }

        public async Task<Response<string>> UploadAsync(IFormFile file, string fileName, string charityId)
        {
            if (file.Length == 0)
                throw new Exception("Empty file");

            var path = await fileStoringService.UploadAsync(file, fileName, charityId);
            return new Response<string>(HttpStatusCode.OK, path);
        }

        public async Task<Response> UpdateKycStatusAsync(AdminKycUpdateDto dto)
        {
            var charity = await charityKycRepository.GetCharityByIdAsync(dto.CharityId);
            if (charity == null)
                return new Response(HttpStatusCode.NotFound, "Charity not found");

            var kycDetails = await charityKycRepository.GetCharityKycDetailsByCharityIdAsync(dto.CharityId);
            if (kycDetails == null)
                return new Response(HttpStatusCode.NotFound, "KYC Details not found");

            charity.KycStatus = dto.Status;
            kycDetails.Status = dto.Status;
            charity.IsKycVerified = dto.Status == CharityKycStatus.Approved;

            var charityUdpate = await charityRepository.UpdateAsync(charity);
            var charityKycUpdate = await charityKycRepository.UpdateAsync(kycDetails);

            string finalHtml = GetEmailTemplate(dto.Status, dto.Reason);

            var success = await this.emailMessageService.SendEmailAsync(
            charity.Email,
            "KYC Status Update",
            finalHtml
            );

            if (!charityUdpate || !charityKycUpdate)
            {
                return new Response(HttpStatusCode.BadRequest, "Charity KYC submission failed.");
            }

            return new Response(HttpStatusCode.OK, $"Charity KYC {dto.Status} Updated");
        }

        private static string GetEmailTemplate(CharityKycStatus kycStatus, string rejectionReason)
        {
            Console.WriteLine($" directory : {Directory.GetCurrentDirectory()}");

            //string htmlPath1 = Path.Combine(Directory.GetCurrentDirectory(), "Tabarru.Common", "HtmlTemplates", "EmailVerificationBody.html");
            string htmlPath = Path.Combine(AppContext.BaseDirectory, "HtmlTemplates", "KycUpdateEmailBody.html");

            //Console.WriteLine($" html path 1 : {htmlPath1}");
            Console.WriteLine($" html path : {htmlPath}");

            if (!File.Exists(htmlPath))
                throw new FileNotFoundException($"KYC Update Template not found at {htmlPath}");

            string htmlContent = File.ReadAllText(htmlPath);
            var reasonDisplay = kycStatus == CharityKycStatus.Rejected
                                    ? "display:block;"
                                    : "display:none;";

            string finalHtml = htmlContent
                .Replace("{{KYC_STATUS}}", kycStatus.ToString())
                .Replace("{{REJECTION_REASON}}", rejectionReason ?? string.Empty)
                .Replace("{{REASON_DISPLAY}}", reasonDisplay);

            //string finalHtml = htmlContent.Replace("{{CODE}}", emailVerificationCode);
            return finalHtml;
        }

        public async Task<Response<IList<CharityReadDto>>> GetAllCharitiesForAdminAsync()
        {
            var charities = await charityKycRepository.GetAllCharitiesAsync();

            return new Response<IList<CharityReadDto>>(HttpStatusCode.OK, (charities.Select(x => x.MapToDto())).ToList(), ResponseCode.Data);
        }

        public Task<CharityKycStatus> GetCharityKycStatus(string CharityId)
        {
            return charityKycRepository.GetCharityKycStatus(CharityId);
        }

        public async Task<Response<string>> GetKycImageAsync(string path)
        {
            var result = await fileStoringService.GetAsync(path);
            return new Response<string>(HttpStatusCode.OK, result);
        }
    }
}
