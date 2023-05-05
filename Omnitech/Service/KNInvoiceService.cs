using Omnitech.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Omnitech.Dtos;
using Omnitech.Dal.AdoNet;

namespace Omnitech.Service
{
    public class KNInvoiceService
    {
        private readonly KNInvoiceRepository _kNInvoiceRepository;

        public KNInvoiceService(KNInvoiceRepository knInvoiceRepository)
        {
            _kNInvoiceRepository = knInvoiceRepository;
        }
        public async Task<List<KNInvoiceInfo>> GetKNInvoiceInfosByStartDateAndEndDateAsync(DateTime startDate, DateTime endDate)
        {
            List<KNInvoiceInfo> knInvoiceInfos = await _kNInvoiceRepository.GetKNInvoiceInfosByStartDateAndEndDateAsync(startDate, endDate);

            return knInvoiceInfos;
        }

        public async Task<List<KNInvoiceDetail>> AddKNInvoiceDetailsByInvIdAndMeblegAsync(int invId, double mebleg)
        {
            await _kNInvoiceRepository.AddKNInvoiceDetailsByInvIdAndMeblegAsync(invId, mebleg);

            List<KNInvoiceDetail> knInvoiceDetails = await _kNInvoiceRepository.GetKNInvoiceDetailsByInvIdAndMeblegAsync(invId, mebleg);

            return knInvoiceDetails;
        }

        public async Task<List<KNInvoiceDetail>> GetKNInvoiceDetailsByInvIdAndMeblegAsync(int invId, double mebleg)
        {
            List<KNInvoiceDetail> knInvoiceDetails = await _kNInvoiceRepository.GetKNInvoiceDetailsByInvIdAndMeblegAsync(invId, mebleg);

            return knInvoiceDetails;
        }

    }
}
