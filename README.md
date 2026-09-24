<div align="center">

# WebsiteShopping

### Hệ thống Website giới thiệu sản phẩm và Đặt hàng

</div>

---

## Giới thiệu

`WebsiteShopping` là một ứng dụng thương mại điện tử xây dựng trên **ASP.NET Core MVC**, phát triển từ bộ giao diện tĩnh (HTML/CSS/JS) có sẵn cho cả trang khách hàng lẫn trang quản trị, biến chúng thành một hệ thống hoàn chỉnh: khách hàng lướt, chọn, bỏ vào giỏ và chốt đơn; quản trị viên điều khiển toàn bộ hoạt động bán hàng.

---

## Tính năng nổi bật

### Phân hệ Khách hàng

| Tính năng | Mô tả |
| --------- | ------ |
| **Khám phá sản phẩm** | Hiển thị toàn bộ sản phẩm với giao diện bán hàng hiện đại |
| **Lọc theo danh mục** | Điều hướng qua các nhóm sản phẩm (Thời trang, Phụ kiện, Giày dép...) |
| **Chi tiết sản phẩm** | Hình ảnh, giá bán, mô tả cho người mua |
| **Giỏ hàng thông minh** | Thêm/xóa sản phẩm, cập nhật số lượng, tính tổng tiền tự động; trạng thái lưu bằng **Session/Cookie** không mất khi tải lại trang |
| **Thanh toán & Đặt hàng** | Form thông tin (Họ tên, SĐT, Địa chỉ, Ghi chú) → lưu đơn hàng vào DB → trang thành công và dọn sạch giỏ hàng |

### Phân hệ Quản trị

| Tính năng | Mô tả |
| --------- | ------ |
| **Quản lý Danh mục** | Thêm, sửa, xóa, xem danh sách danh mục sản phẩm |
| **Quản lý Sản phẩm** | CRUD kèm chức năng **upload hình ảnh**, lưu trong `wwwroot/images` |
| **Quản lý Đơn hàng** | Xem danh sách, chi tiết đơn, cập nhật trạng thái: `Chờ xử lý → Đang giao → Hoàn thành / Đã hủy` |
| **Bảo mật** | Đăng nhập Admin (Authentication) trước khi truy cập vùng quản trị |

---

## Công nghệ sử dụng

<div align="center">

![ASP.NET](https://img.shields.io/badge/ASP.NET_Core_10-512BD4?style=for-the-badge&logo=.net&logoColor=white)
![EFCore](https://img.shields.io/badge/Entity_Framework_Core-204E60?style=for-the-badge&logo=entity-framework&logoColor=white)
![SQLServer](https://img.shields.io/badge/SQL_Server-CC2927?style=for-the-badge&logo=microsoft-sql-server&logoColor=white)
![Razor](https://img.shields.io/badge/Razor_Views-5C2D91?style=for-the-badge&logo=blazor&logoColor=white)
![Bootstrap](https://img.shields.io/badge/Bootstrap-7952B3?style=for-the-badge&logo=bootstrap&logoColor=white)
![jQuery](https://img.shields.io/badge/jQuery-0769AD?style=for-the-badge&logo=jquery&logoColor=white)

</div>

- **ASP.NET Core MVC** — Kiến trúc MVC, tổ chức bằng **Areas** (tách rõ phân hệ Client & Admin)
- **Entity Framework Core** — Tiếp cận **Code-First** với **EF Migrations** tự sinh cơ sở dữ liệu
- **SQL Server** — Cơ sở dữ liệu quan hệ
- **Razor Views + Partial Views** — Tách `_Header`, `_Footer`, `_Sidebar`, `_Navigation` tái sử dụng tối đa mã nguồn
- **Data Annotations** — Xác thực dữ liệu phía server

---

## Cấu trúc dự án

```
Webshopping/
├── Areas/Admin/              # Phân hệ quản trị
│   ├── Controllers/          #   DashboardController
│   └── Views/
│       ├── Dashboard/        #   Giao diện admin (Startmin)
│       └── Shared/           #   _LayoutAdmin, _Navigation, Error
├── Controllers/              # HomeController (trang chủ, privacy, error)
├── Models/                   # Các thực thể dữ liệu + ViewModel
├── Views/
│   ├── Home/                 # Trang chủ khách hàng
│   ├── Product/              # Shop, chi tiết sản phẩm, giỏ hàng, checkout
│   └── Shared/               # _Layout, _Header, _Footer, _Sidebar, _TopDiscount
├── wwwroot/                  # Tài nguyên tĩnh
│   ├── css/ + css_admin/     #   Stylesheet cho Client & Admin
│   ├── js/ + js_admin/       #   Scripts cho Client & Admin
│   ├── img/                  #   Hình ảnh sản phẩm, logo, background
│   └── lib/                  #   jQuery, Bootstrap, jquery-validation...
├── Program.cs                # Điểm khởi động + định tuyến (Admin / Areas)
├── WebsiteShopping.csproj    # .NET 10 + EF Core SqlServer
└── appsettings.json          # Cấu hình ứng dụng
```

---

## Thiết kế cơ sở dữ liệu

```
Category 1 ── 0..* Product 1 ── 0..* OrderDetail 0..* ── 1 Order
   (Danh mục)   (Sản phẩm)       (Chi tiết đơn)      (Đơn hàng)
```

| Bảng | Vai trò |
| ---- | ------- |
| `Category` | Nhóm các danh mục sản phẩm |
| `Product` | Thông tin sản phẩm (tên, giá, mô tả, hình ảnh) — thuộc một danh mục |
| `Order` | Tổng quan đơn hàng (người mua, SĐT, địa chỉ, ghi chú, trạng thái) |
| `OrderDetail` | Chi tiết từng sản phẩm trong đơn (số lượng, đơn giá) |

---

## Lộ trình phát triển

| Giai đoạn | Nhiệm vụ trọng tâm | Tiến độ |
| --------- | ------------------ | :-----: |
| **Tuần 1** | Khởi tạo dự án, tích hợp giao diện tĩnh, cấu hình `_Layout.cshtml` cho Client & Admin | Xong |
| **Tuần 2** | Models + Migration, CRUD Danh mục & Sản phẩm, xử lý upload ảnh | Dang lam |
| **Tuần 3** | Hiển thị sản phẩm động, lọc theo danh mục, bảo mật trang quản trị | Cho |
| **Tuần 4** | Giỏ hàng (Session/Cookie), Checkout, quản lý trạng thái đơn hàng | Cho |
| **Tuần 5** | Validation, dọn dẹp, tài liệu & nghiệm thu | Cho |