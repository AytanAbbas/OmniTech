using Omnitech.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Omnitech.Dtos;
using Omnitech.Dal.AdoNet;

namespace Omnitech.Service
{
    public class PharmacyInvoiceService
    {
        private readonly PharmacyInvoiceRepository _pharmacyInvoiceRepository;

        public PharmacyInvoiceService(PharmacyInvoiceRepository pharmacyInvoiceRepository)
        {
            _pharmacyInvoiceRepository = pharmacyInvoiceRepository;
        }
        public async Task<List<PharmacyInvoiceInfo>> GetPharmacyInvoiceInfosByStartDateAndEndDateAsync(DateTime startDate, DateTime endDate)
        {
            List<PharmacyInvoiceInfo> pharmacyInvoiceInfos = await _pharmacyInvoiceRepository.GetPharmacyInvoiceInfosByStartDateAndEndDateAsync(startDate, endDate);

            return pharmacyInvoiceInfos;
        }

        public async Task<List<PharmacyInvoiceDetail>> GetPharmacyInvoiceDetailsBySourceIndexAndDateAsync(int sourceIndex, DateTime date,double invDesirable)
        {
            List<PharmacyInvoiceDetail> pharmacyInvoiceDetails= await _pharmacyInvoiceRepository.GetPharmacyInvoiceDetailsBySourceIndexAndDateAsync(sourceIndex, date,invDesirable);

            return pharmacyInvoiceDetails;
        }

        public async Task<List<FoodSupplementItem>> GetAllFoodSuppLementItemsAsync()
        {
            return await _pharmacyInvoiceRepository.GetAllFoodSuppLementItemsAsync();
        }

        public async Task AddFoodSupplementItemAsync(int sku, DateTime date, int sourceIndex,double quantity)
        {
            double price = await _pharmacyInvoiceRepository.GetFoodSupplementItemPriceByItemRefAsync(sku);
            await _pharmacyInvoiceRepository.AddFoodSupplementItemAsync(sku, date, sourceIndex,price, quantity);
        }

        public async Task<PharmacyInvoiceItemReplaceResponse> GetPharmacyInvoiceItemReplaceResponseByDateAndSourceIndex( int sourceIndex, DateTime datetime)
        {
            PharmacyInvoiceItemReplaceResponse pharmacyInvoiceItemReplaceResponse = await _pharmacyInvoiceRepository.GetPharmacyInvoiceItemReplaceResponseByDateAndSourceIndex(sourceIndex, datetime);
            pharmacyInvoiceItemReplaceResponse.ELAVE_EDILEN=await _pharmacyInvoiceRepository.GetPharmacyInvoiceAddingItemSumByDateAndSourceIndexAsync(sourceIndex, datetime);
            return pharmacyInvoiceItemReplaceResponse;
        }

        public async Task<PharmacyInvoiceItemReplaceResponse> ReplacePharmacyInvoiceItemAsync(int sourceIndex, DateTime date)
        {
            PharmacyInvoiceItemReplaceResponse pharmacyInvoiceItemReplaceResponse = await _pharmacyInvoiceRepository.ReplacePharmacyInvoiceItemAsync(sourceIndex, date);
            pharmacyInvoiceItemReplaceResponse.ELAVE_EDILEN=await _pharmacyInvoiceRepository.GetPharmacyInvoiceAddingItemSumByDateAndSourceIndexAsync(sourceIndex, date);
            return pharmacyInvoiceItemReplaceResponse;
        }

        public async Task DeletePharmacyInvoiceDetailLine(int logicalref)
        {
            await _pharmacyInvoiceRepository.DeletePharmacyInvoiceDetailLine(logicalref);
        }

        internal Task<List<PharmacyInvoiceDetail>> GetPharmacyInvoiceDetailsBySourceIndexAndDateAsync(int sourceIndex, DateTime date, object invDesirable)
        {
            throw new NotImplementedException();
        }
    }
}
