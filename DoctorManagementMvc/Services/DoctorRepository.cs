using System.Collections.Concurrent;
using DoctorManagementMvc.Models;


namespace DoctorManagementMvc.Services
{
    public class DoctorRepository
    {
        // thread-safe
        private static readonly ConcurrentDictionary<string, Doctor> _doctors =
            new ConcurrentDictionary<string, Doctor>(StringComparer.OrdinalIgnoreCase);

        public IEnumerable<Doctor> All() => _doctors.Values.OrderBy(d => d.Code);

        public Doctor? GetByCode(string code) =>
            string.IsNullOrWhiteSpace(code) ? null :
            (_doctors.TryGetValue(code, out var d) ? d : null);

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

        public bool Update(Doctor d, out string error)
        {
            error = "";
            if (d == null || string.IsNullOrWhiteSpace(d.Code)) { error = "Invalid doctor."; return false; }
            if (!_doctors.ContainsKey(d.Code)) { error = "Doctor code does not exist."; return false; }
            _doctors[d.Code] = d;
            return true;
        }

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
