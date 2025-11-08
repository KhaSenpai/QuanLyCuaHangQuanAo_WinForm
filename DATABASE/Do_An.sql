-- Tạo cơ sở dữ liệu
CREATE DATABASE QuanLyCuaHangLocalBrand;
GO
USE QuanLyCuaHangLocalBrand;
GO

-- 1. CHỨC VỤ
CREATE TABLE CHUC_VU (
    MaChucVu VARCHAR(20) PRIMARY KEY,
    TenChucVu NVARCHAR(50) NOT NULL UNIQUE,
    MoTa NVARCHAR(200)
);
GO

-- 2. THƯƠNG HIỆU
CREATE TABLE THUONG_HIEU (
    MaThuongHieu VARCHAR(20) PRIMARY KEY,
    TenThuongHieu NVARCHAR(50) NOT NULL UNIQUE,
    MoTa NVARCHAR(200)
);
GO

-- 3. LOẠI SẢN PHẨM
CREATE TABLE LOAI_SAN_PHAM (
    MaLoaiSP VARCHAR(20) PRIMARY KEY,
    TenLoaiSP NVARCHAR(50) NOT NULL UNIQUE,
    MoTa NVARCHAR(200)
);
GO

-- 4. CHẤT LIỆU
CREATE TABLE CHAT_LIEU (
    MaChatLieu VARCHAR(20) PRIMARY KEY,
    TenChatLieu NVARCHAR(50) NOT NULL UNIQUE,
    MoTa NVARCHAR(200)
);
GO

-- 5. NHÀ CUNG CẤP
CREATE TABLE NHA_CUNG_CAP (
    MaNCC VARCHAR(20) PRIMARY KEY,
    TenNCC NVARCHAR(100) NOT NULL,
    DiaChi NVARCHAR(100) NOT NULL,
    DienThoai VARCHAR(15) NOT NULL UNIQUE,
    Email VARCHAR(50) UNIQUE CHECK (Email LIKE '%@%.%')
);
GO

-- 6. KHUYẾN MÃI
CREATE TABLE KHUYEN_MAI (
    MaKhuyenMai VARCHAR(20) PRIMARY KEY,
    TenKhuyenMai NVARCHAR(100) NOT NULL,
    PhanTramKhuyenMai DECIMAL(5,2) NOT NULL CHECK (PhanTramKhuyenMai BETWEEN 0 AND 100),
    NgayBatDau DATE NOT NULL,
    NgayKetThuc DATE NOT NULL,
    MoTa NVARCHAR(200),
    CHECK (NgayKetThuc >= NgayBatDau)
);
GO

-- 7. KHÁCH HÀNG
CREATE TABLE KHACH_HANG (
    MaKhachHang VARCHAR(20) PRIMARY KEY,
    TenKhachHang NVARCHAR(50) NOT NULL,
    NgaySinh DATE,
    Email VARCHAR(50) UNIQUE CHECK (Email LIKE '%@%.%'),
    SoDienThoai VARCHAR(15) NOT NULL UNIQUE,
    DiaChi NVARCHAR(100) NOT NULL,
);
GO

-- 8. NHÂN VIÊN
CREATE TABLE NHAN_VIEN (
    MaNhanVien VARCHAR(20) PRIMARY KEY,
    HoTen NVARCHAR(50) NOT NULL,
    NgaySinh DATE NOT NULL CHECK (NgaySinh < GETDATE()),
    GioiTinh NVARCHAR(10) CHECK (GioiTinh IN (N'Nam', N'Nữ', N'Khác')),
    SoDienThoai VARCHAR(15) NOT NULL UNIQUE,
    MaChucVu VARCHAR(20) NOT NULL,
    FOREIGN KEY (MaChucVu) REFERENCES CHUC_VU(MaChucVu)
);
GO

-- 9. TÀI KHOẢN
CREATE TABLE TAI_KHOAN (
    MaNhanVien VARCHAR(20) PRIMARY KEY,
    TenDangNhap VARCHAR(50) NOT NULL UNIQUE,
    MatKhau VARCHAR(100) NOT NULL,
    QuyenTruyCap NVARCHAR(50) NOT NULL CHECK (QuyenTruyCap IN ('Admin', 'User', 'Manager')),
    FOREIGN KEY (MaNhanVien) REFERENCES NHAN_VIEN(MaNhanVien)
);
GO

-- 10. SẢN PHẨM
CREATE TABLE SAN_PHAM (
    MaSP VARCHAR(20) PRIMARY KEY,
    TenSP NVARCHAR(100) NOT NULL,
    SoLuongTon INT NOT NULL DEFAULT 0 CHECK (SoLuongTon >= 0),
    DonGiaBan DECIMAL(18,2) NOT NULL CHECK (DonGiaBan > 0),
    DonGiaNhap DECIMAL(18,2) NOT NULL CHECK (DonGiaNhap > 0),
    MauSac NVARCHAR(50),
    KichCo NVARCHAR(50),
    NgaySanXuat DATE CHECK (NgaySanXuat <= GETDATE()),
    MoTa NVARCHAR(200),
    MaThuongHieu VARCHAR(20) NOT NULL,
    MaLoaiSP VARCHAR(20) NOT NULL,
    MaChatLieu VARCHAR(20) NOT NULL,
    MaNCC VARCHAR(20) NOT NULL,
    FOREIGN KEY (MaThuongHieu) REFERENCES THUONG_HIEU(MaThuongHieu),
    FOREIGN KEY (MaLoaiSP) REFERENCES LOAI_SAN_PHAM(MaLoaiSP),
    FOREIGN KEY (MaChatLieu) REFERENCES CHAT_LIEU(MaChatLieu),
    FOREIGN KEY (MaNCC) REFERENCES NHA_CUNG_CAP(MaNCC)
);
GO


-- 13. HÓA ĐƠN BÁN
CREATE TABLE HOA_DON_BAN (
    MaHoaDon VARCHAR(20) PRIMARY KEY,
    NgayLapHoaDon DATE NOT NULL CHECK (NgayLapHoaDon <= GETDATE()),
    TongTien DECIMAL(18,2) NOT NULL CHECK (TongTien >= 0),
    MaNhanVien VARCHAR(20) NOT NULL,
    MaKhuyenMai VARCHAR(20),
    MaKhachHang VARCHAR(20) NOT NULL,
    DiaChi NVARCHAR(100),
    GhiChu NVARCHAR(200),
    FOREIGN KEY (MaNhanVien) REFERENCES NHAN_VIEN(MaNhanVien),
    FOREIGN KEY (MaKhuyenMai) REFERENCES KHUYEN_MAI(MaKhuyenMai),
    FOREIGN KEY (MaKhachHang) REFERENCES KHACH_HANG(MaKhachHang)
);
GO

-- 14. CHI TIẾT HÓA ĐƠN BÁN
CREATE TABLE CHI_TIET_HOA_DON_BAN (
    MaHoaDon VARCHAR(20),
    MaSP VARCHAR(20),
    SoLuongBan INT NOT NULL CHECK (SoLuongBan > 0),
    DonGia DECIMAL(18,2) NOT NULL CHECK (DonGia > 0),
    ThanhTien DECIMAL(18,2) NOT NULL,
    PRIMARY KEY (MaHoaDon, MaSP),
    FOREIGN KEY (MaHoaDon) REFERENCES HOA_DON_BAN(MaHoaDon) ON DELETE CASCADE,
    FOREIGN KEY (MaSP) REFERENCES SAN_PHAM(MaSP) ON DELETE NO ACTION
);
GO

-- 15. PHIẾU NHẬP
CREATE TABLE PHIEU_NHAP (
    MaPhieuNhap VARCHAR(20) PRIMARY KEY,
    NgayLapPhieuNhap DATE NOT NULL CHECK (NgayLapPhieuNhap <= GETDATE()),
    TongTien DECIMAL(18,2) NOT NULL CHECK (TongTien >= 0),
    MaNCC VARCHAR(20) NOT NULL,
    MaNhanVien VARCHAR(20) NOT NULL,
    DiaChi NVARCHAR(100),
    GhiChu NVARCHAR(200),
    FOREIGN KEY (MaNCC) REFERENCES NHA_CUNG_CAP(MaNCC),
    FOREIGN KEY (MaNhanVien) REFERENCES NHAN_VIEN(MaNhanVien)
);
GO

-- 16. CHI TIẾT PHIẾU NHẬP
CREATE TABLE CHI_TIET_PHIEU_NHAP (
    MaPhieuNhap VARCHAR(20),
    MaSP VARCHAR(20),
    SoLuong INT NOT NULL CHECK (SoLuong > 0),
    DonGia DECIMAL(18,2) NOT NULL CHECK (DonGia > 0),
    ThanhTien DECIMAL(18,2) NOT NULL,
    PRIMARY KEY (MaPhieuNhap, MaSP),
    FOREIGN KEY (MaPhieuNhap) REFERENCES PHIEU_NHAP(MaPhieuNhap) ON DELETE CASCADE,
    FOREIGN KEY (MaSP) REFERENCES SAN_PHAM(MaSP) ON DELETE NO ACTION
);
GO


-- 18. HINH_ANH
CREATE TABLE HINH_ANH (
    MaHinhAnh VARCHAR(20) PRIMARY KEY,
    MaSP VARCHAR(20) NOT NULL,
    DuongDanHinh NVARCHAR(200) NOT NULL,
    FOREIGN KEY (MaSP) REFERENCES SAN_PHAM(MaSP) ON DELETE CASCADE
);
GO

-- Tạo chỉ mục để tối ưu hóa truy vấn
CREATE NONCLUSTERED INDEX IX_SAN_PHAM_MaSP ON SAN_PHAM(MaSP);
CREATE NONCLUSTERED INDEX IX_SAN_PHAM_MaThuongHieu ON SAN_PHAM(MaThuongHieu);
CREATE NONCLUSTERED INDEX IX_CHI_TIET_HOA_DON_BAN_MaHoaDon ON CHI_TIET_HOA_DON_BAN(MaHoaDon);
CREATE NONCLUSTERED INDEX IX_CHI_TIET_PHIEU_NHAP_MaPhieuNhap ON CHI_TIET_PHIEU_NHAP(MaPhieuNhap);
CREATE NONCLUSTERED INDEX IX_HINH_ANH_MaSP ON HINH_ANH(MaSP);
GO


-- Trigger để kiểm tra và cập nhật ThanhTien trong CHI_TIET_HOA_DON_BAN
CREATE TRIGGER TRG_CheckThanhTien_ChiTietHoaDonBan
ON CHI_TIET_HOA_DON_BAN
AFTER INSERT, UPDATE
AS
BEGIN
    UPDATE CHI_TIET_HOA_DON_BAN
    SET ThanhTien = inserted.SoLuongBan * inserted.DonGia
    FROM inserted
    WHERE CHI_TIET_HOA_DON_BAN.MaHoaDon = inserted.MaHoaDon
    AND CHI_TIET_HOA_DON_BAN.MaSP = inserted.MaSP;
END;
GO

-- Trigger để kiểm tra và cập nhật ThanhTien trong CHI_TIET_PHIEU_NHAP
CREATE TRIGGER TRG_CheckThanhTien_ChiTietPhieuNhap
ON CHI_TIET_PHIEU_NHAP
AFTER INSERT, UPDATE
AS
BEGIN
    UPDATE CHI_TIET_PHIEU_NHAP
    SET ThanhTien = inserted.SoLuong * inserted.DonGia
    FROM inserted
    WHERE CHI_TIET_PHIEU_NHAP.MaPhieuNhap = inserted.MaPhieuNhap
    AND CHI_TIET_PHIEU_NHAP.MaSP = inserted.MaSP;
END;
GO

-- Trigger để cập nhật SoLuongTon khi thêm/sửa/xóa CHI_TIET_PHIEU_NHAP (nhập kho)
CREATE TRIGGER TRG_UpdateSoLuongTon_NhapKho
ON CHI_TIET_PHIEU_NHAP
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    BEGIN TRANSACTION;
    -- Khóa bảng SAN_PHAM để tránh xung đột
    UPDATE SAN_PHAM WITH (ROWLOCK, UPDLOCK)
    SET SoLuongTon = SoLuongTon + (
        SELECT SUM(ISNULL(inserted.SoLuong, 0) - ISNULL(deleted.SoLuong, 0))
        FROM inserted
        FULL OUTER JOIN deleted ON inserted.MaSP = deleted.MaSP
        WHERE inserted.MaSP = SAN_PHAM.MaSP OR deleted.MaSP = SAN_PHAM.MaSP
        GROUP BY inserted.MaSP, deleted.MaSP
    )
    WHERE MaSP IN (SELECT MaSP FROM inserted UNION SELECT MaSP FROM deleted);
    COMMIT TRANSACTION;
END;
GO

-- Trigger để cập nhật SoLuongTon khi thêm/sửa/xóa CHI_TIET_HOA_DON_BAN (xuất kho)
CREATE TRIGGER TRG_UpdateSoLuongTon_XuatKho
ON CHI_TIET_HOA_DON_BAN
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    BEGIN TRANSACTION;
    -- Kiểm tra đủ số lượng tồn trước khi xuất
    IF EXISTS (
        SELECT 1
        FROM inserted i
        JOIN SAN_PHAM sp ON i.MaSP = sp.MaSP
        WHERE i.SoLuongBan > sp.SoLuongTon
    )
    BEGIN
        RAISERROR ('Số lượng tồn kho không đủ để bán!', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END;

    -- Cập nhật SoLuongTon
    UPDATE SAN_PHAM WITH (ROWLOCK, UPDLOCK)
    SET SoLuongTon = SoLuongTon - (
        SELECT SUM(ISNULL(inserted.SoLuongBan, 0) - ISNULL(deleted.SoLuongBan, 0))
        FROM inserted
        FULL OUTER JOIN deleted ON inserted.MaSP = deleted.MaSP
        WHERE inserted.MaSP = SAN_PHAM.MaSP OR deleted.MaSP = SAN_PHAM.MaSP
        GROUP BY inserted.MaSP, deleted.MaSP
    )
    WHERE MaSP IN (SELECT MaSP FROM inserted UNION SELECT MaSP FROM deleted);
    COMMIT TRANSACTION;
END;
GO

-- Trigger để cập nhật TongTien trong HOA_DON_BAN
CREATE TRIGGER TRG_UpdateTongTien_HoaDonBan
ON CHI_TIET_HOA_DON_BAN
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    UPDATE HOA_DON_BAN
    SET TongTien = ISNULL((
        SELECT SUM(cthdb.ThanhTien) * (1 - ISNULL(km.PhanTramKhuyenMai, 0) / 100)
        FROM CHI_TIET_HOA_DON_BAN cthdb
        LEFT JOIN KHUYEN_MAI km ON HOA_DON_BAN.MaKhuyenMai = km.MaKhuyenMai
        WHERE cthdb.MaHoaDon = HOA_DON_BAN.MaHoaDon
            AND (km.MaKhuyenMai IS NULL OR GETDATE() BETWEEN km.NgayBatDau AND km.NgayKetThuc)
        GROUP BY cthdb.MaHoaDon, km.PhanTramKhuyenMai
    ), 0)
    WHERE MaHoaDon IN (SELECT MaHoaDon FROM inserted UNION SELECT MaHoaDon FROM deleted);
END;
GO

-- Trigger để cập nhật TongTien trong PHIEU_NHAP
CREATE TRIGGER TRG_UpdateTongTien_PhieuNhap
ON CHI_TIET_PHIEU_NHAP
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    UPDATE PHIEU_NHAP
    SET TongTien = ISNULL((
        SELECT SUM(ThanhTien)
        FROM CHI_TIET_PHIEU_NHAP ctpn
        WHERE ctpn.MaPhieuNhap = PHIEU_NHAP.MaPhieuNhap
    ), 0)
    WHERE MaPhieuNhap IN (SELECT MaPhieuNhap FROM inserted UNION SELECT MaPhieuNhap FROM deleted);
END;
GO

-- Trigger để kiểm tra khuyến mãi
CREATE TRIGGER TRG_CheckKhuyenMai
ON HOA_DON_BAN
AFTER INSERT, UPDATE
AS
BEGIN
    IF EXISTS (
        SELECT 1
        FROM inserted i
        JOIN KHUYEN_MAI km ON i.MaKhuyenMai = km.MaKhuyenMai
        WHERE GETDATE() NOT BETWEEN km.NgayBatDau AND km.NgayKetThuc
    )
    BEGIN
        RAISERROR ('Khuyến mãi không còn hiệu lực!', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END;
END;
GO
