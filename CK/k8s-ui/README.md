# K8s Manager - Kubernetes Management Application

Ứng dụng quản lý Kubernetes với WinForm UI và RESTful API backend.

## Yêu cầu hệ thống

- .NET 8.0 SDK
- SQL Server (LocalDB hoặc SQL Server Express)
- Windows OS (cho WinForm)

## Cấu trúc dự án

```
k8s-ui/
├── Api/              # RESTful API backend (ASP.NET Core)
├── Controllers/      # WinForm Controllers
├── Views/           # WinForm Views
├── Services/        # Business Logic Services
├── Models/          # Domain Models
├── Database/        # SQL Scripts
└── Infrastructure/  # Data Access Layer
```

## Cách chạy ứng dụng

### 1. Khởi động API Server

```powershell
cd Api
dotnet run
```

API sẽ chạy trên `http://localhost:5000`

### 2. Khởi động WinForm Application

```powershell
# Build project
dotnet build K8sManager.csproj

# Chạy ứng dụng
.\bin\Debug\net8.0-windows\K8sManager.exe
```

**Hoặc từ Visual Studio:** Nhấn F5 hoặc Ctrl+F5

## Tài khoản test

| Username | Password | Role |
|----------|----------|------|
| admin | Admin123! | Administrator |
| operator | Operator123! | Operator |
| viewer | Viewer123! | Viewer |

## Cách test chương trình

### Test 1: Login và Authentication

1. Khởi động API và WinForm app
2. Đăng nhập với tài khoản `admin` / `Admin123!`
3. **Kết quả mong đợi**: Vào màn hình chính thành công

### Test 2: Kubernetes Context Management

1. Sau khi login, kiểm tra **Context dropdown** (góc trên trái)
2. Click vào dropdown
3. **Kết quả mong đợi**: Hiển thị 3 contexts:
   - `docker-desktop` (selected)
   - `production`
   - `development`

### Test 3: API Health Check

```powershell
# Kiểm tra API server đang chạy
Invoke-WebRequest -Uri "http://localhost:5000/health" -UseBasicParsing | Select-Object StatusCode, Content
```

**Kết quả mong đợi:**
```json
{
  "status": "Healthy",
  "timestamp": "2025-10-24T...",
  "version": "1.0.0"
}
```

### Test 4: Swagger API Documentation

1. Mở browser và truy cập: `http://localhost:5000/swagger`
2. **Kết quả mong đợi**: Hiển thị Swagger UI với tất cả endpoints

### Test 5: Audit Logs

1. Trong WinForm app, click menu **Audit Logs**
2. **Kết quả mong đợi**: Hiển thị danh sách các hoạt động đã thực hiện (login, API calls, etc.)

### Test 6: Templates CRUD

1. Click menu **Templates**
2. Tạo template mới với:
   - Name: `nginx-deployment`
   - Type: `Deployment`
   - Content: (YAML content)
3. **Kết quả mong đợi**: Template được tạo thành công và hiển thị trong danh sách

### Test 7: Role-Based Access Control

1. Logout khỏi admin
2. Login với `viewer` / `Viewer123!`
3. Thử thực hiện các thao tác như tạo/xóa template
4. **Kết quả mong đợi**: Viewer chỉ có quyền xem, không có quyền tạo/xóa/sửa

### Test 8: Kubernetes Resources (Khi có cluster)

**Lưu ý**: Test này chỉ hoạt động khi có Kubernetes cluster thực sự

1. Đảm bảo kubectl có thể kết nối cluster: `kubectl cluster-info`
2. Trong WinForm app, chọn context từ dropdown
3. **Kết quả mong đợi**: 
   - Hiển thị danh sách Namespaces
   - Có thể xem Pods, Deployments, Services

### Test 9: Settings & Cluster Configuration

1. Click menu **Settings**
2. Xem danh sách Cluster Configurations
3. Thử thêm cluster config mới
4. **Kết quả mong đợi**: Config được lưu vào database

### Test 10: Error Handling (Cluster Unreachable)

1. Login vào app (không có K8s cluster thực)
2. **Kết quả mong đợi**:
   - App vẫn khởi động thành công
   - Context dropdown hiển thị 3 contexts
   - Status bar hiển thị warning: "Contexts loaded, but cluster unreachable"
   - Không có error dialog

## Troubleshooting

### API không khởi động được

```powershell
# Kiểm tra port 5000 có bị chiếm không
Get-NetTCPConnection -LocalPort 5000 -ErrorAction SilentlyContinue

# Nếu bị chiếm, kill process đó hoặc đổi port trong appsettings.json
```

### Database connection error

1. Kiểm tra connection string trong `appsettings.json`
2. Đảm bảo SQL Server đang chạy
3. Chạy script tạo database: `Database/create_tables.sql`

### "No context" error trong WinForm

1. Kiểm tra file kubeconfig tồn tại: `~/.kube/config`
2. Nếu không có, tạo file mock theo `CONTEXT_MANAGEMENT_GUIDE.md`
3. Đảm bảo path trong `appsettings.json` đúng

### Build errors

```powershell
# Clean và rebuild
dotnet clean
Remove-Item -Recurse -Force obj, bin
dotnet build
```

## API Endpoints chính

- `GET /health` - Health check
- `POST /api/auth/login` - Login
- `GET /api/audit-logs` - Lấy audit logs
- `GET /api/templates` - Lấy danh sách templates
- `POST /api/templates` - Tạo template mới
- `GET /api/settings` - Lấy settings
- `GET /api/cluster-configs` - Lấy cluster configurations

## Log Files

- API logs: `Api/logs/`
- WinForm error log: `bin/Debug/net8.0-windows/error.log`
- K8s debug log: `bin/Debug/net8.0-windows/k8s-debug.log`

## Tài liệu chi tiết

- [TEST_SCENARIO.md](TEST_SCENARIO.md) - Kịch bản test đầy đủ
- [CONTEXT_MANAGEMENT_GUIDE.md](CONTEXT_MANAGEMENT_GUIDE.md) - Hướng dẫn quản lý Kubernetes contexts
- [API_INTEGRATION_STATUS.md](API_INTEGRATION_STATUS.md) - Trạng thái tích hợp API
- [COMPREHENSIVE_DOCUMENTATION.md](COMPREHENSIVE_DOCUMENTATION.md) - Tài liệu kỹ thuật toàn diện

