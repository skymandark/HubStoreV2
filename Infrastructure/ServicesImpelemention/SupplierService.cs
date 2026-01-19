using Core.Domin;
using Core.Services.OrderServices;
using Core.ViewModels.SupplierViewModels;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.ServicesImpelemention
{
    public class SupplierService : ISupplierService
    {
        private readonly ApplicationDbContext _context;

        public SupplierService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<SupplierListDto>> GetSuppliers()
        {
            return await _context.Suppliers
                .OrderByDescending(s => s.CreatedAt)
                .Select(s => new SupplierListDto
                {
                    SupplierId = s.SupplierId,
                    Code = s.Code,
                    Name = s.Name,
                    ContactInfo = s.ContactInfo,
                    Phone = s.Phone,
                    Email = s.Email,
                    IsActive = s.IsActive,
                    CreatedAt = s.CreatedAt
                }).ToListAsync();
        }

        public async Task<SupplierRequestDto> GetSupplier(int id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);
            if (supplier == null) return null;

            return new SupplierRequestDto
            {
                SupplierId = supplier.SupplierId,
                Code = supplier.Code,
                Name = supplier.Name,
                ContactInfo = supplier.ContactInfo,
                Address = supplier.Address,
                Phone = supplier.Phone,
                Email = supplier.Email,
                IsActive = supplier.IsActive
            };
        }

        public async Task<int> CreateSupplier(SupplierRequestDto dto, string user)
        {
            var supplier = new Supplier
            {
                Code = dto.Code,
                Name = dto.Name,
                ContactInfo = dto.ContactInfo,
                Address = dto.Address,
                Phone = dto.Phone,
                Email = dto.Email,
                IsActive = dto.IsActive,
                CreatedBy = user,
                CreatedAt = DateTime.UtcNow
            };

            _context.Suppliers.Add(supplier);
            await _context.SaveChangesAsync();
            return supplier.SupplierId;
        }

        public async Task UpdateSupplier(SupplierRequestDto dto, string user)
        {
            var supplier = await _context.Suppliers.FindAsync(dto.SupplierId);
            if (supplier == null) throw new Exception("Supplier not found");

            supplier.Code = dto.Code;
            supplier.Name = dto.Name;
            supplier.ContactInfo = dto.ContactInfo;
            supplier.Address = dto.Address;
            supplier.Phone = dto.Phone;
            supplier.Email = dto.Email;
            supplier.IsActive = dto.IsActive;
            supplier.ModifiedBy = user;
            supplier.ModifiedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteSupplier(int id, string user)
        {
            // Note: In a real system, we might use soft delete or check for dependencies.
            // For now, we'll implement a basic delete or just deactivate.
            var supplier = await _context.Suppliers.FindAsync(id);
            if (supplier != null)
            {
                _context.Suppliers.Remove(supplier);
                await _context.SaveChangesAsync();
            }
        }

        public async Task ToggleSupplierStatus(int id, string user)
        {
            var supplier = await _context.Suppliers.FindAsync(id);
            if (supplier != null)
            {
                supplier.IsActive = !supplier.IsActive;
                supplier.ModifiedBy = user;
                supplier.ModifiedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }
    }
}
