using System.Collections.Concurrent;
using DoctorManagementMvc.Models;

namespace DoctorManagementMvc.Services
{
    public class DoctorRepository
    {
        // Thread-safe
        private static readonly ConcurrentDictionary<string, Doctor> _doctors =
            new ConcurrentDictionary<string, Doctor>(StringComparer.OrdinalIgnoreCase);

        // ✅ Khối khởi tạo tĩnh: Hash cứng dữ liệu 3 bác sĩ + mỗi bác sĩ có 2 bệnh nhân
        static DoctorRepository()
        {
            var d1 = new Doctor
            {
                Code = "D001",
                Name = "Nguyễn Văn A",
                Specialization = "Tim mạch",
                Availability = 10,
                Patients = new List<Patient>
                {
                    new Patient { Id = "P001", Name = "Phạm Văn X", Age = 40, Disease = "Tăng huyết áp", Code = "D001" },
                    new Patient { Id = "P002", Name = "Đỗ Thị Y", Age = 55, Disease = "Rối loạn nhịp tim", Code = "D001" }
                }
            };

            var d2 = new Doctor
            {
                Code = "D002",
                Name = "Trần Thị B",
                Specialization = "Nhi khoa",
                Availability = 8,
                Patients = new List<Patient>
                {
                    new Patient { Id = "P003", Name = "Ngô Minh Z", Age = 5, Disease = "Cảm cúm", Code = "D002" },
                    new Patient { Id = "P004", Name = "Lê Bảo K", Age = 8, Disease = "Sốt siêu vi", Code = "D002" }
                }
            };

            var d3 = new Doctor
            {
                Code = "D003",
                Name = "Lê Văn C",
                Specialization = "Da liễu",
                Availability = 5,
                Patients = new List<Patient>
                {
                    new Patient { Id = "P005", Name = "Nguyễn Hồng M", Age = 22, Disease = "Viêm da dị ứng", Code = "D003" },
                    new Patient { Id = "P006", Name = "Trần Hải T", Age = 30, Disease = "Mụn trứng cá", Code = "D003" }
                }
            };

            _doctors[d1.Code] = d1;
            _doctors[d2.Code] = d2;
            _doctors[d3.Code] = d3;
        }

        // Lấy tất cả bác sĩ
        public IEnumerable<Doctor> All() => _doctors.Values.OrderBy(d => d.Code);

        // Lấy theo mã
        public Doctor? GetByCode(string code) =>
            string.IsNullOrWhiteSpace(code) ? null :
            (_doctors.TryGetValue(code, out var d) ? d : null);

        // Thêm bác sĩ mới
        public bool Add(Doctor d, out string error)
        {
            error = "";
            if (d == null || string.IsNullOrWhiteSpace(d.Code)) { error = "Invalid doctor."; return false; }
            if (!_doctors.TryAdd(d.Code, d))
            {
                error = $"Doctor code {d.Code} is duplicate.";
                return false;
            }
            return true;
        }

        // Cập nhật thông tin bác sĩ
        public bool Update(Doctor d, out string error)
        {
            error = "";
            if (d == null || string.IsNullOrWhiteSpace(d.Code)) { error = "Invalid doctor."; return false; }
            if (!_doctors.ContainsKey(d.Code)) { error = "Doctor code does not exist."; return false; }
            _doctors[d.Code] = d;
            return true;
        }

        // Xóa bác sĩ
        public bool Delete(string code, out string error)
        {
            error = "";
            if (!_doctors.TryRemove(code, out _))
            {
                error = "Doctor code does not exist.";
                return false;
            }
            return true;
        }

        // Tìm kiếm bác sĩ theo tên hoặc chuyên khoa
        public IEnumerable<Doctor> Search(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return All();
            var key = input.ToLowerInvariant();
            return _doctors.Values.Where(d =>
                (d.Name ?? "").ToLower().Contains(key) ||
                (d.Specialization ?? "").ToLower().Contains(key))
                .OrderBy(d => d.Code);
        }
    }
}
