# K8s Manager - Hướng Dẫn Cài Đặt Database

## Cài Đặt Nhanh

### Phương Án 1: Sử Dụng SQL Server Management Studio (SSMS)

1. Mở SSMS và kết nối đến SQL Server instance của bạn
2. Mở và thực thi các script theo thứ tự sau:
   - `create_database.sql` - Tạo database K8sManager
   - `create_tables.sql` - Tạo tất cả các bảng với indexes
   - `seed_data.sql` - Thêm dữ liệu người dùng và cài đặt mặc định

### Phương Án 2: Sử Dụng sqlcmd (Command Line)

```powershell
# Tạo database
sqlcmd -S (localdb)\MSSQLLocalDB -i create_database.sql

# Tạo các bảng
sqlcmd -S (localdb)\MSSQLLocalDB -i create_tables.sql

# Thêm dữ liệu khởi tạo
sqlcmd -S (localdb)\MSSQLLocalDB -i seed_data.sql
```

### Phương Án 3: Sử Dụng PowerShell Script

```powershell
# Chạy tất cả scripts theo thứ tự
$server = "(localdb)\MSSQLLocalDB"
sqlcmd -S $server -i create_database.sql
sqlcmd -S $server -i create_tables.sql
sqlcmd -S $server -i seed_data.sql
```

## Thông Tin Đăng Nhập Mặc Định

Sau khi chạy `seed_data.sql`, bạn có thể đăng nhập với:

| Tên đăng nhập | Mật khẩu | Vai trò |
|----------|----------|------|
| admin | Admin@123 | Admin |
| operator | Operator@123 | Operator |
| viewer | Viewer@123 | Viewer |

## Cấu Trúc Database

### Các Bảng Được Tạo

1. **AppUser** - Tài khoản người dùng với xác thực
2. **UserSession** - Phiên đăng nhập của người dùng với JWT tokens
3. **ClusterConfig** - Cấu hình Kubernetes cluster
4. **AuditLog** - Nhật ký audit bất biến (lưu trữ 7 năm)
5. **Template** - Metadata của YAML template
6. **TemplateVersion** - Nội dung YAML được version control
7. **Favorite** - Bookmark của người dùng cho các resources
8. **AppSetting** - Cài đặt cấu hình ứng dụng

### Connection String

Cập nhật `appsettings.json` với:

```json
{
  "ConnectionStrings": {
    "Default": "Server=(localdb)\\MSSQLLocalDB;Database=K8sManager;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

## Kiểm Tra

Kiểm tra xem database đã được tạo thành công chưa:

```sql
USE K8sManager;
GO

-- Liệt kê tất cả các bảng
SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE';

-- Đếm số người dùng
SELECT COUNT(*) AS UserCount FROM AppUser;

-- Xem cài đặt mặc định
SELECT * FROM AppSetting;
```

## Xử Lý Sự Cố

### Lỗi: Database đã tồn tại

Nếu bạn gặp lỗi "database already exists", bạn có thể xóa và tạo lại:

```sql
USE master;
GO

DROP DATABASE K8sManager;
GO
```

Sau đó chạy lại các scripts.

### Lỗi: Không thể kết nối đến server

Đảm bảo SQL Server LocalDB đang chạy:

```powershell
# Khởi động LocalDB
sqllocaldb start MSSQLLocalDB

# Kiểm tra trạng thái
sqllocaldb info MSSQLLocalDB
```

## Migration Từ Database Hiện Có

Nếu bạn có database K8sManager hiện có, hãy backup trước:

```sql
BACKUP DATABASE K8sManager
TO DISK = 'C:\Backups\K8sManager_Backup.bak'
WITH FORMAT, COMPRESSION;
```

## Các Bước Tiếp Theo

1. Cập nhật cấu hình cluster trong bảng `ClusterConfig` với đường dẫn kubeconfig thực tế của bạn
2. Đổi mật khẩu mặc định
3. Cấu hình các thiết lập ứng dụng trong bảng `AppSetting`
4. Chạy ứng dụng và test đăng nhập
