namespace QuanLyKhachSan_MVC.NET.Models
{
    public class Modeldata
    {
        public Tang tang { get; set; }
        public List<Tang> listtang { get; set; }
        public Phong phong { get; set; }
        public List<Phong> listphong { get; set; }
        public List<Phong> listphongtrangthai { get; set; }
        public BoPhan boPhan { get; set; }
        public List<BoPhan> listbophan { get; set; }
        public ChucVu chucVu { get; set; }
        public List<ChucVu> listchucVu { get; set; }
        public ViTriBoPhan viTriBoPhan { get; set; }
        public List<ViTriBoPhan> listviTriBoPhan { get; set; }
        public NhanVien nhanVien { get; set; }
        public List<NhanVien> listnhanVien { get; set; }
        public LoaiDatPhong loaiDatPhong { get; set; }
        public List<LoaiDatPhong> listloaiDatPhong { get; set; }
        public DatPhong datPhong { get; set; }
        public List<DatPhong> listdatPhong { get; set; }
        public KhachHang khachhang { get; set; }
        public List<KhachHang> listkhachHangs { get; set; }
        public SanPham sanPham { get; set; }
        public List<SanPham> listsanPham { get; set; }
        public ThueSanPham thueSanPham { get; set; }
        public List<ThueSanPham> listthueSanPham { get; set; }
        public float tongtienhueSanPham { get; set; }
        public TraPhong traphong { get; set; }
        public List<TraPhong> listtraphong { get; set; }
        public LichSuThanhToan lichSuThanhToan { get; set; }
        public List<LichSuThanhToan> listlichSuThanhToan { get; set; }

    }
}
