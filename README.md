# ASP.NET Core MVC Projeleri

Bu repository, ASP.NET Core MVC öğrenme sürecinde geliştirilen iki projeyi içermektedir:

## 1. Basics Projesi

Temel ASP.NET Core MVC kavramlarını öğrenmek için oluşturulmuş bir projedir.

### İçerik:
- MVC (Model-View-Controller) yapısı
- Controller kullanımı
- View yapısı ve Razor syntax
- Model binding
- Form işlemleri
- Layout kullanımı
- Routing yapılandırması

### Önemli Dosyalar:
- `Controllers/EmployeeController.cs`: Çalışan işlemlerinin yönetildiği controller
- `Views/Employee/`: Çalışan görünümlerinin bulunduğu klasör
  - Index.cshtml
  - Index1.cshtml
  - Index2.cshtml
  - Index3.cshtml

## 2. BtkProject

Daha gelişmiş ASP.NET Core MVC özelliklerini içeren bir proje.

### Özellikler:
- Entity Framework Core entegrasyonu
- Repository pattern implementasyonu
- Dependency Injection kullanımı
- CRUD operasyonları
- Authentication ve Authorization
- Validation işlemleri

## Gereksinimler

- .NET Core SDK
- Visual Studio 2019/2022 veya VS Code
- SQL Server (LocalDB veya Express edition)

## Kurulum

1. Repository'yi klonlayın
2. İlgili projenin dizinine gidin
3. Gerekli NuGet paketlerini yükleyin
4. Veritabanı migration'larını uygulayın
5. Projeyi çalıştırın

## Katkıda Bulunma

1. Bu repository'yi fork edin
2. Feature branch'i oluşturun (`git checkout -b feature/YeniOzellik`)
3. Değişikliklerinizi commit edin (`git commit -m 'Yeni özellik eklendi'`)
4. Branch'inizi push edin (`git push origin feature/YeniOzellik`)
5. Pull Request oluşturun

## Lisans

Bu proje MIT lisansı altında lisanslanmıştır. Detaylar için [LICENSE](LICENSE) dosyasına bakınız.
 
