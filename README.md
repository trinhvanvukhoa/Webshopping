<div align="center">

# 🛍️ WebsiteShopping

### Nơi giao diện tĩnh *hồi sinh* thành một đế chế thương mại điện tử

**Hệ thống Website giới thiệu sản phẩm và Đặt hàng**

</div>

---

## 🚀 Câu chuyện của dự án

Thay vì bắt đầu từ con số 0 trên một trang giấy trắng, `WebsiteShopping` được xây dựng trên một thử thách đầy thú vị:

> Nhận những bộ giao diện **HTML/CSS/JS tĩnh** có sẵn cho cả trang khách hàng lẫn trang quản trị, rồi biến chúng thành một ứng dụng web **đầy đủ tính năng** — nơi người dùng có thể lướt, chọn, bỏ vào giỏ và chốt đơn; nơi quản trị viên có thể điều khiển toàn bộ cỗ máy bán hàng bằng vài cú click.

Đây không chỉ là một bài tập. Đây là một **cuộc giải phẫu nghịch đảo**: từ *lớp vỏ* tĩnh, chúng tôi đã (và đang) bơm *hệ thần kinh* dữ liệu vào — cho đến khi bản giao diện cũ tỉnh giấc và bắt đầu quản lý sản phẩm, đơn hàng và khách hàng thực sự. 🧟⚡

---

## ✨ Tính năng nổi bật

### 🛒 Phân hệ Khách hàng — *Nơi mọi thứ bắt đầu*

| Tính năng | Mô tả |
| --------- | ------ |
| **Khám phá sản phẩm** | Hiển thị toàn bộ sản phẩm với giao diện bán hàng hiện đại, bắt mắt |
| **Lọc theo danh mục** | Điều hướng mượt mà qua các nhóm sản phẩm (Thời trang, Phụ kiện, Giày dép…) |
| **Chi tiết sản phẩm** | Hình ảnh, giá bán, mô tả — mọi thứ người mua cần để ra quyết định |
| **Giỏ hàng thông minh** | Thêm/xóa sản phẩm, cập nhật số lượng, tính tổng tiền tự động — trạng thái được lưu bằng **Session/Cookie** nên không bao giờ "bay hơi" khi tải lại trang |
| **Thanh toán & Đặt hàng** | Form điền thông tin trọn vẹn (Họ tên, SĐT, Địa chỉ, Ghi chú) → lưu hóa đơn vào DB → trang chúc mừng + giỏ hàng được dọn sạch |

### 🧠 Phân hệ Quản trị — *Bộ não của cỗ máy bán hàng*

| Tính năng | Mô tả |
| --------- | ------ |
| **Quản lý Danh mục** | Thêm, sửa, xóa, xem danh sách các nhóm sản phẩm |
| **Quản lý Sản phẩm** | Toàn quyền CRUD kèm chức năng **upload hình ảnh** và lưu trữ trong `wwwroot/images` |
| **Quản lý Đơn hàng** | Xem danh sách đơn, xem chi tiết từng đơn, cập nhật trạng thái trực quan: `Chờ xử lý → Đang giao → Hoàn thành / Đã hủy` |
| **Bảo mật cơ bản** | Đăng nhập Admin (Authentication) trước khi chạm vào vùng quản trị |

---

## 🧰 Công nghệ sử dụng

<div align="center">

![ASP.NET](https://img.shields.io/badge/ASP.NET_Core_10-512BD4?style=for-the-badge&logo=.net&logoColor=white)
![MVC](https://img.shields.io/badge/MVC_Pattern-512BD4?style=for-the-badge&logo=.net&logoColor=white)
![EFCore](https://img.shields.io/badge/Entity_Framework_Core-204E60?style=for-the-badge&logo=entity-framework&logoColor=white)
![SQLServer](https://img.shields.io/badge/SQL_Server-CC2927?style=for-the-badge&logo=microsoft-sql-server&logoColor=white)
![Razor](https://img.shields.io/badge/Razor_Views-5C2D91?style=for-the-badge&logo=blazor&logoColor=white)
![Bootstrap](https://img.shields.io/badge/Bootstrap-7952B3?style=for-the-badge&logo=bootstrap&logoColor=white)
![jQuery](https://img.shields.io/badge/jQuery-0769AD?style=for-the-badge&logo=jquery&logoColor=white)

</div>

- **ASP.NET Core MVC** — Kiến trúc Model-View-Controller, tổ chức bằng **Areas** (tách biệt rạch ròi giữa phân hệ Client & Admin)
- **Entity Framework Core** — Tiếp cận **Code-First**, dùng **EF Migrations** để sinh cơ sở dữ liệu
- **SQL Server** — Lưu trữ dữ liệu phi quan hệ
- **Razor Views + Partial Views** — Tách riêng `_Header`, `_Footer`, `_Sidebar`, `_Navigation` để **tái sử dụng tối đa** mã nguồn
- **Data Annotations** — Xác thực dữ liệu phía server (sản phẩm không được trống, SĐT đúng định dạng…)

---

## 🗂️ Cấu trúc dự án

```
Webshopping/
├── 📁 Areas/Admin/              # 🔐 Phân hệ quản trị
│   ├── Controllers/              #   DashboardController
│   └── Views/
│       ├── Dashboard/            #   Giao diện admin đầy đủ (Startmin)
│       └── Shared/              #   _LayoutAdmin, _Navigation, Error
├── 📁 Controllers/              # HomeController (trang chủ, privacy, error)
├── 📁 Models/                    # Các thực thể dữ liệu + ViewModel
├── 📁 Views/
│   ├── Home/                     # 🏠 Trang chủ khách hàng
│   ├── Product/                  # 🛍️ Shop, chi tiết sản phẩm, giỏ hàng, checkout
│   └── Shared/                  # _Layout, _Header, _Footer, _Sidebar, _TopDiscount
├── 📁 wwwroot/                   # 🎨 Tài nguyên tĩnh
│   ├── css/ + css_admin/         #   Stylesheet cho Client & Admin
│   ├── js/ + js_admin/           #   Scripts cho Client & Admin
│   ├── img/                      #   Hình ảnh sản phẩm, logo, background
│   └── lib/                      #   jQuery, Bootstrap, jquery-validation…
├── 📄 Program.cs                 # Điểm khởi động + định tuyến (Admin / Areas)
├── 📄 WebsiteShopping.csproj     # .NET 10 + EF Core SqlServer
└── 📄 appsettings.json           # Cấu hình ứng dụng
```

---

## 🗄️ Thiết kế cơ sở dữ liệu

Bốn thực thể lõi thao tác một vòng đời khép kín: từ **sản phẩm trưng bày** cho đến **hóa đơn được chốt**:

```
┌─────────────┐       ┌──────────────┐       ┌─────────────┐
│  Category   │ 1 ──▷ 0..* Product   │ 1 ──▷ 0..* OrderDetail ◁── 1 ── Order
└─────────────┘       └──────────────┘       └─────────────┘
   (Danh mục)           (Sản phẩm)             (Chi tiết đơn)(Đơn hàng)
```

| Bảng | Vai trò |
| ---- | ------- |
| `Category` | Nhóm các danh mục sản phẩm |
| `Product` | Thông tin sản phẩm (tên, giá, mô tả, hình ảnh) — thuộc một danh mục |
| `Order` | Tổng quan đơn hàng (người mua, SĐT, địa chỉ, ghi chú, trạng thái) |
| `OrderDetail` | Chi tiết từng sản phẩm trong đơn (số lượng, đơn giá) |

---

## 🛤️ Lộ trình phát triển

> Mỗi milestone là một mảnh ghép; kết lại thành bức tranh thương mại điện tử hoàn chỉnh.

| Giai đoạn | Nhiệm vụ trọng tâm | Tiến độ |
| --------- | ------------------ | :-----: |
| **Tuần 1** | Khởi tạo dự án, tích hợp giao diện tĩnh, cấu hình `_Layout.cshtml` cho Client & Admin | ✅ |
| **Tuần 2** | Models + Migration, CRUD Danh mục & Sản phẩm, xử lý upload ảnh | 🚧 |
| **Tuần 3** | Hiển thị sản phẩm động, lọc theo danh mục, bảo mật trang quản trị | ⏳ |
| **Tuần 4** | Giỏ hàng (Session/Cookie), Checkout, quản lý trạng thái đơn hàng | ⏳ |
| **Tuần 5** | Validation, dọn dẹp, tài liệu & nghiệm thu | ⏳ |

---

<div align="center">

*Được xây dựng với 💜 — biến từng dòng HTML tĩnh thành một trải nghiệm mua sắm sống động.*

</div>