namespace BtkProject.Models
{
    public static class Repository
    // repository, veritabanından bağımsız bir sınıf. burada bilgileri geçici olarak tutacak bir geçici veritabanı oluşturuyoruz.
    // bu sayede veritabanından bağımsız bir şekilde verileri tutabiliriz.
    {
        private static List<Candidate> applications = new();

        public static IEnumerable<Candidate> Applications => applications;

        public static void AddApplication(Candidate candidate)
        {
            applications.Add(candidate);
        }
    }
}