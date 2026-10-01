# WebsiteShopping

## Đồ án học tập

- **Tác giả:** Trịnh Văn Vũ Khoa
- **Lớp:** CĐ CNTT 24WEBD
- **Mã sinh viên:** 0306241458

## Công nghệ và yêu cầu phiên bản

- ASP.NET Core MVC, .NET 10 (`net10.0`)
- Entity Framework Core và SQL Server provider `10.0.12`
- SQL Server
- .NET SDK 10.0
- Công cụ EF Core CLI `dotnet-ef` phiên bản `10.0.12`

## Cài đặt

1. Mở thư mục dự án chứa `WebsiteShopping.csproj`.
2. Cấu hình chuỗi kết nối `ConnectionStrings:DevConnection` trong `appsettings.Development.json` để trỏ đến SQL Server của bạn.
3. Khôi phục các gói NuGet:

   ```powershell
   dotnet restore
   ```

4. Nếu chưa cài EF Core CLI, cài phiên bản tương ứng:

   ```powershell
   dotnet tool install --global dotnet-ef --version 10.0.12
   ```

5. Tạo/cập nhật cơ sở dữ liệu:

   ```powershell
   dotnet ef database update
   ```

6. Chạy ứng dụng:

   ```powershell
   dotnet run
   ```